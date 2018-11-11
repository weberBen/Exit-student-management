
# Code from Anderson Carlos Ferreira da Silva (github : @andersoncarlosfs)




#!/usr/bin/env python
# coding: utf-8

# # Website Scraper

# ## Introduction

# This notebook presents web scraping tool.

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
from time import sleep
from os import path


# #### Third-parties

# - [requests]('http://docs.python-requests.org/en/master/')
# - [pandas]('https://pandas.pydata.org/')
# - [pyodbc]('https://github.com/mkleehammer/pyodbc/wiki/')

# In[ ]:


import pandas
import requests
import pyodbc


# ## Arguments

# ## Parameters

# ## Classes

# ### oZe

# In[ ]:


class oZe:

    protocol = 'https://'
    credential = {
        'username': 'username',
        'password': 'pwd'
    }
    root = 'enc.hauts-de-seine.fr/'
    paths = {
        None: '',
        'login': 'my.policy', 
        'users': 'v1/users',
        'classes': 'v1/cours/byEleve'
    }
    timeout = 3600
    
    error = False
    @staticmethod
    def get(url, timeout = datetime.now().timestamp() + timeout):
        '''
            Halting the process
        '''
        while datetime.now().timestamp() < timeout:
            '''
                ?
            '''
            error = False
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
                print("ERREUR OZE :")
                print(type(e))   
                print(e.args)
                print(e)
                print("Vérifier qu'aucune session OZE utilisant ce compte n'est ouverte")
                error = True
            finally:
                '''
                    Closing the session
                '''
                if session is not None:
                    session.close()
        '''
            ?
        '''
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
        '''
            Processing the timetable
        '''
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
        
        data['_id'] = user['_id']

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
        
        start_school = 28800 #8h00
        break_time = 12300 #3h30
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
        
        '''
            set a new array that will be write into a file later
            (set the name of the columns)        '''
        leisures_array = pandas.DataFrame(columns = ['debut', 'fin', 'verification'])
        
        #loop into the table
        for index, row in timetable.iterrows():
            '''
                loop into the leisures of the day
            '''

            if row['dateDebut'] - begin  >= break_time : 
                temp_end = begin + middle_break_time
                #create row
                leisure = {'debut': begin, 'fin': temp_end, 'verification': 1}
                #add data to the array
                leisures_array = leisures_array.append(leisure, ignore_index = True)
            
            
            begin = row['dateFin']
        #------------------------------------- End of the loop
        '''
            At that step begin is the last study hour of the day for the student
            So we add into the database when the student can leave the school in a regular way
        '''

        leisure = {'debut': begin, 'fin': end, 'verification': 0}
        leisures_array = leisures_array.append(leisure, ignore_index = True)
        
        #end of the process onto the time for the student
        
        '''
            Set the array into a correct format used into the database 
        '''
        leisures_array['debut'] = leisures_array['debut'].apply(lambda x: datetime.fromtimestamp(x).strftime('%H:%M'))
        leisures_array['fin'] = leisures_array['fin'].apply(lambda x: datetime.fromtimestamp(x).strftime('%H:%M'))

        leisures_array['_id'] = user['_id']
        
        '''
            Return a list of leisures
        '''
        return leisures_array


# ### Database

# In[ ]:

class Database:

    credential = {
        'Driver': '{ODBC Driver 13 for SQL Server}', #check the name of the server with ODBC tools in windows
        'Server': 'name_instance_server', #for example(localdb)\MSSQLLocalDB
        'Database': 'name_database' #name of the database (can be seen with Sql Server Managment Tool),
        'Trusted_Connection' : 'yes', #windows authentification
		'autocommit': True,
    }
    table = {
        'student': 'TABLE_ELEVES',
        'leisure': 'TABLE_SORTIES_JOURNALIERES'
    }
    
    @staticmethod
    def select(timeout):
        '''
            Halting the process
        '''
        while datetime.now().timestamp() < timeout:            
            '''
                ?
            '''
            connection = None
            try:
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'SELECT Id_eleve AS _id, Nom AS nom, Prenom AS prenom FROM ' + Database.table['student']
                '''
                    Returning the selected rows
                '''
                return pandas.read_sql(sql, connection)
            except Exception as e:           
                print(type(e))   
                print(e.args)
                print(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        '''
            ?
        '''
        return None

    @staticmethod
    def insert(timeout):
        '''
            Halting the process
        '''
        while datetime.now().timestamp() < timeout:            
            '''
                ?
            '''
            connection = None
            try:
                '''
                    ?
                '''
                file = path.join(path.dirname(path.realpath(__file__)), 'leisures.csv')
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'BULK INSERT ' + Database.table['leisure'] + ' FROM \'' + file  +'\' WITH (FIRSTROW = 2, FIELDTERMINATOR = \';\', ROWTERMINATOR = \'\n\')'
                '''
                    Returning the number of inserted rows
                '''                
                return connection.execute(sql).rowcount
            except Exception as e:           
                print(type(e))   
                print(e.args)
                print(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        '''
            ?
        '''
        return None
    
    @staticmethod
    def delete(timeout):
        '''
            Halting the process
        '''
        while datetime.now().timestamp() < timeout:            
            '''
                ?
            '''
            connection = None
            try:
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'TRUNCATE TABLE ' + Database.table['leisure']             
                '''
                    Returning the number of deleted rows
                '''
                return connection.execute(sql).rowcount            
            except Exception as e:           
                print(type(e))   
                print(e.args)
                print(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        '''
            ?
        '''
        return None


# ## Methods

# ## Program

# In[ ]:


print("\n\n-----------------------------------------------")
print("Début de MAJ le : " + str(datetime.now()))
while True:
    '''
        Computing the halting time
    '''
    halt = datetime.now().timestamp() + oZe.timeout
    '''
        Cleaning the database
    '''
    print("\t* Suppression de la table dans la base de doonées le : " + str(datetime.now()))
    data = Database.delete(halt)
    '''
        Retrieving the users
    '''
    local = Database.select(halt)
    '''
        Processing the users
    '''
    if local is None:
        print("ERREUR : base de données vide")
        continue
    '''
        Retrieving the users
    '''
    print("\t* Récupérationn des élèves depuis la base de données locale le : " + str(datetime.now()))
    extern = oZe.users(halt)
    '''
        Processing the users
    '''
    if extern is None:
        print("ERREUR : base de données vide")
        continue
    '''
        ?
    '''
    columns = ['nom', 'prenom']    
    '''
        ?
    '''
    users = pandas.merge(local, extern, on = columns).drop_duplicates(subset = columns)
    '''
        ?
    '''
    users = deque(users.to_dict('records'))
    '''
        Computing the halting time per request
    '''
    chunk = (halt - datetime.now().timestamp()) / len(users)
    '''
        ?
    ''' 
    data = pandas.DataFrame()
    '''
        Processing the users        
    ''' 
    
    print("\t* Récupération et analyse des données depuis OZE le : " + str(datetime.now()))
    print("\t  Veuillez assurer une connexion internet stable durant toute la durée du processus")
    
    compt_pourcent = 0
    delta = 6
    size = int(len(users)/delta) #size of each interval
    i = size
    
    while users:
        
        if i >= size : #display pourcent
            print("\t\t" + str(int(compt_pourcent/delta*100)) + "% effectué le : " + str(datetime.now()))
            compt_pourcent = compt_pourcent + 1
            i = 0

        i = i+1 
        '''
            Popping a user
        ''' 
        user = users.pop()
        '''
            Retrieving the leisures
        '''  
        leisures = oZe.leisures(user, datetime.now().timestamp() + chunk)
        if oZe.error:
            break

        
        if leisures is None:
            '''
                Appending a user
            '''
            users.appendleft(user) 
        else:
            data = data.append(leisures)

    '''
        Halting the process
    '''
    print("\t* Mise à jour de la base de données : " + str(datetime.now()))
    while datetime.now().timestamp() < halt:
        try:
            columns = ['id', '_id', 'debut', 'fin', 'verification']
            leisures = data.reindex(columns = columns)   
            leisures['verification'] = leisures['verification'].astype(int)
            '''
                Persisting the leisures
            '''
            leisures.to_csv('leisures.csv', sep = ';', encoding =  'utf-8-sig', index = False)

            data = Database.insert(halt)
            '''
                Continuing the process
            '''
            break
        except PermissionError as e: 
            print("ERREUR :")
            print(type(e))   
            print(e.args)
            print(e)
            print("Vérifier que le fichier n'est pas utilisé par un autre processus")
            
            break #exit the loop
    '''
        Waiting next process
    '''
    rest = halt - datetime.now().timestamp()
    if rest < 0:
        rest = 0
    print("Fin de MAJ le : " + str(datetime.now()))
    print("\nProchaine mise à jour dans " + str(int(rest/60)) + " minutes")
    sleep(rest)
    
input("Press enter to exit ;)")

# ## Execution

# This web scraping tool is a script. In order to execute this notebook as a script, it is necessary to convert this notebook into executable script.

# ## Evaluation

# ## Observations

# - Some classes do not have a value, a code or an idetifier.
# - oZe uses the Coordinated Universal Time (UTC) for temporal values
# - The database user should have bulk permissions

# ## Lessons learned

# ## References

# - [BULK INSERT with identity (auto-increment) column]('https://stackoverflow.com/questions/10851065/bulk-insert-with-identity-auto-increment-column')
# - [How do I get the UTC time of “midnight” for a given timezone?]('https://stackoverflow.com/questions/373370/how-do-i-get-the-utc-time-of-midnight-for-a-given-timezone')
# - [How to add an empty column to a dataframe?]('https://stackoverflow.com/questions/16327055/how-to-add-an-empty-column-to-a-dataframe')
# - [You do not have permission to use the bulk load statement error]('https://stackoverflow.com/questions/32417776/you-do-not-have-permission-to-use-the-bulk-load-statement-error')

# In[ ]:




