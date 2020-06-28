
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

import default_modules.Tools as Tools
from default_modules.Error import _Error
from default_modules.Database import Database

# #### Third-parties

# - [requests]('http://docs.python-requests.org/en/master/')
# - [pandas]('https://pandas.pydata.org/')
# - [pyodbc]('https://github.com/mkleehammer/pyodbc/wiki/')


# In[ ]:

class ExternalDataProcessor:

    def __init__(self, externalBDInstance, path_to_settings_file, path_tmp_folder_database_file):

        self.externalBD = externalBDInstance        
        self.readXmlFile = Tools.ReadXmlFile(path_to_settings_file)
        self.START_SCHOOL = self.readXmlFile.getStartSchool()
        self.END_SCHOOL = self.readXmlFile.getEndSchool()
        self.PATH_SETTINGS_FILE = path_to_settings_file
        self.PATH_TMP_FOLDER_DATABASE_FILE = path_tmp_folder_database_file
        self.database = Database(path_to_settings_file, path_tmp_folder_database_file)
        self.students = None
    

    timeout = 2400 #40min
    
    
    def processTimeTable(self, user, userExitRegime, timeout = datetime.now().timestamp() + timeout):

        
        start_school = Tools.Timer.timeToSecond(self.START_SCHOOL)
        end_day = Tools.Timer.timeToSecond(time(23,59))
        
        def exitAuthorizationAdded(start_time_s, end_time_s, completeToEndOfDay=False):
            if userExitRegime is not None:
                list_timeSlot = userExitRegime.getFreeTimeSlot(start_time_s, end_time_s)
                
                if len(list_timeSlot)==0:
                    return False
                
                if completeToEndOfDay:
                    list_timeSlot[len(list_timeSlot)-1].End = end_day
                    

                for timeSlot in list_timeSlot:
                    reason = "Temps libre ({0} - {1})".format(timedelta(seconds = timeSlot.Start), timedelta(seconds = timeSlot.End))
                    self.database.addExitAuthorization(user['id_BD_local'], timeSlot.Start, timeSlot.End, verification = False, reason=reason)
                    
                    
                return True
            return False
        
        
        '''
            Retrieving the timetable
        '''
        timetable = self.externalBD.todayCalendar(user, timeout)
        
        '''
            Processing the timetable
        '''
        if timetable is None or timetable.empty:
            start_free_time_period = Tools.Timer.timeToSecond(self.END_SCHOOL)
            end_free_time_period = Tools.Timer.timeToSecond(time(23,59,59))
            self.database.addExitAuthorization(user['id_BD_local'], start_free_time_period, end_free_time_period, verification = False, reason="Fin officielle des cours")
            
            return False
        
        '''
            We need to work on the free hours for each student, and the timetable give us 
            all the leisures hours for each student.
            Thus, we need to sort the array following the time of each record
            
            Notice that the given table is linked with a student "user"
            So all the time in the table is only about the  given user
        '''
        
        timetable = timetable.sort_values(['startTime', 'endTime'])
        
        
        begin = start_school
        start_free_time_period = 0
        end_free_time_period = 0
        start_time = 0
        end_time = 0
        
        for index, row in timetable.iterrows():
            '''
                loop into the leisures of the day
            '''
            start_time = row['startTime']
            end_time = row['endTime']
            deletedLeisure = row['deleted']
            
            
            if start_time<start_school :
                continue
            
            #process free time between leisures
            start_free_time_period = begin
            
            if not deletedLeisure:#leisure has not been removed
                end_free_time_period = start_time
                begin = end_time 
                
                #find an exit regime authorization that matches the free time period and add it into the database
                exitAuthorizationAdded(start_free_time_period, end_free_time_period)
                
                start_free_time_period = begin #reset free time period
                
        #end loop thtough leisures
        end_time_last_leisure = end_time #end of the last leisure for the student (deleted or not)
        
        
        
        if userExitRegime is not None: 
            start_free_time_period = begin
            
            if userExitRegime.ExitEndOfDay: #student can leaves school when he has no more leisure (for example when the last continious leisures in his timetable has been deleted)
                end_free_time_period = end_day
                self.database.addExitAuthorization(user['id_BD_local'], start_free_time_period, end_free_time_period, verification = False, reason="Fin de l'EDT (autorisation <<"+">>)")
                
                
            else:#check for authorization
                end_free_time_period = end_time_last_leisure #free time period goes to the end of student official timetable (event if the last leisure has been deleted today)
                if not exitAuthorizationAdded(start_free_time_period, end_free_time_period, completeToEndOfDay=True) :#student can exit school at the official time of its timetable
                    #free time period goes from the end of the last leisure (event if it has been deleted today) until the end of the day
                    start_free_time_period = end_time_last_leisure
                    end_free_time_period = end_day
                
                    self.database.addExitAuthorization(user['id_BD_local'], start_free_time_period, end_free_time_period, verification = False, reason="Fin officielle des cours")
                    
                    
        else : #student can exit school at the official time of its timetable
            #student can leave school at the end of his last leisure end time in his official timetable (even if the leisure has been deleted today)
            start_free_time_period = end_time_last_leisure
            end_free_time_period = end_day
            
            self.database.addExitAuthorization(user['id_BD_local'], start_free_time_period, end_free_time_period, verification = False, reason="Fin officielle des cours")
           
            
        
        return True
    


# In[ ]:
    def update(self):
        '''
            Computing the halting time
        '''
        
        halt = datetime.now().timestamp() + self.timeout
        
        '''
            Retrieving the users
        '''
        students = self.database.selectAllStudent(halt)
        
        students['nom']=students['nom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
        students['prenom']=students['prenom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
        self.students = students
        if students is None:
            _Error.raiseError("Erreur : base de données locale vide\nAucun élève à sélectionner")
        
        
        #external_data =self.externalBD.getAllUsers(halt)
        #external_data = self.externalBD.processUsers(students)
        #external_data.to_csv('students_oze_info.csv')
        external_data = pandas.read_csv('students_oze_info.csv')
        if external_data is None:
            _Error.raiseError("Erreur : aucun élève accessible depuis la base de données externe")
        
        #merge data
        users, unmatch = self.matcheStudent(halt, students, external_data)
        if unmatch is not None :
            print("\n---------------------------------")
            print("Les étudiants suivant n'ont pas été trouvés sur oZe (leurs informations ne seront donc pas mises à jour)")
            print("\n")
            print(unmatch[['nom', 'prenom', 'id_BD_local']])
            print("\n----------------------------------\n")
        
        '''
            process data
        '''
        self.updateStudentBan(unmatch, halt)
        self.updateStudentExit(users, halt)
        
        return None
    

# In[]

    def matcheStudent(self, halt, internal_data, external_dataframe, process_unmacth=True):
        """
            Into Oze each student have a specific id which is independant from the one use into the local
            database. Thus, we need to get all the student information (last name, first name and used id)
            Then matches the table id (from the local database) and the used id (from oze)
        """
        local = internal_data
        extern = external_dataframe
        
        if extern is None:
            _Error.raiseError("Erreur : aucun élève accessible depuis Oze")
    
        extern['nom']=extern['nom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
        extern['prenom']=extern['prenom'].apply(lambda x : Tools.normalizeString(x, 'ascii'))
        
        #matches table id and used id
        columns = ['nom', 'prenom']    
        users = pandas.merge(local, extern, on = columns).drop_duplicates(subset = columns)
        
        unmatch = None
        if process_unmacth:
            unmatch = local[(~local['nom'].isin(users['nom']))&(~local['prenom'].isin(users['prenom']))]
            if len(unmatch)==0 :
                unmatch = None
        
        users = deque(users.to_dict('records'))
       
        return users, unmatch

        
    # In[]
    def updateStudentExit(self, users, halt):
        print("\t*Mise à jour des autorisations de sorties le : " + str(datetime.now()))
        '''
            Computing the halting time per request
        '''
        chunk = (halt - datetime.now().timestamp()) / len(users)
        '''
            Processing the users        
        ''' 
        
        
        #get exit regime
        tmp = self.database.getExitRegimes(halt)
        
        #convert it inot dictionnay
        exitRegimes = {}
        for exitRegime in tmp:
            exitRegimes[exitRegime.Id] = exitRegime
        
        #get default exit regime
        defaultExitRegime = None
        table_id = self.readXmlFile.getDefaultExitRegimeDbId()
        if table_id != -1:
            if table_id in exitRegimes:
                defaultExitRegime = exitRegimes[table_id]
        
        
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
            
            userExitRegime = None
            regime_id = user['id_exit_regime']
            if regime_id in exitRegimes:
                userExitRegime = exitRegimes[regime_id]
            else :
                userExitRegime = defaultExitRegime
            
            
            error = self.processTimeTable(user, userExitRegime, datetime.now().timestamp() + chunk)
            if not error :
                '''
                    current user has not been updated
                '''
                #users.appendleft(user) 
            
        
        print("\t\t* Mise à jour de la base de données : " + str(datetime.now()))
        self.database.updateExitAuthorizations(halt)
        
        return None
        
    
    # In[]
    def updateStudentBan(self, unmatch, halt):
        print("\t*Mise à jour des interdiction de sorties le : " + str(datetime.now()))
        
        start = datetime.combine(date.today(), self.START_SCHOOL).timestamp()
        end = datetime.combine(date.today(), time(23,59,59)).timestamp()
        '''
        
        update punishment
        
        '''
        print("\t\t*Récupération des punitions non cloturées le : " + str(datetime.now()))
        
        data = self.externalBD.nonClosedPunishement(halt)
        
        if data is not None :
            match_data, unmatch_data = self.matcheStudent(halt,self.students, data, process_unmacth=False)
            
            for row in match_data:
                self.database.addExitBan(row['id_BD_local'], start, end, reason="Punition(s) non cloturee(s)")
        
        '''
        
        update non justify element
        
        '''
        print("\t\t*Récupération des absences/retards non cloturé(e)s le : " + str(datetime.now()))
        
        data = self.externalBD.nonJustifyElement(halt)
        
        if data is not None :
            match_data, unmatch_data = self.matcheStudent(halt,self.students, data, process_unmacth=False)
            
            for row in match_data:
                self.database.addExitBan(row['id_BD_local'], start, end, reason="Absence(s)\Retard(s) non cloture(e)(s)")
        
        ''' 
        update database
        '''
        
        if unmatch is not None:
            for index, row in unmatch.iterrows():
                msg = "Aucun E.D.T trouve sur Oze"
                self.database.addExitBan(row['id_BD_local'], start, end, reason=msg)
                
        
        print("\t\t* Mise à jour de la base de données : " + str(datetime.now()))
        self.database.updateExitBans(halt)
        
        return None

# In[]
    