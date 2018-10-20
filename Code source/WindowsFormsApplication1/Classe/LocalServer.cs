using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using ToolsClass;
using System.Security.Principal;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Dynamic;
using System.Security;
using System.Globalization;

using Student = ToolsClass.Tools.StudentData;
using Agent = ToolsClass.Tools.Agent;
using TimeSlot = ToolsClass.Tools.TimeSlot;

namespace LocalServer
{

    class RequestTags
    {
        /* All the request from client side are made as an Json object (converted in text)
         * To deserialize the object we need to have a structure (with all the possible field)
         * That class is the structure needed to deserialize the Json object as text
         * 
         * When the class will be serialize to Json object all the parameters field will be use 
         * for the Json parameters 
         * For example if the class is :
         * Class class
         * {
         *     public string key1 { get; set;}
         *     public string key2 { get; set;}
         * }
         * Then the resulting Json object will be { key1 : class.key1, key2 : class.key2}
         * In other words, the parameters name ("key1", "key2") have to be the same in the client side and in the server side
       */


        /* all Json object have an object request field in order to know what need to be send
         * the following string is the possible request object
        */
        public const string GET_INFO_FROM_RFID_DOOR = "rfid_from_door";
        public const string ESTABLISH_CONNECTION = "establish_connection";
        public const string GET_REST_TIME_SINCE_BANNED = "rest_time_since_banned";
        public const string DISCONNECTION_FROM_SESSION = "session_disconnection";
        public const string CHECK_IF_SESSION_IS_OPENED = "is_session_opened";
        public const string GET_SEARCH_RESULT = "get_search_result";
        public const string GET_ALL_STUDENT_DIVISION = "get_all_student_division";
        public const string GET_STUDENT_INFO = "get_current_student_info";
        public const string GET_STUDENT_INFO_FOR_SCHOOL_OFFICE = "get_student_for_school_office";
        public const string GET_LENGTH_RFID_ID = "get_length_rfid_id";
        public const string GET_LENGTH_SHORT_SECURE_ID = "get_length_short_secure_id";
        public const string SENSITIVE_DATA = "sensitive_data";
        public const string SENSITIVE_DATA_STOP_STUDENT = "stop_student_as_sensitive_data";
        public const string SENSITIVE_DATA_AUTHORIZE_STUDENT = "authorize_student_as_sensitive_data";
        public const string GET_LOG = "get_log_data";
        public const string SYSTEM_INSTRUCTIONS = "system_instructions";
        public const string GET_STUDY_DATA = "study_data";
        public const string CHANGE_PASSWORD = "set_new_password";
        public const string UPLOAD_FILE = "upload_file";
        public const string GET_VALID_EXTENSIONS_FILE = "get_extension_lists";
        public const string STUDENT_STATE_FILE = "STUDENT_STATE_FILE";
        public const string UNAUTHORIZED_EXIT_SOUND_FILE = "UNAUTHORIZED_EXIT_SOUND_FILE";
        public const string GET_FILE_NAME_ERROR_SOUND = "file_name_error_sound";
        public const string GET_SCHOOL_NAME = "school_name";
        public const string GET_SAVED_REASONS_FOR_EXIT_BAN = "saved_reasons_ban";

        public string request_object { get; set; }

        public string rfid_value { get; set; }
        public int student_table_id { get; set; }
        public string student_last_name { get; set; }
        public string student_first_name { get; set; }
        public string student_division { get; set; }
        public string student_photo { get; set; }
        public int student_exit_authorization { get; set; }
        public List<List<string>> student_authorization_list { get; set; }
        public string student_log { get; set; }

        public string agent_id { get; set; }
        public string agent_password { get; set; }
        public string agent_info_session { get; set; }
        public string agent_short_secure_id { get; set; }
        public string agent_last_name { get; set; }
        public string agent_first_name { get; set; }
        public string agent_job { get; set; }

        public int rest_time_since_bannishement { get; set; }
        public bool reload_client_page { get; set; }
        public string message_to_display { get; set; }
        public bool result_identification_at_connection { get; set; }
        public bool result_disconnection_from_session { get; set; }
        public bool result_check_session_opened { get; set; }

        public string search_word_student { get; set; }
        public List<int> student_id_list { get; set; }
        public List<string> student_info_list { get; set; }
        public List<int> student_division_table_id_list { get; set; }
        public List<string> student_division_list { get; set; }

        public int length { get; set; }
        public object value_sensitive_data { get; set; }
        public object result { get; set; }

        public bool result_authentification_for_sensitive_data { get; set; }
        public bool process_error { get; set; }
        public string type_of_sensitive_data { get; set; }
        public int type_of_table { get; set; }
        public int row_index { get; set; }
        public string text_reason { get; set; }

        public List<string> study_hours { get; set; }
        public List<string> school_days { get; set; }
        public string timeSpan_text_format { get; set; }
        public string date_text_format { get; set; }
        public char timeSlot_separator { get; set; }
        public List<TextTimeSlot> authorizations_list { get; set; }
        public List<int> id_removed_authorization_list { get; set; }

        public string new_password { get; set; }
        public string type_of_uploaded_file { get; set; }
        public Byte[] bytes_array { get; set; }
        public string extension{ get; set; }
        public string[] valid_enxtension_list { get; set; }
        public string file_name { get; set; }

        public struct TextTimeSlot
        {
            public int id;
            public string start_date;
            public string end_date;
            public string day_of_week;
            public string start_time;
            public string end_time;
            public bool read_only;
            public string meta_text;

        }
    }

    class DynamicClass
    {
        /*Allow to create class dynamically*/
        private dynamic response;


        public DynamicClass()
        {
            response = new ExpandoObject();
        }

        public void AddParm(string key, object value)
        {
            /*add parameters to the class equivalent to "object parms1 { get; set; }"*/
            ((IDictionary<String, Object>)response).Add(key, value);
        }

        public dynamic create()
        {
            return response;
        }
    }
    
    class DataRequest
    { 
        private static Encoding RESPONSE_ENCODING = Encoding.UTF8;
        public const string TAG_COOKIE_CLIENT_REF = "userRef";

        public static Encoding getEncoding()
        {
            return RESPONSE_ENCODING;
        }

        /* All the request from online session can not be process because of the accreditation level of the user behind the session
         * Thus, for all the possible request we have compare the needed accreditation level of the corresponding action and
         * the accrediation level of the user. If the user accreditation level is not enough then we have to abort the current process
         * That can be made by adding a comparason for each possible request and if the comparaison is false break outsite the process
         * But to simplify the view we made it differetly : at te beging of the process we get the user accreditation level, 
         * then when we find the needed action to do, we get the needed accreditation level and set a a parameters of the following property
         * To compare the two accredidation level and if the level is not enough throw an exeption (which act as a break for the process) 
       */
        private int needed_rigth = -1; //parameters of the property
        private int agent_right;//agent accredidation level

        public int CertifyAccreditationLevel//property
        {
            get { return needed_rigth; }
            set//when we set the needed accreditation level the following statement are executed
            {
                needed_rigth = value;//get the set value

                if (!SecurityManager.rightLevelEnoughToAccesSystem(agent_right, needed_rigth))//compare the accredidation level
                {
                    throw new LowSecurityAccesNeeded("Tentative d'accès à un domaine non couvert par le niveau d'accréditation");
                    //throw a custum exeption
                }//else do nothing and let the process keep going
            }
        }


        public static Tuple <string, byte []> getFormatedResponse(string response)
        {
            /*Return the content type of the response and an byte array that represent the response value*/
            return Tuple.Create("text/plain", RESPONSE_ENCODING.GetBytes(response));
        }

        public string getJsonResponseToRequest(ref string json_request_to_text, ref Agent input_agent)
        {
            /* All the request from client side are send as Json string
             * And all the Json request are process here
            */
            RequestTags json_request;//request from client side

            DynamicClass response = new DynamicClass();
            /*If we use the class RequestTags to send data, we will send all the possible field 
             * For example if we just need to send the object of the request and a unique value
             * we don't want to send all the oder field of the class RequestTags
             * In other word we have to create a dynamic class with only the needed values
            */
            agent_right = input_agent.getRight();//get the accredidation level of the current agent
            try
            {
                json_request = JsonConvert.DeserializeObject<RequestTags>(json_request_to_text);
                /* Send the object request for client side, which is the same as the given object request from client side
                 * ((IDictionary<String, Object>)response).Add(key<string>, value<object>) allow to create a fiel "key"
                 * with the value "value" (for example : "response.key" give the object "value")
                 * 
                 * To get the name of the parameters use in the Json obejct (that need to be the same in the client side 
                 * and in the server side, we just get the name of the parameters from the class RequestTags used to 
                 * deserialize the Json object. To get the name of the variable we use the function nameof(object or class or properties)
                */
                response.AddParm(nameof(json_request.request_object), json_request.request_object);

                DataBase database = new DataBase();

                //find the correct request to process
                switch (json_request.request_object)//oject of the request
                {
                    case RequestTags.GET_LENGTH_RFID_ID:
                        {
                            //get the number of char for all the RFID id
                            response.AddParm(nameof(json_request.length), Settings.LengthRfidId);
                        }
                        break;
                    case RequestTags.GET_LENGTH_SHORT_SECURE_ID:
                        {
                            //get the number of char for all the short secure id
                            response.AddParm(nameof(json_request.length), SecurityManager.NUMBER_ELEMENTS_SHORT_ID);
                        }
                        break;
                    case RequestTags.GET_SAVED_REASONS_FOR_EXIT_BAN:
                        {
                            /*instead of having to write a reason (if agent need to set an exit ban), 
                             * agent can choose one that has been saved previously
                            */
                            response.AddParm(nameof(json_request.result), Settings.ExitBanReasonsList);
                        }
                        break;
                    case RequestTags.GET_INFO_FROM_RFID_DOOR:
                        {
                            //client ask if the student (given by its RFID id) can leave the school
                            bool process_error = false;
                            string message_to_display = "";

                            if (!Tools.isRfidFormatCorrect(json_request.rfid_value))//if the RFID idformat is correct 
                            {
                                process_error = true;
                                message_to_display = "Le format de l'identifiant RFID n'est pas correct";
                            }
                            else
                            {

                                //get actual date and time
                                DateTime now = DateTime.Now;
                                DateTime now_date = DateTime.ParseExact(now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);//acutal date + 00:00:00
                                int day_of_week = (int)now.DayOfWeek;
                                TimeSpan now_time = DateTime.Now.TimeOfDay.Add(Definition.PRECISION_BEFORE_EXIT_TIME);
                                /* We can not keep the the actual time because any humain infrastructure have strict time
                                 * In fact student could leave the school few minutes before the regular time.
                                 * Moreover, the actual time is compare to all the start time and end time of an hour inside the database (for that student)
                                 * In other words, the actual time, which we will call time, will be compare like : if( time>= start_hour_time and time<=end_hour_time)
                                 * The fact is that student can leaves the school minutes before the start time but must not leeave the school few minutes after
                                 * the end of the hour (which is really close to the start of the next study hour)
                                 * Finally, we have to add somes minutes to the actual time (and not subtract). 
                                 * In that way, the comparaison will be : if(time>= start_time - few_minutes and time<=end_time-few_minutes)
                                 * (By the way, if we subtract few minutes to the actual date te comparaison will be :
                                 *  if(time>= start_time + few_minutes and time<= end_time + few_minutes). As we see, the result is that student
                                 *  can not leave the school until few minutes after the regular hour and can leave the school even if the regular end 
                                 *  of the hour is ended)
                                */

                                Student student = database.getStudentByRFID(json_request.rfid_value);//find the corresponding student
                                if (!student.error)
                                {
                                    //send back data
                                    response.AddParm(nameof(json_request.student_last_name), student.lastName);
                                    response.AddParm(nameof(json_request.student_first_name), student.firstName);
                                    response.AddParm(nameof(json_request.student_division), student.division);
                                    response.AddParm(nameof(json_request.student_photo),
                                        Tools.getStudentPhotoName(student.lastName, student.firstName, student.division));

                                    int authorization = database.getExitCertificationForStudent(student.tableId, now_date, day_of_week, now_time.Subtract(Definition.PRECISION_BEFORE_EXIT_TIME));
                                    if (authorization == Definition.ERROR_INT_VALUE)
                                    {
                                        process_error = true;
                                        message_to_display = "Une erreur est survenue sur le server";
                                    }
                                    else
                                    {
                                        response.AddParm(nameof(json_request.student_exit_authorization), authorization);
                                    }
                                }else
                                {
                                    process_error = true;
                                    message_to_display = "Une erreur est survenue sur le server";
                                }
                            }

                            response.AddParm(nameof(json_request.process_error), process_error);
                            response.AddParm(nameof(json_request.message_to_display), message_to_display);
                        }
                        break;
                    case RequestTags.ESTABLISH_CONNECTION:
                        {//client try to acces to the app (authentification)

                            //get the input password
                            SecureString password = new SecureString();
                            foreach (char c in json_request.agent_password)
                            {
                                password.AppendChar(c);
                            }
                            password.MakeReadOnly();

                            input_agent = SecurityManager.setOnlineConnection(json_request.agent_id, password, input_agent.ipAdress);
                            //get the agent given by its id and password

                            string message_to_display;
                            if (input_agent.error)//an error occurs during the process
                            {
                                message_to_display = "Excusez-nous : une erreur est survenue sur notre serveur."
                                    + "\nIl faut entrer de nouveau vos informations de connexion";
                            }
                            else
                            {
                                message_to_display = "";
                            }
                            response.AddParm(nameof(json_request.message_to_display), message_to_display);


                            if (input_agent.tableId == -1)//no agent with the given id and password was found in the database
                            {
                                response.AddParm(nameof(json_request.result_identification_at_connection), false);
                            }
                            else//agent was found
                            {
                                //send back some informations about the foud agent
                                response.AddParm(nameof(json_request.result_identification_at_connection), true);
                                response.AddParm(nameof(json_request.agent_last_name), input_agent.lastName);
                                response.AddParm(nameof(json_request.agent_first_name), input_agent.firstName);
                                response.AddParm(nameof(json_request.agent_job), input_agent.job);
                            }

                            if (password != null)
                            {
                                password.Dispose();
                            }

                        }
                        break;
                    case RequestTags.DISCONNECTION_FROM_SESSION:
                        {//client want to end his session
                            SecurityManager.setOnlineDisconnection(ref input_agent);
                            response.AddParm(nameof(json_request.result_disconnection_from_session), true);

                        }
                        break;
                    case RequestTags.CHECK_IF_SESSION_IS_OPENED:
                        {   /* user try to open the app and the client side ask to the server if that client is already connected
                             * in order to know which page to open or which scritp to load
                             * 
                             * In fact the client app used javascritp to load the correct content and then ask to the server for that content
                            */
                                response.AddParm(nameof(json_request.result_check_session_opened), SecurityManager.isOnlineSessionOpened(ref input_agent));
                            response.AddParm(nameof(json_request.agent_last_name), input_agent.lastName);
                            response.AddParm(nameof(json_request.agent_first_name), input_agent.firstName);
                            response.AddParm(nameof(json_request.agent_job), input_agent.job);
                        }
                        break;
                    case RequestTags.GET_SEARCH_RESULT:
                        {
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);

                            /* A better way would have been to send data step by step and not when we get all the data
                             * But the number of student is not enough to consider that
                            */
                            var tuple = database.getStudentDataBySearch(json_request.search_word_student);
                            response.AddParm(nameof(json_request.student_id_list), tuple.Item1);
                            response.AddParm(nameof(json_request.student_info_list), tuple.Item2);

                        }
                        break;
                    case RequestTags.GET_ALL_STUDENT_DIVISION:
                        {
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);

                            var tuple = database.getAllStudentDivision();
                            response.AddParm(nameof(json_request.student_division_table_id_list), tuple.Item1);
                            response.AddParm(nameof(json_request.student_division_list), tuple.Item2);
                        }
                        break;
                    case RequestTags.GET_STUDENT_INFO:
                        {
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);

                            int student_table_index = json_request.student_table_id;

                            Student student = database.getStudentByIndex(student_table_index);

                            response.AddParm(nameof(json_request.student_table_id), student_table_index);
                            response.AddParm(nameof(json_request.student_last_name), student.lastName);
                            response.AddParm(nameof(json_request.student_first_name), student.firstName);
                            response.AddParm(nameof(json_request.student_division), student.division);
                            response.AddParm(nameof(json_request.student_photo),
                                Tools.getStudentPhotoName(student.lastName, student.firstName, student.division));

                        }
                        break;
                    case RequestTags.SENSITIVE_DATA:
                        {
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);
                            //set the needed accreditation level. If the user accreditation level is not enough the following part is not executed

                            /* Inside the client app (when user is connected) they can modify some part of the database
                             * And in practice when someone is connected to the app, other people could use the app
                             * (because disconnect and connect with other informations takes to log time)
                             * Thus, we anticipated that behaviour and decided to set a short secure id to know which agent modify the database
                            */
                            bool res_authentification = false;
                            string message_to_display = "";

                            SecureString short_id = new SecureString();
                            foreach (char c in json_request.agent_short_secure_id)
                            {
                                short_id.AppendChar(c);
                            }
                            short_id.MakeReadOnly();
                            //get the user short secure id

                            var tuple = SecurityManager.certifyShortSecureId(short_id, input_agent.ipAdress);
                            int agent_id_by_short_secure_id = tuple.Item1;
                            int agent_right_by_short_secure_id = tuple.Item2;
                            //get the agent id which correspond to the input short secure id

                            agent_right = agent_right_by_short_secure_id;//set the new right

                            if (agent_id_by_short_secure_id != -1 && agent_right_by_short_secure_id == input_agent.getRight())//agent was found
                            {
                                /* Agent can use another account to set sensitive data but that account must have the same right as the agent 
                                 * (retrieve by its short secure id). This prevent the fact that agent with lower accreditation level could 
                                 * try to access some options with the short secure id of an agent with higher rights
                                 * (because agent just have to guess the short secure id, which by construction has fery few characters instead of guessing
                                 * the password which is much more secure)
                                */
                                res_authentification = true;
                            }

                            if (short_id != null)
                            {
                                short_id.Dispose();
                            }

                            response.AddParm(nameof(json_request.result_authentification_for_sensitive_data), res_authentification);

                            if (res_authentification)
                            {
                                bool process_error = false;
                                bool incorrect_parms = false;
                                int res;

                                switch (json_request.type_of_sensitive_data)
                                {
                                    case RequestTags.SENSITIVE_DATA_AUTHORIZE_STUDENT:
                                        {
                                            /* User can set an exit authorization for student or division*/
                                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_CHEF_SCHOOL_OFFICE);
                                            //new accreditation level for that action

                                            List<RequestTags.TextTimeSlot> list_authorizations = json_request.authorizations_list;

                                            //set new authorization for student
                                            foreach (RequestTags.TextTimeSlot textTimeSlot in json_request.authorizations_list)
                                            {
                                                DateTime start_date = Tools.stringToDateTime(textTimeSlot.start_date);
                                                DateTime end_date = Tools.stringToDateTime(textTimeSlot.end_date);
                                                int day_of_week = Definition.DAYS_OF_THE_WEEK.LastIndexOf(textTimeSlot.day_of_week);
                                                TimeSpan start_time = Tools.stringToTimeSpan(textTimeSlot.start_time);
                                                TimeSpan end_time = Tools.stringToTimeSpan(textTimeSlot.end_time);


                                                //check if the given authorization format is correct
                                                if (start_date == Definition.DEFAULT_DATETIME || end_date == Definition.DEFAULT_DATETIME
                                                    || day_of_week == -1
                                                    || start_time == Definition.DEFAULT_TIMESPAN || end_time == Definition.DEFAULT_TIMESPAN
                                                    || start_date > end_date || start_time > end_time)
                                                {//wrong given parameters
                                                    incorrect_parms = true;
                                                    //we don't stop the process here and just wait until an object with correct parameters if found

                                                }else//save the data
                                                {
                                                    res = database.setAuthorization(json_request.type_of_table, json_request.row_index,
                                                            start_date, end_date, day_of_week, start_time, end_time, agent_id_by_short_secure_id, textTimeSlot.meta_text);
                                                    if (res != Definition.NO_ERROR_INT_VALUE)
                                                    {
                                                        process_error = true;
                                                        //don't stop the process neither
                                                    } 
                                                }
                                            }//end add new authorizations


                                            //remove authorization for student
                                            foreach (int index in json_request.id_removed_authorization_list)
                                            {
                                                res = database.removeAuthorization(index, agent_id_by_short_secure_id);
                                                if (res != Definition.NO_ERROR_INT_VALUE)
                                                {
                                                    process_error = true;
                                                    //don't stop the process neither
                                                }
                                            }

                                            //end of modification of the database

                                            //send back informations to the user (if there was an error or if some parameters was not valid)
                                            if (process_error)
                                            {
                                                message_to_display = "Une erreur est survenue : certaines modifications n'ont pas été prises en compte";
                                            }else if(incorrect_parms)
                                            {
                                                message_to_display = "Certains paramètres ne sont pas valides et n'ont donc pas été pris en compte";
                                            }

                                        }//end of case
                                        break;
                                    case RequestTags.SENSITIVE_DATA_STOP_STUDENT:
                                        {
                                            /* User can also stop a student when he will try to exit the school*/
                                             
                                            res = database.setStopForStudent(json_request.type_of_table, json_request.row_index,
                                                                    agent_id_by_short_secure_id, json_request.text_reason);
                                            if (res != Definition.NO_ERROR_INT_VALUE)
                                            {
                                                process_error = true;
                                                message_to_display = "Une erreur est survenue, nous ne pouvons pas enregistrer votre demande";
                                            }
                                        }//end of case
                                        break;
                                }

                                response.AddParm(nameof(json_request.process_error), process_error);
                            }//end of res_authentification

                            response.AddParm(nameof(json_request.message_to_display), message_to_display);

                        }
                        break;
                    case RequestTags.GET_STUDENT_INFO_FOR_SCHOOL_OFFICE:
                        {
                            /* When user start a search and finally find the correct student we send him back informations about that student*/

                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);


                            int student_table_index = json_request.student_table_id;
                            /*In fact when user search student and see resulat, all that result are send with the database table index of the corresponding student
                             * For example if the user see on the scree : "student 1 - 4A" in the backround (on the client side) 
                             * we have "student 1 - 4A" <=> 32 (where 32 is the database table index of the student 1 in 4A)
                            */

                            Student student = database.getStudentByIndex(student_table_index);//get student by id

                            response.AddParm(nameof(json_request.student_table_id), json_request.student_table_id);
                            response.AddParm(nameof(json_request.student_last_name), student.lastName);
                            response.AddParm(nameof(json_request.student_first_name), student.firstName);
                            response.AddParm(nameof(json_request.student_division), student.division);
                            response.AddParm(nameof(json_request.student_photo),
                                Tools.getStudentPhotoName(student.lastName, student.firstName, student.division));

                        }
                        break;
                    case RequestTags.GET_LOG:
                        {
                            /*User want to see all the modifications made on a student or on a division*/
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);

                            string res = database.retriveAllInformationsByIndex(json_request.type_of_table, json_request.row_index);
                            response.AddParm(nameof(json_request.result), res);
                        }
                        break;
                    case RequestTags.GET_STUDY_DATA:
                        {
                            /*User wan to know the study hours of the school*/
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);


                            List<TimeSlot> list_timeslot = database.getExitAuthorizations(json_request.type_of_table, json_request.row_index);
                            List<RequestTags.TextTimeSlot> list_formated_timeSlot = new List<RequestTags.TextTimeSlot>();

                            /* We get all the result as a list of TimeSlot object which can not be send to the client as it 
                             * because of an automatic conversion of date and time (into string) with potentialy an unknow format
                             * So we have to convert the TimeSlot object into a string before send it to the client side
                             * 
                             * In fact we could have use a TimeSlot object with string value but when client send back data he will
                             * also have to send them in a certain structure. So to simplify the probleme we create specifically a new stcurture 
                             * to convert the data and get the data from client
                            */
                            foreach (TimeSlot timeSlot in list_timeslot)//convert all the time slot into text (at the correct text format)
                            {//convert the object into a string object 
                                if (!timeSlot.error)
                                {
                                    RequestTags.TextTimeSlot textTimeSlot = new RequestTags.TextTimeSlot();
                                    textTimeSlot.id = timeSlot.id;
                                    textTimeSlot.start_date = Tools.dateToStringFromDateTime(timeSlot.startDate);
                                    textTimeSlot.end_date = Tools.dateToStringFromDateTime(timeSlot.endDate);
                                    textTimeSlot.day_of_week = Definition.DAYS_OF_THE_WEEK[timeSlot.dayOfWeek];
                                    textTimeSlot.start_time = Tools.timeToStringFromTimeSpan(timeSlot.startTime);
                                    textTimeSlot.end_time = Tools.timeToStringFromTimeSpan(timeSlot.endTime);
                                    textTimeSlot.read_only = timeSlot.readOnly;
                                    textTimeSlot.meta_text = timeSlot.metaText;

                                    list_formated_timeSlot.Add(textTimeSlot);
                                }
                            }

                            response.AddParm(nameof(json_request.timeSpan_text_format), Definition.TIMESPAN_FORMAT);
                            response.AddParm(nameof(json_request.date_text_format), Definition.DATE_FORMAT);
                            response.AddParm(nameof(json_request.timeSlot_separator), Definition.TIMESLOT_SEPARATOR);
                            response.AddParm(nameof(json_request.study_hours), Settings.getStudyHoursToText());
                            response.AddParm(nameof(json_request.school_days), Tools.getTextSchoolDays());
                            response.AddParm(nameof(json_request.authorizations_list), list_formated_timeSlot);
                            //send a list of object within string value converted with a specific format
                        }
                        break;
                    case RequestTags.CHANGE_PASSWORD:
                        {
                            /* User want to change is password*/
                            CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_SCHOOL_OFFICE_MEMBER);


                            bool process_error = false;
                            string message_to_display = "";

                            SecureString password = new SecureString();
                            foreach (char c in json_request.agent_password)
                            {
                                password.AppendChar(c);
                            }
                            password.MakeReadOnly();

                            if (!SecurityManager.certifyConnection(json_request.agent_id, password, input_agent.ipAdress))
                            {//the authentifications informations don't matches with the current session informations
                                process_error = true;
                                message_to_display = "Les informations de connexions ne sont valides.\n"
                                    + "Nous vous rappelons que ne pouvez pas modifier votre mot de passe depuis un autre compte que le votre";
                            }
                            else//current id and password are correct, then change the password
                            {
                                SecureString new_password = new SecureString();
                                foreach (char c in json_request.new_password)
                                {
                                    new_password.AppendChar(c);
                                }
                                new_password.MakeReadOnly();

                                if (!SecurityManager.formatPasswordCorrect(new_password))//format of the password is not correct
                                {
                                    process_error = true;
                                    message_to_display = "Format du mot de passe incorrect :\n" + Definition.INCORRECT_PASSWORD_FORMAT_TEXT;
                                }
                                else//format of the password correct
                                {
                                    var tuple = SecurityManager.setNewAgentPassword(new_password);
                                    //get the hash of the password (with the corresponding new salt)
                                    if (database.changeAgentPassword(input_agent.tableId, tuple.Item1, tuple.Item2) != Definition.NO_ERROR_INT_VALUE)
                                    {//error when trying to save the change
                                        process_error = true;
                                        message_to_display = "Une erreur est survenue, le nouveau mot de passe n'a pas pu être enregistré";
                                    }
                                    else
                                    {//notify the agent that his password changed (by mail)

                                        string header = "CHANGEMENT DE MOT DE PASSE";
                                        string body = "Votre mot de passe viens d'être modifié. "
                                            + "Pour des raisons de sécurité nous ne communiquerons pas le nouveau mot de passe.<br>"
                                            + "Si vous n'êtes pas à l'origine de ce changement, veuillez en informer "
                                            + "l'administration au plus vite afin de régulariser la situation.";
                                        Tools.sendMail(header, body, new List<string> { database.getAgentMailAdress(input_agent.tableId) });
                                        //we don't use input_agent.mailAdress because that value is not update since the user has been authentificated

                                        SecurityManager.setOnlineDisconnection(ref input_agent);//disconnection
                                        response.AddParm(nameof(json_request.reload_client_page), true);
                                    }
                                }

                                new_password.Dispose();
                            }
                            password.Dispose();

                            response.AddParm(nameof(json_request.process_error), process_error);
                            response.AddParm(nameof(json_request.message_to_display), message_to_display);
                        }
                        break;
                    case RequestTags.GET_FILE_NAME_ERROR_SOUND:
                        {
                            /* When student try to leave the school while they don't have the permission
                             * the client app play a song
                             * So here user ask for the name of the song to play
                            */
                            response.AddParm(nameof(RequestTags.file_name), Settings.ErrorAudioFileName);
                        }
                        break;
                    case RequestTags.GET_VALID_EXTENSIONS_FILE:
                        {
                            /* User can upload file to the server side and ask for the valid extension according 
                             * to the type of the file he want to upload
                            */
                            response.AddParm(nameof(RequestTags.STUDENT_STATE_FILE), Definition.VALID_STUDENT_FILE_EXTENSION);
                            response.AddParm(nameof(RequestTags.UNAUTHORIZED_EXIT_SOUND_FILE), Definition.VALID_AUDIO_EXTENSION);

                        }
                        break;
                    case RequestTags.UPLOAD_FILE:
                        {
                            /* User upload files. All the data are send as bytes array*/

                            bool process_error = false;
                            string message_to_display = "";
                            string path = "";

                            if ((json_request.bytes_array != null) && (json_request.bytes_array.Length != 0))
                            {//if the byte array is not null or not empty
                                switch (json_request.type_of_uploaded_file)
                                {//user also send what type of file he is uploding
                                    case RequestTags.STUDENT_STATE_FILE:
                                        CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_ADMINISTRATION);
                                        //user upload the student state file
                                        path = Definition.PATH_TO_FOLDER_DOCUMENT + Definition.NAME_STUDENTS_STATE_FILE + json_request.extension;
                                        break;
                                    case RequestTags.UNAUTHORIZED_EXIT_SOUND_FILE:
                                        CertifyAccreditationLevel = SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_CHEF_SCHOOL_OFFICE);
                                        //user uplod an audio file (the song play when student can not leave the school)
                                        path = Definition.PATH_TO_FOLDER_AUDIO_FILES + Definition.PUBLIC_FOLDERS_NAME + Definition.SEPARATOR + Definition.NAME_ERROR_AUDIO + json_request.extension;
                                        break;
                                }

                                try
                                {
                                    //write the file into the specific folder with a specific name (determine by the type of file uploded)
                                    File.WriteAllBytes(path, json_request.bytes_array);

                                    if(json_request.type_of_uploaded_file == RequestTags.STUDENT_STATE_FILE)
                                    {
                                        if(Tools.readStudentsStateFile()!=Definition.NO_ERROR_INT_VALUE)//update the database with the new file
                                        {
                                            process_error = true;
                                            message_to_display = "Impossible de mettre à jour la base de données depuis le fichier fourni !";
                                        }
                                    }
                                }
                                catch
                                {//error while trying saving the file
                                    process_error = true;
                                    message_to_display = "Impossible d'enregistrer le fichier";
                                }

                            }else
                            {
                                process_error = true;
                                message_to_display = "Le fichier envoyé est vide";
                            }

                            response.AddParm(nameof(json_request.process_error), process_error);
                            response.AddParm(nameof(json_request.message_to_display), message_to_display);

                        }
                        break;
                    case RequestTags.GET_SCHOOL_NAME:
                        {
                            /*User want to get the name of the school*/
                            response.AddParm(nameof(json_request.result), Settings.SchoolName);
                        }
                        break;
                    default://the request is not process by the app
                        {
                            response.AddParm("", "");
                        }
                        break;
                }

                return JsonConvert.SerializeObject(response.create());
                //convert the dynamic class into a Json object, then convert the Json object into string

            }
            catch (LowSecurityAccesNeeded)// custum exeption
            {//user try to acces a process with not enough accreditation level
                response = new DynamicClass();
                json_request = new RequestTags();

                string message_to_display = "Vous tentez d'accéder à un processus non couvert par votre niveau d'accréditation";
                response.AddParm(nameof(json_request.request_object), RequestTags.SYSTEM_INSTRUCTIONS);
                response.AddParm(nameof(json_request.process_error), true);
                response.AddParm(nameof(json_request.message_to_display), message_to_display);

                return JsonConvert.SerializeObject(response.create());
                //convert the dynamic class into a Json object, then convert the Json object into string
            }catch(Exception e)//error
            {
                Error.details = "" + e;
                Error.error = "ICRWS";
                return "";
            }

            
        }


        public static string setRestTimeBanishment(string json_request_to_text, DateTime banishment_start_time)
        {
            /*Send back to the client the remaining time since his banishment*/
                            RequestTags json_request;
            DynamicClass response = new DynamicClass();

            try
            {
                json_request = JsonConvert.DeserializeObject<RequestTags>(json_request_to_text);

                response.AddParm(nameof(json_request.request_object), json_request.request_object);
            }
            catch
            {
                return "";
            }

            if(json_request.request_object== RequestTags.GET_REST_TIME_SINCE_BANNED)
            {
                TimeSpan rest_time = SecurityManager.getRestTimeSinceBanishment(banishment_start_time);
                response.AddParm(nameof(json_request.rest_time_since_bannishement), rest_time.TotalMinutes);

                return JsonConvert.SerializeObject(response.create());
            }else
            {
                return "";
            }
        }



        public static string reloadClientPage()
        {
            /*Send back to the client the remaining time since his banishment*/

            RequestTags json_request;
            DynamicClass response = new DynamicClass();

            response.AddParm(nameof(json_request.request_object), RequestTags.SYSTEM_INSTRUCTIONS);
            response.AddParm(nameof(json_request.reload_client_page), true);

            return JsonConvert.SerializeObject(response.create());
        }

    }

    public class HttpServer : IDisposable
    {
        /* Multithreading local server
         *  This class allow user to start a server from his computer
         *  This server can handle mutiple threads that run asynchronous
         *  In others words that server can deal in the same time with request
         *  from multiple clients. The maximun number of request dealt at the same time
         *  is fixed to avoid the app to create too many new threads and slow down all the process
         *  So when all the possible threads are set for a specific request, all the other request have
         *  to wait that a new thread appears available to be process
         *  
         *  part of the code from :https://stackoverflow.com/questions/4672010/multi-threading-with-net-httplistener
        */
        private readonly int _maxThreads;
        private readonly HttpListener _listener;
        private readonly Thread _listenerThread;
        private readonly ManualResetEvent _stop, _idle;
        private readonly Semaphore _busy;

        


        private int maxThreads = 50; //number of threads allowed to run simultaneously

        /*When the client send a RFID code, the server response to it by 
         * sending back all accuate informations about the corresponding student
         * All these data are seperate with specific tags in order to be easily identify later
         * by the client. To transmit data we use : <attribute>value</attribute> (like in XML)
         * For example the server response could be : "<FirstName>jean</FirstName><SecondName>patrick</SecondName>"
       */

        public HttpServer()
        {
            _maxThreads = maxThreads;
            _stop = new ManualResetEvent(false);
            _idle = new ManualResetEvent(false);
            _busy = new Semaphore(maxThreads, maxThreads);
            _listener = new HttpListener();
            _listenerThread = new Thread(HandleRequests);
            _listenerThread.IsBackground = true; //when the app is close, all the threads will also be closed
        }


        private static bool IsAdministrator()
        {
            /*
             * check if the app is run with administrator prvileges
            */
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }


        public int Start()
        {
            /*The function try to start a server from the computer on the local network
             * If the function can't do it, the function is aborted
             * Return 0 if the server is correctly started, elese -1
            */

            if (!IsAdministrator())
            {
                /*In the manifest file, it is specified that the app need to be
                 * launched with administrator privileges in order to start the server
                 * but we check if the app is really set with these privileges
                */
                Error.error = "FTLLSBRAWAP";
                return Definition.ERROR_INT_VALUE;
            }

            if (_listener.IsListening)
            {
                Error.error = "TTLMIOTS";
                return Definition.ERROR_INT_VALUE;
            }

            string URL_access = "http://" + Settings.ServerIpAdress + ":" + Settings.ServerPort + "/";
            try
            {
                _listener.Prefixes.Add(URL_access); //set http listener 
                _listener.Start();
                _listenerThread.Start(); //set thread to handle request from clients

                Tools.writeUrlServerToFile(URL_access); //save the actual path to server for client app to connect to that server

                return Definition.NO_ERROR_INT_VALUE;
            }
            catch (Exception e) //can't start the server
            {
                Error.details = "" + e;
                Error.error = "FTSLS";
                Tools.sendMail("Erreur critique", "Impossible de démarer le server :\n"+e, Settings.MailsList);

                return Definition.ERROR_INT_VALUE;
            }
        }


        public void Dispose() //dispose the threads
        { Stop(); }



        public void Stop() //stop all threads
        {
            if (!_listener.IsListening)
            {
                return;
            }

            try
            {
                _stop.Set();
                _listenerThread.Join();
                _idle.Reset();

                //wait for all the last running threads and then stop them

                _busy.WaitOne();
                if (_maxThreads != 1 + _busy.Release())
                {
                    _idle.WaitOne();
                }


                _listener.Stop();

            }
            catch (Exception e)
            {
                Error.details = "" + e;
                Error.error = "CCTLS";
            }


        }


        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                var context = _listener.BeginGetContext(ListenerCallback, null);

                if (0 == WaitHandle.WaitAny(new[] { _stop, context.AsyncWaitHandle }))
                {
                    return;
                }
            }


        }


        private void ListenerCallback(IAsyncResult ar)
        {
            _busy.WaitOne();
            try
            {
                HttpListenerContext context;

                if (!_listener.IsListening)
                {
                    return;
                }
                try //try to get context
                {
                    context = _listener.EndGetContext(ar);

                }
                catch (HttpListenerException e)
                {
                    Error.details = "" + e;
                    Error.error = "CGCOCR";
                    return;
                }

                if (_stop.WaitOne(0, false))//if stop has been initiate
                {
                    return;
                }

                try
                {


                    string content_type = "";
                    byte[] responseArray = null;


                    string client_ip_adress = context.Request.RemoteEndPoint.Address.ToString();//get just the ip adress without the port
                    string cookie;
                    try
                    {
                        cookie = context.Request.Cookies[DataRequest.TAG_COOKIE_CLIENT_REF].Value;//client cookie
                    }
                    catch
                    {
                        cookie = null;
                    }

                    Agent agent = SecurityManager.getOnlineAgentFromIpAdress(client_ip_adress, cookie);
                    /*The call of that function is essential to the rest of the code because it update clients informations
                     * and add new client into memory
                     * After that call, all informations about client has been updated
                   */

                    if (agent.banned)//agent is banned
                    {
                        string request_content = readRequest(context);
                        try
                        {
                            RequestTags json_request = JsonConvert.DeserializeObject<RequestTags>(request_content);
                            if (json_request.request_object != null)//client has send a post request (page has not been reloaded yet)
                            {
                                string response = DataRequest.setRestTimeBanishment(request_content, agent.banishmentStartTime);
                                //send the remaining time since client could log in again
                                if (response != "")//client want to know the remaining time
                                {
                                    var tuple = DataRequest.getFormatedResponse(response);
                                    content_type = tuple.Item1;
                                    responseArray = tuple.Item2;
                                }
                                else
                                {
                                    response = DataRequest.reloadClientPage();
                                    var tuple = DataRequest.getFormatedResponse(response);
                                    content_type = tuple.Item1;
                                    responseArray = tuple.Item2;
                                }
                            }

                        }catch //client just reload the page
                        {
                            var tuple = getFormatedResponseForHtmlPage(Definition.PATH_TO_HTML_BANNED_PAGE);
                            content_type = tuple.Item1;
                            responseArray = tuple.Item2;
                        }

                    }
                    else
                    {

                        /*The place of the call is importante :
                        * Image object have to be load into the cache memory before using it 
                        * Thus, images have to be available before the any html page is displayed
                        */
                        var tuple = AjaxRequest(context, ref agent);//handle all ajax request from the client side
                        content_type = tuple.Item1;
                        responseArray = tuple.Item2;

                        if (responseArray == null)//client don't send ajax request
                        {
                            tuple = otherRequest(context, ref agent);//handle all the json request from the client side
                            content_type = tuple.Item1;
                            responseArray = tuple.Item2;
                        }

                        if (responseArray == null)//client don't send Json request
                        { 
                            //send back the main HTML page
                            tuple = getFormatedResponseForHtmlPage(Definition.PATH_TO_HTML_START_PAGE);
                            content_type = tuple.Item1;
                            responseArray = tuple.Item2;
                        }

                        /*Clent session is associate with the windows account and not with the computer itself
                         * To achieve that after client succeed in connecting to the database, we send back a specific string
                         * that will be stored as a cookie (and will be send with each request from that client)
                         * Thus, client are identify by their ip adress and by their cookie 
                         * (because cookie is relative to a sessions and not to a computer)
                        */

                        Cookie cookie_to_send = new Cookie();
                        cookie_to_send.Name = DataRequest.TAG_COOKIE_CLIENT_REF;
                        cookie_to_send.Value = agent.sessionId;
                        cookie_to_send.Expires = DateTime.Now.Add(TimeSpan.FromMinutes(SecurityManager.TIMEOUT_ONLINE_SESSION_MIN));

                        context.Response.SetCookie(cookie_to_send);

                    }

                    //send response to the client side
                    context.Response.ContentType = content_type;
                    context.Response.ContentEncoding = DataRequest.getEncoding();
                    context.Response.OutputStream.Write(responseArray, 0, responseArray.Length); // display html page

                }catch
                {
                    /* FROM : https://stackoverflow.com/questions/429439/specified-network-name-is-no-longer-available-in-httplistener
                     * 
                     * Occasionally, the service fails with "Specified network name is no longer available". 
                     * It appears to be thrown when I write to the output buffer of the HttpListenerResponse
                     * << ListenerCallback() Error: The specified network name is no longer available at 
                     * System.Net.HttpResponseStream.Write(Byte[] buffer, Int32 offset, Int32 size) >>
                     * 
                     * when a ContentLength64 is specified and KeepAlive is false. It seems as though the client is 
                     * inspecting the Content-Length header (which, by all possible accounts, is set correctly, since 
                     * I get an exception with any other value) and then saying "Whelp I'm done KTHXBYE" and closing 
                     * the connection a little bit before the underlying HttpListenerResponse stream was expecting it 
                     * to. For now, I'm just catching the exception and moving on.
                     * 
                    */
                }
                finally
                {
                    //close the stream between client and server
                    context.Response.KeepAlive = false; // set the KeepAlive bool to false
                    context.Response.Close(); // close the connection
                }
            }



            finally
            {
                if (_maxThreads == 1 + _busy.Release())
                {
                    _idle.Set();
                }
            }
        }



        private string readRequest(HttpListenerContext context)
        {
            /*Return the request from client side as text*/
                    string request_content = "";

            try
            {
                HttpListenerRequest request = context.Request;
                using (var reader = new StreamReader(request.InputStream,
                                                        request.ContentEncoding))
                {
                    request_content = reader.ReadToEnd(); //get the data of the client request
                }
            }
            catch { }

            return request_content;
        }


        private byte[] getByteFromFile(string path_to_file)
        {
            /*Retunr an enconding byte array from a file given by its path*/
            string file = File.ReadAllText(path_to_file, DataRequest.getEncoding());
            return DataRequest.getEncoding().GetBytes(file);
        }

        private Tuple<string, byte[]> getFormatedResponseForHtmlPage(string path_to_file)
        {
            /*When we have to send an HTML content we send the content type and then an byte array that represent the content*/

            /* Browser automatically parse into XML response from HTTP request
            *  In some browsers (like FireFox or IE) if the parsing process fails, 
            *  then it will stop all the script (an error occurs)
            * 
            *  So to avoid getting an error we have to specify the content type (XML, Json, text,..)
            *  Since we parse ourself the request from client to server and from server to client
            *  the content type will be always "text/plain" exept when the request is for displaying the main window
            *  (where content type is HTML)
            */
            return Tuple.Create("text/html", getByteFromFile(path_to_file));
        }



        private Tuple<string, byte[]> AjaxRequest(HttpListenerContext context, ref Agent agent)
        {
            /*Function that gather all the possible client ajax request
            */
            byte[] responseArray = null;
            string content_type = "";


            string[] fileName_list =
            {
                context.Request.QueryString[Definition.AJAX_IMAGE_TAG],
                context.Request.QueryString[Definition.AJAX_AUDIO_TAG],
                context.Request.QueryString[Definition.AJAX_JAVASCRIPT_TAG],
                context.Request.QueryString[Definition.AJAX_HTML_TAG],
                context.Request.QueryString[Definition.AJAX_DOWNLOAD_FILE]
            };
            //all possible request from client side

            int index;
            /*search if one request is not null (in the whole list just one request can be not null)
             * If the string list contains null string it's mean that client does not send ajax request
            */
            index = 0;
            while( (index< fileName_list.Length) && fileName_list[index]==null)
            {
                index++;
            }



            if (index != fileName_list.Length)
            {//client use ajax request
                /* When we have the file name we need to knwo in which folder it is
                 * All ajax request are associate with a specific folder (or for particular case with multiple folders)
                */
                string[][] directories_list =
                {
                new string [] { Definition.PATH_TO_FOLDER_IMAGES_HTML_PAGES, Definition.PATH_TO_FOLDER_STUDENTS_PHOTOS },
                new string [] { Definition.PATH_TO_FOLDER_AUDIO_FILES },
                new string [] { Definition.PATH_TO_FOLDER_JAVASCRIPT },
                new string [] { Definition.PATH_TO_FOLDER_HTML_PAGES },
                new string [] {Definition.PATH_TO_FOLDER_DOWLOAD_FILE }
                };

                //content type for all ajax request
                string[] contentType_list =
                {
                "image/*",
                "audio/*",
                "application/javascript",
                "text/html",
                ""
                };

                string path_to_file = Tools.getPathInPrivateOrPublicDirectoryFromFileName(directories_list[index], fileName_list[index]);
                //get the path to file (no matter if the file is private or public)
                
                if ( (!SecurityManager.isOnlineSessionOpened(ref agent)) && (!Tools.fileIsPublic(path_to_file)) )
                {//if the client is not registered and try to get a private file
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;//send an error to the client page
                    content_type = "text/plain";
                    responseArray = DataRequest.getEncoding().GetBytes("");//send empty content
                }
                else
                {
                    try
                    {
                        if ( (index == 0) && (path_to_file==""))//image
                        {
                            path_to_file = Definition.PATH_TO_DEFAULT_STUDENT_PHOTO;
                        }
                        

                        content_type = contentType_list[index];
                        responseArray = File.ReadAllBytes(path_to_file);
                    }
                    catch { }
                }

            }
            return Tuple.Create(content_type, responseArray);
        }



        private Tuple<string, byte[]> otherRequest(HttpListenerContext context, ref Agent agent)
        {
            /*Client ask for a specific infomration trough Json data*/

            byte[] responseArray = null;
            string content_type = "";
            string request_content = readRequest(context);





            try
            {
                //data from client side as send as Json object as text
                DataRequest Response= new DataRequest();
                string response = Response.getJsonResponseToRequest(ref request_content, ref agent);

                if (response != "")
                {
                    var tuple = DataRequest.getFormatedResponse(response);
                    content_type = tuple.Item1;
                    responseArray = tuple.Item2;
                }

            } catch { }

            return Tuple.Create(content_type, responseArray);
        }



    }




}








