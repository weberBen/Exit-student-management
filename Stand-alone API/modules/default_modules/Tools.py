"""
    Gather all the elementary function to get informations from the settings file (file that save all
    the preferences of the user)
"""

import xml.etree.ElementTree
import datetime, time
import unicodedata
import struct

from default_modules.Error import _Error
 
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
    
    def __init__(self, path_to_settings_file):
        self.PATH_TO_FILE = path_to_settings_file
    
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
    NAME_FIELD_NAMED_PIPE = "Named_pipe"
    
    
    def loadFile(self):
        ''' return the settings file loaded into memory '''
        try :
            return xml.etree.ElementTree.parse(self.PATH_TO_FILE).getroot()
        except PermissionError as e:
            value = "Impossible d'accèder au fichier :"+self.PATH_TO_FILE
            value += "\n"+str(e)
            _Error.raiseError(value)
        except Exception as e:  
                value = "Problème avec le fichier :"+self.PATH_TO_FILE
                value +="\n"+str(type(e))
                value +="\n"+str(e.args)
                value +="\n"+str(e)
                _Error.raiseError(value)
        return None
    
    
    def getNamedPipe(self):
        e = self.loadFile()
        
        for elem in e.findall('field'):
            if elem.get('name') == self.NAME_FIELD_NAMED_PIPE:
                return elem.get(self.NAME_FIELD_NAMED_PIPE)
    
    
    def getExitBreak(self):
        e = self.loadFile()
        
        for elem in e.findall('field'):
            if elem.get('name') == self.NAME_FIELD_EXIT_BREAK:
                return datetime.datetime.strptime(elem.get(self.NAME_FIELD_EXIT_BREAK), Timer.TIME_FORMAT).time()
    
    
    def getStartSchool(self):
        return self.getStudyHours()[0].Start
    
    
    def getDatabaseCredential(self):
        data ={}
        
        e = self.loadFile()
        
        for elem in e.findall('div'):
            if elem.get('name') == self.NAME_DIV_DATABASE_PARMS:
                for item in elem.findall('item'):
                    data[item.get('name')] = item.text
        #end loop
                    
        return {
                    'Driver': '{' + data[self.NAME_FIELD_DRIVER_NAME] +'}',
                    'Server': ''+data[self.NAME_FIELD_SERVER_NAME] +'', #.\SQLEXPRESS  (localdb)\MSSQLLocalDB
                    'Database': ''+ data[self.NAME_FIELD_DATABASE_NAME] +'',
                    'Trusted_Connection' : 'yes',
                	'autocommit': True,
                }
    
    def getStudyHours(self):
        """ get all he study hour of the school"""
        hours = []

        e = self.loadFile()
        #parse the XML file
        
        #get the correct xml element that contains all the study hours
        for elem in e.findall('div'):
            if elem.get('name') == self.NAME_DIV_STUDY_HOUR:#element with study hours
                for item in elem.findall('item'):#loop into all the hours
                    value = item.text#string of the time slot (write into the file as "start-end")
                    
                    """In all the timeslot, when we reach the lunch time we will see "DÉJEUNER" and not a timeslot
                    bacause the lunch time is stored separatly"""
                    if value.encode('utf-8') != self.TAG_LUNCH_TIME and value != self.TAG_LUNCH_TIME:#check if the current hour is the lunch time
                        _time = value.split(self.TIME_SEPARATOR)#get the start and end of the timeslot
                        
                        #convert the element into a time object
                        start = Timer.stringToTime(_time[0])
                        end = Timer.stringToTime(_time[1])
                        
                        if start!=0 and end !=0:#conversion string to time worked
                            timeslot = Timer.TimeSlot(start, end) #crea a new timeSlot object
                            hours.append(timeslot)#add the timeslot into the list
                    else:#if the current hour is the lunch hour
                        for elem in e.findall('field'):
                            if elem.get('name') == self.NAME_FIELD_LUNCH_TIME:#get the element that contains the lunch time
                                _time = elem.get(self.NAME_FIELD_LUNCH_TIME).split(self.TIME_SEPARATOR)#get the timeslot as string
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
    
    
    def getStudyDays(self):
        """Return the list of all the school day
        All the day of the school are saved as integer where sunday is 0"""
        days = []
        
        e = self.loadFile()
        
        for elem in e.findall('div'):
            
            if elem.get('name') == self.NAME_DIV_SCHOOL_DAYS:#element with all the day of school
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
    
    def __init__(self, path_to_settings_file):
        self.ReadXmlFile = ReadXmlFile(path_to_settings_file)
        
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
    
    
    
    def waitingTimeToSecond(self):
        """ when the method is called retrun the number of second until the next collect of the data, accroding
        to the school hours and days. In fact the collect of the data can happened only between two hours of study
        """
        today = datetime.datetime.today()
        _time = today.time()
        day_of_week = today.weekday()
        
        school_hours = self.ReadXmlFile.getStudyHours()
        
        if len(school_hours)==0 :
            return 3600 #1h
        
        if not day_of_week in self.ReadXmlFile.getStudyDays():
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

class NamedPipe:
    def __init__(self, pipeName):
        self.pipeName = pipeName
        
        file = None
        msg = None
        num_try = 0;
        
        while num_try<30:
            try:
             file = open(r'\\.\pipe\{0}'.format(self.pipeName), 'r+b', 0)
             break
            except FileNotFoundError:
               msg =  "Impossible d'ouvrir le pipe nommé (pipe inexistant)"
            except Exception as e:
                 msg = "Erreur lors de l'ouverture du pipe nommé : \n"+str(e)
            time.sleep(1)
            num_try+=1
        
        if msg!=None :
            _Error.raiseError(msg)
        self.serverPipe =  file
        
        
    def write(self, data):

        try :
            self.serverPipe.write(struct.pack('I', len(data)) + str.encode(data))   # Write str length and str
            self.serverPipe.seek(0)                               # EDIT: This is also necessary
        except Exception as e:
         _Error.raiseError("Erreur lors de l'écriture dans le pipe nommé : \n"+str(e))
    
            
