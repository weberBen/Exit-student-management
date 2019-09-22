"""
    class use to update the database
"""


from datetime import datetime
import os 

import pandas
import pyodbc

import default_modules.Tools as Tools
from default_modules.Error import _Error



def getVerificationValue(verification):
    if verification : 
        return 1
    else :
        return 0
    
def getComputerTableId(timeout, credential, table, id_computer):
    ''' get all the student information saved into the database'''
    msg = None
    while datetime.now().timestamp() < timeout: 
        msg = None
        connection = None
        try:
            '''
                Opening a connection
            '''
            connection = pyodbc.connect(**credential)
            '''
                Building the query
            '''
            sql = "SELECT Id_agent FROM " + table + " WHERE Identifiant='" + id_computer +"'"
            '''
                Returning the selected rows
            '''
            
            res = pandas.read_sql(sql, connection)
            if len(res)!=1 :
                msg = "Impossible de trouver l'index de l'agent '"+ Database.COMPUTER_AUTHENTIFICATION_ID +"' dans la base de donnée pour l'utiliser comme responsable lors de l'enregistrement des informations dans la base de données"
                break
            return res.Id_agent[0]
        
        except Exception as e: 
            msg = "Erreur lors de la selection des élèves dans la base de données locale"
            msg +="\n"+str(type(e))
            msg += "\n"+str(e.args)
            msg += "\n"+str(e)
        finally:
            '''
                Closing the connection
            '''
            if connection is not None:
                connection.close()
    
    if not(msg is None):
        _Error.raiseError(msg)


class Database:

    credential = Tools.ReadXmlFile.getDatabaseCredential()
    
    table = {
                'student': 'TABLE_ELEVES',
                'leisure': 'TABLE_SORTIE_TEMPORAIRE',
                'agent' : 'TABLE_AGENTS',
                'ban' : 'TABLE_BLOCAGE_TEMPORAIRE'
            }
    
    DIRECTORY_FILES = "temp_data"
    FILE_NAME_EXIT = "exit_authorizations.csv"
    FILE_NAME_BAN = "exit_bans.csv"
    
    COMPUTER_AUTHENTIFICATION_ID = "Computer"
    TYPE_STUDENT_TABLE = 0
    TYPE_DIVISION_TABLE = 2
    timeout = 600 #10min
    COMPUTER_TABLE_ID = getComputerTableId(datetime.now().timestamp() + timeout, credential, table['agent'], COMPUTER_AUTHENTIFICATION_ID)
    
    
    c_exit = {"id" : "Id eleve", "start" : "Heure debut", "end" : "Heure fin", "verif": "verification", "reason": "Motif"}
    exit_array = pandas.DataFrame(columns = ['None', c_exit['id'], c_exit['start'], c_exit['end'], c_exit['verif'], c_exit['reason']])
    """ When we update student informations, we don't save the modification into the database for each student
        we wait until we process all the student from the database, then save it into a csv file
        And then we load all the informations from that file into the database (after deleting all the data
        into the corresponding table)
        To make the script easier the columnz into the file are ordered as thez are into the database
        (when a column into the database can't not be update or not need to be update we just create a None column
        that is empty)
    """
    
    c_ban = {"type" : "Type table", "id": "Id table", "start": "Heure debut", "end":"Heure fin", "reason":"Motif","agent" : "Id agent"}
    ban_array = pandas.DataFrame(columns = ['None', c_ban['type'], c_ban['id'], c_ban['start'], c_ban['end'], c_ban['reason'], c_ban['agent']])
    
    
    
    @staticmethod
    def addExitAuthorization(student_table_id, timepan_start_time, timepan_end_time, verification, reason=""):
        """ add an information about the student into the corresponding array 
            Be aware that the "student_table_id" is the primary key of the table
        """
        c_exit = Database.c_exit
        start_time = Tools.Timer.timespanToString(timepan_start_time)
        end_time = Tools.Timer.timespanToString(timepan_end_time)
        
        data = {c_exit['id']: student_table_id, c_exit['start']: start_time, c_exit['end'] : end_time, c_exit['verif']: getVerificationValue(verification), c_exit['reason'] : reason}
        Database.exit_array = Database.exit_array.append(data, ignore_index = True)
    
    @staticmethod
    def updateExitAuthorizations(timeout):
        '''save the corresponding array as csv file, delete the corresponding table, insert all the new data'''
        filename = Database.FILE_NAME_EXIT
        
        #we save all the csv file into the subdirectory "DIRECTORY_FILES"
        filepath = os.path.join(os.getcwd(), Database.DIRECTORY_FILES)
        filepath = os.path.join(filepath, filename)
        
        try:
            Database.exit_array.to_csv(filepath, sep = ';', encoding =  'utf-8-sig', index = False)
            
        except PermissionError as e: 
            msg = "Erreur : impossible de sauvegarder les données traitées"
            msg +="\n"+str(type(e))
            msg += "\n"+str(e.args)
            msg +="\n"+ str(e)
            _Error.raiseError(msg)
            
        Database.truncateTable(Database.table["leisure"], timeout)
        Database.bulkInsert(Database.table["leisure"], filepath, timeout)
        Database.exit_array = Database.exit_array[0:0] #remove all row from dataframe
    
    @staticmethod
    def addExitBan(student_table_id, timepan_start_time, timepan_end_time, reason):
        """ add an information about the student into the corresponding array 
            Be aware that the "student_table_id" is the primary key of the table
        """
        c_ban = Database.c_ban
        start_time = Tools.Timer.timespanToString(timepan_start_time)
        end_time = Tools.Timer.timespanToString(timepan_end_time)
        
        data = {c_ban['type'] : Database.TYPE_STUDENT_TABLE, c_ban['id'] : student_table_id, c_ban['start'] :start_time, c_ban['end'] : end_time, c_ban['agent'] : Database.COMPUTER_TABLE_ID, c_ban['reason'] : reason }
        Database.ban_array = Database.ban_array.append(data, ignore_index = True)
    
    @staticmethod
    def updateExitBans(timeout):
        '''save the corresponding array as csv file, delete the corresponding table, insert all the new data'''
        filename = Database.FILE_NAME_BAN
        #we save all the csv file into the subdirectory "DIRECTORY_FILES"
        filepath = os.path.join(os.getcwd(), Database.DIRECTORY_FILES)
        filepath = os.path.join(filepath, filename)
        
        try:
            Database.ban_array.to_csv(filepath, sep = ';', encoding =  'utf-8-sig', index = False)
            
        except PermissionError as e: 
            msg = "Erreur : impossible de sauvegarder les données traitées"
            msg +="\n"+str(type(e))
            msg += "\n"+str(e.args)
            msg +="\n"+ str(e)
            _Error.raiseError(msg)
            
        Database.truncateTable(Database.table["ban"], timeout)
        Database.bulkInsert(Database.table["ban"], filepath, timeout)
        Database.ban_array = Database.ban_array[0:0] #remove all row from dataframe
        
    @staticmethod
    def clearUpdatedTable(timeout = datetime.now().timestamp() + timeout):
        Database.truncateTable(Database.table["leisure"], timeout)
        Database.truncateTable(Database.table["ban"], timeout)
    
    @staticmethod
    def selectAllStudent(timeout):
        ''' get all the student information saved into the database'''
        
        msg = None
        while datetime.now().timestamp() < timeout: 
            msg = None
            connection = None
            try:
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'SELECT Id_eleve AS id_BD_local, Nom AS nom, Prenom AS prenom FROM ' + Database.table['student']
                '''
                    Returning the selected rows
                '''
                return pandas.read_sql(sql, connection)
            except Exception as e: 
                msg = "Erreur lors de la selection des élèves dans la base de données locale"
                msg +="\n"+str(type(e))
                msg += "\n"+str(e.args)
                msg += "\n"+str(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        
        if not(msg is None):
            _Error.raiseError(msg)
        
        return None
    @staticmethod
    def bulkInsert(table, filepath, timeout):
        '''
            Halting the process
        '''
        msg = None
        while datetime.now().timestamp() < timeout: 
            msg = None

            connection = None
            try:
                
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'BULK INSERT ' + table + ' FROM \'' + filepath  +'\' WITH (FIRSTROW = 2, FIELDTERMINATOR = \';\', ROWTERMINATOR = \'\n\')'
                '''
                    Returning the number of inserted rows
                '''                
                return connection.execute(sql).rowcount
            except Exception as e: 
                msg = "Erreur lors de l'insertion des heures de sorties dans la base de données locale"
                msg +="\n"+str(type(e))
                msg += "\n"+str(e.args)
                msg += "\n"+str(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        
        
        if not(msg is None):
            _Error.raiseError(msg)
        
        return None
    
    @staticmethod
    def truncateTable(table, timeout):
        '''
            Halting the process
        '''
        msg = None
        while datetime.now().timestamp() < timeout:  
            msg = None
            
            connection = None
            try:
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**Database.credential)
                '''
                    Building the query
                '''
                sql = 'TRUNCATE TABLE ' + table             
                '''
                    Returning the number of deleted rows
                '''
                return connection.execute(sql).rowcount            
            except Exception as e:  
                msg = "Erreur lors de la purge d'une table de la base de données locale"
                msg +="\n"+str(type(e))
                msg += "\n"+str(e.args)
                msg += "\n"+str(e)
            finally:
                '''
                    Closing the connection
                '''
                if connection is not None:
                    connection.close()
        
        
        if not(msg is None):
            _Error.raiseError(msg)
        
        return None

        