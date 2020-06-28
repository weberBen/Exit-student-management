
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