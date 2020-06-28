
"""
class ExternalBD:
    
    def getAllUsers(halt):
        '''
        must return all the student into the external database as a dataframe which must at least contains two columns :
            "nom" : the last name of the student as a string
            "prenom" : the first name of the student as a string
        
        The output object will then be merge with the data of the internal database and all the operation on student will
        be on the new output object (that keep intact the columns of the external data)
        '''
        
        return None
    
    def todayCalendar(user, timeout):
        ''' 
        return a dataframe object that represents all the leisures of the student represented by the object "user" that fit the format 
        of the dataframe as a row in the function "getAllUsers"
        the output dataframe must have three columns : 
            "startTime" : the start point of the leisure in second as an integer
            "endtime" : the end point of the leisure in second as an integer
            "leisure" : the name of the leisure as a string
            "deleted" : a boolean that represents if the leisure is deleted or not
        
        '''
        return None
    
    
    
    def nonClosedPunishement(halt):
        '''
        must return all the student of the external database that have a punishment. The output object is a dataframe 
        which must at least contains two columns :
            "nom" : the last name of the student as a string
            "prenom" : the first name of the student as a string
        
        '''
        return None
    
    def nonJustifyElement(halt):
        '''
        must return all the student of the external database that have an absence. The output object is a dataframe 
        which must at least contains two columns :
            "nom" : the last name of the student as a string
            "prenom" : the first name of the student as a string
        
        '''
        
        return None

"""

# In[ ]:


from collections import deque
from datetime import date, datetime, time, timedelta
import pytz
from dateutil.parser import parse

import pandas
import requests

import default_modules.Tools as Tools
from default_modules.Error import _Error
from custom_modules.CustomSettings import CustomSettings
import urllib.parse
from tqdm import tqdm


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


class ExternalBD:

    
    def __init__(self):
        
        self.credential = { 
                'username': CustomSettings.OZE_USERNAME,
                'password': CustomSettings.OZE_PWD
                }

        
        self.START_DATE_CHECKING_PUNISHMENT = CustomSettings.START_DATE_CHEKING_PUNISHMENT
        self.START_DATE_CHECKING_NON_CLOSE_ELEMENTS = CustomSettings.START_DATE_CHECKING_NON_CLOSED_ELEMENTS
        self.DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE = CustomSettings.DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE
        self.DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT = CustomSettings.DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT
        
        
    protocol = 'https://'
    root = 'enc.hauts-de-seine.fr/'
    paths = {
        None: '',
        'login': 'my.policy', 
        'users': 'v1/users',
        'classes': 'v1/cours/byEleve',
        'schoolLife' : 'v2/viescolaire',
        'usersearch' : 'v1/usersearch'
    }
    timeout = 2400 #40min
    
    
    def get(self, url, timeout = datetime.now().timestamp() + timeout, toDf=True):
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
                request = session.get(self.url())
                '''
                    Sending the credential
                '''
                request = session.post(self.url('login'), data = self.credential)
                '''
                    Retrieving data
                '''
                request = session.get(url)
                '''
                    Returning content
                '''
                
                if toDf:
                    return pandas.read_json(request.content)
                else :
                    return request.json()
            
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
        
    
    def url(self, path = None, api = False):
        url = self.root
        if api:
            url = 'api-' + url
        return self.protocol + url + self.paths[path]
   
    def processUsers(self, df_internalDbUsers, timeout = datetime.now().timestamp() + timeout):
        '''
        associate the external  for each user
        '''
        print("\t\t*Extraction des informations étudiantes en ligne : " + str(datetime.now()))
        
        base_url = self.url('usersearch', True)
        base_url = base_url + '?ctx_etab=0921241Z'
        base_url = base_url + '&aLimit=1'
        base_url = base_url + '&aPourConnectionLocale=false'
        base_url = base_url + '&aProfils=ELEVE'
        base_url = base_url + '&aSearchStr='
        
        compt_pourcent = 0
        delta = 6
        size = int(len(df_internalDbUsers)/delta) #size of each interval
        i = size
        
        list_dic = []
        for index, row in df_internalDbUsers.iterrows():
            
            if i >= size : #display pourcent
                print("\t\t\t" + str(int(compt_pourcent/delta*100)) + "% effectué le : " + str(datetime.now()))
                compt_pourcent = compt_pourcent + 1
                i = 0
            i = i+1 
            
            label = row['nom'] + " " + row['prenom']
            label = urllib.parse.quote_plus(label)
            
            url = base_url + label
            
            data = self.get(url, timeout=timeout, toDf=False)
            if data is None:
                continue
            if len(data)==0:
                continue
            
            data = data[0]
            student_id = data['id']
            
            list_dic.append({'nom':row['nom'], 'prenom':row['prenom'], 'id':student_id})
        
        return pandas.DataFrame(list_dic)
    
    def getAllUsers(self, timeout = datetime.now().timestamp() + timeout):
        '''
            Building the url
        '''
        url = self.url('users', True)
        url = url + '?aUai=0921241Z'
        url = url + '&aCategory=Eleve'
        #url="https://api-enc.hauts-de-seine.fr/v1/users/search?ctx_profil=PRINCIPAL&ctx_etab=0921241Z&aUais=0921241Z&aProfil=ELEVE&aSort=Nom&aSort=Prenom&aFields=Id&aFields=Nom&aFields=Prenom&aFields=Profils&aFields=Classes&aFields=Groups&aFields=Email&aFields=Desactive&aFields=AAF&aFields=_createdDate&aFields=Uais&aFields=DateDeNaissance&aFields=SyncFusion&aFields=_deletedDate&aFields=_deletedStatus&aFields=TelSMS&aEtat=all&aGroupesRattaches=true&aClassOnlyForTeachers=false"
        print("url="+url)
        '''
            Retrieving the users
        '''
        
        data = self.get(url, timeout)
        '''
            Processing the users
        '''
        if data is None:
            return None
            
        if data.empty:
            print("empty")
            return data
        
        '''
            Processing the users
        '''
        data = data[['id','nom', 'prenom']]
        '''
            Return a list of users
        '''
        return data
    
    
    def todayCalendar(self, user, timeout = datetime.now().timestamp() + timeout):
        '''
            Computing the period
        '''
        
        ''' To get the day dd/mm/yyyy need to set the start of the range 
        to the previous day at "start_day_hour" (for example 22:00)
        and the end of the range need to be set to the current day at "start_day_hour"
        
        For example to get all the courses of a student at 01/10/2018 (dd/mm/yyyy)
        the begin of the range is 30/09/2018 at 22h00 and the end is 01/10/2018 at 22h00
        '''
        
        
        _time = datetime.strptime('12:00','%H:%M').time()
        _date = date.today()
        dt = datetime.combine(_date, _time)
        dt = parse(dt.strftime("%Y-%m-%dT%H:%M:%SZ"))
        localtime = dt.astimezone (pytz.timezone('Europe/Paris')) #local timezone because of summer/winter time delta
        start_day_hour = '2{0}:00:00.000Z'.format(4-(localtime.time().hour -12)) #22:00 or 23:00
        
        
        begin = _date - timedelta(1) #previous day
        end = _date
        
        '''
            Building the url
        '''
        url = self.url('classes', True) 
        url = url + '?ctx_etab=0921241Z&aEleve=' 
        url = url + user['id'] 
        url = url + '&aIncludeDateBornes=true'
        url = url + begin.strftime('&aDateDebut=%Y-%m-%d'+'T'+start_day_hour) 
        url = url + end.strftime('&aDateFin=%Y-%m-%d'+'T'+start_day_hour) 
        url = url + '&aDeletedStatus=1' #include deleted courses
        url = url + '&aRange='
        
        '''
            Retrieving the timetable
        '''
        data = self.get(url, timeout)
        if data is None:
            return None
            
        if data.empty:
            return data
        '''
            Processing the timetable
        '''
        
        if data is None:
            return None
            
        if data.empty:
            return data
        
        #-------------------------------------------------------------
        def timeConversion(str_dateTime):
            dt = parse(str_dateTime)
            localtime = dt.astimezone (pytz.timezone('Europe/Paris')) #local timezone because of summer/winter time delta
            
            return Tools.Timer.timeToSecond(localtime.time()) #return time in second
        #-------------------------------------------------------------    
        output = pandas.DataFrame(data, columns = ['startTime', 'endTime', 'leisure', 'deleted']) 
        
        
        output['startTime'] = data['dateDebut'].apply(lambda x: timeConversion(x))
        output['endTime'] = data['dateFin'].apply(lambda x: timeConversion(x))
        output['leisure'] = data['matieres'].apply(lambda x : x[0]["libelle"])
        output['deleted'] = data['_deletedStatus'].apply(lambda x: x==1) #1 for a deleted leisure
        
        '''
        data = data[data['elevesDel'].apply(lambda x : isInIterableObject(user['id'], x))==False]
        """When school remove a leisure it can be for the whole student or for some students in that class
        When the leisure is remove for the whole students it considered as deleted (DeletedStatus=0)
        but for the other case we need to check if the id of the current student is in the list of id that don't
        have that leisure (elevesDel=null or elevesDel=[student_id_1, student_id_2, ...])
        
        The previous statement remove all the leisures that contains the current student id in its
        list of deleted student for that leisure
        """
        This case means that the leisure is not deleted but the student has manually been removed from that one because of a punishment
        (so it's no a free time period)
        '''

        
        return output
    
    
    
    def nonClosedPunishement(self, timeout = datetime.now().timestamp() + timeout):
        start_date = self.START_DATE_CHECKING_PUNISHMENT
        end_date = date.today() + timedelta(days=-self.DAYS_BEFORE_CHECKING_SCHOOL_PUNISHMENT)
        
        if start_date>end_date :
            return None
        
        url = self.url('schoolLife', True)
        url = url + '?ctx_etab=0921241Z' 
        url = url + start_date.strftime('&aDateDebut=%Y-%m-%dT23:00:00.000Z')
        url = url + end_date.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aNaturesID=16' #punishment only
        url = url + '&aStatutID=16' #non close status
        url = url + '&aMotifID=MOTIF_TOUT'
        url = url + '&aTypeIncidentID=TYPE_INCIDENT_TOUT'
        url = url + '&aTypePunitionID=TYPE_PUNITION_TOUT'
        url = url + '&aElevesSansAbsence=false'
                                                             
        data = self.get(url, timeout)
        
        return data
    
    def nonJustifyElement(self, timeout = datetime.now().timestamp() + timeout):
        start_date = self.START_DATE_CHECKING_NON_CLOSE_ELEMENTS
        end_date = date.today() + timedelta(days=-self.DAYS_BEFORE_CHECKING_SCHOOL_LIFE_ABSENCE)
        
        if start_date>end_date :
            return None
        
        url = self.url('schoolLife', True)
        url = url + '?ctx_etab=0921241Z' 
        url = url + start_date.strftime('&aDateDebut=%Y-%m-%dT23:00:00.000Z')
        url = url + end_date.strftime('&aDateFin=%Y-%m-%dT23:00:00.000Z') 
        url = url + '&aNaturesID=2'
        url = url + '&aNaturesID=4'
        url = url + '&aStatutID=16' #non close status
        url = url + '&aMotifID=MOTIF_TOUT'
        url = url + '&aTypeIncidentID=TYPE_INCIDENT_TOUT'
        url = url + '&aTypePunitionID=TYPE_PUNITION_TOUT'
                                                                         
        data = self.get(url, timeout)
        
        return data
    
    
# In[]


