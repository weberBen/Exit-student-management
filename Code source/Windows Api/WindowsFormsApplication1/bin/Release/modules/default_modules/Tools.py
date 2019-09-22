"""
    Gather all the elementary function to get informations from the settings file (file that save all
    the preferences of the user)
"""

import xml.etree.ElementTree
import os 
import datetime
import unicodedata

from default_modules.Error import _Error

# In[]print sys.path

def pathToSettingsFile():
        """ The settings file is in the parent folder of the directory "modules"
            because script can be inside childs directory in "modules", we have to check if 
            the parent folder of the current directory (the directory from where the script is executes)
            is the directory "modules". Then we will just have to get the parent directory
        """
        PARENT_DIRECTORY = "modules"
        NAME_FILE = "SettingsFile.xml"
        PATH_SEPARATOR = os.path.join(" ","")[1:]
    
        path = os.getcwd()
        array =  path.split(PATH_SEPARATOR)
        
        while len(array)>0 and array[len(array)-1]!=PARENT_DIRECTORY:
            path = os.path.dirname(path)
            array =  path.split(PATH_SEPARATOR)
        
        if len(array)>0:
            return os.path.join(os.path.dirname(path), NAME_FILE) #path = "...\modules"
        
        return ""

 
# In[]
def normalizeString(text, codex):
    out= unicodedata.normalize('NFKD', text).encode(codex, 'ignore')
    out = out.decode(codex)
    return out.lower()

# In[]
        
class ReadXmlFile :
    """ 
        Retrieve all the needed data from the settings file 
    """
    
    PATH_TO_FILE = pathToSettingsFile()
    
    TIME_SEPARATOR = '-'
    NAME_DIV_STUDY_HOUR = "Study_time"
    TAG_LUNCH_TIME = "DÉJEUNER"
    NAME_FIELD_LUNCH_TIME = "Lunch_time"
    NAME_DIV_SCHOOL_DAYS = "School_days"
    
    NAME_DIV_DATABASE_PARMS = "Database"
    NAME_FIELD_DRIVER_NAME = "Sql_driver_name"
    NAME_FIELD_SERVER_NAME = "Sql_server_domain_name"
    NAME_FIELD_DATABASE_NAME = "Sql_database_name"
    NAME_FIELD_EXIT_BREAK = "Length_exit_break"
    
    
    @staticmethod
    def loadFile():
        ''' return the settings file loaded into memory '''
        try :
            return xml.etree.ElementTree.parse(ReadXmlFile.PATH_TO_FILE).getroot()
        except PermissionError as e:
            value = "Impossible d'accèder au fichier :"+ReadXmlFile.PATH_TO_FILE
            value += "\n"+str(e)
            _Error.raiseError(value)
        except Exception as e:  
                value = "Problème avec le fichier :"+ReadXmlFile.PATH_TO_FILE
                value +="\n"+str(type(e))
                value +="\n"+str(e.args)
                value +="\n"+str(e)
                _Error.raiseError(value)
        return None
    @staticmethod
    def getExitBreak():
        e = ReadXmlFile.loadFile()
        
        for elem in e.findall('field'):
            if elem.get('name') == ReadXmlFile.NAME_FIELD_EXIT_BREAK:
                return datetime.datetime.strptime(elem.get(ReadXmlFile.NAME_FIELD_EXIT_BREAK), Timer.TIME_FORMAT).time()
    @staticmethod
    def getStartSchool():
        return ReadXmlFile.getStudyHours()[0].Start
    
    @staticmethod
    def getDatabaseCredential():
        data ={}
        
        e = ReadXmlFile.loadFile()
        
        for elem in e.findall('div'):
            if elem.get('name') == ReadXmlFile.NAME_DIV_DATABASE_PARMS:
                for item in elem.findall('item'):
                    data[item.get('name')] = item.text
        #end loop
                    
        return {
                    'Driver': '{' + data[ReadXmlFile.NAME_FIELD_DRIVER_NAME] +'}',
                    'Server': ''+data[ReadXmlFile.NAME_FIELD_SERVER_NAME] +'', #.\SQLEXPRESS  (localdb)\MSSQLLocalDB
                    'Database': ''+ data[ReadXmlFile.NAME_FIELD_DATABASE_NAME] +'',
                    'Trusted_Connection' : 'yes',
                	'autocommit': True,
                }
    @staticmethod
    def getStudyHours():
        """ get all he study hour of the school"""
        hours = []

        e = ReadXmlFile.loadFile()
        #parse the XML file
        
        #get the correct xml element that contains all the study hours
        for elem in e.findall('div'):
            if elem.get('name') == ReadXmlFile.NAME_DIV_STUDY_HOUR:#element with study hours
                for item in elem.findall('item'):#loop into all the hours
                    value = item.text#string of the time slot (write into the file as "start-end")
                    
                    """In all the timeslot, when we reach the lunch time we will see "DÉJEUNER" and not a timeslot
                    bacause the lunch time is stored separatly"""
                    if value.encode('utf-8') != ReadXmlFile.TAG_LUNCH_TIME and value != ReadXmlFile.TAG_LUNCH_TIME:#check if the current hour is the lunch time
                        _time = value.split(ReadXmlFile.TIME_SEPARATOR)#get the start and end of the timeslot
                        
                        #convert the element into a time object
                        start = Timer.stringToTime(_time[0])
                        end = Timer.stringToTime(_time[1])
                        
                        if start!=0 and end !=0:#conversion string to time worked
                            timeslot = Timer.TimeSlot(start, end) #crea a new timeSlot object
                            hours.append(timeslot)#add the timeslot into the list
                    else:#if the current hour is the lunch hour
                        for elem in e.findall('field'):
                            if elem.get('name') == ReadXmlFile.NAME_FIELD_LUNCH_TIME:#get the element that contains the lunch time
                                _time = elem.get(ReadXmlFile.NAME_FIELD_LUNCH_TIME).split(ReadXmlFile.TIME_SEPARATOR)#get the timeslot as string
                                #convert the string as a time object
                                start = Timer.stringToTime(_time[0])
                                end = Timer.stringToTime(_time[1])
                                
                                if start!=0 and end !=0:
                                    timeslot = Timer.TimeSlot(start, end) 
                                    hours.append(timeslot)#add the lunch time into the list
                                break#exit the search of the lunch time element
                break#exit the search of the study hours
                        #test 
        return hours
    
    @staticmethod
    def getStudyDays():
        """Return the list of all the school day
        All the day of the school are saved as integer where sunday is 0"""
        days = []
        
        e = ReadXmlFile.loadFile()
        
        for elem in e.findall('div'):
            
            if elem.get('name') == ReadXmlFile.NAME_DIV_SCHOOL_DAYS:#element with all the day of school
                for item in elem.findall('item'):#loop into all the days
                    day = (int(item.text) -1)%7 #in pyhton monday is 0
                    days.append(day) 
                break#exit the search of the school days
        
        return days

# In[]

class Timer :
    """ 
        The main function of that class is "waitingTimeToSecond" which return the time we need
        to wait before the next update of the database. That time follow the study hour of the school
    """
    TIME_FORMAT = "%H:%M" #time are save as string into the database
    
    class TimeSlot():
        def __init__(self, field1, field2):
            self.Start = field1
            self.End = field2
        
    @staticmethod
    def timespanToString(value):
        return datetime.datetime.fromtimestamp(value).strftime(Timer.TIME_FORMAT)
    
    @staticmethod
    def datetimeTostring(value):
        return value.strftime(Timer.TIME_FORMAT)
    
    @staticmethod
    def stringToTime(value):
        try :
            return datetime.datetime.strptime(value, Timer.TIME_FORMAT).time()
        except :
            return None
    
    @staticmethod
    def substractTimeToSecond(time1, time2):
        """ Return the number of seconds between to time object"""
        today = datetime.date.today()
        if time1 >=time2:
            delta = datetime.datetime.combine(today, time1) - datetime.datetime.combine(today, time2)
        else:
            delta = datetime.datetime.combine(today, time2) - datetime.datetime.combine(today, time1)
        
        return  delta.seconds
    @staticmethod
    def timeToSecond(_time):
        """ convert a time object as seconds"""
        return 3600*_time.hour + 60*_time.minute
    
    
    @staticmethod
    def waitingTimeToSecond():
        """ when the method is called retrun the number of second until the next collect of the data, accroding
        to the school hours and days. In fact the collect of the data can happened only between two hours of study
        """
        today = datetime.datetime.today()
        _time = today.time()
        day_of_week = today.weekday()
        
        school_hours = ReadXmlFile.getStudyHours()
        
        if len(school_hours)==0 :
            return 3600 #1h
        
        if not day_of_week in ReadXmlFile.getStudyDays():
            return Timer.substractTimeToSecond(Timer.stringToTime("23:59"), _time) + Timer.timeToSecond(school_hours[0].Start)
            """ |(not a school day)
                | <- 8h00 (= first school hour = school_hours[0].Start)
                |
                |
                | <- 14h30 (=time)
                |
                |
                | <- 23h59
                |(next day)
                | <- 8h00 (first school hour)
                
                If the current day is not a school day, then we will wait until the start of the next time
                Then we return : (23h59 - current_time) + (first_school_hour - 0h00)
                
                Noitice that the formula also work when the current time is between 0h00 and first_school_hour
            """
        
        #at that step the current day is a school day
        
        if _time >= school_hours[0].Start :
            i = 0
            while (i < len(school_hours) and _time > school_hours[i].Start):
                i +=1
            
            if i == len(school_hours) :
                return Timer.substractTimeToSecond(Timer.stringToTime("23:59"), _time) + Timer.timeToSecond(school_hours[0].Start)
            else:
                return int( Timer.substractTimeToSecond(school_hours[i].Start, _time) + 0.5*Timer.substractTimeToSecond(school_hours[i].End, school_hours[i].Start) )
                """ |
                    | <- 8h00
                    |   <- 8h30 (=current time)
                    | <- 9h00
                    |
                    | <- 10h00
                    
                    Whn we exit the loop we get the next timeslot "superior" to the current time
                    If the current time is 8h30, then we want to update data somewhere (factor k) 
                    between 9h00 and 10h00 (the next time slot superior to the timeslot that contains the current
                    time, in our case 9h00-10h00 >= 8h00-9h00 and 8h30 is in 8h00-9h00)
                    
                    So to get the time until the next update we return : 
                        (start_next_time_slot - current time) + k*(end_next_time_slot - start_next_time_slot)
                """
        else:
            return Timer.substractTimeToSecond(school_hours[0].Start,_time)
            """ when current_time is inferior to the first school hour, we just need to until that time """

# In[]