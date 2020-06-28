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

    table = {
                'student': 'TABLE_ELEVES',
                'regime' : 'TABLE_REGIMES_SORTIE',
                'leisure': 'TABLE_SORTIE_TEMPORAIRE',
                'agent' : 'TABLE_AGENTS',
                'ban' : 'TABLE_BLOCAGE_TEMPORAIRE',
                'RegimeLabel' : 'TABLE_REGIMES_SORTIE',
                'RegimeRelation' : 'TABLE_RELATIONS_REGIME_PERMISSION',
                'RegimeAuthorization': 'TABLE_PERMISSIONS_REGIME'
            }
    
    FILE_NAME_EXIT = 'exit_authorizations.csv'
    FILE_NAME_BAN = 'exit_bans.csv'
    
    COMPUTER_AUTHENTIFICATION_ID = 'Computer'
    TYPE_STUDENT_TABLE = 0
    TYPE_DIVISION_TABLE = 2
    timeout = 600 #10min
    
    def __init__(self, path_to_settings_file, path_temp_database_folder):
        self.ReadXmlFile = Tools.ReadXmlFile(path_to_settings_file)
        self.path_temp_database_folder = path_temp_database_folder
        self.credential = self.ReadXmlFile.getDatabaseCredential()
        self.COMPUTER_TABLE_ID = getComputerTableId(datetime.now().timestamp() + Database.timeout, self.credential, Database.table['agent'], Database.COMPUTER_AUTHENTIFICATION_ID)
        self.c_exit = {"id" : "Id eleve", "start" : "Heure debut", "end" : "Heure fin", "verif": "verification", "reason": "Motif"}
        self.exit_array = pandas.DataFrame(columns = ['None', self.c_exit['id'], self.c_exit['start'], self.c_exit['end'], self.c_exit['verif'], self.c_exit['reason']])
        """ When we update student informations, we don't save the modification into the database for each student
        we wait until we process all the student from the database, then save it into a csv file
        And then we load all the informations from that file into the database (after deleting all the data
        into the corresponding table)
        To make the script easier the columnz into the file are ordered as thez are into the database
        (when a column into the database can't not be update or not need to be update we just create a None column
        that is empty)
        """
    
        self.c_ban = {"type" : "Type table", "id": "Id table", "start": "Heure debut", "end":"Heure fin", "reason":"Motif","agent" : "Id agent"}
        self.ban_array = pandas.DataFrame(columns = ['None', self.c_ban['type'], self.c_ban['id'], self.c_ban['start'], self.c_ban['end'], self.c_ban['reason'], self.c_ban['agent']])
    
    
    
    def getExitRegimes(self,timeout):
        msg = None
        list_regime = []
        
        while datetime.now().timestamp() < timeout:  
            msg = None
            connection = None
            try:
                
                '''
                get all exit regime label and id from the database
                '''
                connection = pyodbc.connect(**self.credential)
                
                
                sql = "SELECT Label, SortieFinJournee, Id_regime From "+self.table['RegimeLabel']
                res = pandas.read_sql(sql, connection)
                
                #create list of regime
                for index, row in res.iterrows():
                    
                    regime = Tools.ExitRegime(label=row[0], exitEndOfDay=row[1] , tableId=row[2])
                    list_regime.append(regime)
                
                '''
                get all authorizations associated to the regimes
                '''
                
                index = 0
                for regime in list_regime:
                    #check data format
                    
                    if regime is None:
                        list_regime.pop(index)
                        continue
                    
                    table_id = regime.Id
                    if table_id==-1:
                        list_regime.pop(index)
                        continue
                    
                    #get authorizations
                    
                    sql = "SELECT P.Label, P.Periode, P.Debut, P.Fin, P.Id_permission FROM "+self.table['RegimeRelation']+" R, "+self.table['RegimeAuthorization']+" P WHERE P.Id_permission = R.Id_permission AND R.Id_regime={0}".format(table_id)
                    res = pandas.read_sql(sql, connection)
                    
                    for index, row in res.iterrows():
                        
                        label = row[0]
                        period = Tools.Timer.timeToSecond(Tools.Timer.stringToTime(row[1]))
                        start = Tools.Timer.timeToSecond(Tools.Timer.stringToTime(row[2]))
                        end = Tools.Timer.timeToSecond(Tools.Timer.stringToTime(row[3]))
                        table_id = row[4]
                        
                        
                        authorization = Tools.ExitRegimeAuthorization(label, period, start, end, table_id)
                        regime.addAuthorization(authorization)
                    
                    
                    index = index +1
                
                return list_regime
            
            except Exception as e: 
                msg = "Impossible d'accèder aux régimes de sortie dans la base de données"
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
        
    
    def addExitAuthorization(self, student_table_id, start_time_second, end_time_second, verification, reason=""):
        """ add an information about the student into the corresponding array 
            Be aware that the "student_table_id" is the primary key of the table
        """
        start_time = Tools.Timer.timeSecondToString(start_time_second)
        end_time = Tools.Timer.timeSecondToString(end_time_second)
        
        data = {self.c_exit['id']: student_table_id, self.c_exit['start']: start_time, self.c_exit['end'] : end_time, self.c_exit['verif']: getVerificationValue(verification), self.c_exit['reason'] : reason}
        self.exit_array = self.exit_array.append(data, ignore_index = True)
    
    
    def updateExitAuthorizations(self, timeout):
        '''save the corresponding array as csv file, delete the corresponding table, insert all the new data'''
        filename = Database.FILE_NAME_EXIT
        
        #we save all the csv file into the subdirectory "DIRECTORY_FILES"
        filepath = os.path.join(self.path_temp_database_folder, filename)
        
        try:
            self.exit_array.to_csv(filepath, sep = ';', encoding =  'utf-8-sig', index = False)
            
        except PermissionError as e: 
            msg = "Erreur : impossible de sauvegarder les données traitées"
            msg +="\n"+str(type(e))
            msg += "\n"+str(e.args)
            msg +="\n"+ str(e)
            _Error.raiseError(msg)
            
        self.truncateTable(self.table["leisure"], timeout)
        self.bulkInsert(self.table["leisure"], filepath, timeout)
        self.exit_array = self.exit_array[0:0] #remove all row from dataframe
    
    
    def addExitBan(self, student_table_id, timepan_start_time, timepan_end_time, reason):
        """ add an information about the student into the corresponding array 
            Be aware that the "student_table_id" is the primary key of the table
        """
        
        start_time = Tools.Timer.timespanToString(timepan_start_time)
        end_time = Tools.Timer.timespanToString(timepan_end_time)
        
        data = {self.c_ban['type'] : Database.TYPE_STUDENT_TABLE, self.c_ban['id'] : student_table_id, self.c_ban['start'] :start_time, self.c_ban['end'] : end_time, self.c_ban['agent'] : self.COMPUTER_TABLE_ID, self.c_ban['reason'] : reason }
        self.ban_array = self.ban_array.append(data, ignore_index = True)
    
    
    def updateExitBans(self, timeout):
        '''save the corresponding array as csv file, delete the corresponding table, insert all the new data'''
        filename = Database.FILE_NAME_BAN
        #we save all the csv file into the subdirectory "DIRECTORY_FILES"
        filepath = os.path.join(self.path_temp_database_folder, filename)
        
        try:
            self.ban_array.to_csv(filepath, sep = ';', encoding =  'utf-8-sig', index = False)
            
        except PermissionError as e: 
            msg = "Erreur : impossible de sauvegarder les données traitées"
            msg +="\n"+str(type(e))
            msg += "\n"+str(e.args)
            msg +="\n"+ str(e)
            _Error.raiseError(msg)
            
        self.truncateTable(self.table["ban"], timeout)
        self.bulkInsert(self.table["ban"], filepath, timeout)
        self.ban_array = self.ban_array[0:0] #remove all row from dataframe
        
    
    def clearUpdatedTable(self, timeout = datetime.now().timestamp() + timeout):
        self.truncateTable(self.table["leisure"], timeout)
        self.truncateTable(self.table["ban"], timeout)
    
    
    def selectAllStudent(self, timeout):
        ''' get all the student information saved into the database'''
        
        msg = None
        while datetime.now().timestamp() < timeout: 
            msg = None
            connection = None
            try:
                
                '''
                    Opening a connection
                '''
                connection = pyodbc.connect(**self.credential)
                
                '''
                    Building the query
                '''
                sql  = 'SELECT Id_eleve AS id_BD_local, Nom AS nom, Prenom AS prenom, DemiPension as half_board_days, Id_regime as id_exit_regime, '
                sql += '(SELECT R.Label FROM '+ Database.table['regime']  +' R WHERE R.Id_regime=E.Id_regime) as label_exit_regime '
                sql += 'FROM ' + Database.table['student'] +' E '
                
                
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
    
    def bulkInsert(self, table, filepath, timeout):
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
                connection = pyodbc.connect(**self.credential)
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
    
    
    def truncateTable(self, table, timeout):
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
                connection = pyodbc.connect(**self.credential)
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

        