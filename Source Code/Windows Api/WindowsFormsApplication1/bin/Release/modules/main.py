""" 
    If you need to update the database frequently from an external data source
    (for exemple update the database from an online database) then you can get
    the given script.
    
    The following script use serval sub script to help you : 
        * Tools.py (defautl_modules) : get informations from the settings file update through the windows application
            The script also contain a method to get the remaining time before the next update (if you want to 
            make your script autonomous and update the database every ...). That method return the remaining time
            until the middle of the next studying hour (to avoid having the update when student need to leave the school)
            
        * Error.py : use to raise an exception anywhere in the scripts to easily exit the process when an error occurs
        
        * Database.py : use to update the local database
        

All the scripts are gather into the subdirectory of the file of the windows application "modules". Then scripts are
split into two categories :
    * default_modules : that are the general script to get informations and update the database
    * custom_modules : that will contains all your scripts
    
    

Code based on the script from Anderson Carlos Ferreira da Silva (github : @andersoncarlosfs)
Update by Benjamin Weber (10/2019)
"""

from time import sleep
from datetime import datetime, date, timedelta, time
import os, sys 


# In[] PATH ENVIRONEMENT



PATH_FOLDER_SERVER_MODULES = "C:\Program Files\Application controle des sorties\modules"
if not(PATH_FOLDER_SERVER_MODULES in sys.path):
    sys.path.append(PATH_FOLDER_SERVER_MODULES)


PATH_SETTINGS_FILE = "C:/Program Files/Application controle des sorties/SettingsFile.xml"
PATH_TEMP_DATABASE_FOLDER = "C:/Program Files/Application controle des sorties/modules/temp_data"
PATH_SETTINGS_FILE = os.path.normpath(PATH_SETTINGS_FILE)
PATH_TEMP_DATABASE_FOLDER = os.path.normpath(PATH_TEMP_DATABASE_FOLDER)



# In[] LOAD MODULES



from default_modules.Tools import Timer, NamedPipe, ReadXmlFile
from default_modules.Database import Database

from default_modules.ExternalDataBaseBackend import ExternalDataProcessor
from custom_modules.ExternalBD import ExternalBD




#%% Start communication with c# API (to notify the server from error)



print("Etablissement de la communication avec le serveur...")
readXmlFile = ReadXmlFile(PATH_SETTINGS_FILE)
pipe = NamedPipe(readXmlFile.getNamedPipe())
pipe.write("online")#all other state will cause send an error signal to the server



#%% Main script

'''
    while error :
        *update the database
        *get next time until the next update
        *wait
    <- error occurs (exception thrown)
    *play song
'''

print("Récupération des paramètres depuis le fihcier de paramétrage...")
timer = Timer(PATH_SETTINGS_FILE)
print("Ouverture de la base de données...")
database = Database(PATH_SETTINGS_FILE, PATH_TEMP_DATABASE_FOLDER)
database.clearUpdatedTable();


externalBD = ExternalBD()
processor = ExternalDataProcessor(externalBD, PATH_SETTINGS_FILE, PATH_TEMP_DATABASE_FOLDER)
        
while True:
    try :
        print("\n\n-----------------------------------------------")
        print("****** Début de MAJ le : " + str(datetime.now())+" ******")

        processor.update()
        
        print("\n****** Fin de MAJ le : " + str(datetime.now())+ " ******")
        print("\n\n-----------------------------------------------")
        
        rest = timer.waitingTimeToSecond()
        next_datetime = datetime.today() + timedelta(seconds=rest)
        print("\nProchaine mise à jour le " + next_datetime.strftime("%d-%m-%Y %H:%M") )
        
        sleep(rest)
    except Exception as e:
        database.clearUpdatedTable();
        
        print("\n\n\n")
        print(e)
        break
    

#error when exit the loop
print("\n\n!!!!!!!!!!!! ECHEC DE LA MISE A JOUR !!!!!!!!!!!!")
pipe.final("error")#signal error (when the script is closed, the pipe will be closed. Then the server interprets that event as an error)



