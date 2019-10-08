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
Update by Benjamin Weber (01/12/2018)
"""

from time import sleep
from datetime import datetime, date, timedelta, time
import os, sys 
import winsound as ws


path = "C:\Program Files\Application controle des sorties\modules"
if not(path in sys.path):
    sys.path.append(path)
    

from default_modules.Tools import Timer, NamedPipe, ReadXmlFile
from default_modules.Database import Database
from custom_modules.Oze import update, oZe


"""
    while error :
        *update the database
        *get next time until the next update
        *wait
    <- error occurs (exception thrown)
    *play song
"""
#start porgramm to avoid sleep mode
#subprocess.call(['C:\\Program Files\\Application controle des sorties\\DontSleep.exe'])

#%% VARIABLES

### -------------------------- PATH ENVIRONMENT
PATH_SETTINGS_FILE = "path_to_xml_file" # Example : C:/Program Files/...
PATH_TEMP_DATABASE_FOLDER = "path_to_tmp_file_for_python"

PATH_SETTINGS_FILE = os.path.normpath(PATH_SETTINGS_FILE)
PATH_TEMP_DATABASE_FOLDER = os.path.normpath(PATH_TEMP_DATABASE_FOLDER)

### -------------------------- DATE ENVIRONMENT
now = datetime.now()
start_year = None

month = now.month
if month>=9 : #setpember->december
    start_year = now.year
else :#◘jenuary->august
    start_year = now.year - 1
    
START_DATE_CHEKING_PUNISHMENT = date(start_year, 9, 5) #years/month/day
START_DATE_CHECKING_NON_CLOSED_ELEMENTS = date(start_year, 9, 5) #years/month/day

### -------------------------- OZE ENVIRONMENT
OZE_USERNAME = "usr_id"
OZE_PWD = "usr_pwd"

#%% Start communication with c# API (to notify the server from error)

readXmlFile = ReadXmlFile(PATH_SETTINGS_FILE)
pipe = NamedPipe(readXmlFile.getNamedPipe())
pipe.write("online")#all other state will cause send an error signal to the server


#%% Main script

timer = Timer(PATH_SETTINGS_FILE)
database = Database(PATH_SETTINGS_FILE, PATH_TEMP_DATABASE_FOLDER)
database.clearUpdatedTable();
while True:
    try :
        print("\n\n-----------------------------------------------")
        print("****** Début de MAJ le : " + str(datetime.now())+" ******")
        
        update(OZE_USERNAME, OZE_PWD, 
               PATH_SETTINGS_FILE, PATH_TEMP_DATABASE_FOLDER,
               START_DATE_CHEKING_PUNISHMENT,
               START_DATE_CHECKING_NON_CLOSED_ELEMENTS)
        
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
pipe.write("error")#signal error (when the script is closed, the pipe will be closed. Then the server interprets that event as an error)
