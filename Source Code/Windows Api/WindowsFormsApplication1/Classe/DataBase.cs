using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;
using ToolsClass;
using System.Security;
using Newtonsoft.Json;
using StudentData = ToolsClass.Tools.StudentData;
using Agent = ToolsClass.Tools.Agent;
using TimeSlot = ToolsClass.Tools.TimeSlot;
using StudentExitRegimeAuthorization = ToolsClass.Tools.StudentExitRegimeAuthorization;
using StudentExitRegime = ToolsClass.Tools.StudentExitRegime;

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using WindowsFormsApplication1.Forms;
using System.Text;
using Microsoft.VisualBasic.FileIO;
/* The app itself check the online database (the website) frequently to gather informations about student as their free hour, their not fenced punishment
* and unjustified absence.
* Then all that informations are distributed in different tables into our local database.
* In fact we need to have our own database because we have to associate each student to a unique RFID id. And we cannot add all that student
* in a system file because we need to make opertion on that informations.
* Morover each modification modification on a student is then register in a specific table with the id of the person in charge of that information
* (which is also store into the database)
* 
* The database gather 7 tables :
*  - "TABLE_AUTORISATION_SORTIE" which store all exit authorization
*  An authorization is composed of 
*      * a date from where the authorization start "DateDebut"
*      * a date when the authorization end "DateFin"
*      * a time slot (when the authorization start in time and when it stop) "HeureDebut", "HeureFin"
*      * potentialy a reason for that authorization "Motif"
*      * the id of the person in charge of that authorization (the person who set the authorization) "Id_agent"
*  The table also contains :
*      * the type of authorization : authorization can be delivered for one student or for the whole division of the student
*        Then to know if the authoriztion is for a student or for a division we set a different value for each case "Type"
*      * the index of the student or the division (in their own table) concerned by that authorization "Id_table"
*  - "TABLE_BLOCAGE" which store when student can not leave the school. That table have the same columns as the table "TABLE_AUTORISATION_SORTIE"
*  - "TABLE_AGENTS" which store all information about the potentiel person in charge of the modifications
*      * Since the index of each row is unique, we use it as the id of the agent "Id_agent"
*      * the last name of the agent "Nom"
*      * the first name of the agent "Prenom"
*      * the job of the agent "Travail"
*      * the mail adress of the agent "AdresseMail". That column is require because we send the agent password through mail
*      * an id to acces the app "Identifiant". The id is unique in the whole database
*      * a password, which has been hashed, to acess the app "MotDePAssehache"
*      * the salt of the hashed password "HashSalt"
*      * a short secure id (like when user want to acces a windows session with a four digit number) "IdentifiantCourtSecurise"
*        The short secure id is unique in the whole database
*  - "TABLE_CLASSES" which store all the division in the school
*      * we use the index of each row as the id of the current division in the whole database "Id_classe"
*      * a colum that represent the name of the divisions (like "3A", "6b",...) "Classe"
*  - "TABLE_REGISTRE" which store all the modification made into the database      
*      * the type concerned by the modification (student or division) "Type"
*      * the index of the concerned student or division "Id_table"
*      * the modification as plain text "Modification"
*  - "TABLE_ELEVES" which store all the informations about the student itself
*      * the index of the table is used as the id of the student in the whole database "Id_eleve"
*      * the student last name "Nom"
*      * the student first name "Prenom"
*      * the id of the student division "Id_classe"
*      * the student sex as an integer value "Sexe"
*      * the half bord days as plain text "DemiPension"
*      * the rfid id (that has been given to the student) "Id_RFID"
*  - "TABLE_RESULTATS" which store the result of all the process to know when the student can leave the school. This table avoid
*  to retrive all informations about a student when he asks for exit the school (whiwh is faster)
*  - "TABLE_SORTIE_TEMPORAIRE" which store all informations about the student schedule that has been found online
*  That table saved when the student can leaves the school on the whole current day (and those informations are only accecible from online)
*  Thus, the app frequently update that table
*      * the index of the table (primary key) "Id"
*      * the id of the student "Id_eleve"
*      * the start of the time slot when student can leave the school "HeureDebut"
*      * the end of the time slot when student can leave the school "HeureFin"
*      * an indication if agent have to check other student informations before letting him leave "VerificationRequise"
*      
*      
* Notice :
*  * Most of the table have a column to set the date and time when the current row was update (or create)
*  * Most of the tables have a column for an integer value which represent the fact that the row will not be take in consideration for search
*  That column represent the fact that the row is now deleted from the database. In fact when user want to delete a row we updat that value
*  but the data remain in the database for a certain time (but data are no more available for user)
*  * All the date and time are store in the database as plain text to avoid automatic conversion. Like that we can keep
*  control on the format of the conversion
*  * Most of the tables have a column that represent the type of data (if the data concern a student or a division). The type "Type" is an integer value
*  * Most of the tables have a column that indicate the table index of the data (the table index of the student or of the division) "Id_table"
*/


/*All the index of the database must be superior or equal to 0 because -1 is used in all the code as a default value
 */

class DataBase
{
    private static string PATH_TO_DATABASE = Definition.CURRENT_DIRECTORY + Definition.SEPARATOR + Definition.ParmsDatabase.DatabaseName;
    private string connexionDataBaseString =
         @"Data Source =" + Definition.ParmsDatabase.ServerName + ";"
        + "AttachDbFilename =" + PATH_TO_DATABASE + ";"
        + "Integrated Security=True;"
        + "MultipleActiveResultSets=True;"
        + "Connect Timeout=120;";

    private const int DATABASE_NOT_OPENED_VALUE = -1;
    private const int DELETE_VALUE = Definition.DELETE_VALUE_DATABASE;
    private const int DO_NOT_DELETE_VALUE = Definition.DO_NOT_DELETE_VALUE_DATABASE;
    private const int MAXIMUN_NUMBER_OF_RETRIES_SQL_REQUEST = 5;



    public SqlConnection openDataBase()
    {
        SqlConnection connection_database = new SqlConnection(connexionDataBaseString);
        {
            try
            {
                connection_database.Open();//load database as SQL database

            }
            catch (Exception e)
            {
                Error.details = "" + e;
                Error.error = "CODB";
            }
        }
        return connection_database;
    }


    public void closeDataBase(SqlConnection connection_database)
    {
        try
        {
            connection_database.Close(); //load database as SQL database
        }
        catch { }

    }

    private bool canRetry(SqlException sqlEx)
    {
        return ((sqlEx.Number == 1205) // 1205 = Deadlock
        || (sqlEx.Number == -2) // -2 = TimeOut
        || (sqlEx.Number == 3989) // 3989 = New request is not allowed to start because it should come with valid transaction descriptor
        || (sqlEx.Number == 3965) // 3965 = The PROMOTE TRANSACTION request failed because there is no local transaction active.
        || (sqlEx.Number == 3919) // 3919 Cannot enlist in the transaction because the transaction has already been committed or rolled back
        || (sqlEx.Number == 3903)); // The ROLLBACK TRANSACTION request has no corresponding BEGIN TRANSACTION.
    }

    private SqlDataReader executeReaderRequest(SqlConnection connection_database, string request, 
        List<string> parameters = null, List<object> value = null)
    {
        /* From an sql request as plain text, "request", send the request to database and return the associate data reader
         * If the request contains parameters all their representation in the text request are stored in the list "parameters"
         * and their associate value in the list "value" (at the same position), and add value to the sql command
         * If there is a probleme the function return null
         * 
         * Since the following script will be used multpile time we use it as a function to avoid rewrite the same script
        */

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = request; //set request

        /*If the request contains parameters then the list parameters is different from that default value which is null
            * Then for all parameters in that list we associate te value to the sql command
        */
        if (parameters != null)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                cmd.Parameters.AddWithValue(parameters[i], value[i]);//link parameters string with its value
            }
        }

        for (int number_of_retries = 0; number_of_retries < MAXIMUN_NUMBER_OF_RETRIES_SQL_REQUEST; number_of_retries++)
        {
            try
            {
                cmd.Connection = connection_database;
                
                return cmd.ExecuteReader();//execute the sql command
            }
            catch (SqlException e_sql)
            {
                /*Retry the request if the exeption is retriable*/
                if (canRetry(e_sql))
                {
                    if ( (connection_database != null) && (connection_database.State != ConnectionState.Closed) )
                    {
                        connection_database.Close();//close connexion
                    }
                    connection_database = openDataBase();//open new connexion

                    continue;
                }
                Error.details = "Requête non valide : " + request + "\n\n" + e_sql;
                Error.error = "CERTDB";

                break;
            }
            catch (Exception e)
            {
                Error.details = "Requête non valide : " + request + "\n\n" + e;
                Error.error = "CERTDB";
                break;
            }

        }

        if ( (connection_database == null) || (connection_database.State != ConnectionState.Open) )
        {
            connection_database = openDataBase();//close connexion
        }

        /* at the end of the function, if we reach the end of the loop, the connection is closed
         * But we need to keep an open connection at the end of the function because that connection will be use
         * in the rest of the master function (the function which call that one)
         * As all object is passed as reference (and not as copy) and as an SqlConnection is an object, when we modify
         * the object in that function, we modify the object into the master function
        */

        return null;
    }



    private int executeNonQueryRequest(string request, List<string> parameters = null, List<object> value = null)
    {
        /* ROMrom an sql request as plain text, "request", send the request to database and return the associate data reader
         * If the request contains parameters all their representation in the text request are stored in the list "parameters"
         * and their associate value in the list "value" (at the same position), and add value to the sql command
         * If there is a probleme the function return null
         * 
         * Since the following script will be used multpile time we use it as a function to avoid rewrite the same script
        */
        SqlCommand cmd = new SqlCommand();

        for (int number_of_retries = 0; number_of_retries < MAXIMUN_NUMBER_OF_RETRIES_SQL_REQUEST; number_of_retries++)
        {
            SqlConnection connection_database = openDataBase();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = request; //set request

            /*If the request contains parameters then the list parameters is different from that default value which is null
             * Then for all parameters in that list we associate te value to the sql command
            */
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    cmd.Parameters.AddWithValue(parameters[i], value[i]);//link parameters string with its value
                }
            }

            try
            {

                cmd.Connection = connection_database;

                cmd.ExecuteNonQuery();

                return Definition.NO_ERROR_INT_VALUE;

            }
            catch (SqlException e_sql)
            {
                /*Retry the request if the exeption is retriable*/
                if (canRetry(e_sql))
                {
                    if (connection_database != null)
                    {
                        connection_database.Close();//close connexion
                    }

                    continue;
                }
                Error.details = "La requête suivante n'a pas pu être effectuée : " + request + "\n\n\n" + e_sql;
                Error.error = "CERTDB";

                break;
            }
            catch (Exception e)
            {
                Error.details = "La requête suivante n'a pas pu être effectuée : " + request + "\n\n\n" + e;
                Error.error = "CERTDB";
            }
            finally
            {

                connection_database.Close();
            }
        }
        return Definition.ERROR_INT_VALUE;

   }
    


    public int purgeAllData()
    {
        /*
         * Delete all the data of all the table from the database without delete the tables (exept the table that contains agent informations)
         */
        int res = 0;
        string request;

        string[] list = new string[] {"TABLE_ELEVES", "TABLE_SORTIE_TEMPORAIRE", "TABLE_BLOCAGE", "TABLE_BLOCAGE_TEMPORAIRE", "TABLE_REGISTRE", "TABLE_AUTORISATION_SORTIE", "TABLE_CLASSES",
        "TABLE_REGIMES_SORTIE", "TABLE_RELATIONS_REGIME_PERMISSION", "TABLE_PERMISSIONS_REGIME"};

        foreach(string table in list)
        {
            request = "TRUNCATE TABLE " + table;
            if (executeNonQueryRequest(request) != Definition.NO_ERROR_INT_VALUE)
            {
                res++;
            }
        }

        if (res==0)//no error
        {
            return Definition.NO_ERROR_INT_VALUE;
        }else
        {
            return Definition.ERROR_INT_VALUE;
        }

    }

    public int removeExitRegime(int table_id_regime)
    {
        string request;
        int errCode;

        //because Id_regime in TABLE_REGIMES_SORTIE is a foreign key for the TABLE_ELEVES and TABLE_RELATION_REGIME_PERMISSION, then we must update that table before deleting the row in the TABLE_REGIMES_SORTIE
        request = "DELETE FROM TABLE_RELATIONS_REGIME_PERMISSION WHERE Id_regime=@table_id ";
        errCode = executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id" }),
            new List<object>(new object[] { table_id_regime }));

        if (errCode != Definition.NO_ERROR_INT_VALUE)
            return Definition.ERROR_INT_VALUE;

        request = "UPDATE TABLE_ELEVES SET Id_regime=null WHERE Id_regime=@table_id ";
        errCode = executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id" }),
            new List<object>(new object[] { table_id_regime }));

        if (errCode != Definition.NO_ERROR_INT_VALUE)
            return Definition.ERROR_INT_VALUE;

        request = "DELETE FROM TABLE_REGIMES_SORTIE WHERE Id_regime=@table_id ";
        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id"}),
            new List<object>(new object[] { table_id_regime}));
    }

    public int removeAuthorizationExitRegime(int table_id_authorization_regime)
    {
        string request;
        int errCode;

        //because Id_permission in TABLE_PERMISSIONS_REGIME is a foreign key for the TABLE_RELATIONS_REGIME_PERMISSION, then we must update that table before deleting the row in the TABLE_PERMISSIONS_REGIME
        request = "DELETE FROM TABLE_RELATIONS_REGIME_PERMISSION WHERE Id_permission=@table_id ";
        errCode = executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id" }),
            new List<object>(new object[] { table_id_authorization_regime }));

        if (errCode != Definition.NO_ERROR_INT_VALUE)
            return Definition.ERROR_INT_VALUE;

        request = "DELETE FROM TABLE_PERMISSIONS_REGIME WHERE Id_permission=@table_id ";
        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id" }),
            new List<object>(new object[] { table_id_authorization_regime }));
    }

    public int addAuthorizationExitRegime(string name, TimeSpan timePeriod, TimeSpan start, TimeSpan end)
    {
        string request = "INSERT INTO TABLE_PERMISSIONS_REGIME (Label, Periode, Debut, Fin) VALUES (@label, @period, @start, @end) ";
        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@label", "@period", "@start", "@end"}),
            new List<object>(new object[] { name, Tools.timeToStringFromTimeSpan(timePeriod), Tools.timeToStringFromTimeSpan(start), Tools.timeToStringFromTimeSpan(end) }));
    }

    public int addAuthorizationExitRegime(List<StudentExitRegimeAuthorization> list)
    {
        foreach(StudentExitRegimeAuthorization authorization in list)
        {
            if (addAuthorizationExitRegime(authorization.name, authorization.period, authorization.start, authorization.end) != Definition.NO_ERROR_INT_VALUE)
                return Definition.ERROR_INT_VALUE;
        }

        return Definition.NO_ERROR_INT_VALUE;
    }


    public int updateAuthorizationExitRegime(int table_id, string name, TimeSpan timePeriod, TimeSpan start, TimeSpan end)
    {
        string request = "UPDATE TABLE_PERMISSIONS_REGIME SET Label=@label, Periode=@period, Debut=@start, Fin=@end WHERE Id_permission=@table_id ";
        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@table_id", "@label", "@period", "@start", "@end" }),
            new List<object>(new object[] { table_id, name, Tools.timeToStringFromTimeSpan(timePeriod), Tools.timeToStringFromTimeSpan(start), Tools.timeToStringFromTimeSpan(end) }));
    }

    public int updateAuthorizationExitRegime(List<StudentExitRegimeAuthorization> list)
    {
        foreach (StudentExitRegimeAuthorization authorization in list)
        {
            if (authorization.Id == -1)
            {
                if (addAuthorizationExitRegime(authorization.name, authorization.period, authorization.start, authorization.end) != Definition.NO_ERROR_INT_VALUE)
                    return Definition.ERROR_INT_VALUE;
            }
            else
            {
                if (updateAuthorizationExitRegime(authorization.Id, authorization.name, authorization.period, authorization.start, authorization.end) != Definition.NO_ERROR_INT_VALUE)
                    return Definition.ERROR_INT_VALUE;
            }
        }

        return Definition.NO_ERROR_INT_VALUE;
    }


    public int updateExitRegime(StudentExitRegime regime)
    {
        string request;
        int errCode;

        SqlConnection connection_database = openDataBase();
        SqlDataReader sql_reader;

        try
        {
            //update exit regime
            if (regime.Id == -1)//add new exit regime
            {
                request = "INSERT INTO TABLE_REGIMES_SORTIE (Label, SortieFinJournee) VALUES (@label, @exitEndOfDay) ";
                errCode = executeNonQueryRequest(request,
                    new List<string>(new string[] { "@label", "@exitEndOfDay" }),
                    new List<object>(new object[] { regime.name, regime.exitEndOfDay }));

                if (errCode != Definition.NO_ERROR_INT_VALUE)
                    return Definition.ERROR_INT_VALUE;

                //get id of the inserted row
                request = "SELECT Id_regime FROM TABLE_REGIMES_SORTIE WHERE Label=@label ";
                sql_reader = executeReaderRequest(connection_database, request,
                           new List<string>(new string[] { "@label" }),
                           new List<object>(new object[] { regime.name }));

                if (sql_reader != null && sql_reader.HasRows)
                {
                    sql_reader.Read();//the is only one authorization with that name
                    regime.Id = sql_reader.GetInt32(0);
                }
                else
                {
                    return Definition.ERROR_INT_VALUE;
                }

            }else//update existing exit regime
            {
                request = "UPDATE TABLE_REGIMES_SORTIE SET Label=@label, SortieFinJournee=@exitEndDay WHERE Id_regime=@table_id ";
                errCode = executeNonQueryRequest(request,
                    new List<string>(new string[] { "@label", "@exitEndDay", "@table_id" }),
                    new List<object>(new object[] { regime.name, regime.exitEndOfDay, regime.Id }));

                if (errCode != Definition.NO_ERROR_INT_VALUE)
                    return Definition.ERROR_INT_VALUE;

                //remove all link between the current regime and permissions (to set new one after)
                request = "DELETE FROM Table_RELATIONS_REGIME_PERMISSION WHERE Id_regime=@table_id ";
                errCode = executeNonQueryRequest(request,
                    new List<string>(new string[] { "@table_id" }),
                    new List<object>(new object[] { regime.Id }));

                if (errCode != Definition.NO_ERROR_INT_VALUE)
                    return Definition.ERROR_INT_VALUE;
            }

            //add relation between existing permissions and the current regime (permissions must have been inserted before)
        
            foreach (string authorization_name in regime.authorizations)
            {
                request = "SELECT Id_permission FROM TABLE_PERMISSIONS_REGIME WHERE Label=@label ";
                sql_reader = executeReaderRequest(connection_database, request,
                           new List<string>(new string[] { "@label" }),
                           new List<object>(new object[] { authorization_name }));

                if (sql_reader != null && sql_reader.HasRows)
                {
                    sql_reader.Read();//the is only one authorization with that name
                    int Id_authorization = sql_reader.GetInt32(0);

                    request = "INSERT INTO Table_RELATIONS_REGIME_PERMISSION (Id_regime, Id_permission) VALUES (@id_regime, @id_authorization) ";
                    errCode = executeNonQueryRequest(request,
                               new List<string>(new string[] { "@id_regime", "@id_authorization" }),
                               new List<object>(new object[] { regime.Id, Id_authorization }));

                    if (errCode != Definition.NO_ERROR_INT_VALUE)
                        return Definition.ERROR_INT_VALUE;
                }
                else//the permission does not exists
                {
                    return Definition.ERROR_INT_VALUE;
                }
            }

        }
        catch
        {
            return Definition.ERROR_INT_VALUE;
        }
        finally
        {
            closeDataBase(connection_database);
        }



        return Definition.NO_ERROR_INT_VALUE;
    }

    public int updateExitRegime(List<StudentExitRegime> list)
    {
        foreach (StudentExitRegime regime in list)
        {
            if (updateExitRegime(regime) != Definition.NO_ERROR_INT_VALUE)
                return Definition.ERROR_INT_VALUE;
        }

        return Definition.NO_ERROR_INT_VALUE;
    }


    public string getExitRegimeLabel(int regime_table_id)
    {
        SqlConnection connection_database = openDataBase();

        try
        {
            string request = "SELECT Label FROM TABLE_REGIMES_SORTIE WHERE Id_regime=@table_id ";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@table_id" }),
                        new List<object>(new object[] { regime_table_id }));

            if (sql_reader != null && sql_reader.HasRows)
            {
                sql_reader.Read();//the is only one authorization with that name
                return sql_reader.GetString(0);
            }
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            closeDataBase(connection_database);
        }

        return null;
    }

    public int getExitRegimeId(string regime_label)
    {
        if (regime_label == null)
            return -1;

        SqlConnection connection_database = openDataBase();

        try
        {
            string request = "SELECT Id_regime FROM TABLE_REGIMES_SORTIE WHERE Label=@label ";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@label" }),
                        new List<object>(new object[] { regime_label }));

            if (sql_reader != null && sql_reader.HasRows)
            {
                sql_reader.Read();//the is only one authorization with that name
                return sql_reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            return -1;
        }
        finally
        {
            closeDataBase(connection_database);
        }

        return -1;
    }


    public string getExitRegimeLabel(SqlDataReader reader, int index)
    {
        if (reader == null)
            return null;
        if (index >= reader.FieldCount)
            return null;

        int id;
        if (reader.IsDBNull(index))
        {
            int default_id = Definition.DEFAULT_EXIT_REGIME_DATABASE_TABLE_ID;
            if (default_id == -1)
                return null;
            else
                id = default_id;
        }else
        {
            id = reader.GetInt32(index);
        }

        return getExitRegimeLabel(id);
    }
    

    public Tuple<List<StudentExitRegimeAuthorization>, List<StudentExitRegime>> getAllExitRegime()
    {
        StudentExitRegimeAuthorization authorization;
        StudentExitRegime regime;
        List<StudentExitRegimeAuthorization> list_authorizations = new List<StudentExitRegimeAuthorization>();
        List<StudentExitRegime> list_regime = new List<StudentExitRegime>();
        string request;
        SqlDataReader sql_reader;

        SqlConnection connection_database = openDataBase();

        try
        {
            //get all authorizations
            request = "SELECT Id_permission, Label, Periode, Debut, Fin FROM Table_PERMISSIONS_REGIME ";

            sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    authorization = new StudentExitRegimeAuthorization();
                    authorization.toDefault();

                    //fill fields
                    authorization.Id = sql_reader.GetInt32(0);
                    authorization.name = sql_reader.GetString(1);
                    authorization.period = Tools.stringToTimeSpan(sql_reader.GetString(2));
                    authorization.start = Tools.stringToTimeSpan(sql_reader.GetString(3));
                    authorization.end = Tools.stringToTimeSpan(sql_reader.GetString(4));

                    list_authorizations.Add(authorization);
                }
            }

            //get all the exit regime label and id
            request = "SELECT Id_regime, Label, SortieFinJournee FROM TABLE_REGIMES_SORTIE ";

            sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    //add new exit regime
                    regime = new StudentExitRegime();
                    regime.toDefault();

                    regime.Id = sql_reader.GetInt32(0);
                    regime.name = sql_reader.GetString(1);
                    regime.exitEndOfDay = sql_reader.GetBoolean(2);

                    //get all the authorization associate to the current regime
                    request = "SELECT P.Label " +
                     "FROM Table_PERMISSIONS_REGIME P, Table_RELATIONS_REGIME_PERMISSION Re " +
                     "WHERE Re.Id_permission = P.Id_permission AND Re.Id_regime = @table_id ";

                    SqlDataReader sql_reader_temp = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@table_id" }),
                        new List<object>(new object[] { regime.Id }));

                    if (sql_reader_temp != null)
                    {
                        while (sql_reader_temp.Read())
                        {
                            regime.authorizations.Add(sql_reader_temp.GetString(0));
                        }

                    }

                    list_regime.Add(regime);
                }

            }

        }catch
        {
            return null;
        }
        finally
        {
            closeDataBase(connection_database);
        }

        return Tuple.Create(list_authorizations, list_regime);
    }

    public int clearDatabase()
    {
        /* User can delete agent or student from the database. But in fact when they "delete" the person from the database
         * they update the profil which remain in the database for a certain amont of time
         * After that time, we permanently remove the data from database
         * Except table that deal with person, all other table keep data until a certain amont of time, after that time
         * we remove the corresponding row from database
        */

        TimeSpan time_before_suppression = TimeSpan.FromDays(SecurityManager.TIME_BEFORE_SUPPRESSION_OF_DATA_FROM_DATABASE_DAYS);
        DateTime now = DateTime.Now;
        //saving period

        SqlConnection connection_database = openDataBase();

        try
        {
            //-------------------------- Delete old exit ban from database
            string request = "SELECT Id, DateFin, DateEdition FROM TABLE_BLOCAGE";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request);
            
            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    DateTime end_date = Tools.stringToDateTime(sql_reader.GetString(1));
                    DateTime modification_date = Tools.stringToDateTime(sql_reader.GetString(2));
                    /* If the exit ban is no longer usable/active (the end of the ban is exceed for a while)
                     * and if the modification date is also exceed for a while
                     * Then we can delete the record from database
                    */
                    if (end_date < now && now.Subtract(end_date) > time_before_suppression && now.Subtract(modification_date) > time_before_suppression)
                    {
                        //delete from database
                        request = "DELETE FROM TABLE_BLOCAGE WHERE Id=@row_index";
                        executeNonQueryRequest(request,
                            new List<string>(new string[] { "@row_index" }),
                            new List<object>(new object[] { sql_reader.GetInt32(0) }));
                    }
                }

            }

            //-------------------------- Delete old exit authorization from database
            request = "SELECT Id, DateFin, DateEdition FROM TABLE_AUTORISATION_SORTIE";
            sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    DateTime end_date = Tools.stringToDateTime(sql_reader.GetString(1));
                    DateTime modification_date = Tools.stringToDateTime(sql_reader.GetString(2));
                    /* If the exit authorization is no longer usable/active (the end of the ban is exceed for a while)
                     * and if the modification date is also exceed for a while
                     * Then we can delete the record from database
                    */
                    if (end_date < now && now.Subtract(end_date) > time_before_suppression && now.Subtract(modification_date) > time_before_suppression)
                    {
                        //delete from database
                        request = "DELETE FROM TABLE_AUTORISATION_SORTIE WHERE Id=@row_index";
                        executeNonQueryRequest(request,
                            new List<string>(new string[] { "@row_index" }),
                            new List<object>(new object[] { sql_reader.GetInt32(0) }));
                    }
                }

            }


            //-------------------------- Delete old register input from database
            request = "SELECT Id, DateEdition FROM TABLE_REGISTRE";
            sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    DateTime modification_date = Tools.stringToDateTime(sql_reader.GetString(1));
                    if (now.Subtract(modification_date) > time_before_suppression)
                    {
                        //delete from database
                        request = "DELETE FROM TABLE_REGISTRE WHERE Id=@row_index";
                        executeNonQueryRequest(request,
                            new List<string>(new string[] { "@row_index" }),
                            new List<object>(new object[] { sql_reader.GetInt32(0) }));
                    }
                }

            }
            

            //-------------------------- Delete student saved as delete person from the database
            request = "SELECT Id_eleve, DateEdition FROM TABLE_ELEVES WHERE Supprimer=@delete_value";
            sql_reader = executeReaderRequest(connection_database, request,
                            new List<string>(new string[] { "@delete_value" }),
                            new List<object>(new object[] { DELETE_VALUE }));

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    DateTime modification_date = Tools.stringToDateTime(sql_reader.GetString(1));
                    if (now.Subtract(modification_date) > time_before_suppression)
                    {
                        request = "DELETE FROM TABLE_ELEVES WHERE Id_eleve=@row_index ; "
                                + "DELETE FROM TABLE_AUTORISATION_SORTIE WHERE Type=@type_student_table AND Id_table=@row_index ; "
                                + "DELETE FROM TABLE_BLOCAGE WHERE Type=@type_student_table AND Id_table=@row_index ; "
                                + "DELETE FROM TABLE_REGISTRE WHERE Type=@type_student_table AND Id_table=@row_index ; ";
                        //delete all informations registered about the current student
                        executeNonQueryRequest(request,
                            new List<string>(new string[] { "@type_student_table", "@row_index" }),
                            new List<object>(new object[] { Definition.TYPE_OF_STUDENT_TABLE, sql_reader.GetInt32(0) }));
                    }

                }
            }


            //-------------------------- Delete agent saved as delete person from the database
            request = "SELECT Id_agent, DateEdition FROM TABLE_AGENTS WHERE Supprimer=@delete_value";
            sql_reader = executeReaderRequest(connection_database, request,
                            new List<string>(new string[] { "@delete_value" }),
                            new List<object>(new object[] { DELETE_VALUE }));

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    DateTime modification_date = Tools.stringToDateTime(sql_reader.GetString(1));
                    if (now.Subtract(modification_date) > time_before_suppression)
                    {
                        request = "DELETE FROM TABLE_AGENTS WHERE Id_agent=@row_index";
                        //delete all informations registered about the current student
                        executeNonQueryRequest(request,
                            new List<string>(new string[] { "@row_index" }),
                            new List<object>(new object[] { sql_reader.GetInt32(0) }));
                    }

                }
            }

        }
        catch
        {
            return Definition.ERROR_INT_VALUE;
        }
        finally
        {
            closeDataBase(connection_database);
        }

        return Definition.NO_ERROR_INT_VALUE;
    }


    public Tuple<List<int>, List<string>> getInfoFromRfidStudent()
    {
        /*Retrieve as lists all the student id (from database) that are not associate with an RFID id
         * and all the used RFID in the data base
        */

        List<int> list_student_id_with_no_rfid = new List<int>();
        //lists of the student id (from database) not associate with an RFID id
        List<string> list_rfid_id = new List<string>();
        //list of all existant RFID id in the database
        SqlConnection connection_database = openDataBase();
        try
        {
            string request = "SELECT Id_eleve, Id_RFID FROM TABLE_ELEVES WHERE Supprimer=@do_not_delete_value";

            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@do_not_delete_value" }), new List<object>(new object[] { DO_NOT_DELETE_VALUE }));

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {

                    if (Tools.isRfidFormatCorrect(sql_reader.GetString(1)))//if the field is non null and non empty
                    {
                        list_rfid_id.Add(sql_reader.GetString(1));
                    }
                    else//student does not have an RFID id
                    {
                        list_student_id_with_no_rfid.Add(sql_reader.GetInt32(0));
                    }
                }

            }
        }
        catch { }

        closeDataBase(connection_database);

        return Tuple.Create(list_student_id_with_no_rfid, list_rfid_id);
    }


    public bool isRfidFree(string rfid, int student_table_index=-1)
    {
        /* 
         * Test if the given rfid "rfid" is already taken by a student (other than the given student trough its index into the database)
        */

        SqlConnection connection_database = openDataBase();
        try
        {
            string request = "SELECT Id_eleve FROM TABLE_ELEVES WHERE "
                + "Id_eleve != @student_table_index AND Id_RFID=@rfid ";
            //we don't add "AND Supprimer=@do_not_delete_value" because we want to be sure that the RFDI id is the only one in the database


            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@student_table_index", "@do_not_delete_value", "@rfid" }),
                new List<object>(new object[] { student_table_index, DO_NOT_DELETE_VALUE, rfid }));

            if (sql_reader != null)
            {
                if (!sql_reader.HasRows)
                {
                    return true;
                }
            }

        }catch { }

        closeDataBase(connection_database);

        return false;
    }


    /*---------------------------------------------  UPDATE STUDENT TABLE FROM FILE -------------------------------------------------------------------------------------*/

    public int updateStudentTableFromFile(string path_update_file, string path_file_error)
    {
        StringBuilder error_file;
        try
        {
            //error file
            File.WriteAllText(path_file_error, "");//overwrite file
            error_file = new StringBuilder();
        }catch
        {
            return Definition.ERROR_INT_VALUE;
        }

        try
        {
            Tools.ParmsStudentsStateFile parms = Settings.StudentsStateFileParameters;
            DataBase database = new DataBase();

            //set headers
            var newLine = string.Format("{0};{1};{2};{3};{4}", "Nom", "Prenom", "Ligne origine", "Erreur", "Pris en compte");
            error_file.AppendLine(newLine);

            bool error = false;


            using (TextFieldParser csvParser = new TextFieldParser(path_update_file))
            {
                csvParser.SetDelimiters(new string[] { Char.ToString(parms.separatorCsv) });
                csvParser.HasFieldsEnclosedInQuotes = parms.encapsulationByQuote;

                // Skip the row with the column names
                csvParser.ReadLine();

                //start updating database
                List<Tuple<StudentData, long>> list = new List<Tuple<StudentData, long>>();
                StudentData student = new StudentData();
                int count;
                int res;

                count = 1;
                student.toDefault();
                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    if (count % 100 == 0)//we save student into the database block of n students by block of n students at once
                    {
                        res = updateStudentTable(list, error_file);
                        if (res != Definition.NO_ERROR_INT_VALUE)
                            error = true;

                        //save error file
                        File.AppendAllText(path_file_error, error_file.ToString());

                        //clear variable
                        list.Clear();
                        count = 1;
                        error_file = new StringBuilder();
                    }

                    //check format information
                    if (!Tools.isNormalizedStudentInfo(fields[parms.lastNameIndex], name: true) || !Tools.isNormalizedStudentInfo(fields[parms.firstNameIndex], name: true)
                        || !Tools.isNormalizedStudentInfo(fields[parms.divisionIndex], division: true))
                    {
                        error = true;
                        writeIntoErrorFile(error_file, fields[parms.lastNameIndex], fields[parms.firstNameIndex], csvParser.LineNumber, "Le format de la ligne n'est pas correct", false);
                        continue;
                    }

                    student.toDefault();
                    student.lastName = fields[parms.lastNameIndex];
                    student.firstName = fields[parms.firstNameIndex];
                    student.division = fields[parms.divisionIndex];
                    student.sex = Tools.SexIntFromString(fields[parms.sexIndex], parms.femaleShortname); //sex (int)
                    student.halfBoardDays = Tools.getNumericHalfBoardDaysFromString(parms, fields[parms.halfBoardDaysIndex]); //half-board regime (string)
                    student.labelRegime = (parms.exitRegimeIndex == -1) ? null : fields[parms.exitRegimeIndex];
                    if(student.labelRegime!=null && student.labelRegime.Length==0)
                    {
                        student.labelRegime = null;
                    }
                    
                    list.Add(Tuple.Create(student, csvParser.LineNumber));

                    count++;
                }
                updateStudentTable(list, error_file);
            }
            File.AppendAllText(path_file_error, error_file.ToString());

            if (error)//we save student into the database block of 200 students by block of 200 students at once
            {
                return Definition.ERROR_INT_VALUE;
            }

        }catch(Exception e)
        {
            Error.details = "Erreur lors de la lecture du fichier source pour la mise à jour des informations étudiantes : \n"+e;
            Error.error = "CUDFF";
            
            writeIntoErrorFile(error_file, null, null, -1, ""+e, false);
            File.AppendAllText(path_file_error, error_file.ToString());

            return Definition.ERROR_INT_VALUE;
        }

        return Definition.NO_ERROR_INT_VALUE;
    }


    private int updateStudentTable(List<Tuple<StudentData, long>> list, StringBuilder error_file)
    {
        /*In fact openning and closing the database mutiple times in a short period makes SQL server crashe
         * So to avoid that problem, we gather students as medium block (for exemple, we split the file that contains student informations
         * into small pieces of 100 students) in order to use the same connection string for that block (then close the database)
         * In orther word, we open once the database to insert all the student of a block (for example 100 student at onceà, then close the database 
         * and start again
         * 
         * We don't want to let open the database while inserting all the student of the file, because the number of student could be important
         * and keep the database opened is not a good idea
        */
        SqlConnection connection_database;
        int res;
        StudentData student;
        long lineNumber;
        int student_id;
        int division_id;
        int regime_id;
        int errCode;
        bool error = false;

        connection_database = openDataBase();

        foreach (var item in list)
        {
            student = item.Item1;
            lineNumber = item.Item2;

            //get division id of the student
            var tmp = getStudentDivisionId(connection_database, student.division);
            division_id = tmp.Item1;
            if (tmp.Item2.Length!=0)
            {
                error = true;

                writeIntoErrorFile(error_file, student.lastName, student.firstName, lineNumber, tmp.Item2, false);
                continue;
            }

            //get exit regime id for the student
            if (student.labelRegime == null)//raise no error (default value for the exit regime)
            {
                regime_id = -1;

            }else
            { 
                tmp = getStudentExitRegimeId(connection_database, student.labelRegime);
                regime_id = tmp.Item1;
                if (tmp.Item2.Length != 0)
                {
                    error = true;

                    writeIntoErrorFile(error_file, student.lastName, student.firstName, lineNumber, tmp.Item2, true);
                    regime_id = -1;
                }
            }

            //get student id (if it already exists into the database)
            tmp = getStudentId(connection_database, student.lastName, student.firstName);
            student_id = tmp.Item1;
            if (tmp.Item2.Length != 0)
            {
                error = true;

                writeIntoErrorFile(error_file, student.lastName, student.firstName, lineNumber, tmp.Item2, false);
                continue;
            }

            //update the database
            if(student_id==-1)//new student
            {
                  errCode = addStudent(connection_database, student.lastName, student.firstName, division_id, regime_id, student.sex, student.halfBoardDays);

            } else//update data for the student
            {
                errCode = updateStudent(connection_database, student_id, division_id, regime_id, student.sex, student.halfBoardDays);
            }

            if(errCode!=Definition.NO_ERROR_INT_VALUE)
            {
                error = true;

                writeIntoErrorFile(error_file, student.lastName, student.firstName, lineNumber, "un problème sur la base de donnees est survenu", false);
                continue;
            }
        }

        closeDataBase(connection_database);

        return (error)?Definition.ERROR_INT_VALUE:Definition.NO_ERROR_INT_VALUE;
    }

    private void writeIntoErrorFile(StringBuilder error_file, string lastName, string firstName, long lineNumber, string error, bool added)
    {
        var newLine = string.Format("{0};{1};{2};{3};{4}", firstName, lastName, lineNumber, error, added);
        error_file.AppendLine(newLine);
    }

    private Tuple<int, string> getStudentDivisionId(SqlConnection connection_database, string division)
    {
        int divison_table_id;
        SqlDataReader sql_reader;
        string request;

        try
        {
            request = "SELECT Id_classe FROM TABLE_CLASSES WHERE Classe=@division ";

            sql_reader = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@division" }),
                        new List<object>(new object[] { division }));

            if ((sql_reader != null) && (sql_reader.HasRows))//division already exists
            {
                sql_reader.Read();
                divison_table_id = sql_reader.GetInt32(0);
            }
            else//new division
            {
                request = "INSERT INTO TABLE_CLASSES (Classe) OUTPUT INSERTED.Id_classe VALUES (@division) ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection_database;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = request; //set request
                cmd.Parameters.AddWithValue("@division", division);

                divison_table_id = (int)cmd.ExecuteScalar();//get id inserted row
            }

        }catch(Exception e)
        {
            divison_table_id = -1;
            return Tuple.Create(divison_table_id, ""+e);
        }

        return Tuple.Create(divison_table_id, "");
    }

    private Tuple<int, string> getStudentExitRegimeId(SqlConnection connection_database, string labelRegime)
    {
        if(labelRegime==null)
        {
            return Tuple.Create(-1, "");
        }

        int table_id;
        SqlDataReader sql_reader;
        string request;

        try
        {
            request = "SELECT Id_regime FROM TABLE_REGIMES_SORTIE WHERE Label=@label ";

            sql_reader = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@label" }),
                        new List<object>(new object[] { labelRegime }));

            if ((sql_reader != null) && (sql_reader.HasRows))//division already exists
            {
                sql_reader.Read();
                table_id = sql_reader.GetInt32(0);
            }
            else//new division
            {
                return Tuple.Create(-1, "Le regime de sortie <<"+labelRegime+">> n'existe pas dans la base de donnees");
            }

        }
        catch (Exception e)
        {
            return Tuple.Create(-1, "" + e);
        }

        return Tuple.Create(table_id, "");
    }


    private Tuple<int, string> getStudentId(SqlConnection connection_database, string last_name, string first_name)
    {
        int table_id = -1;
        SqlDataReader sql_reader;
        string request;

        try
        {
            request = "SELECT Id_eleve FROM TABLE_ELEVES WHERE dbo.NormalizeText(Nom)=dbo.NormalizeText(@last_name) "
            + "AND dbo.NormalizeText(Prenom)=dbo.NormalizeText(@first_name)";

            sql_reader = executeReaderRequest(connection_database, request,
                        new List<string>(new string[] { "@last_name", "@first_name" }),
                        new List<object>(new object[] { last_name, first_name }));

            if (sql_reader != null)
            {
                if (sql_reader.HasRows)
                {
                    sql_reader.Read();
                    return Tuple.Create(sql_reader.GetInt32(0), "");
                }else
                {
                    return Tuple.Create(-1, "");
                }
            }

        }catch (Exception e)
        {
            return Tuple.Create(-1, "" + e);
        }

        return Tuple.Create(table_id, "");
    }

    private int updateStudent(SqlConnection connection_database, int student_table_id, int division_table_id, int regime_table_id, int sex, int[] half_board_days)
    {
        string date = Tools.dateTimeToString(DateTime.Now);

        if (regime_table_id == -1)
        {
            string request = "UPDATE TABLE_ELEVES SET DemiPension=@half_board_days, Supprimer=@do_not_delete_value, Sexe=@sex, Id_regime=null, Id_classe=@id_division, DateEdition=@modification_date WHERE Id_eleve=@student_table_id ";


            return executeNonQueryRequest(request,
                new List<string>(new string[] { "@student_table_id", "@half_board_days", "@do_not_delete_value", "@modification_date", "@sex", "@id_division" }),
                new List<object>(new object[] { student_table_id, Tools.arrayToText(half_board_days), DO_NOT_DELETE_VALUE, date, sex, division_table_id }));
        }
        else
        {
            string request = "UPDATE TABLE_ELEVES SET DemiPension=@half_board_days, Supprimer=@do_not_delete_value, Sexe=@sex, Id_regime=@id_regime, Id_classe=@id_division, DateEdition=@modification_date WHERE Id_eleve=@student_table_id ";


            return executeNonQueryRequest(request,
                new List<string>(new string[] { "@student_table_id", "@half_board_days", "@do_not_delete_value", "@modification_date", "@sex", "@id_division", "@id_regime" }),
                new List<object>(new object[] { student_table_id, Tools.arrayToText(half_board_days), DO_NOT_DELETE_VALUE, date, sex, division_table_id, regime_table_id }));
        }
    }


    private int addStudent(SqlConnection connection_database, string last_name, string first_name, int division_table_id, int regime_table_id, int sex, int[] half_board_days)
    {
        string date = Tools.dateTimeToString(DateTime.Now);

        if (regime_table_id != -1)
        {
            string request = "INSERT INTO TABLE_ELEVES (Nom,Prenom,Id_classe, Sexe, DemiPension, Id_regime, Id_RFID, DateEdition, Supprimer) "
                            + "VALUES (@last_name,@first_name,@division_table_id,@sex,@half_board_days,@id_regime, @id_rfid, @modification_date, @do_not_delete_agent) ";

            return executeNonQueryRequest(request,
            new List<string>(new string[] { "@last_name", "@first_name", "@division_table_id", "@sex", "@half_board_days", "@id_regime", "@id_rfid", "@modification_date", "@do_not_delete_agent" }),
            new List<object>(new object[] { last_name, first_name, division_table_id, sex, Tools.arrayToText(half_board_days), regime_table_id, "", date, DO_NOT_DELETE_VALUE }));
        }else
        {
            string request = "INSERT INTO TABLE_ELEVES (Nom,Prenom,Id_classe, Sexe, DemiPension, Id_regime, Id_RFID, DateEdition, Supprimer) "
                            + "VALUES (@last_name,@first_name,@division_table_id,@sex,@half_board_days,null, @id_rfid, @modification_date, @do_not_delete_agent) ";

            return executeNonQueryRequest(request,
            new List<string>(new string[] { "@last_name", "@first_name", "@division_table_id", "@sex", "@half_board_days", "@id_rfid", "@modification_date", "@do_not_delete_agent" }),
            new List<object>(new object[] { last_name, first_name, division_table_id, sex, Tools.arrayToText(half_board_days), "", date, DO_NOT_DELETE_VALUE }));
        }
    }


    /*----------------------------------------------------------------------------------------------------------------------------------*/

    public Tuple<List<int>,List<string>> getStudentDataBySearch(string search_word)
    {
        /*From a search string get all the corresponding data from the table "TABLE_ELEVES"
         * that stored all the student informations
         * The function retunr two list :
         *  The first one contains all the id from the table (the primary key) that will allow us to identify student easier later
         *  The second one stored all the corresponding information about found student (as a unique string)
         * For example : return ([id1,id2], [string1,string2])
         * 
         * Else return empty lists
       */
        
        List<int> list_id = new List<int>();//list of id from table
        List<string> list_result = new List<string>();//list of corresponding informations

        if ((string.IsNullOrEmpty(search_word)) || (string.IsNullOrWhiteSpace(search_word)))
        {
            return Tuple.Create(list_id, list_result);
        }


        SqlConnection connection_database = openDataBase();
        try
        {

            string request = "DECLARE @normalized_word as varchar(1000); SET @normalized_word = dbo.NormalizeText(@search_word); "
                + "SELECT E.Id_eleve,E.Nom ,E.Prenom ,C.Classe,E.Sexe FROM TABLE_ELEVES AS E "
                + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
                + "AND (dbo.NormalizeText(E.Nom) LIKE '%'+@normalized_word+'%' OR dbo.NormalizeText(E.Prenom) LIKE '%'+@normalized_word+'%' OR E.Id_RFID=@search_word OR C.Classe LIKE @search_word+'%') "
                + "AND E.Supprimer=@do_not_delete_value "
                + "ORDER BY C.Classe, E.Nom, E.Prenom ASC ";//dbo.NormalizeText() custum sql function to normalize input text
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@search_word", "@do_not_delete_value" }),
                new List<object>(new object[] { search_word, DO_NOT_DELETE_VALUE }));

            if (sql_reader != null)
            {
                string res;
                while (sql_reader.Read())
                {
                    //Gather all information about a student as a unique string following the format : "LAST_NAME first_name (division) - SEXE"
                    res = sql_reader.GetString(1).ToUpper() + " " + sql_reader.GetString(2).ToLower() + " (" + sql_reader.GetString(3) + ") - ";
                    if (sql_reader.GetInt32(4) == Definition.ID_FEMALE)
                    {
                        res += "SEXE FEMME";
                    }
                    else
                    {
                        res += "SEXE HOMME";
                    }

                    list_result.Add(res);
                    list_id.Add(sql_reader.GetInt32(0));
                }

            }
        }
        catch { }

        closeDataBase(connection_database);

        return Tuple.Create(list_id,list_result);
    }


    public Tuple<List<int>, List<string>> getAllStudentDivision()
    {
        /*
         * Return all the (differente) student division into a list
         * Else return an empty list
       */

        List<int> list_id_result = new List<int>();
        List<string> list_result = new List<string>();//list of corresponding informations
        SqlConnection connection_database = openDataBase();

        try
        {
            string request = "SELECT DISTINCT Classe, Id_classe FROM TABLE_CLASSES ORDER BY Classe ASC ";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {
                while (sql_reader.Read())
                {
                    list_id_result.Add(sql_reader.GetInt32(1));
                    list_result.Add(sql_reader.GetString(0));
                }

            }

        }catch { }

        closeDataBase(connection_database);

        return Tuple.Create(list_id_result, list_result);
    }


    public StudentData getStudentByIndex(int student_table_id)
    {
        /*
         * From the student database id return all the retrieve information as a structure :
         *  Last name
         *  First Name
         *  Division
         *  Sex
         *  Hals board days
         *  
         * Else return a structure with default value
         */
        SqlConnection connection_database = openDataBase();

        
        StudentData student = new StudentData();
        student.toDefault();
        student.error = true;

        try
        {

            string request = "SELECT E.Nom,E.Prenom,C.Classe,E.Sexe,E.DemiPension,E.Id_RFID, E.Id_regime FROM TABLE_ELEVES AS E "
                + "JOIN TABLE_CLASSES AS C ON E.Id_classe= C.Id_classe AND E.Id_eleve=@table_student_id AND E.Supprimer=@do_not_delete_value";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@table_student_id", "@do_not_delete_value" }),
                new List<object>(new object[] { student_table_id, DO_NOT_DELETE_VALUE }));

            if (sql_reader != null)
            {


                if (sql_reader.HasRows)
                {
                    sql_reader.Read();
                    student.tableId = student_table_id;
                    student.lastName = sql_reader.GetString(0);
                    student.firstName = sql_reader.GetString(1);
                    student.division = sql_reader.GetString(2);
                    student.sex = sql_reader.GetInt32(3);
                    student.halfBoardDays = Tools.textToArray<int>(sql_reader.GetString(4));
                    student.idRFID = sql_reader.GetString(5);
                    student.labelRegime = getExitRegimeLabel(sql_reader, 6);

                }
                student.error = false;
            }

        }catch { }

        closeDataBase(connection_database);
        return student;
    }

    public Tuple<int, string> getExitCertificationForStudent(int student_table_id, DateTime date, int day_of_week, TimeSpan time)
    {
        /* The main use of that function is when student try to leave the school.
         * Thus, the result must appears as fast as possible at the exit door of the school. To achieve that goal we don't use anymore the regular
         * custom function used before to retrieve the result of the database (because this function nhave lists as parameters that need to be 
         * create in memory before being used, which is time consuming).
         * 
         * To know if a student can leave the school we have to check :
         *  - if an agent set an exit ban for that student (if so, the student can not leave the school)
         *  - if the regular schedule of the student allow him to leave the school (if so the student can leave the school)
         *  - if an agent set a special exit authorization for that student (if so, the student can leave the school)
         *  - if this is lunch time we check if the student can leave the school (we check that at last because it will be useful 
         *    less often than the other conditions (at least once a day compare to more than once a day for the other condition) 
         * The exit ban overpass all the other possible exit authorizations
        */
        string exit_reason =""; 

        for (int number_of_retries = 0; number_of_retries < MAXIMUN_NUMBER_OF_RETRIES_SQL_REQUEST; number_of_retries++)
        {/*catch Sql exeptions and retry the request until the request has succeed or we reach the maximun number of allowed retries*/
            using (SqlConnection connection_database = openDataBase())
            {
                try
                {
                    SqlDataReader sql_reader;
                    SqlCommand cmd;

                    //----------------------------------- check if agent not allowed the student to exit
                    cmd = new SqlCommand();
                    cmd.Connection = connection_database;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = //manual ban
                                      "SELECT B.DateDebut, B.DateFin, B.HeureDebut, B.HeureFin, Ag.Nom, Ag.Prenom, Ag.Travail, Ag.Id_agent "
                                    + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE AS B ON ((E.Id_eleve = B.Id_table AND B.Type = @type_student_table) "
                                    + "OR (E.Id_classe=B.Id_table AND B.Type = @type_division_table)) AND E.Id_eleve=@student_table_id AND B.Supprimer=@do_not_delete_value "
                                    + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
                                    + "JOIN TABLE_AGENTS AS Ag ON Ag.Id_agent = B.Id_agent ";

                    cmd.Parameters.AddWithValue("@type_student_table", Definition.TYPE_OF_STUDENT_TABLE);
                    cmd.Parameters.AddWithValue("@type_division_table", Definition.TYPE_OF_DIVISION_TABLE);
                    cmd.Parameters.AddWithValue("@student_table_id", student_table_id);
                    cmd.Parameters.AddWithValue("@do_not_delete_value", DO_NOT_DELETE_VALUE);

                    sql_reader = cmd.ExecuteReader();//execute the sql command
                    if ((sql_reader != null) && (sql_reader.HasRows))
                    {
                        while (sql_reader.Read())
                        {
                            if ((Tools.stringToDateTime(sql_reader.GetString(1)) >= date && Tools.stringToDateTime(sql_reader.GetString(0)) <= date)
                                && (Tools.stringToTimeSpan(sql_reader.GetString(3)) >= time && Tools.stringToTimeSpan(sql_reader.GetString(2)) <= time)
                               )
                            {
                                exit_reason = "Décision prise ";
                                try
                                {
                                    if ((sql_reader.GetInt32(7) == Definition.COMPUTER_TABLE_ID_DATABASE))
                                    {
                                        exit_reason += "sans intervention humaine";
                                    } else
                                    {
                                        exit_reason += "par " +Tools.getFormatedStringAgentInfo(sql_reader.GetString(4), sql_reader.GetString(5), sql_reader.GetString(6));
                                    }
                                }
                                catch
                                {
                                    exit_reason += "par un agent";
                                }


                                return Tuple.Create(Definition.EXIT_UNAUTHORIZED_VALUE, exit_reason); //if there is an exit ban we don't check other result
                            }

                        }
                    }//no exit ban


                    //----------------------------------- check if there is an exit ban for the current student

                    cmd = new SqlCommand();
                    cmd.Connection = connection_database;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = //manual ban
                                      "SELECT B.HeureDebut, B.HeureFin, B.Motif "
                                    + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE_TEMPORAIRE AS B ON ((E.Id_eleve = B.Id_table AND B.Type = @type_student_table) "
                                    + "OR (E.Id_classe=B.Id_table AND B.Type = @type_division_table)) AND E.Id_eleve=@student_table_id "
                                    + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe ";

                    cmd.Parameters.AddWithValue("@type_student_table", Definition.TYPE_OF_STUDENT_TABLE);
                    cmd.Parameters.AddWithValue("@type_division_table", Definition.TYPE_OF_DIVISION_TABLE);
                    cmd.Parameters.AddWithValue("@student_table_id", student_table_id);

                    sql_reader = cmd.ExecuteReader();//execute the sql command
                    if ((sql_reader != null) && (sql_reader.HasRows))
                    {
                        while (sql_reader.Read())
                        {
                            if(sql_reader.GetString(2)!=null)
                            {
                                exit_reason = sql_reader.GetString(2);
                            }
                            exit_reason += " (BD externe)";

                            if (Tools.stringToTimeSpan(sql_reader.GetString(1)) >= time && Tools.stringToTimeSpan(sql_reader.GetString(0)) <= time)
                            {
                                return Tuple.Create(Definition.EXIT_UNAUTHORIZED_VALUE, exit_reason); //if there is an exit ban we don't check other result
                            }

                        }
                    }//no exit ban



                    //----------------------------------- check the student schedule allows him to exit
                    cmd = new SqlCommand();
                    cmd.Connection = connection_database;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT HeureDebut, HeureFin, VerificationRequise, Motif FROM TABLE_SORTIE_TEMPORAIRE "
                                    + "WHERE Id_eleve=@student_table_id ";

                    cmd.Parameters.AddWithValue("@student_table_id", student_table_id);

                    sql_reader = cmd.ExecuteReader();//execute the sql command
                    if ((sql_reader != null) && (sql_reader.HasRows))
                    {
                        while (sql_reader.Read())
                        {
                            if (Tools.stringToTimeSpan(sql_reader.GetString(1)) >= time && Tools.stringToTimeSpan(sql_reader.GetString(0)) <= time)
                            {
                                if (sql_reader.GetString(3) != null)
                                {
                                    exit_reason = sql_reader.GetString(3) + " ";
                                }
                                exit_reason += "(BD externe)";

                                if (sql_reader.GetInt32(2) == Definition.NO_VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE)
                                {
                                    return Tuple.Create(Definition.EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE, exit_reason);
                                    //the student can leave the school
                                }
                                else
                                {
                                    return Tuple.Create(Definition.EXIT_AUTHORIZED_UNDER_CONDITION_VALUE, exit_reason);
                                    //the student can in theory leave the school, but an agent need to certify it
                                }
                            }
                        }
                    }

                    //----------------------------------- check if there is exit authorization for the current student
                    cmd = new SqlCommand();
                    cmd.Connection = connection_database;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT A.DateDebut, A.DateFin, A.Jour, A.HeureDebut, A.HeureFin , Ag.Nom, Ag.Prenom, Ag.Travail, Ag.Id_agent "
                                    + "FROM TABLE_ELEVES AS E JOIN TABLE_AUTORISATION_SORTIE AS A ON ((E.Id_eleve = A.Id_table AND A.Type = @type_student_table) "
                                    + "OR (E.Id_classe=A.Id_table AND A.Type = @type_division_table)) AND E.Id_eleve=@student_table_id AND A.Supprimer=@do_not_delete_value "
                                    + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
                                    + "JOIN TABLE_AGENTS AS Ag ON Ag.Id_agent=A.Id_agent ";

                    cmd.Parameters.AddWithValue("@type_student_table", Definition.TYPE_OF_STUDENT_TABLE);
                    cmd.Parameters.AddWithValue("@type_division_table", Definition.TYPE_OF_DIVISION_TABLE);
                    cmd.Parameters.AddWithValue("@student_table_id", student_table_id);
                    cmd.Parameters.AddWithValue("@do_not_delete_value", DO_NOT_DELETE_VALUE);

                    sql_reader = cmd.ExecuteReader();//execute the sql command
                    if ((sql_reader != null) && (sql_reader.HasRows))
                    {
                        while (sql_reader.Read())
                        {
                            if ((sql_reader.GetInt32(2) == day_of_week)
                                && (Tools.stringToDateTime(sql_reader.GetString(1)) >= date && Tools.stringToDateTime(sql_reader.GetString(0)) <= date)
                                && (Tools.stringToTimeSpan(sql_reader.GetString(4)) >= time && Tools.stringToTimeSpan(sql_reader.GetString(3)) <= time)
                               )
                            {
                                
                                exit_reason = "Décision prise ";
                                try
                                {
                                    if ((sql_reader.GetInt32(8) == Definition.COMPUTER_TABLE_ID_DATABASE))
                                    {
                                        exit_reason += "sans intervention humaine";
                                    }
                                    else
                                    {
                                        exit_reason += "par " + Tools.getFormatedStringAgentInfo(sql_reader.GetString(5), sql_reader.GetString(6), sql_reader.GetString(7));
                                    }
                                }
                                catch
                                {
                                    exit_reason += "par un agent";
                                }

                                return Tuple.Create(Definition.EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE, exit_reason);
                                //the student can leave the school
                            }
                        }
                    }


                    //----------------------------------- check if the student can leave the school for lunch
                    if (time >= Definition.LUNCH_TIME.startTime && time <= Definition.LUNCH_TIME.endTime)//it's lunch time
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = connection_database;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DemiPension FROM TABLE_ELEVES WHERE Id_eleve=@student_table_id AND Supprimer=@do_not_delete_value ";

                        cmd.Parameters.AddWithValue("@student_table_id", student_table_id);
                        cmd.Parameters.AddWithValue("@do_not_delete_value", DO_NOT_DELETE_VALUE);

                        sql_reader = cmd.ExecuteReader();//execute the sql command
                        if ((sql_reader != null) && (sql_reader.HasRows))
                        {
                            sql_reader.Read();
                            if (!sql_reader.IsDBNull(0))
                            {
                                int[] half_board_days = Tools.textToArray<int>(sql_reader.GetString(0));
                                if(!Tools.isHalBoardDay(half_board_days, day_of_week).Item2)//student eat at home today
                                {
                                    return Tuple.Create(Definition.EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE, "Pause déjeuner");//student eat at home today
                                }
                            }
                        }
                    }

                }//end try
                catch (SqlException e_sql)
                {
                    /*Retry the request if the exeption is retriable*/
                    if (canRetry(e_sql))
                    {
                        if (number_of_retries < MAXIMUN_NUMBER_OF_RETRIES_SQL_REQUEST)
                        {
                            continue;
                            //instructions below are not executed
                        }
                    }
                }
                catch
                {
                    return Tuple.Create(Definition.ERROR_INT_VALUE, "Erreur système");
                }

            }//end using

        }

        return Tuple.Create(Definition.EXIT_UNAUTHORIZED_VALUE, "Information sur l'E.D.T de l'élève introuvable");
        //the student can not leave the school
    }


    public StudentData getStudentByRFID(string rfid)
    {
        /*
         * From the student database id return all the retrieve information as a structure (from the given rfid tag "rfid") :
         *  Last name
         *  First Name
         *  Division
         *  Sex
         *  Hals board days
         *  
         * Else return a structure with default value
         */
        SqlConnection connection_database = openDataBase();

         
        StudentData student = new StudentData();
        student.toDefault();
        student.error = true;

        try
        {
            string request = "SELECT E.Id_eleve, E.Nom,E.Prenom,C.Classe,E.Sexe,E.DemiPension, E.Id_regime FROM TABLE_ELEVES AS E "
            + "JOIN TABLE_CLASSES AS C ON E.Id_classe=C.Id_classe AND E.Id_RFID=@rfid AND E.Supprimer=@do_not_delete_value";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@rfid", "@do_not_delete_value" }),
                new List<object>(new object[] { rfid, DO_NOT_DELETE_VALUE }));


            if (sql_reader != null)
            {
                if (sql_reader.HasRows)
                {
                    sql_reader.Read();
                    student.tableId = sql_reader.GetInt32(0);
                    student.lastName = sql_reader.GetString(1);
                    student.firstName = sql_reader.GetString(2);
                    student.division = sql_reader.GetString(3);
                    student.sex = sql_reader.GetInt32(4);
                    student.halfBoardDays = Tools.textToArray<int>(sql_reader.GetString(5));
                    student.labelRegime = getExitRegimeLabel(sql_reader, 6);
                    student.idRFID = rfid;
                }
                student.error = false;
            }

        }
        catch { }

        closeDataBase(connection_database);

        return student;
    }



    public int setStudentDeletedFromIdTable (int student_table_id)
    {
        /*
         * From the student id use in database, update the student by changing its delete value
         * In fact when user want to delete student he will be no longer avilable but still saved in database
         * for a certain periode
        */

        StudentData student = getStudentByIndex(student_table_id);

        string request = "UPDATE TABLE_ELEVES SET Supprimer=@delete_value, DateEdition=@modification_date WHERE Id_eleve=@student_table_id ";
        return executeNonQueryRequest(request, 
            new List<string>(new string[] { "@student_table_id", "@modification_date", "@delete_value"}), 
            new List<object>(new object[] { student_table_id, Tools.dateTimeToString(DateTime.Now), DELETE_VALUE }));

    }


    public int changeStudentRfid(int student_table_id, string rfid_id)
    {
        /*Allow to change the RFID for a student givent by its id in the database
         * We assume that the format oh the RFID is correct here
       */


        string request = "UPDATE TABLE_ELEVES SET Id_RFID=@rfid_id, DateEdition=@modification_date WHERE Id_eleve=@student_table_id AND Supprimer=@do_not_delete_value";
        int res = executeNonQueryRequest(request, 
            new List<string>(new string[] { "@student_table_id", "@rfid_id", "@modification_date", "@do_not_delete_value" }), 
            new List<object>(new object[] { student_table_id, rfid_id, Tools.dateTimeToString(DateTime.Now), DO_NOT_DELETE_VALUE }));


        if (res!=Definition.NO_ERROR_INT_VALUE)
        {
            return Definition.ERROR_INT_VALUE;
        }else
        {
            return Definition.NO_ERROR_INT_VALUE;
        }
    }


    public int setStopForStudent(int type_of_table, int row_index, int id_agent, string reason="")
    {
        /*Stop student at the exit of the school*/

        string request = "INSERT INTO TABLE_BLOCAGE (Type, Id_table, DateDebut, DateFin, HeureDebut, HeureFin, Motif, Id_agent, Supprimer, DateEdition) "
                       + "VALUES (@type, @row_index,@start_date,@end_date,@start_hour,@end_hour, @reason,@id_agent,@do_not_delete_value, @modification_date)";

        //get tomorrow date at 00:00:00
        DateTime now = DateTime.Now;

        string start_date = Tools.dateToStringFromDateTime(now);
        string end_date = start_date;

        string start_hour = Tools.timeToStringFromTimeSpan(TimeSpan.ParseExact("00:01:00", "hh\\:mm\\:ss", CultureInfo.InvariantCulture));
        string end_hour = Tools.timeToStringFromTimeSpan(TimeSpan.ParseExact("23:59:59", "hh\\:mm\\:ss", CultureInfo.InvariantCulture));//new DateTime(1, 1, 1) return datetime with time set to zero (midnight)


        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@type","@row_index","@start_date", "@end_date","@start_hour","@end_hour", "@reason", "@id_agent","@do_not_delete_value", "@modification_date" }),
            new List<object>(new object[] { type_of_table, row_index, start_date , end_date, start_hour, end_hour, reason,  id_agent, DO_NOT_DELETE_VALUE, Tools.dateTimeToString(now)  }));
    }


    public int setAuthorization(int type_of_table, int row_index, DateTime start_date, DateTime end_date, int day_of_week
                                            , TimeSpan start_hour, TimeSpan end_hour, int id_agent, string reason = "")
    {
        /*Add authorization to the database for a specific student given but its index*/

        string request = "INSERT INTO TABLE_AUTORISATION_SORTIE (Type, Id_table, DateDebut, DateFin, Jour, HeureDebut, HeureFin, Motif, Id_agent, Supprimer, DateEdition) "
                       + "VALUES (@type, @row_index,@start_date,@end_date, @day_of_week,@start_hour,@end_hour, @reason,@id_agent,@do_not_delete_value, @modification_date)";

        string t_start_date = Tools.dateToStringFromDateTime(start_date);
        string t_end_date = Tools.dateToStringFromDateTime(end_date);
        string t_start_hour = Tools.timeToStringFromTimeSpan(start_hour);
        string t_end_hour = Tools.timeToStringFromTimeSpan(end_hour);

        return executeNonQueryRequest(request,
            new List<string>(new string[] { "@type", "@row_index", "@start_date", "@end_date", "@day_of_week","@start_hour", "@end_hour", "@reason", "@id_agent", "@do_not_delete_value", "@modification_date" }),
            new List<object>(new object[] { type_of_table, row_index, t_start_date, t_end_date, day_of_week, t_start_hour, t_end_hour, reason, id_agent, DO_NOT_DELETE_VALUE, Tools.dateTimeToString(DateTime.Now) }));
    }


    public int removeAuthorization (int row_index, int agent_table_index)
    {
        /*Remove authorization of the database for a specific student given but its index*/

        SqlConnection connection_database = openDataBase();

        string request = "SELECT Type, Id_table FROM TABLE_AUTORISATION_SORTIE WHERE Id=@table_index";

        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@table_index" }),
            new List<object>(new object[] { row_index }));

        try
        {
            if ((sql_reader != null) && (sql_reader.HasRows))
            {
                sql_reader.Read();

                /*If the authorization is given to the whole division of that student, then we keep it inside the database*/
                string modification = "suppression de l'autorisation de sortie";
                int res = writeToRegiter(Definition.TYPE_OF_AUTHORIZATION_TABLE, row_index, agent_table_index, modification);

                if (res != Definition.NO_ERROR_INT_VALUE)
                {
                    return res;
                }

                //delete exit authorization from database
                request = "UPDATE TABLE_AUTORISATION_SORTIE SET Supprimer=@delete_value WHERE Id=@table_index";

                return executeNonQueryRequest(request,
                    new List<string>(new string[] { "@delete_value", "@table_index" }),
                    new List<object>(new object[] { DELETE_VALUE, row_index }));
            }

        }
        catch
        {
            connection_database.Close();
        }


        return Definition.ERROR_INT_VALUE;
    }




    public List<TimeSlot> getExitAuthorizations(int type_of_table, int row_index)
    {
        /*Return all the exit authorization about a student or a division*/
        if(type_of_table==Definition.TYPE_OF_STUDENT_TABLE)
        {
            return getExitAuthorizationsForStudent(row_index);
        }
        else if( type_of_table==Definition.TYPE_OF_DIVISION_TABLE)
        {
            return getExitAuthorizationsForDivision(row_index);
        }
        else
        {
            return new List<TimeSlot>();
        }

   }


    public List<TimeSlot> getExitAuthorizationsForStudent(int row_index)
    {
        /*Return all the exit authorization about a student given by its id*/
        List<TimeSlot> list_res = new List<TimeSlot>();

        SqlConnection connection_database = openDataBase();

        string request = "SELECT A.Id, A.Type, C.Classe, A.DateDebut, A.DateFin, A.Jour, A.HeureDebut, A.HeureFin,A.Motif, AG.Nom, AG.Prenom,AG.Travail, AG.Id_agent "
            + "FROM TABLE_ELEVES AS E JOIN TABLE_AUTORISATION_SORTIE AS A ON ((E.Id_eleve = A.Id_table AND A.Type = @type_student_table) "
            + "OR (E.Id_classe=A.Id_table AND A.Type = @type_division_table)) AND E.Id_eleve=@student_table_id AND A.Supprimer=@do_not_delete_value "
            + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
            + "JOIN TABLE_AGENTS AS AG ON A.Id_agent=AG.Id_agent ";

        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@student_table_id", "@type_student_table", "@type_division_table", "@do_not_delete_value" }),
            new List<object>(new object[] { row_index, Definition.TYPE_OF_STUDENT_TABLE, Definition.TYPE_OF_DIVISION_TABLE, DO_NOT_DELETE_VALUE }));


        try
        {
            if (sql_reader != null)
            {
                TimeSlot timeSlot = new TimeSlot();
                DateTime end_date;
                TimeSpan end_time;

                while (sql_reader.Read())
                {
                    end_date = Tools.stringToDateTime(sql_reader.GetString(4));
                    end_time = Tools.stringToTimeSpan(sql_reader.GetString(7));

                    /*To display an exit authorization, we need to check if it is still effective
                     * In other word we need to check if the last date and time of the exit authorization is smaller than
                     * the current date and day. 
                     * For example :
                     * | Sunday | Monday | Tuesday | Wednesday | Thursday | friday | saturday |
                     * ^                                                                       ^
                     * |                       ^                    ^                          |                
                     * start date              |                    |
                     * of the exit          current day          day of the week            end of the exit 
                     * authorization                            when authorization          authorization
                     *                                          is effective
                     * 
                    */

                    if (DateTime.Now <= end_date.Add(end_time)) //check if the exit authorization is still effective
                    { 
                        timeSlot.toDefault();

                        timeSlot.id = sql_reader.GetInt32(0);
                        timeSlot.startDate = Tools.stringToDateTime(sql_reader.GetString(3));
                        timeSlot.endDate = end_date;
                        timeSlot.dayOfWeek = sql_reader.GetInt32(5);
                        timeSlot.startTime = Tools.stringToTimeSpan(sql_reader.GetString(6));
                        timeSlot.endTime = end_time;

                        string info = "IDENTIFIANT : " + sql_reader.GetInt32(0);
                        info += "\nEffectif pour : ";
                        if (sql_reader.GetInt32(1) == Definition.TYPE_OF_STUDENT_TABLE)
                        {
                            info += "cet élève uniquement";
                            timeSlot.readOnly = false;
                        }
                        else if (sql_reader.GetInt32(1) == Definition.TYPE_OF_DIVISION_TABLE)
                        {
                            info += "classe de cet élève - " + sql_reader.GetString(2);
                            timeSlot.readOnly = true;
                        }
                        /* User can only modifiy data that correspond to the student :
                         * if the actual authorization is given to the whole division of that student, user can not edit that authorization
                         * until he he considers only the authorization from the division
                         * In other words, if the user want all the authorization of the student he can only edit informations owned by that student
                         * If the user want to modify authorization of a division he can only edit the informations "owned" by that division
                        */
                        info += "\nMotif : " + sql_reader.GetString(8);
                        info += "\n\nRESPONSABLE : ";
                        if (sql_reader.GetInt32(12) == Definition.COMPUTER_TABLE_ID_DATABASE)
                        {
                            info += "Action effectuée automatiquement";
                        }
                        else
                        {
                            info += Tools.getFormatedStringForSessionInfo(sql_reader.GetString(9), sql_reader.GetString(10), sql_reader.GetString(11));
                        }

                        timeSlot.metaText = (info == null) ? "" : info;

                        list_res.Add(timeSlot);
                    }
                }
            }
        }catch
        {
            connection_database.Close();
        }

        return list_res;

    }




    public List<TimeSlot> getExitAuthorizationsForDivision(int row_index)
    {
        /*Return all the exit authorization about a division given by its id*/
                    List<TimeSlot> list_res = new List<TimeSlot>();

        SqlConnection connection_database = openDataBase();

        string request = "SELECT Id, A.DateDebut, A.DateFin, A.Jour,A. HeureDebut, A.HeureFin, A.Motif,  AG.Nom, AG.Prenom, AG.Travail, AG.Id_agent  "
                       + "FROM TABLE_AUTORISATION_SORTIE AS A "
                       + "JOIN TABLE_AGENTS AS AG ON AG.Id_agent=A.Id_agent AND A.Type=@type_of_table AND A.Id_table=@row_index AND A.Supprimer=@do_not_delete_value ";

        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@type_of_table", "@row_index", "@do_not_delete_value" }),
            new List<object>(new object[] { Definition.TYPE_OF_DIVISION_TABLE, row_index, DO_NOT_DELETE_VALUE }));

        try
        {
            if (sql_reader != null)
            {
                TimeSlot timeSlot = new TimeSlot();
                DateTime end_date;
                TimeSpan end_time;

                while (sql_reader.Read())
                {
                    end_date = Tools.stringToDateTime(sql_reader.GetString(2));
                    end_time = Tools.stringToTimeSpan(sql_reader.GetString(5));


                    if (DateTime.Now <= end_date.Add(end_time)) //check if the exit authorization is still effective
                    {
                        timeSlot.toDefault();

                        timeSlot.id = sql_reader.GetInt32(0);
                        timeSlot.startDate = Tools.stringToDateTime(sql_reader.GetString(1));
                        timeSlot.endDate = end_date;
                        timeSlot.dayOfWeek = sql_reader.GetInt32(3);
                        timeSlot.startTime = Tools.stringToTimeSpan(sql_reader.GetString(4));
                        timeSlot.endTime = end_time;
                        timeSlot.readOnly = false;

                        string info = "IDENTIFIANT : " + sql_reader.GetInt32(0);
                        info += "\nEffectif pour : classe entière";
                        info += "\nMotif : " + sql_reader.GetString(6);
                        info += "\n\nRESPONSABLE : ";
                        if (sql_reader.GetInt32(10) == Definition.COMPUTER_TABLE_ID_DATABASE)
                        {
                            info += "Action effectuée automatiquement";
                        }
                        else
                        {
                            info += Tools.getFormatedStringForSessionInfo(sql_reader.GetString(7), sql_reader.GetString(8), sql_reader.GetString(9));
                        }

                        timeSlot.metaText = (info==null)?"":info;


                        list_res.Add(timeSlot);
                    }
                }

            }
        }
        catch
        {
            connection_database.Close();
        }

        return list_res;

    }





    public int updateStudentModificationTime(int student_table_index)
    {
        /* When user change something that are linked to the student but not into the database (like the student photo)
         * we update the last modification date for that student into the database
        */
        string request = "UPDATE TABLE_ELEVES SET DateEdition=@modification_date WHERE Id_eleve=@student_table_id AND Supprimer=@do_not_delete_value";
        int res = executeNonQueryRequest(request,
            new List<string>(new string[] { "@student_table_id", "@modification_date", "@do_not_delete_value" }),
            new List<object>(new object[] { student_table_index, Tools.dateTimeToString(DateTime.Now), DO_NOT_DELETE_VALUE }));

        if (res != Definition.NO_ERROR_INT_VALUE)
        {
            return Definition.ERROR_INT_VALUE;
        }
        else
        {
            return Definition.NO_ERROR_INT_VALUE;
        }

    }


    public int writeToRegiter(int type, int row_index, int agent_table_id, string modification)
    {
        /*Add new agent into the database*/
        string request = "INSERT INTO TABLE_REGISTRE (Type, Id_table, Modification, DateEdition, Id_agent) "
            + "VALUES (@type_of_table,@index_row,@modification,@modification_date,@agent_table_id)";


        int res = executeNonQueryRequest(request,
            new List<string>(new string[] { "@type_of_table","@index_row", "@modification", "@modification_date", "@agent_table_id"}),
            new List<object>(new object[] { type, row_index, modification, Tools.dateTimeToString(DateTime.Now), agent_table_id }));

        return res;

    }


    private string getModificationText(string type, string date, string agent_info, string other_info, string current_state="")
    {
        /*Return a formated string from the given parameters*/

        const string new_entry = "\n\n--------------------   NOUVELLE ENTREE :   --------------------";
        const string header = "TYPE : ";
        const string state = "ÉTAT : ";
        const string person_in_charge = "RESPONSABLE : ";
        const string edition_date = "DATE D'ÉDITION : ";

        string res = "";
        res += new_entry;
        res += "\n" + header + type;
        res += "\n" + state + current_state;
        res += "\n" + edition_date + date;
        res += "\n" + person_in_charge + agent_info;
        res += "\n";
        res += "\n" + other_info;

        return res;

    }


    public string retrieveAllInformationsByIndex(int type_of_table, int row_index)
    {
        SqlConnection connection_database = openDataBase();
        string request = null;
        SqlDataReader sql_reader=null;

        List<object> authorization_list = new List<object>();
        List<object> ban_list = new List<object>();
        List<object> modification_profil_list = new List<object>();
        List<object> regular_exit_list = new List<object>();

        /*-----------------------------------------------------------------------------------------------
         * EXIT AUTHORIZATION
         * -----------------------------------------------------------------------------------------------
        */
        if (type_of_table == Definition.TYPE_OF_STUDENT_TABLE)
        {
            request = "SELECT A.Id, A.Type, C.Classe, A.DateDebut, A.DateFin, A.Jour, A.HeureDebut, A.HeureFin,A.Motif, A.Supprimer, A.DateEdition, A.Id_agent, AG.Nom, AG.Prenom,AG.Travail "
                + "FROM TABLE_ELEVES AS E JOIN TABLE_AUTORISATION_SORTIE AS A ON ((E.Id_eleve = A.Id_table AND A.Type = @type_of_student_table) "
                + "OR (E.Id_classe=A.Id_table AND A.Type = @type_division_table)) AND E.Id_eleve=@student_table_id "
                + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
                + "JOIN TABLE_AGENTS AS AG ON A.Id_agent=AG.Id_agent ";


            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@student_table_id", "@type_of_student_table", "@type_division_table"}),
                new List<object>(new object[] { row_index, Definition.TYPE_OF_STUDENT_TABLE, Definition.TYPE_OF_DIVISION_TABLE}));
        }
        else if(type_of_table == Definition.TYPE_OF_DIVISION_TABLE)
        {
            request = "SELECT DISTINCT A.Id, A.Type, C.Classe, A.DateDebut, A.DateFin, A.Jour, A.HeureDebut, A.HeureFin,A.Motif, A.Supprimer,A.DateEdition, A.Id_agent, AG.Nom, AG.Prenom,AG.Travail "
               + "FROM TABLE_ELEVES AS E JOIN TABLE_AUTORISATION_SORTIE AS A ON (A.Type = @type_division_table AND A.Id_table=@division_table_id) "
               + "JOIN TABLE_CLASSES AS C ON C.Id_classe=A.Id_table "
               + "JOIN TABLE_AGENTS AS AG ON A.Id_agent=AG.Id_agent ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@division_table_id", "@type_division_table" }),
                new List<object>(new object[] { row_index, Definition.TYPE_OF_DIVISION_TABLE}));
        }

        if (sql_reader != null)
        {
            int day;
            DateTime date;
            DateTime end_d;
            TimeSpan end_t;
            List<object> records;
            SqlDataReader temp_sql_reader;
            bool effective;
            string agent;
            string reason;

            while (sql_reader.Read())
            {
                records = new List<object>();

                /* Each exit authorization is define by a start date, end date, start time, end time, day of the week
                 * And each authorization is effective only on the timeslot built by the start and end time at the day of the week
                 * from the start date to the end date
                 *  
                 * For example :
                 * | Sunday | Monday | Tuesday | Wednesday | Thursday | friday | saturday |
                 * ^                                                                       ^
                 * |                       ^                    ^                          |                
                 * start date              |                    |
                 * of the exit          current day          day of the week            end of the exit 
                 * authorization                            when authorization          authorization
                 *                                          is effective
                 * 
                 * In our case, we want to sort all the authorization. So, we need to sort them according to their end date
                 * and to the day of the week when the authorization is effective. For example is we have two authorization with the same 
                 * start and end date but one takes place on monday and the other on friday, we want to see the one on monday before the one on friday 
                 * (if the authorizations are sorted in descending order)
                */

                end_d = Tools.stringToDateTime(sql_reader.GetString(4));
                day = sql_reader.GetInt32(5);
                end_t = Tools.stringToTimeSpan(sql_reader.GetString(7));
                /*if the substraction between the day when the authorization is effective and the day of the week of the end date of the authorization
                 * is negative, then we need to substract the result from the end date, else we need to add it, in order to reach the correct date when the 
                 * authorization finish
                */
                date = end_d.AddDays(day - (int)end_d.DayOfWeek).Add(end_t);

                effective = false;
                if (DateTime.Now <= date)
                {
                    effective = true && (sql_reader.GetInt32(9) == Definition.DO_NOT_DELETE_VALUE_DATABASE);
                }
                
                if (sql_reader.GetInt32(11) == Definition.COMPUTER_TABLE_ID_DATABASE)
                {
                    agent = "Action effectuée automatiquement";
                }
                else
                {
                    agent = Tools.getFormatedStringForSessionInfo(sql_reader.GetString(12), sql_reader.GetString(13), sql_reader.GetString(14));
                }

                reason = (sql_reader.GetString(8) == null) ? "" : sql_reader.GetString(8);
                //create element
                var v = new
                {
                    Date = Tools.ConvertToTimestamp(date),
                    Id = sql_reader.GetInt32(0),
                    State = sql_reader.GetInt32(9),
                    Effective = effective,
                    Scope = sql_reader.GetInt32(1),
                    Division = sql_reader.GetString(2),
                    Start_date = sql_reader.GetString(3),
                    End_date = sql_reader.GetString(4),
                    Start_time = sql_reader.GetString(6),
                    End_time = sql_reader.GetString(7),
                    Day_of_the_week = Tools.getStringFromIntegerDay(sql_reader.GetInt32(5)),
                    In_charge = agent,
                    Reason = reason,
                    Records = records

                };

                //retrieve all modification made on that exit authorization
                request = "SELECT R.DateEdition, R.Modification, AG.Nom, AG.Prenom, AG.Travail, AG.Id_agent "
                        + "FROM TABLE_REGISTRE AS R JOIN TABLE_AGENTS AS AG ON R.Id_agent=AG.Id_agent "
                        + "AND R.Type=@type AND R.Id_table=@table_id ";

                temp_sql_reader = executeReaderRequest(connection_database, request,
                                    new List<string>(new string[] { "@type", "@table_id" }),
                                    new List<object>(new object[] { Definition.TYPE_OF_AUTHORIZATION_TABLE, sql_reader.GetInt32(0) }));

                while ((temp_sql_reader != null) && (temp_sql_reader.Read()))
                {
                    if (sql_reader.GetInt32(5) == Definition.COMPUTER_TABLE_ID_DATABASE)
                    {
                        agent = "Action effectuée automatiquement";
                    }
                    else
                    {
                        agent = Tools.getFormatedStringForSessionInfo(temp_sql_reader.GetString(2), temp_sql_reader.GetString(3), temp_sql_reader.GetString(4));
                    }

                    records.Add(new {
                                        Date = Tools.ConvertToTimestamp(Tools.stringToDateTime(temp_sql_reader.GetString(0))),
                                        Modification_date = temp_sql_reader.GetString(0),
                                        Modification = temp_sql_reader.GetString(1),
                                        In_charge = agent
                                });
                }

                //add element to the list to return
                authorization_list.Add(v);
            }
        }

        /*-----------------------------------------------------------------------------------------------
         * EXIT BAN
         * -----------------------------------------------------------------------------------------------
        */

        /*---------------------------------- manual ban ----------------------------------*/
        sql_reader = null;
        if (type_of_table == Definition.TYPE_OF_STUDENT_TABLE)
        {
            //manual ban
            request = "SELECT B.Type, C.Classe, B.DateDebut, B.DateFin, B.HeureDebut, B.HeureFin, B.Supprimer, B.DateEdition, B.Id_agent, B.Motif, AG.Nom, AG.Prenom,AG.Travail "
            + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE AS B ON ((E.Id_eleve = B.Id_table AND B.Type = @type_student_table) "
            + "OR (E.Id_classe=B.Id_table AND B.Type = @type_division_table)) AND E.Id_eleve=@student_table_id "
            + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
            + "JOIN TABLE_AGENTS AS AG ON B.Id_agent=AG.Id_agent ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@student_table_id", "@type_student_table", "@type_division_table" }),
                new List<object>(new object[] { row_index, Definition.TYPE_OF_STUDENT_TABLE, Definition.TYPE_OF_DIVISION_TABLE }));

        } else if (type_of_table == Definition.TYPE_OF_DIVISION_TABLE)
        {
            //manual ban
            request = "SELECT DISTINCT B.Type, C.Classe, B.DateDebut, B.DateFin, B.HeureDebut, B.HeureFin, B.Supprimer,B.DateEdition,B.Id_agent,B.Motif, AG.Nom, AG.Prenom,AG.Travail "
            + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE AS B ON (B.Type = @type_division_table AND B.Id_table=@division_table_id) "
            + "JOIN TABLE_CLASSES AS C ON C.Id_classe=B.Id_table "
            + "JOIN TABLE_AGENTS AS AG ON B.Id_agent=AG.Id_agent ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@division_table_id", "@type_division_table" }),
                new List<object>(new object[] { row_index,  Definition.TYPE_OF_DIVISION_TABLE }));
        }


        if (sql_reader != null)
        {
            DateTime date;
            bool effective;
            string agent;
            string reason;

            while (sql_reader.Read())
            {
                date = Tools.stringToDateTime(sql_reader.GetString(3));
                effective = false;

                if (DateTime.Now <= date.Add(Tools.stringToTimeSpan(sql_reader.GetString(5))))
                {
                    effective = true && (sql_reader.GetInt32(6)==Definition.DO_NOT_DELETE_VALUE_DATABASE);
                }


                if (sql_reader.GetInt32(8)==Definition.COMPUTER_TABLE_ID_DATABASE)
                {
                    agent = "Action effectuée automatiquement";
                }else
                {
                    agent = Tools.getFormatedStringForSessionInfo(sql_reader.GetString(10), sql_reader.GetString(11), sql_reader.GetString(12));
                }

                reason = (sql_reader.GetString(9) == null) ? "" : sql_reader.GetString(9);

                //create element
                var v = new
                {
                    Date = Tools.ConvertToTimestamp(date),
                    State = sql_reader.GetInt32(6),
                    Effective = effective,
                    Scope = sql_reader.GetInt32(0),
                    Division = sql_reader.GetString(1),
                    Start_date = sql_reader.GetString(2),
                    End_date = sql_reader.GetString(3),
                    Start_time = sql_reader.GetString(4),
                    End_time = sql_reader.GetString(5),
                    In_charge = agent,
                    Reason = reason,
                };

                ban_list.Add(v);
            }
        }


                        /*---------------------------------- temporary ban ----------------------------------*/
        sql_reader = null;
        if (type_of_table == Definition.TYPE_OF_STUDENT_TABLE)
        {
            //manual ban
            request = "SELECT B.Type, C.Classe, B.HeureDebut, B.HeureFin, B.Id_agent, B.Motif, AG.Nom, AG.Prenom,AG.Travail "
            + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE_TEMPORAIRE AS B ON ((E.Id_eleve = B.Id_table AND B.Type = @type_student_table) "
            + "OR (E.Id_classe=B.Id_table AND B.Type = @type_division_table)) AND E.Id_eleve=@student_table_id "
            + "JOIN TABLE_CLASSES AS C ON C.Id_classe=E.Id_classe "
            + "JOIN TABLE_AGENTS AS AG ON B.Id_agent=AG.Id_agent ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@student_table_id", "@type_student_table", "@type_division_table" }),
                new List<object>(new object[] { row_index, Definition.TYPE_OF_STUDENT_TABLE, Definition.TYPE_OF_DIVISION_TABLE }));

        }
        else if (type_of_table == Definition.TYPE_OF_DIVISION_TABLE)
        {
            //manual ban
            request = "SELECT DISTINCT B.Type, C.Classe, B.HeureDebut, B.HeureFin, B.Id_agent, B.Motif, AG.Nom, AG.Prenom,AG.Travail "
            + "FROM TABLE_ELEVES AS E JOIN TABLE_BLOCAGE_TEMPORAIRE AS B ON (B.Type = @type_division_table AND B.Id_table=@division_table_id) "
            + "JOIN TABLE_CLASSES AS C ON C.Id_classe=B.Id_table "
            + "JOIN TABLE_AGENTS AS AG ON B.Id_agent=AG.Id_agent ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@division_table_id", "@type_division_table" }),
                new List<object>(new object[] { row_index, Definition.TYPE_OF_DIVISION_TABLE }));
        }

        if (sql_reader != null)
        {
            DateTime date;
            string agent;
            string reason;

            while (sql_reader.Read())
            {
                date = DateTime.Now;

                if (sql_reader.GetInt32(4) == Definition.COMPUTER_TABLE_ID_DATABASE)
                {
                    agent = "Action effectuée automatiquement";
                }
                else
                {
                    agent = Tools.getFormatedStringForSessionInfo(sql_reader.GetString(6), sql_reader.GetString(7), sql_reader.GetString(8));
                }

                reason = (sql_reader.GetString(5) == null) ? "" : sql_reader.GetString(5);
                //create element
                var v = new
                {
                    Date = Tools.ConvertToTimestamp(date),
                    State = Definition.DO_NOT_DELETE_VALUE_DATABASE,
                    Effective = true,
                    Scope = sql_reader.GetInt32(0),
                    Division = sql_reader.GetString(1),
                    Start_date = Tools.dateToStringFromDateTime(date),
                    End_date = Tools.dateToStringFromDateTime(date),
                    Start_time = sql_reader.GetString(2),
                    End_time = sql_reader.GetString(3),
                    In_charge = agent,
                    Reason = reason,
                };

                ban_list.Add(v);
            }
        }

        /*-----------------------------------------------------------------------------------------------
         * PROFIL MODIFICATION
         * -----------------------------------------------------------------------------------------------
        */

        request = "SELECT R.Modification, R.DateEdition, AG.Nom, AG.Prenom, AG.Travail, AG.Id_agent "
            + "FROM TABLE_REGISTRE AS R "
            + "JOIN TABLE_AGENTS AS AG ON R.Id_agent=AG.Id_agent AND R.Type=@type_of_table AND R.Id_table=@row_index";

        sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@type_of_table", "@row_index" }),
            new List<object>(new object[] { type_of_table, row_index }));


        if (sql_reader != null)
        {
            DateTime date;
            string agent;
            string reason;

            while (sql_reader.Read())
            {
                date = Tools.stringToDateTime(sql_reader.GetString(1));

                if (sql_reader.GetInt32(5) == Definition.COMPUTER_TABLE_ID_DATABASE)
                {
                    agent = "Action effectuée automatiquement";
                }
                else
                {
                    agent = Tools.getFormatedStringForSessionInfo(sql_reader.GetString(2), sql_reader.GetString(3), sql_reader.GetString(4));
                }

                reason = (sql_reader.GetString(0) == null) ? "" : sql_reader.GetString(0);

                //create element
                var v = new
                {
                    Date = Tools.ConvertToTimestamp(date),
                    Modification_date = sql_reader.GetString(1),
                    In_charge = agent,
                    Reason = reason,
                };

                modification_profil_list.Add(v);
            }
        }


        /*-----------------------------------------------------------------------------------------------
         * DAILY EXIT
         * -----------------------------------------------------------------------------------------------
        */
        if (type_of_table == Definition.TYPE_OF_STUDENT_TABLE)
        {
            request = "SELECT HeureDebut, HeureFin, VerificationRequise, Motif "
                + "FROM TABLE_SORTIE_TEMPORAIRE WHERE Id_eleve=@row_index ";

            sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@row_index" }),
                new List<object>(new object[] { row_index }));


            if (sql_reader != null)
            {
                DateTime date;
                bool verification;
                string reason;

                while (sql_reader.Read())
                {
                    date = DateTime.Now.Add(Tools.stringToTimeSpan(sql_reader.GetString(1)));
                    reason = (sql_reader.GetString(3) == null) ? "" : sql_reader.GetString(3);

                    verification = false;
                    if (sql_reader.GetInt32(2)!=Definition.EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE)
                    {
                        verification = true;
                    }
                    //create element
                    var v = new
                    {
                        Date = Tools.ConvertToTimestamp(date),
                        Start_time = sql_reader.GetString(0),
                        End_time = sql_reader.GetString(1),
                        Verification = verification,
                        Reason = reason
                    };

                    regular_exit_list.Add(v);
                }
            }
        }

        closeDataBase(connection_database);

        return JsonConvert.SerializeObject(new { Authorization = authorization_list, Ban = ban_list, Profil = modification_profil_list, Regular = regular_exit_list });
            
   }


    public int FusionStudentsFromIds(List<int> list_id)
    {
        /*A part of the database that contains all student informations ("TABLE_ELEVES") is populated by a soruce file
         * which is nammed here student state file. To the grabbed data we add a unique RFID id. When the source file 
         * is updated we just update a part of the existant informations in the database, depends on which informations change.
         * To update information we identify each student in the file by their last name, first name, division and sex.
         * If we found a correspondace between the student in the file and in the database, we update all other informations that
         * are not the authentifications data (last name, first name, division, sex). 
         * Thus, if one authentification information changes in the source file we could not find any correspondance in the database
         * In this case, we create a new student in the database that correspond to the new informations. But if a student just change his 
         * division or there was a mistake in the previous version of the source file and the sex of a student does not matches with the reality
         * we would like to transfer all the data (expect authentification data) from the last row of the database that represent thta student 
         * and the new one.
         * For example if Adam Joe in 4A (M) becomes Adam Joe in 4B (M) we want to transfer all the corresponding data (punitions, rfid code, photo)
         * of Adam Joe in 4A (M) to Adam Joe in 4B (M) in the database. Because inf fact when the new source file (that contains Adam Joe in 4B (M))
         * was read we found that there is no updated information about Adam Joe in 4A (M) (because there is no more Adam Joe in 4A (M) 
         * in the source file) and we found a new student to add to the database. 
         * In other words, in the database there is two row : one for Adam Joe in 4A (M) and one for Adam Joe in 4B (M)
         * 
         * This function allow from a list of student id (form the database table "TABLE_ELEVES") to transfer all non authentification informations
         * from all the selected student to the latest version of that stduent. In fact user will select all the students he want to gather
         * and the function will organsize them from the student that have the oldest modification date from the one who have the newest 
         * (in other words the list is organized by their modifications date in an descendant order). When we found the latest modified student, 
         * named Student 0, we check all the null or empty informations that need to be update (here only the photo and the RFID id). 
         * Then we check all the student that have non null information following the order. If we found one student that have the 
         * corresponding information we update the Student 0 with that information. We repeat the operation for all null or empty informations. 
         * Then we delete all the other students (exept the Student 0) from the database and remove all the corresponding photos from folder
         * And we send back the id (from database) of the Student 0 (the one who have gather all informations from oldest versions)
        */

                if (list_id.Count < 2)//need at least 2 students to gather informations into one
        {
            return Definition.ERROR_INT_VALUE;
        }

        SqlConnection connection_database = openDataBase();

        try
        {
            //First gather all informations about the given student (by the list "list_id" that contains the student if from database)
            List<string> parms = new List<string>();
            List<object> values = new List<object>();

            //Since we have multiple id to found we use the SQL statement IN (value1,value2)...
            //SELECT informations FROM table WHERE id IN (@id_0, @id_1,... @id_n) if we have n student id in the list "lits_id"
            string condition_request = " IN (";
            string id = "@id_";


            for (int i = 0; i < list_id.Count - 1; i++)
            {
                condition_request += (id + i) + ","; //"id_i,"

                parms.Add(id + i);//add the string "@id_0/1/2/.../n" as parameters for the SQL request
                values.Add(list_id[i]);//add the corresponding value 
            }

            condition_request += id + (list_id.Count - 1) + ")";//"id_n )" and not "id_n,)"
            parms.Add(id + (list_id.Count - 1));
            values.Add(list_id[list_id.Count - 1]);


            string request = "SELECT E.Id_eleve, E.Nom, E.Prenom,C.Classe, E.Sexe, E.Id_RFID, E.DateEdition FROM TABLE_ELEVES AS E "
                + "JOIN TABLE_CLASSES AS C ON E.Id_classe=C.Id_classe AND E.Id_eleve " + condition_request;
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request, parms, values);

            if (sql_reader != null)
            {
                List<DateTime> list_dates = new List<DateTime>();//list that will contains all the modifications date of the each student
                List<StudentData> list_students = new List<StudentData>();//list of the corresponding student informations as structure

                //start reading the retrieved information
                while (sql_reader.Read())
                {
                    StudentData student = new StudentData();
                    student.toDefault();

                    student.tableId = sql_reader.GetInt32(0);
                    student.lastName = sql_reader.GetString(1);
                    student.firstName = sql_reader.GetString(2);
                    student.division = sql_reader.GetString(3);
                    student.sex = sql_reader.GetInt32(4);
                    student.idRFID = sql_reader.GetString(5);

                    DateTime date = Tools.stringToDateTime(sql_reader.GetString(6));

                    int index = getIndexElementToAddInOrderList(ref list_dates, date);//get teh index where to add the new element "date"
                                                                                      /*We use index to order the "list_dates" and set the student infromations in the list "list_students" at the same 
                                                                                       * position of the corresponding modification date in the list list_date
                                                                                       * For example if list_dates=[date1, date3, date2] then list_students=[student1,student3,student2]
                                                                                       * The list list_student "list_students" follows the order of the list "list_dates"
                                                                                     */
                    if (index == list_dates.Count)//add at the end
                    {
                        list_dates.Add(date);
                        list_students.Add(student);
                    }
                    else//insert into the list
                    {
                        list_dates.Insert(index, date);
                        list_students.Insert(index, student);
                    }

                }//at this step student 0 is at the index 0 of the list "list_students" 
                 //because at the same index in the list "list_dates" there is the newest modifications date of a student information
                closeDataBase(connection_database);


                /* Browse all information about student and copy them to the Student 0
                 * If a student (who is not the Student 0) have a special exit authorization, then we don't copy it to the Student 0
                 * (in other words, the Student 0 will not have that exit authorization) bacause when agent decide to let student leave from school
                 * they authorize that specific student at that time (and the app save their identity).
                 * In other words, gather all authorization implies to much responsabilities problem
                */
                bool rfid_found = false;
                bool photo_found = false;

                string modification_content_for_final_student = ""; //for the Student 0
                string info_final_student = list_students[0].getFormatedStudentInfo();

                for (int k = 0; k < list_students.Count; k++)
                {
                    /* We keep the fist photo we found and then delete the photo of all other student
                     * Then delete all other student
                    */

                    //get new RFID id from other student info
                    if ((!rfid_found) && (!string.IsNullOrEmpty(list_students[k].idRFID)))
                    {
                        /*Since all the used structures to saved student informations are saved in a list
                         * we cannot modify the information of a specific structure from the list
                         * So we creat one with the same informations and modify the found information
                         * and then replace the structure in the list by the new one
                        */

                        rfid_found = true;

                        //upadate the found information in the row of the Student 0
                        request = "UPDATE TABLE_ELEVES SET Id_RFID=@id_rfid, DateEdition=@modification_date WHERE Id_eleve=@id_student ";

                        int res = executeNonQueryRequest(request,
                            new List<string>(new string[] { "@id_student", "@id_rfid", "modification_date" }),
                            new List<object>(new object[] { list_students[0].tableId, list_students[k].idRFID, Tools.dateTimeToString(DateTime.Now) }));

                        if (res != Definition.NO_ERROR_INT_VALUE)
                        {
                            return res;
                        }

                    }

                    //get new photo
                    string photo_path = Tools.getStudentPhotoPath(list_students[k].lastName, list_students[k].firstName, list_students[k].division);
                    if ((!photo_found) && (photo_path != ""))//we don't have find a photo yet
                    {
                        photo_found = true; //find one (and don't delete it for the moment)

                        //copie the image with a new name
                        string path_to_new_photo = Definition.PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS
                        + Tools.getStudentPhotoNameWithoutExtension(list_students[0].lastName, list_students[0].firstName, list_students[0].division)
                        + Path.GetExtension(photo_path);
                        //There is no made function to rename file so we have to copy the file with the desired name, and after delete the old file
                        if (path_to_new_photo != photo_path)
                        {
                            if (Tools.copyFile(photo_path, path_to_new_photo) != Definition.NO_ERROR_INT_VALUE)//copy file with new name
                            {
                                return Definition.ERROR_INT_VALUE;
                            }
                        }/* else means that the curret student (list_students[0]) has already a photo (so the name of the photo is already 
                  *  correct and there is no need to rename the file)
                 */
                    }


                    //save that modification for each student
                    if (k != 0)
                    {
                        modification_content_for_final_student += "Fusion de l'élève : " + list_students[k].getFormatedStudentInfo();
                        modification_content_for_final_student += "\n";


                        string modification = "";
                        modification += "fusion de cet l'élève avec l'élève : " + info_final_student;

                        if (writeToRegiter(Definition.TYPE_OF_STUDENT_TABLE, list_students[k].tableId, SecurityManager.getConnectedAgentTableIndex(), modification) != Definition.NO_ERROR_INT_VALUE)
                        {
                            return Definition.ERROR_INT_VALUE;
                        }
                    }//else add another modification content


                    //delete student from database (exept Student 0)
                    if ( (k!=0) && (setStudentDeletedFromIdTable(list_students[k].tableId) != Definition.NO_ERROR_INT_VALUE) )
                    {
                        return Definition.ERROR_INT_VALUE;
                    }



                }

                //save modification content for the Student 0
                modification_content_for_final_student += "En : " + info_final_student;
                if (writeToRegiter(Definition.TYPE_OF_STUDENT_TABLE, list_students[0].tableId, SecurityManager.getConnectedAgentTableIndex(), modification_content_for_final_student)
                        != Definition.NO_ERROR_INT_VALUE)
                {
                    return Definition.ERROR_INT_VALUE;
                }




                return list_students[0].tableId; //no error and return the id from database of Student 0

            }


        }catch
        {
            return Definition.ERROR_INT_VALUE;
        }
        finally
        {
            closeDataBase(connection_database);
        }

        return Definition.ERROR_INT_VALUE;

    }


    private int getIndexElementToAddInOrderList(ref List<DateTime> list, DateTime element)
    {
        /*return the index of the orderly decreasingly list "list" where to put the element "element"*/
        int i = 0;
        while ((i < list.Count) && (list[i] > element))
        {
            i++;
        }

        return i;
    }

   

    public int isShortSecureIdAgentFree(SecureString short_secure_id)
    {
        /*Since each agent have a short id (which is like a password) to create a new one we have to check in the database
         * if the givent short id "short_secure_id" as an encrypted string is already associate with an agent
         * The function return 1 if the given short id is already taken
         * else return Definition.NO_ERROR_INT_VALUE if the given short id is free
         * else return Definition.ERROR_INT_VALUE if an error occurred
        */

        SqlConnection connection_database = openDataBase();

        try
        {
            string request = "SELECT IdentifiantCourtSecurise FROM TABLE_AGENTS";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request);

            if (sql_reader != null)
            {

                while (sql_reader.Read())
                {
                    if ( (!sql_reader.IsDBNull(0)) && (SecurityManager.SecureStringEquals(short_secure_id, DataProtection.unprotect(sql_reader.GetString(0)))) )
                    //check if the given short id and the one from database (also encrypted) are equal
                    {
                        return 1;//the given short id is taken
                    }
                }
            }
            else//error
            {
                return Definition.ERROR_INT_VALUE;
            }
        }finally
        {
            closeDataBase(connection_database);
        }

        return Definition.NO_ERROR_INT_VALUE;//the given short id is free
    }


    public int isIdAgentFree(string id_agent, int agent_table_id = -1)
    {
        /*Some agents have an id (and not short id) and a pasword.
         * The function check if the given is "id_agent" is already taken by another agent that the one given 
         * (through its index in the database "agent_table_id")
         * The function return 1 if the given id is already taken
         * else return Definition.NO_ERROR_INT_VALUE if the given short id is free
         * else return Definition.ERROR_INT_VALUE if an error occurred
        */

        SqlConnection connection_database = openDataBase();

        string request = "SELECT Id_agent, Identifiant FROM TABLE_AGENTS";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request);

        if (sql_reader != null)
        {

            while (sql_reader.Read())
            {
                if (sql_reader.GetInt32(0) != agent_table_id && !sql_reader.IsDBNull(1) && sql_reader.GetString(1) == id_agent)
                {
                    return 1;//if the id of the database is the same as the given id and if this is not the id of the given agent
                }
            }
        }
        else//error
        {
            closeDataBase(connection_database);
            return Definition.ERROR_INT_VALUE;
        }

        closeDataBase(connection_database);
        return Definition.NO_ERROR_INT_VALUE;//the given id is free
    }



    public int addNewAgent(Agent agent)
    {
        /*Add new agent into the database*/
        string request = "INSERT INTO TABLE_AGENTS (Nom,Prenom,Travail,AdresseMail, Droit,Identifiant,MotDePassehache,HashSalt, IdentifiantCourtSecurise, Supprimer, DateEdition) "
            + "VALUES (@last_name,@first_name,@job,@mail_adress,@right,@id,@hashed_password,@salt,@short_secured_id,@delete, @modification_date)";

       
        int res = executeNonQueryRequest(request,
            new List<string>(new string[] {"@last_name", "@first_name", "@job", "@mail_adress","@right", "@id", "@hashed_password","@salt", "@short_secured_id", "@delete","@modification_date" }),
            new List<object>(new object[] { agent.lastName, agent.firstName, agent.job, agent.mailAdress, agent.getRight(), agent.id, agent.hashedPassword, agent.saltForHashe, agent.shortSecureId, DO_NOT_DELETE_VALUE, Tools.dateTimeToString(DateTime.Now) }));
        
        return res;
    }

    public Tuple<List<int>,List<string>> getAllAgents()
    {
        /*Return a list of all the index in the database of agent
         * and a list of string that represent the infomormation about the corresponding agent
         * For example retrun [12=index of agent James Patrick who is the school director,...],["James Patrick (School director)",...]
        */
        List<string> list_results = new List<string>();
        List<int> list_ids = new List<int>();

        SqlConnection connection_database = openDataBase();
        
        string request = "SELECT Id_agent, Nom, Prenom, Travail, Droit, Identifiant FROM TABLE_AGENTS WHERE Id_agent!=@id_computer_agent AND Supprimer=@value_not_deleted ";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@value_not_deleted", "@id_computer_agent" }), 
            new List<object>(new object[] {DO_NOT_DELETE_VALUE, Definition.COMPUTER_TABLE_ID_DATABASE }));

        if (sql_reader != null)
        {
            while (sql_reader.Read())
            {
                string item = "";
                if (!sql_reader.IsDBNull(1) && !sql_reader.IsDBNull(2) && !sql_reader.IsDBNull(3))
                {
                    item = sql_reader.GetString(1).ToUpper() + " " + sql_reader.GetString(2).ToLower() + "( " + sql_reader.GetString(3).ToUpper() + " )";
                    //item = "last_name_agent_to_uppercase first_name_agent_to_lowercase (job_agent_to_uppercase)"
                }
                list_results.Add(item);
                list_ids.Add(sql_reader.GetInt32(0));
            }
        }

        closeDataBase(connection_database);

        return Tuple.Create(list_ids, list_results);
    }


    public int getNumberAgent()
    {

        /* retrun the number of not deleted agent (computer agent excluded) */
        SqlConnection connection_database = openDataBase();
    
        string request = "SELECT COUNT(*) FROM TABLE_AGENTS WHERE Id_agent!=@id_computer_agent AND Supprimer=@value_not_deleted ";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@value_not_deleted", "@id_computer_agent" }),
            new List<object>(new object[] { DO_NOT_DELETE_VALUE, Definition.COMPUTER_TABLE_ID_DATABASE }));

        int count = -1;
        try
        {
            if (sql_reader != null)
            {
                if (sql_reader.HasRows)
                {
                    sql_reader.Read();
                    count = sql_reader.GetInt32(0);
                }
            }

        }
        catch
        {
            count = -1;
        }

        closeDataBase(connection_database);

        return count;
    }

    public Agent getAgentByIndex(int agent_table_id)
    {
        /* Return a sructure of the corresponding agent (through its index in the database "agent_table_id")
         * that store all the information about that agent
        */
        Agent agent = new Agent();
        agent.toDefault();

        SqlConnection connection_database = openDataBase();

        
        string request = "SELECT  Nom, Prenom, Travail, AdresseMail, Droit, Identifiant FROM TABLE_AGENTS WHERE Id_agent=@agent_table_id AND Supprimer=@do_not_delete_value ";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@agent_table_id", "@do_not_delete_value" }), 
            new List<object>(new object[] { agent_table_id, DO_NOT_DELETE_VALUE }));

        try
        {
            if (sql_reader != null)
            {
                if (sql_reader.HasRows)
                {
                    sql_reader.Read();
                    agent.tableId = agent_table_id;
                    agent.lastName = sql_reader.GetString(0);
                    agent.firstName = sql_reader.GetString(1);
                    agent.job = sql_reader.GetString(2);
                    agent.mailAdress = sql_reader.GetString(3);
                    agent.setRigth(sql_reader.GetInt32(4));
                    agent.id = sql_reader.GetString(5);

                    agent.hashedPassword = "";
                    agent.shortSecureId = "";
                }
            }

        }catch { }

        closeDataBase(connection_database);

        return agent;
    }

    public string getAgentMailAdress(int agent_table_id)
    {
        string mail_adress = "";
        SqlConnection connection_database = openDataBase();

        string request = "SELECT AdresseMail FROM TABLE_AGENTS WHERE Supprimer=@value_not_deleted AND Id_agent=@agent_table_id";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@agent_table_id", "@value_not_deleted" }), 
            new List<object>(new object[] { agent_table_id, DO_NOT_DELETE_VALUE }));

        if (sql_reader != null)
        {
            if (sql_reader.HasRows)
            {
                sql_reader.Read();

                if (!sql_reader.IsDBNull(0))
                {
                    mail_adress = sql_reader.GetString(0);
                }
            }
        }

        closeDataBase(connection_database);

        return mail_adress;

    }

    public int getComputerTableId()
    {
        int table_id = -1;
        SqlConnection connection_database = openDataBase();

        string request = "SELECT Id_agent FROM TABLE_AGENTS WHERE Identifiant=@computer_id ";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@computer_id" }),
            new List<object>(new object[] { Definition.COMPUTER_AUTHENTIFICATION_ID_DATABASE }));

        if (sql_reader != null)
        {
            if (sql_reader.HasRows)
            {
                sql_reader.Read();

                if (!sql_reader.IsDBNull(0))
                {
                    table_id = sql_reader.GetInt32(0);
                }
            }
        }

        closeDataBase(connection_database);

        return table_id;
    }

    public Agent getAgentByAuthentificationId(string id)
    {
        /*By an id and a hashed password return the corresponding agent from database into a structure
         * If there is no correspondnace retunr the structure initialize with default value
         * Authentification id (different from the table index) is unique for each agent into the database
        */
        Agent agent = new Agent();
        agent.toDefault();
        agent.error = true;

        SqlConnection connection_database = openDataBase();

        try
        {
            string request = "SELECT Id_agent, Nom, Prenom, Travail, AdresseMail, Droit, Identifiant, MotDePasseHache, HashSalt "
                + "FROM TABLE_AGENTS WHERE Supprimer=@value_not_deleted AND Identifiant=@id";
            SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
                new List<string>(new string[] { "@id", "@value_not_deleted" }), new List<object>(new object[] { id, DO_NOT_DELETE_VALUE }));

            if (sql_reader != null)
            {
                if (sql_reader.HasRows)
                {
                    sql_reader.Read();

                    if (sql_reader.GetInt32(0) != Definition.COMPUTER_TABLE_ID_DATABASE)
                    {
                        agent.tableId = sql_reader.GetInt32(0);
                        agent.lastName = sql_reader.GetString(1);
                        agent.firstName = sql_reader.GetString(2);
                        agent.job = sql_reader.GetString(3);
                        agent.mailAdress = sql_reader.GetString(4);
                        agent.setRigth(sql_reader.GetInt32(5));
                        agent.id = sql_reader.GetString(6);
                        agent.hashedPassword = sql_reader.GetString(7);
                        agent.saltForHashe = sql_reader.GetString(8);

                        agent.shortSecureId = "";//default value because that short secure id is only encrypted
                    }
                }
                agent.error = false;
            }
        }catch { }


        closeDataBase(connection_database);

        return agent;
    }



    public Tuple<int, int> getAgentSecurityInfoByShortSecureId(SecureString short_secure_id)
    {
        /* 
         * From the given short secure id return the corresponding database table index of the agent
        */

        int agent_id = -1;//error value
        int agent_right = -1;

        SqlConnection connection_database = openDataBase();


        string request = "SELECT Id_agent, IdentifiantCourtSecurise, Droit FROM TABLE_AGENTS WHERE Supprimer=@value_not_deleted ";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@value_not_deleted" }), new List<object>(new object[] { DO_NOT_DELETE_VALUE }));

        if (sql_reader != null)
        {
            if (sql_reader.HasRows)
            {
                while (sql_reader.Read())
                {
                    if ( (!sql_reader.IsDBNull(1)) && (sql_reader.GetInt32(0)!=Definition.COMPUTER_TABLE_ID_DATABASE) )
                    {
                        if (SecurityManager.SecureStringEquals(short_secure_id, DataProtection.unprotect(sql_reader.GetString(1))))
                        {
                            agent_id = sql_reader.GetInt32(0);
                            agent_right = sql_reader.GetInt32(2);
                            break;
                        }
                    }
                }
            }
        }

        closeDataBase(connection_database);

        return Tuple.Create(agent_id, agent_right);
    }


    public string getAgentEncryptedShortSecureId(int agent_table_id)
    {
        /*By an id and a hashed password return the corresponding agent from database into a structure
         * If there is no correspondnace retunr the structure initialize with default value
        */

        if(agent_table_id==Definition.COMPUTER_TABLE_ID_DATABASE)
        {
            return null;
        }

        SqlConnection connection_database = openDataBase();


        string request = "SELECT IdentifiantCourtSecurise FROM TABLE_AGENTS WHERE Id_agent=@agent_table_id AND Supprimer=@do_not_delete_value";
        SqlDataReader sql_reader = executeReaderRequest(connection_database, request,
            new List<string>(new string[] { "@agent_table_id", "@do_not_delete_value" }), 
            new List<object>(new object[] { agent_table_id, DO_NOT_DELETE_VALUE }));

        if ( (sql_reader != null) && (sql_reader.HasRows) )
        {
            sql_reader.Read();

            if (!sql_reader.IsDBNull(0))
            {
                return sql_reader.GetString(0);
            }
                
            
        }

        return null;
    }

    public int updateAgent(Agent agent)
    {
        /*Update information about an agent "agent"*/

        if(agent.tableId==Definition.COMPUTER_TABLE_ID_DATABASE)
        {
            return Definition.ERROR_INT_VALUE;
        }

        List<string> parms = new List<string>(new string[] { "@agent_table_id","@last_name", "@first_name", "@job", "@mail_adress","@right", "@id", "@delete", "@modification_date" });
        List<object> values = new List<object>(new object[] { agent.tableId, agent.lastName, agent.firstName, agent.job, agent.mailAdress,
            agent.getRight(), agent.id, agent.getDeleteAgentValue(), Tools.dateTimeToString(DateTime.Now) });


        string request = " UPDATE TABLE_AGENTS SET Nom=@last_name, Prenom=@first_name,Travail=@job, AdresseMail=@mail_adress, Droit=@right, "
            + "Identifiant=@id, Supprimer=@delete, DateEdition= @modification_date ";


        /* password and short id are never saved into the structure Agent when they come from database
         * So we set default value to these fields which is an empty string
         * When the user change one of these fields the default value ("") is replace by the given value
         * So if after all the modification password and short id fiels are still set to default it imply that 
         * user does not change these field
        */
        if (agent.hashedPassword != "")
        {//if the user change is password 
            request += ",MotDePasseHache = @hashed_password, HashSalt=@salt";
            parms.Add("@hashed_password");
            parms.Add("@salt");
            values.Add(agent.hashedPassword);
            values.Add(agent.saltForHashe);
        }
        if (agent.shortSecureId != "")
        {//if the user change its short id
            request += ",IdentifiantCourtSecurise = @short_secure_id";
            parms.Add("@short_secure_id");
            values.Add(agent.shortSecureId);
        }

        request += " WHERE Id_agent = @agent_table_id";

        if(executeNonQueryRequest(request, parms, values)!=Definition.NO_ERROR_INT_VALUE)
        {
            return Definition.ERROR_INT_VALUE;
        }

        return Definition.NO_ERROR_INT_VALUE;

    }



    public int changeAgentPassword(int agent_table_id, string new_hashed_password, string salt)
    {
        /*Update information about an agent given by its database table index*/
        string request = "UPDATE TABLE_AGENTS SET MotDePasseHache = @hashed_password, HashSalt=@salt, DateEdition=@modification_date "
                       + "WHERE Id_agent = @agent_table_id ";

        return executeNonQueryRequest(request,
        new List<string>(new string[] { "@hashed_password","@salt", "@modification_date", "@agent_table_id"}),
        new List<object>(new object[] { new_hashed_password, salt, Tools.dateToStringFromDateTime(DateTime.Now), agent_table_id}));

    }


    public int changeAgentShortSecureId(int agent_table_id, string short_secure_id)
    {
        /*Update information about an agent given by its database table index*/
        string request = "UPDATE TABLE_AGENTS SET IdentifiantCourtSecurise = @short_secure_id, DateEdition=@modification_date "
                       + "WHERE Id_agent = @agent_table_id ";

        return executeNonQueryRequest(request,
        new List<string>(new string[] { "@short_secure_id", "@modification_date", "@agent_table_id" }),
        new List<object>(new object[] {  short_secure_id,Tools.dateToStringFromDateTime(DateTime.Now), agent_table_id }));

    }
}

