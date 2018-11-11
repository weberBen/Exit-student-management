using System;
using System.Collections.Generic;
using System.Security;
using System.Runtime.InteropServices;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsFormsApplication1.Forms;
using WindowsFormsApplication1;
using System.Runtime.Serialization;

using Agent = ToolsClass.Tools.Agent;

public class LowSecurityAccesNeeded : Exception
{
        /* Create a custom exeption that represent a security probleme
         * When online user try to acces a file or ask for something he is not allowed to
         * according to his accreditation level, then we throw that exception type
         */

        /// <summary>
        /// Just create the exception
        /// </summary>
        public LowSecurityAccesNeeded()
          : base()
        {
        }

        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public LowSecurityAccesNeeded(String message)
          : base(message)
        {
        }

        /// <summary>
        /// Create the exception with description and inner cause
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public LowSecurityAccesNeeded(String message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected LowSecurityAccesNeeded(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
}

static class SecurityManager
{
    /* The class is the center of the app security : its deal with all connected user (online or physically)*/

    
    
    /* According to their job all school member (exept the students) have an acces to the app
     * But that acces is limited.
     * To define that limitation we give to each member an accreditation level (the lower level is 0 and give acces to the whole app)
     * A member can acces all the porcess that is for its accreditation level and for higher level
    */
    //name of the accreditations
    public const string RIGHT_DEUS = "Deus ex machina";
    public const string RIGHT_ADMINISTRATION = "Administration";
    public const string RIGHT_CHEF_SCHOOL_OFFICE = "Responsable vie scolaire";
    public const string RIGHT_SCHOOL_OFFICE_MEMBER = "Membre vie scolaire";
    //Accreditaion value is integer and refer to the following list of accreditation
    public static List<string> RIGHTS_LIST = new List<string> { RIGHT_DEUS, RIGHT_ADMINISTRATION, RIGHT_CHEF_SCHOOL_OFFICE, RIGHT_SCHOOL_OFFICE_MEMBER };

    public const int NUMBER_ELEMENTS_SHORT_ID = 4;
    private const int NUMBER_ALPHA_ELEMENTS_SHORT_ID = 1;
    private const int MIN_NUMERIC_ELEMENT_SHORT_ID = 0;
    private const int MAX_NUMERIC_ELEMENT_SHORT_ID = 9;
    private const int NUMBER_ELEMENT_FOR_COOKIES = 10;

    private const string CHAR_VALUE_FOR_PASSWORD = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const int MIN_CHAR_IN_PASSWORD = 10;
    private static System.Timers.Timer timer;
    private const int TIMEOUT_PHYSICAL_SESSION_MIN = 15; //min
    private const int TIME_TIMER = TIMEOUT_PHYSICAL_SESSION_MIN * 60000; //ms
    private static Agent connected_agent;

    private const int NUMBER_CONNECTION_ATTEMPS_ALLOWED = 3;
    public const int TIMEOUT_ONLINE_SESSION_MIN = 30;//min
    private const int TIME_BANNED_SESSION_MIN = 120;//min
    private static List<Agent> client_list = new List<Agent>();
    private static List<Agent> unknown_agent_list = new List<Agent>(); 
    private static List<Agent> banned_ip_adress_list = new List<Agent>();
    /* Since that variable is static all the call to that class will have the same value for "guide"
     * Moreover that varaible is initialize just one time in the whole app (so all the call will have the same start point)
     * It's important because it's like randon() : random function are efficient only if we start it once and then use it
     * not start it multiple time (in that case the result may be similaire to other one)
     * 
     * With a static members only one copy of that members exist through all the created instance of the class
     * So when first call to that class, static field are initialize before any instance of the class has been created (and before the static construtor)
     * So inside an application domaine (inside the app) static field are initialize only once time (but if there are multiple domain application, 
     * then static field will be initialize in each domain application)
     * 
     * <<The static field variable initializers of a class correspond to a sequence of assignments that are executed in the textual order 
     * in which they appear in the class declaration. If a static constructor exists in the class, execution of the static field initializers 
     * occurs immediately prior to executing that static constructor. Otherwise, the static field initializers are executed at an 
     * implementation-dependent time prior to the first use of a static field of that class.>>
     * 
     * (more details here : http://www.damirscorner.com/blog/posts/20151212-CanAStaticFieldInitializeMultipleTimes.html)
    */
    private static DataBase database = new DataBase();

    public const int TIME_BEFORE_SUPPRESSION_OF_DATA_FROM_DATABASE_DAYS = 31;


    /*---------------------------------------------------- Tools for connexion ------------------------------------------------*/

    public static bool rightLevelEnoughToAccesSystem(int agent_right, int needed_right)
    {
        /*return true if the user accreditation level "agent_right" is enough to acces someting that have a access level of "needed_right"
         * Of course an agent with a specific level accreditation level can access all data with lowser accreditation level */

        if ((agent_right >= 0) && (agent_right <= needed_right))//max level is 0 and after all level decrease
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static string getFormatedSessionInfo()
    {
        /*When an user is connected to the app (physically) we store temporarily all its informations into a struct Agent 
         * (like his last name, first name, job, level of right,...)
         * Then we display a part of that information later
        */
        if(connected_agent.tableId==-1)
        {
            return "";
        }else
        {
            return ToolsClass.Tools.getFormatedStringForSessionInfo(connected_agent.lastName, connected_agent.firstName, connected_agent.job);
        }

    }


    public static void setAgentShortSecureId(ref Agent agent)
    {
        /* If the user does not need an id and a password we give him an short id
         * First we decide what format have to be this short id :
         *  - The number of element in it "NUMBER_ELEMENTS_SHORT_ID"
         *  - The number of text element "UMBER_ALPHA_ELEMENTS_SHORT_ID"
         *  - The maximun numeric element MAX_NUMERIC_ELEMENT_SHORT_ID (for example 9)
         *  - The minimun numeric element MIN_NUMERIC_ELEMENT_SHORT_ID (for example 0)
         * Then we mixed all that properties in a random way
         * After we check in the database if that short id is already associate with an agent
         * If yes we restart the process
         * If no we give the short id to that agent through teh structure (and not in the database yet)
         * 
         * In fact, since the short id is short we cannot let the user choose its own short id 
         * (because the probabilities that he chooses one that is already associate with an agent is to high)
         */

        Random random = new Random();
        SecureString short_id;

        char start_char = '0';
        int element;
        int is_short_id_free;

        do
        {
            short_id = new SecureString(); //The short id is like a password
            //set randomly the position of all the text element in the short id
            List<int> list_index_alpha_element = new List<int>();
            for (int i = 0; i < NUMBER_ALPHA_ELEMENTS_SHORT_ID; i++)
            {
                list_index_alpha_element.Add(random.Next(0, NUMBER_ELEMENTS_SHORT_ID - 1));
            }

            //set all the element of the short id
            for (int i = 0; i < NUMBER_ELEMENTS_SHORT_ID; i++)
            {
                if (list_index_alpha_element.LastIndexOf(i) != -1)//set text element
                {
                    short_id.AppendChar(CHAR_VALUE_FOR_PASSWORD[random.Next(0, CHAR_VALUE_FOR_PASSWORD.Length - 1)]);
                }
                else//set numeric element
                {
                    element = random.Next(MIN_NUMERIC_ELEMENT_SHORT_ID, MAX_NUMERIC_ELEMENT_SHORT_ID);
                    short_id.AppendChar(Convert.ToChar(start_char + element));
                }



            }

            is_short_id_free = database.isShortSecureIdAgentFree(short_id);

            //check if the short id is already taken (even by the actual agent)
        } while (is_short_id_free == 1 && is_short_id_free != ToolsClass.Definition.ERROR_INT_VALUE);
        //while the short id is taken and while there is no error
        if (is_short_id_free == ToolsClass.Definition.NO_ERROR_INT_VALUE)//find one correct short id
        {
            agent.shortSecureId = DataProtection.protect(short_id);//give the id to the agent (not in the database yet)
        }

        if (short_id != null)
        {
            short_id.Dispose();
        }

        element = MAX_NUMERIC_ELEMENT_SHORT_ID; //can't find the previous used element to build the short secure id
    }


    public static bool isIdFree(string id, int agent_table_id)
    {
        /*Check in the database if the id is alerady associate with an user (the id and not the short id)*/
        return database.isIdAgentFree(id, agent_table_id) != 1;
    }


    public static void setAgentId(ref Agent agent)
    {
        /* The user can change its id or can ask for one
         * The simpliest way to choose one is to build the id with the agent last name and fisrt name with a number
        */

        string id = "";
        int i = 0;
        do
        {
            if (i > 0)
            {
                id = agent.firstName.ToLower() + "." + agent.lastName.ToLower() + i;
            }
            else
            {
                id = agent.firstName.ToLower() + "." + agent.lastName.ToLower();
            }
            i++;

        } while (!isIdFree(id, agent.tableId));
        //while the id is taken or while there is no error

        agent.id = id;//set the id to the agent (not in the database)

    }


    public unsafe static bool SecureStringEquals(SecureString s1, SecureString s2)
    {
        /* Since short ids are SHORT we can not use a hashing function to save them because the collison risk is too hight
         * So we have to encrypte them
         * In order to check if two short id are the same we have to decrypte them and compare them 
         * (because the encryptation result change even if the input is the same
         * The function return true if two encrypted string are equal, else fasle
        */

        if ((s1 == null || s2 == null) || (s1.Length != s2.Length))
        {
            return false;
        }


        //Compare all char in the string in a secure way
        IntPtr bstr1 = IntPtr.Zero;
        IntPtr bstr2 = IntPtr.Zero;

        RuntimeHelpers.PrepareConstrainedRegions();

        try
        {
            bstr1 = Marshal.SecureStringToBSTR(s1);//create secure space in memory
            bstr2 = Marshal.SecureStringToBSTR(s2);

            unsafe
            {
                for (Char* ptr1 = (Char*)bstr1.ToPointer(), ptr2 = (Char*)bstr2.ToPointer();
                    *ptr1 != 0 && *ptr2 != 0;
                     ++ptr1, ++ptr2)
                {
                    if (*ptr1 != *ptr2)//compare char using pointer to the secure space in memory
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        finally
        {
            /* In fact all the following insctruction remove from memroy the Securestring (and not the copy of the securestring)
             * if (bstr1 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr1);//set all the secure space to a standard value
            }

            if (bstr2 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr2);
            }

            s1.Dispose();
            s2.Dispose();*/


        }
    }




    public unsafe static bool formatPasswordCorrect(SecureString s1)
    {
        /*If the user change his password we need to check if it is enough secure
         * The length of the password must be superior the the minimun length "MIN_CHAR_IN_PASSWORD"
         * The password must have at least one uppercase element
         * The password must have at least one lowercase element
         * The password must have at least one numeric element
         * 
         * Here the password is not hashed yet, be it is in a secure array in the memory
         * 
         * The function return true if the format of the password is correct, else false
         */

        if (s1.Length < MIN_CHAR_IN_PASSWORD)//password is not hashed or encrypted
        {
            return false;
        }

        IntPtr bstr1 = IntPtr.Zero;
        RuntimeHelpers.PrepareConstrainedRegions();
        int validation_step = 0;
        bool step_1 = false, step_2 = false, step_3 = false;

        try
        {
            bstr1 = Marshal.SecureStringToBSTR(s1);

            unsafe
            {
                for (Char* ptr1 = (Char*)bstr1.ToPointer(); *ptr1 != 0; ++ptr1)
                {
                    if ((!step_1) && (*ptr1 >= 'a' && *ptr1 <= 'z'))//one lowercase
                    {
                        validation_step++;
                        step_1 = true;
                    }
                    else if ((!step_2) && (*ptr1 >= 'A' && *ptr1 <= 'Z'))//one uppercase
                    {
                        validation_step++;
                        step_2 = true;
                    }
                    else if ((!step_3) && (*ptr1 >= '0' && *ptr1 <= '9'))//one numeric element
                    {
                        validation_step++;
                        step_3 = true;
                    }

                    if (validation_step == 3)//format of the password is correct
                    {
                        return true;
                    }
                }
            }

            return true;
        }
        finally
        {
            if (bstr1 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }



    public static void maintenance()
    {
        cleanClientList();
    }


    /*---------------------------------------------------- Physical connexion ------------------------------------------------*/

    public static int getConnectedAgentRight()
    {
        return connected_agent.getRight();
    }

    public static int getConnectedAgentTableIndex()
    {
        return connected_agent.tableId;
    }

    public static int setConnection(string id, SecureString secure_given_password)
    {
        /*When a user is physically connected to the app we retrieve all the information about him in a structure Agent from the database
         * If an agent is find in the database from the input id and password (given after being in an hashing function) 
         * then we set a timer (that represent the connection time left before disconection ) and save temporarily all the informations in a structure
        */

        if(id=="" || secure_given_password == null)
        {
            connected_agent.toDefault();
            if (timer != null)
            {
                timer.Stop();
            }

            return ToolsClass.Definition.ERROR_INT_VALUE; ;
        }


        Agent agent = database.getAgentByAuthentificationId(id);
    
        if(agent.error)
        {
            return ToolsClass.Definition.ERROR_INT_VALUE;
        }


        //Hash the password with salt saved in the database
        if (agent.hashedPassword != DataProtection.getSaltHashedString(secure_given_password, agent.saltForHashe))
        {//if the stored hashe password is not equal to the given hashe password (with the salt saved in the database)
            agent.toDefault();//reset agent
        }


        if(agent.tableId!=-1 )//agent is found
        {
            connected_agent = agent;//save temporarily the information

            //Set a timer for reason detailed later
            if(timer!=null)
            {
                timer.Stop();
            }
            timer = new System.Timers.Timer();
            timer.Interval = TIME_TIMER;
            timer.Elapsed += new ElapsedEventHandler(onTimeEnd); ; //event timer tick
            timer.Start();

            return ToolsClass.Definition.NO_ERROR_INT_VALUE;
        }
        else
        {
            connected_agent.toDefault();
            if (timer!=null)
            {
                timer.Stop();
            } 
        }

        return ToolsClass.Definition.ERROR_INT_VALUE;
    }

    public static void disconnection()
    {
        /*disconnection (automatically or physically) of the agent*/
        connected_agent.toDefault();//remove informations about the previous agent
        DisposeAllNonCriticalForms();
    }


    public static void DisposeAllNonCriticalForms()
    {
        /*Close all the windows exept the main window "MainForm" and the 
         * one "DisplayWebSiteForm" for checking online data about the students (that need to stay open during the process)*/
        foreach (Form form in Application.OpenForms)
        {
            if (form.Name != typeof(MainForm).Name /*&& form.Name != typeof(OnlineDatabaseForm).Name*/)
            {
                form.Close();
            }
        }
    }


    private static void onTimeEnd(object source, ElapsedEventArgs e) //handler when the time counter is finished
    {
        /*When a agent is physically connected to the app we start a timer that represent the resting time before disconnection
         * At this step there is no more time left
         * First we ask the user if we want to stay connected by a autoclosing message form (that close after a specific time if there is no answer)
         * If there is no answer or the agent does not want the stay connected we close the session
         * Else we restart a new timer
        */
        timer.Stop();

        /*If the form that check online data about the student is opened it's means that user can't use the app
         * In order to avoid perturbating the data grabbing if that form is openned then we just disconnect the previous user
         * without showing him the autoclosing box that ask the user if he wants to remain connected or not
        */
        bool is_online_update_data_running = false;
        foreach (Form form in Application.OpenForms)
        {
            /* if (form.Name == typeof(OnlineDatabaseForm).Name)
             {
                 is_online_update_data_running = true;//the form is opened
             }*/
        }

        if (is_online_update_data_running)
        {
            disconnection();
            return;
        }

        string title = "Demande de confirmation";
        string message = "Utilisez vous encore l'application ?";
        string text_button_ok = "Oui";
        string text_button_cancel = "Quiter";
        int timeout = 10000;//ms

        AutoClosingMessageBox Form = new AutoClosingMessageBox(title, message, timeout, text_button_ok, text_button_cancel);
        Form.ShowDialog();
        if (Form.getDialogResult() == true)//user confirm we want the keep using the app
        {
            timer.Start();

        }
        else//user quit or timeout
        {
            disconnection();

        }
    }


    /*---------------------------------------------------- Online connexion ------------------------------------------------*/
    private static void cleanClientList()
    {
        /* When an client try to connect to the server we search its information into the client list. If he was previously into the client list, we update the information
         * else we add the client into the database. Thus, all the profile are loaded into the client list until the agent try to access to the server again.
         * In other words, the size of the client could increase exponentially at the end of the day if the don't remove all the profile that are not used anymore.
         * 
         * This function loop into the client list and find the profiles that are no longer banned or the profiles that has been automatically disconnected
         * to remove it from the database
        */

        Agent agent;
        for (int i = 0; i < client_list.Count; i++)
        {
            agent = client_list[i];

            if (updateBannedAgent(agent) || updateRegisteredAgent(agent, agent.sessionId))
            {
                //non used profile found
                client_list.RemoveAt(i);
            }
        }
    }


    public static Agent setOnlineConnection(string id, SecureString secure_given_password, string ip_adress)
    {
        /* When client try to establish a connection as a registered agent, he send his id and password.
         * Then we check in the database those informations matches with an agent into the database
         * If yes we gather all information about that agent into a structure and save it into a list "client_list"
         * and the information will stay in it until the client disconnect himself or the session expired
         * If no we save the ip adress of the client and update his number of attemps into the list "client_list"
         * 
         * 
         * If agent was found into the database that function return it
         * else retrun a standard agent with default values set
         * 
         * On the server side, we have mutiple possible cases :
         *  - agent is recognise as registered agent into the database
         *  - agent is unknown but try to connect
         *  - agent is banned (because he fails to many time to connect)
         * All agents are (temporarily) indistinctly stored into memory (trough the list "client_list")
         * We can differianciate agent because of its internal parameters that are not the same if the agent is registered or banned or unknown
         * We delete those informations from memory only when user states evoluate (for exemple if agent has been banned but now he wait enough time
         * we remove information about that user from memory or when agent is disconnected we remove its informations)
         * In orther words, the only way to if agent is registered (so allow to access the website) is to look if the ip adress is saved in memory 
         * and if he is not banned.
         * 
         * The case "unknown agent" held to figure out the number of attemps client did : in that case we stored the ip adress and increase
         * the number of attemps by one each time he fails to connect. Then when he try again but the number of attemps is too high that client 
         * became banned. In fact, if agent try to connect few time the ip adress of the user computer will be stored as long as a new agent 
         * use the same computer and succeed in connecting with the server. In otherwise, the informations will remain into memory
         * The banishment is not assiociate to the a windows session but to a computer
         * But when agent is registered he is from its windows session on a specific computer (and not only from the computer)
         * 
         * This is the only function that can add a new user
         * 
       */

        Agent agent = new Agent();
        agent.toDefault();

        if (id == "" || secure_given_password == null)
        {
            return agent;
        }

        /*When the database is not used form a while the cache are clear (the database become "cold")
         * So after a certain time if a user try to connect to the database it will have to wait
         * that the database end to load and then wait to have the result of the request
         * But the needed amount of time will almost always overpass the maximun connection time of the databse
         * which will procude an error
        */
        agent = database.getAgentByAuthentificationId(id);//could return an agent with error set to true 

        if (agent.error)
        {//error from the server (not the fault of the client)
            return agent;
        }

        //Hash the password with salt saved in the database
        if (agent.hashedPassword != DataProtection.getSaltHashedString(secure_given_password, agent.saltForHashe))
        {//if the stored hashe password is not equal to the given hashe password (with the salt saved in the database)
            agent.toDefault();//reset agent
        }

        int index = getIndexAgentFromIpAdress(ip_adress);
        if (index == -1)//add new user
        {
            Agent temp_agent = new Agent();
            temp_agent.toDefault();
            
            setEssentialParmsForAgent(ref temp_agent, ip_adress);
            client_list.Add(temp_agent);
            index = client_list.Count - 1;
        }


        if (agent.tableId == -1)
        {//there is no corresponding agent into the database (wrong id or password or both)
            agent = client_list[index];
            /*All client that send a request to the server are saved into memory with there ip adress
             * So at that step when we search for a client define by its ip adress the result 
             * will never be out of range 
            */
            setNewAttempForAgent(ref agent);
            /* If the user fail to connect we already have its information into memory 
             * In this case, we update the information abour that agent
             * We can not directly add the agent into the database because agent are found by their ip adress (at the first match)
             * In other words, if we have multiple agent set in memory with the same ip adress just one will be retrieve
           */
        }
        else
        {
            //agent was found into the database
            agent.numberOfConnectionAttemps = 0;
            //reset the number of attemp
            agent.sessionId = setCookiesForClient();
            //all the cookie are set at the connection and will remain the same until the next connection
        }


        setEssentialParmsForAgent(ref agent, ip_adress);
        //registerAgentNewAttemp use agent.lastConnectionTime so order insctruction is here important and setEssentialParmsForAgent() update lastConnectionTime


        client_list[index] = agent;
        //again index will never be out of range
        return agent;

    }


    public static void setOnlineDisconnection(ref Agent agent)
    {
        /* When user want to be disconnected from the sever we remove all information about him from memory
         * so the next time he will try to connect to the server there will be no more informations about him
         * and he will have to connect again
        */
        if (agent.tableId != -1)
        {
            /*If the client is in memory and registered
             * because if a client who is not registered try to diconnect it will remove all his informations from server 
             * (and that's a problem if the user will be banned soon because he made too much attemps)
             * (even the number of attemps). In other words, it could be a way to avoid beeing banned after multiple failed
             * So to be disconnected client have to be registered
             * (at that step assume that session expiration has been cheked before)
           */
            client_list.Remove(agent);
            agent.toDefault();
        }
    }

    public static int getIndexAgentFromIpAdress(string ip_adress)
    {
        /*From an ip adress return the index of the macthing agent into the list "client_list"
         * or if no agent with that ip adress was found return the length of the list
        */
        int i;

        i = 0;
        while ((i < client_list.Count) && (client_list[i].ipAdress != ip_adress))
        {
            i++;
        }

        if(i==client_list.Count)
        {
            return -1;
        }

        return i;

    }

    public static string setCookiesForClient()
    {
        /*Since there is multiple session on the same computer, determine a session only by the ip adress of the compture is not enough
         * So when user connect to the server we send a random number that client will send later with ist request
         * in order to know if it is the same user (and not another user with another window account that use the same computer)
       */

        return Guid.NewGuid().ToString("N");

        /*  NewGuide() return a unique 128 bits key (not random but unique by avoid collision).
         * In fact the number of possibilities with a 128 bits key are so important that the porbablities of dublications
         * are really low 
         * 
         * In fact we can guess what number is return since the function are based on machine specification and time and other stuff
         * but :
         * .NET Web Applications call Guid.NewGuid() to create a GUID which is in turn ends up calling the CoCreateGuid() COM 
         * function a couple of frames deeper in the stack.
         *  From the MSDN Library : 
         *      <<The CoCreateGuid function calls the RPC function UuidCreate, which creates a GUID, a globally unique 128-bit integer. 
         *      Use the CoCreateGuid function when you need an absolutely unique number that you will use as a persistent identifier in a 
         *      distributed environment.To a very high degree of certainty, this function returns a unique value – no other invocation, 
         *      on the same or any other system (networked or not), should return the same value.>>
         *      
         *      <<The UuidCreate function generates a UUID that cannot be traced to the ethernet/token ring address of the computer on which 
         *      it was generated. It also cannot be associated with other UUIDs created on the same computer.>>
         * (more here : https://stackoverflow.com/questions/643445/how-easily-can-you-guess-a-guid-that-might-be-generated)
         * So guess is possible but too hard to be donne quickly 
         * AND, that key is not the only way to identidy user : user are define by that key and by his ip adress
        */
    }

    private static void setNewAttempForAgent(ref Agent agent)
    {
        /*When user fails to connect to the database we update his number of attemps 
         * and eventually make him as a banned agent 
        */


        if (DateTime.Now.Subtract(agent.lastConnectionTime) >= TimeSpan.FromMinutes(TIME_BANNED_SESSION_MIN))
        {
            /* Since the number of attemps is related to the computer and not the windows session
                * When nobody try to connect to the database from the computer for a certain time
                * we reset the number of attemps assiociate to that computer
            */
            agent.numberOfConnectionAttemps = 0;
        }

        agent.numberOfConnectionAttemps = agent.numberOfConnectionAttemps + 1;
        //increase the number of attemps

        if (agent.numberOfConnectionAttemps >= NUMBER_CONNECTION_ATTEMPS_ALLOWED)
        {//to many tries
            string ip_adress = agent.ipAdress;
            agent.toDefault();//not necessary but just to be careful
            setEssentialParmsForAgent(ref agent, ip_adress);
            agent.banned = true;//agent is now banned
            agent.banishmentStartTime = DateTime.Now;
        }


    }


    public static bool shortIdCorrespondToAgentByIndex(ref Agent agent, SecureString short_id_to_compare)
    {
        /* Check if the given short secure id is the one store in the database for the current agent
         * If so return true, elese return false
        */
        string encrypted_short_id = database.getAgentEncryptedShortSecureId(agent.tableId);

        if (encrypted_short_id == null)
        {//error from database
            agent.error = true; //change are not store into memory just temporarely into that structure
            return false;
        }

        int index = client_list.LastIndexOf(agent);//index can not be equal to -1 here because of the previous statement (from other method)

        SecureString agent_short_id = DataProtection.unprotect(encrypted_short_id);//get the saved short secure id from database for that specific agent

        bool res_authentification = SecureStringEquals(agent_short_id, short_id_to_compare);//compare the saved short id and the given one

        if (!res_authentification)
        {
            setNewAttempForAgent(ref agent);
        }
        else
        {
            agent.numberOfConnectionAttemps = 0;
            //reset the number of attemp
        }
        client_list[index] = agent;//update agent data


        if (agent_short_id != null)
        {
            agent_short_id.Dispose();
        }
        if (short_id_to_compare != null)
        {
            short_id_to_compare.Dispose();
        }

        return res_authentification;
    }


    public static Tuple<int, int> certifyShortSecureId(SecureString short_id , string ip_adress)
    {
        /*Identify agent by its short secure id*/

        int index = getIndexAgentFromIpAdress(ip_adress);//can  not be equal to -1 because all agent are registered before the call of that function
        Agent current_agent = client_list[index];//get the agent infromation from the current open registered session

        var tuple = database.getAgentSecurityInfoByShortSecureId(short_id);
        int agent_id = tuple.Item1;
        int agent_right = tuple.Item2;

        if (agent_id ==-1)
        {//if the stored hashe password is not equal to the given hashe password (with the salt saved in the database)
            //if user try too many times to changes its account informations with the wrong actual authentification informations we block that Ip adress
            setNewAttempForAgent(ref current_agent);
        }
        else
        {
            //agent was found into the database
            current_agent.numberOfConnectionAttemps = 0;
        }

        client_list[index] = current_agent;//save informations
        //again index will never be out of range

        return Tuple.Create(agent_id, agent_right);
    }


    public static bool certifyConnection(string id, SecureString secure_given_password, string ip_adress)
    {
        /* When online user want to change his account information, he have to enter is id and password again
         * to validate the changes.
         * When the user send us the changes with its actual id and password we check into the database if the id and the password
         * are the same for that agent than the one saved
         * The agent information from database are the one who was connected previously. In other word, only the user who register to acces 
         * the app can change its account informations from the current session
         * 
         * If the authentification informations matches the method return true, else false
        */
         
        bool certification = false;//resul

        int index = getIndexAgentFromIpAdress(ip_adress);//can  not be equal to -1 because all agent are registered before the call of that function
        Agent current_agent = client_list[index];//get the agent infromation from the current open registered session
        Agent needed_agent = database.getAgentByAuthentificationId(id);
        /* Get the informations of the agent found by the given id (into the database) 
         * and not from the current agent because we want user to modify their account informations only from their own session and not from
         * other user session
       */


        //Hash the password with salt saved in the database
        if (needed_agent.hashedPassword != DataProtection.getSaltHashedString(secure_given_password, needed_agent.saltForHashe))
        {//if the stored hashe password is not equal to the given hashe password (with the salt saved in the database)
            //if user try too many times to changes its account informations with the wrong actual authentification informations we block that Ip adress
            setNewAttempForAgent(ref current_agent);
            certification = false;
        }
        else
        {
            //agent was found into the database
            current_agent.numberOfConnectionAttemps = 0;
            certification = true;
        }

        client_list[index] = current_agent;//save informations
        //again index will never be out of range

        return certification;
    }

    public static Agent getOnlineAgentFromIpAdress(string ip_adress, string cookie)
    {
        /* That function update informations about saved agent into the list "client_list"
         * (for exemple figure out if the agent is disconnected, or not any more banned)
         * Then update that infomration into memory and return it
        */

        Agent agent = new Agent();
        agent.toDefault();
        setEssentialParmsForAgent(ref agent, ip_adress);

        int index = getIndexAgentFromIpAdress(ip_adress);
        if (index != -1)
        {//if agent is already into memory
            agent = client_list[index];

            bool index_refer_to_the_same = (!changeBannedAgent(ref agent)) && (!changeRegisteredAgent(ref agent, cookie));
            //update information and if there is no more reason to keep that information into memory remove it from the list "client_list"
            setEssentialParmsForAgent(ref agent, ip_adress);
            /* order of insctruction are important here because when call setEssentialParmsForAgent the last 
             * connection time is update. So if we put that call before checking data about client then the last connection time
             * will be the current time which is not correct for the rest of the code
             * In fact it is the last connection time before that request (request which explain why we can set the 
             * current time as the last connection time, because without that request the last connection time will be the one previously saved
             * and not the current time)
            */

            if (index_refer_to_the_same)
            {// else agent has been removed from the list so index does not refer to he same as previously
                client_list[index] = agent;
            }

        }//if the client is not registered we do nothing
        return agent;
    }

    private static bool updateBannedAgent(Agent agent)
    {
        /* return true is the agent is no longer banned
         * else return false
        */

        if ( (agent.banned == true) && (getRestTimeSinceBanishment(agent.banishmentStartTime) < TimeSpan.FromMinutes(0)) )
        {
            return true;
        }

        return false;
    }

    private static bool changeBannedAgent(ref Agent agent)
    {
        /*Check if the given agent is no longer banned
         * If so we remove it from memory
         * 
         * If agent is removed return true, else false
        */
        if(updateBannedAgent(agent))
        {//agent is no longer banned
            client_list.Remove(agent);

            string ip_adress = agent.ipAdress;
            agent.toDefault();//not necessary but just to be careful
            setEssentialParmsForAgent(ref agent, ip_adress);
            agent.banned = false;
            /* order of instructions is important here because we have to update agent information only after removing it from database 
             * else there will be no corresponding agent into memory
             * 
             * We update agent even if we remove it from memory because it will be used later to know what the client
             * can acces at that time for that request
             * Then for other request, since there will be no more corresponding agentin in memory, client will have to connect again
            */

            return true;
        }

        return false;
    }


    public static TimeSpan getRestTimeSinceBanishment(DateTime banishment_dateTime)
    {
        /*Return the remain time since agent has been banned*/
        return TimeSpan.FromMinutes(TIME_BANNED_SESSION_MIN).Subtract(DateTime.Now.Subtract(banishment_dateTime));
    }

    private static bool updateRegisteredAgent(Agent agent, string session_id)
    {
        /*return true if the agent is in the client_list but its expiration time of the session
          else return false
         */

        /*If the agent has not been authentified*/
        if (agent.tableId == -1)
        {
            if (DateTime.Now.Subtract(agent.lastConnectionTime) >= TimeSpan.FromMinutes(TIMEOUT_ONLINE_SESSION_MIN))
            {
                /*If the last connection between the client and the server is older than the expiration time of the session
                 we reset the profil
                 Notice that in that way the number of attemp will be reset if the agent wait enough time (expiration time of the session)
                */

                return true;
            }

        }
        else
        {
            /*If the agent has been authentified, if his session id matches to the saved one or if the session expired, we reset the profil*/
            if ((DateTime.Now.Subtract(agent.lastConnectionTime) >= TimeSpan.FromMinutes(TIMEOUT_ONLINE_SESSION_MIN))
                || ((agent.sessionId == null) || (agent.sessionId != session_id))
               )
            {
                return true;
            }
        }

        return false;
    }

    private static bool changeRegisteredAgent(ref Agent agent, string session_id)
    {
        /* Check if the given agent is still connected (the session does not expire)
         * If he is no, we remove it from memory
         * 
         * If agent is removed return true, else false
        */

        if (updateRegisteredAgent(agent, session_id))
        {
            /* If agent has been connected but the session expired (or user is connected to the same computer with an other windows sessions)
             * 
             * In fact if client connect from a computer to be considered as connected he will need to have the given cookie (send during
             * its connection attemps)
             * If he does not have the same cookie it's mean that another client use the same computer but not the same windows session
            */

            client_list.Remove(agent);

            string ip_adress = agent.ipAdress;
            agent.toDefault();//necessary
            setEssentialParmsForAgent(ref agent, ip_adress);
            //order of instructions is important here (as previously said)
            return true;
        }

        return false;
    }


    public static bool isOnlineSessionOpened(ref Agent agent)
    {
        /* When client open a tab and succeed in connecting to the database
         * then if a close the tab and open it again we send back to the client
         * information about the open session
         * If the agent exist into the memory (in the list "client_list") then return true,
         * else return false
       */
        if(agent.tableId==-1)
        {
            return false;
        }else
        {
            return true;
        }
    }


    public static Tuple<string, string> setNewAgentPassword(SecureString new_password)
    {
        /* With the given password return the corresponding hash with a new salt (which will have to be saved into the database)*/
        string salt = DataProtection.getSaltForHashes();
        string hashed_password = DataProtection.getSaltHashedString(new_password, salt);

        return Tuple.Create(hashed_password, salt);
    }


    private static void setEssentialParmsForAgent(ref Agent agent , string ip_adress)
    {
        /*When client is reset or update two informations must be completed in order to avoid fatal error in the code 
         * We must update the ip adress of the client and the current time which is the last time client connect to the server
        */
        agent.ipAdress = ip_adress;
        agent.lastConnectionTime = DateTime.Now;
    }



}

