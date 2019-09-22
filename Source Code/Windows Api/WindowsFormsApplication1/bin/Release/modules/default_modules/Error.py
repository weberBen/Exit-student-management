""" 
    When an erro occure into a script, a call to the method "raiseError" will raise an exceptio
    to stop all the current process
    
    That class is then used to gather all the error 
"""

class _Error :
    @staticmethod
    def raiseError(error):
        raise Exception(error)