
# 
# ### User Story: Scraping Websites

# ### Goals

# 1. To retrieve the timetable from the users each hour.
#     1. To convert timetables into CSV with semicolon as a separator:
#         - class: string
#         - debut: timestamp (seconds)
#         - fin: timestamp (seconds)
#         - matiere: string
#         - nom: string
#         - prenom: string
#         
# 1. To compute the free time of each user from his timetable.
# 2. To persist the free time into a database 
#     1. To insert the free time by bulk

# ## Requirements

# ### Imports

# #### Standard

# In[ ]:


from collections import deque
from datetime import date, datetime, time, timedelta

import pandas
import requests


import default_modules.Tools as Tools
from default_modules.Error import _Error
from default_modules.Database import Database

# #### Third-parties

# - [requests]('http://docs.python-requests.org/en/master/')
# - [pandas]('https://pandas.pydata.org/')
# - [pyodbc]('https://github.com/mkleehammer/pyodbc/wiki/')


# In[ ]:
def isInIterableObject(elem_to_find, object_list):
    try :
        return elem_to_find in object_list
    except :
        return False


class oZe:

    protocol = 'https://'
    credential = {
        'username': 'usrname',
        'password': 'pwd'
    }
    root = 'enc.hauts-de-seine.fr/'
    paths = {
        None: '',
        'login': 'my.policy', 
        'users': 'v1/users',
        'classes': 'v1/cours/byEleve',
        'schoolLife' : 'v2/viescolaire'
    }
    timeout = 2400 #40min
    
    DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE = 3
    DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT = 1
    
    START_SCHOOL = Tools.ReadXmlFile.getStartSchool()
    BREAK_TIME = Tools.ReadXmlFile.getExitBreak()
    
    START_DATE_CHECKING_PUNISHMENT = date(2018, 12, 5) #years/month/day
    START_DATE_CHECKING_NON_CLOSE_ELEMENTS = date(2018, 12, 5)
     
    @staticmethod
    def get(url, timeout = datetime.now().timestamp() + timeout):
        '''
            Halting the process
        '''
        msg = None
        while datetime.now().timestamp() < timeout:
            '''
                ?
            '''
            msg = None
            session = None
            try:
                '''
                    Opening a session
                '''
                session = requests.Session()               
                '''
                    Sending a request
                '''
                request = session.get(oZe.url())
                '''
                    Sending the credential
                '''
                request = session.post(oZe.url('login'), data = oZe.credential)
                '''
                    Retrieving data
                '''
                request = session.get(url)
                '''
                    Returning content
                '''
                return pandas.read_json(request.content)
            except Exception as e:
                msg = "Requête rejetée par Oze : tentative de connexion à l'url \""+url+"\""
                msg += "\n" + str(type(e))
                msg += "\n" +str(e.args)
                msg += "\n" + str(e)
            finally:
                '''
                    Closing the session
                '''
                if session is not None:
                        session.close()

        if not(msg is None):
           _Error.raiseError(msg)
        
        return None
        
    @staticmethod
    def url(path = None, api = False):
        url = oZe.root
        if api:
            url = 'api-' + url
        return oZe.protocol + url + oZe.paths[path]
   
    @staticmethod
    def users(timeout = datetime.now().timestamp() + timeout):
        '''
            Building the url
        '''
        url = oZe.url('users', True)
        url = url + '?aUai=0921241Z'
        url = url + '&aCategory=Eleve'
        
        '''
            Retrieving the users
        '''
        data = oZe.get(url, timeout)
        '''
            Processing the users
        '''
        if data is None:
            return None
            
        if data.empty:
            return data
        
        '''
            Processing the users
        '''
        data = data[['id','nom', 'prenom']]
        '''
            Return a list of users
        '''
        return data
    
    @staticmethod
    def timetable(user, timeout = datetime.now().timestamp() + timeout):
        '''
            Computing the period
        '''
        today = date.today()
        weekday = (today.weekday() + 1) % 7
        begin = today - timedelta(days = weekday)
        end = today + timedelta(days = -weekday, weeks = 1)
        
        '''
            Building the url
        '''
        url = oZe.url('classes', True) 
        url = url + '?ctx_etab=0921241Z&aEleve=' 
        url = url + user['id'] 
        url = url + begin.strftime('&aDateDebut=%Y-%m-%dT23:00:00.000Z') 
        url = url + end.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aDeletedStatus=0'
  
        '''
            Retrieving the timetable
        '''
        data = oZe.get(url, timeout)
        if data is None:
            return None
            
        if data.empty:
            return data
        '''
            Processing the timetable
        '''
        data = data[data['elevesDel'].apply(lambda x : isInIterableObject(user['id'], x))==False]
        """When school remove a leisure it can be for the whole student or for some students in that class
        When the leisure is remove for the whole students it considered as deleted (DeletedStatus=0)
        but for the other case we need to check if the id of the current student is in the list of id that don't
        have that leisure (elevesDel=null or elevesDel=[student_id_1, student_id_2, ...])
        
        The previous statement remove all the leisures that contains the current student id in its
        list of deleted student for that leisure
        """
        
        if data is None:
            return None
            
        if data.empty:
            return data


        data = data[['classes', 'matieres', 'dateDebut', 'dateFin']]
       
        data['dateDebut'] = pandas.to_datetime(data['dateDebut'], format ='%Y-%m-%dT%H:%M:%SZ').astype('int64')
        data['dateDebut'] = data['dateDebut'].apply(lambda x: x // 1000000000)
        
        data['dateFin'] = pandas.to_datetime( data['dateFin'], format ='%Y-%m-%dT%H:%M:%SZ').astype('int64')
        data['dateFin'] = data['dateFin'].apply(lambda x: x // 1000000000)
        
        data = data['classes'].apply(pandas.Series).merge(data, right_index = True, left_index = True)
        data = data.drop(['classes'], axis = 1)
        data = data.melt(id_vars = ['matieres', 'dateDebut', 'dateFin'], value_name = 'class')
        data = data.drop('variable', axis = 1)
        data['class'] = data['class'].apply(lambda x: x['libelle'] if isinstance(x, dict) and 'libelle' in x else None)
        
        data = data['matieres'].apply(pandas.Series).merge(data, right_index = True, left_index = True)
        data = data.drop(['matieres'], axis = 1)
        data = data.melt(id_vars = ['class', 'dateDebut', 'dateFin'], value_name = 'matiere')
        data = data.drop('variable', axis = 1)
        data['matiere'] = data['matiere'].apply(lambda x: x['libelle'] if isinstance(x, dict) and 'libelle' in x else None)
        
        data['nom'] = user['nom']
        
        data['prenom'] = user['prenom']
        
        data['id_BD_local'] = user['id_BD_local']

        '''
            Return a list of classes
        '''
        return data
    
    @staticmethod
    def leisures(user, timeout = datetime.now().timestamp() + timeout):
        '''
            Retrieving the timetable
        '''
        timetable = oZe.timetable(user, timeout)
        '''
            Processing the timetable
        '''
        if timetable is None:
            return None
            
        if timetable.empty:
            return timetable
        
        '''
            Computing the period between 8h and 24h
        '''
        '''
            Because timetable contains all the leisures of the week we have to
            extarct only the leisures of the current day
            
            datetime.combine(date.today(), time()) give us a datetime set to the current day
            at 00h00
        '''
        
        start_school = Tools.Timer.timeToSecond(oZe.START_SCHOOL)
        break_time = Tools.Timer.timeToSecond(oZe.BREAK_TIME)
        middle_break_time = break_time/2
        begin = datetime.combine(date.today(), time()).timestamp() + start_school
        end = begin + (86340-start_school) # begin + (23h59 - start_school)    
        '''
            Extract the leisures of the current day  
        '''
        
        timetable = timetable[timetable['dateFin'].between(begin, end)]
        
        '''
            We need to work on the free hours for each student, and the timetable give us 
            all the leisures hours for each student.
            Thus, we need to sort the array following the time of each record
            
            Notice that the given table is linked with a student "user"
            So all the time in the table is only about the  given user
        '''
        timetable = timetable.sort_values(['dateDebut', 'dateFin'])
        
        
        #loop into the table
        for index, row in timetable.iterrows():
            '''
                loop into the leisures of the day
            '''

            if row['dateDebut'] - begin  >= break_time : 
                temp_end = begin + middle_break_time
                #create row
                Database.addExitAuthorization(user['id_BD_local'], begin, temp_end, verification = True, reason="Pause meridienne")
            
            
            begin = row['dateFin']
        #------------------------------------- End of the loop
        '''
            At that step begin is the last study hour of the day for the student
            So we add into the database when the student can leave the school in a regular way
        '''

        Database.addExitAuthorization(user['id_BD_local'], begin, end, verification = False, reason="Fin de l'emplois du temps")
        '''
            Return a list of leisures
        '''
        return 1
    
    
    @staticmethod
    def schoolLifeIncident(timeout = datetime.now().timestamp() + timeout):
        end_date = date.today()
        
        url = oZe.url('schoolLife', True)
        url = url + '?ctx_etab=0921241Z' 
        url = url + '&aDateDebut=2018-08-15T22:00:00.000Z' #start of the school years
        url = url + end_date.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aNaturesID=2'
        url = url + '&aNaturesID=4'
        url = url + '&aNaturesID=16'
        url = url + '&aStatutID=16'
        url = url + '&aMotifID=MOTIF_TOUT'
        url = url + '&aTypeIncidentID=TYPE_INCIDENT_TOUT'
        url = url + '&aTypePunitionID=TYPE_PUNITION_TOUT'
        
        #data = oZe.get(url, timeout)
        
        return url
    
    def nonClosedPunishement(timeout = datetime.now().timestamp() + timeout):
        start_date = oZe.START_DATE_CHECKING_PUNISHMENT
        end_date = date.today() + timedelta(days=-oZe.DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT)
        
        if start_date>end_date :
            return None
        
        url = oZe.url('schoolLife', True)
        url = url + '?ctx_etab=0921241Z' 
        url = url + start_date.strftime('&aDateDebut=%Y-%m-%dT23:00:00.000Z')
        url = url + end_date.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aNaturesID=16' #punishment only
        url = url + '&aStatutID=16' #non close status
        url = url + '&aMotifID=MOTIF_TOUT'
        url = url + '&aTypeIncidentID=TYPE_INCIDENT_TOUT'
        url = url + '&aTypePunitionID=TYPE_PUNITION_TOUT'
                 
                                                             
        data = oZe.get(url, timeout)
        if data is not None:
            data = data[['id']]
        
        return data
    
    def nonJustifyElement(timeout = datetime.now().timestamp() + timeout):
        start_date = oZe.START_DATE_CHECKING_NON_CLOSE_ELEMENTS
        end_date = date.today() + timedelta(days=-oZe.DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE)
        
        if start_date>end_date :
            return None
        
        url = oZe.url('schoolLife', True)
        url = url + '?ctx_etab=0921241Z' 
        url = url + start_date.strftime('&aDateDebut=%Y-%m-%dT23:00:00.000Z')
        url = url + end_date.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aNaturesID=2'
        url = url + '&aNaturesID=4'
        url = url + '&aStatutID=16' #non close status
        url = url + '&aMotifID=MOTIF_TOUT'
        url = url + '&aTypeIncidentID=TYPE_INCIDENT_TOUT'
        url = url + '&aTypePunitionID=TYPE_PUNITION_TOUT'
                                                                         
        data = oZe.get(url, timeout)
        if data is not None:
            data = data[['id']]
        
        return data

# In[ ]:
def update():
    '''
        Computing the halting time
    '''
    halt = datetime.now().timestamp() + oZe.timeout
    users = matcheStudent(halt)
    
    updateStudentBan(users, halt)
    updateStudentExit(users, halt)
    
    
    return None
    

# In[]

def matcheStudent(halt):
    '''
        Retrieving the users
    '''
    local = Database.selectAllStudent(halt)
    local['nom']=local['nom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
    local['prenom']=local['prenom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
    
    '''
        Processing the users
    '''
    if local is None:
        _Error.raiseError("Erreur : base de données locale vide\nAucun élève à sélectionner")
    
    '''
        Retrieving the users
    '''
    """
        Into Oze each student have a specific id which is independant from the one use into the local
        database. Thus, we need to get all the student information (last name, first name and used id)
        Then matches the table id (from the local database) and the used id (from oze)
    """
    extern = oZe.users(halt)#get all student ids
    if extern is None:
        _Error.raiseError("Erreur : aucun élève accessible depuis Oze")

    extern['nom']=extern['nom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
    extern['prenom']=extern['prenom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
    
    #matches table id and used id
    columns = ['nom', 'prenom']    
    users = pandas.merge(local, extern, on = columns).drop_duplicates(subset = columns)
    
    unmatch = local[(~local['nom'].isin(users['nom']))&(~local['prenom'].isin(users['prenom']))]
    if len(unmatch)!=0 :
        print("\n---------------------------------")
        print("Les étudiants suivant n'ont pas été trouvés sur oZe (leurs informations ne seront donc pas mises à jour)")
        print("\n")
        print(unmatch)
        print("\n----------------------------------\n")
    
    users = deque(users.to_dict('records'))
   
    return users

# In[]
def searchStudent(users, oze_id):
    
    if users is None:
        return None
    
    for i in range(len(users)):
        user = users[i]
        if user['id'] == oze_id:
            return user
        
    return None
# In[]
def updateStudentExit(users, halt):
    print("\t*Mise à jour des autorisations de sorties le : " + str(datetime.now()))
    '''
        Computing the halting time per request
    '''
    chunk = (halt - datetime.now().timestamp()) / len(users)
    '''
        Processing the users        
    ''' 
    
    print("\t\t* Récupération et analyse des données depuis OZE le : " + str(datetime.now()))
    print("\t\t  Veuillez assurer une connexion internet stable durant toute la durée du processus")
    
    compt_pourcent = 0
    delta = 6
    size = int(len(users)/delta) #size of each interval
    i = size
    
    for user in users:
        
        if i >= size : #display pourcent
            print("\t\t\t" + str(int(compt_pourcent/delta*100)) + "% effectué le : " + str(datetime.now()))
            compt_pourcent = compt_pourcent + 1
            i = 0

        i = i+1 
        '''
            Retrieving the leisures
        '''  
        leisures = oZe.leisures(user, datetime.now().timestamp() + chunk)
        
        if leisures is None:
            '''
                Appending a user
            '''
            users.appendleft(user) 
    
    
    print("\t\t* Mise à jour de la base de données : " + str(datetime.now()))
    Database.updateExitAuthorizations(halt)
    
    return None
    
# In[]
def updateStudentBan(users, halt):
    print("\t*Mise à jour des interdiction de sorties le : " + str(datetime.now()))
    
    start = datetime.combine(date.today(), oZe.START_SCHOOL).timestamp()
    end = datetime.combine(date.today(), time(23,59,59)).timestamp()
    '''
    
    update punishment
    
    '''
    print("\t\t*Récupération des punitions non cloturées le : " + str(datetime.now()))
    data = oZe.nonClosedPunishement(halt)
    
    if data is not None :
        for oze_id in data['id']:
            student = searchStudent(users,oze_id)
            if student is not None:
                Database.addExitBan(student['id_BD_local'], start, end, reason="Punition(s) non cloturee(s)")
    '''
    
    update non justify element
    
    '''
    print("\t\t*Récupération des absences/retards non cloturé(e)s le : " + str(datetime.now()))
    data = oZe.nonJustifyElement(halt)
    if data is not None :
        for oze_id in data['id']:
            student = searchStudent(users,oze_id)
            if student is not None:
                Database.addExitBan(student['id_BD_local'], start, end, reason="Absence(s)\Retard(s) non cloture(e)(s)")
    
    ''' 
    update database
    '''
    
    print("\t\t* Mise à jour de la base de données : " + str(datetime.now()))
    Database.updateExitBans(halt)
    
    return None

# In[]
    