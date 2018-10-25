using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security;
using System.Net.Sockets;
using System.Globalization;
using System.Net.Mail;

using ParmsMailsSender = ToolsClass.Tools.ParmsMailsSender;
using ParmsStudentsStateFile = ToolsClass.Tools.ParmsStudentsStateFile;


namespace ToolsClass
{

    static class Definition
    {
        /*The class gather all informations cross class, that method from other class could use*/

        public static string SEPARATOR = "" + Path.DirectorySeparatorChar;
        public static string CURRENT_DIRECTORY = Directory.GetCurrentDirectory();

        //file names (with extension)
        public const string NAME_TOOLS_FILE = "SettingsFile.xml";
        public const string NAME_HTML_START_PAGE = "menu.html";
        public const string NAME_HTML_BANNED_PAGE = "banned_page.html";
        public const string NAME_ERROR_AUDIO = "error_audio"; //name of the sound played when student can't leave the school
        public const string NAME_STUDENTS_STATE_FILE = "regime_students";//student state (first name, second name, division, sex, half-boarder/external)
        public const string NAME_DEFAULT_STUDENT_PHOTO = "default_student_photo.png";
        public const string NAME_LOG_ERROR_FILE = "log_error.txt";
        //folder names
        public const string FOLDER_NAME_HELP= "Aide";
        public const string FOLDER_NAME_HTML_ELEMENT = "html";
        public const string FOLDER_NAME_JAVASCRIPT_FILE = "javascript";
        public const string FOLDER_NAME_HTML_PAGES = "html_pages"; //folder that contains all the html page that could be send to client
        public const string FOLDER_NAME_IMAGE_HTML_PAGES = "images_html_pages"; //folder that contains images used in the html pages
        public const string FOLDER_NAME_AUDIO = "audio_files"; //contains all audio files that are included in html page
        public const string FOLDER_NAME_STUDENTS_PHOTOS = "photos_eleves";
        public const string FOLDER_NAME_DOCUMENTS = "documents";
        public const string FOLDER_NAME_DOWLOAD_FILE = "download_files";
        public const string PRIVATE_FOLDERS_NAME = "private";
        public const string PUBLIC_FOLDERS_NAME = "public";

        //path to files
        public static string PATH_TO_TOOLS_FILE = CURRENT_DIRECTORY + SEPARATOR + NAME_TOOLS_FILE;
        public static string PATH_TO_LOG_ERROR_FILE = CURRENT_DIRECTORY + SEPARATOR + NAME_LOG_ERROR_FILE;
        public static string PATH_TO_FOLDER_HTML_ELEMENT = CURRENT_DIRECTORY + SEPARATOR + FOLDER_NAME_HTML_ELEMENT + SEPARATOR;
        public static string PATH_TO_FOLDER_JAVASCRIPT = PATH_TO_FOLDER_HTML_ELEMENT + FOLDER_NAME_JAVASCRIPT_FILE + SEPARATOR;
        public static string PATH_TO_FOLDER_DOWLOAD_FILE = PATH_TO_FOLDER_HTML_ELEMENT + FOLDER_NAME_DOWLOAD_FILE + SEPARATOR;
        public static string PATH_TO_FOLDER_HTML_PAGES = PATH_TO_FOLDER_HTML_ELEMENT + FOLDER_NAME_HTML_PAGES + SEPARATOR;
        public static string PATH_TO_FOLDER_STUDENTS_PHOTOS = CURRENT_DIRECTORY + SEPARATOR + FOLDER_NAME_STUDENTS_PHOTOS + SEPARATOR;
        public static string PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS = PATH_TO_FOLDER_STUDENTS_PHOTOS + PUBLIC_FOLDERS_NAME + SEPARATOR;
        public static string PATH_TO_FOLDER_IMAGES_HTML_PAGES = PATH_TO_FOLDER_HTML_ELEMENT + FOLDER_NAME_IMAGE_HTML_PAGES + SEPARATOR;
        public static string PATH_TO_FOLDER_AUDIO_FILES = CURRENT_DIRECTORY + SEPARATOR + FOLDER_NAME_AUDIO + SEPARATOR;
        public static string PATH_TO_FOLDER_DOCUMENT = CURRENT_DIRECTORY + SEPARATOR + FOLDER_NAME_DOCUMENTS + SEPARATOR;
        public static string PATH_TO_FOLDER_HELP = PATH_TO_FOLDER_DOCUMENT + FOLDER_NAME_HELP + SEPARATOR;
        public static string PATH_TO_DEFAULT_STUDENT_PHOTO = PATH_TO_FOLDER_IMAGES_HTML_PAGES + PUBLIC_FOLDERS_NAME + SEPARATOR + NAME_DEFAULT_STUDENT_PHOTO;
        public static string PATH_TO_HTML_START_PAGE = PATH_TO_FOLDER_HTML_PAGES + PUBLIC_FOLDERS_NAME + SEPARATOR + NAME_HTML_START_PAGE;
        public static string PATH_TO_HTML_BANNED_PAGE = PATH_TO_FOLDER_HTML_PAGES + PUBLIC_FOLDERS_NAME + SEPARATOR + NAME_HTML_BANNED_PAGE;

        public const string AJAX_IMAGE_TAG = "im"; //ex : src="images.ashx?im=NameImage.png"
        public const string AJAX_AUDIO_TAG = "s";
        public const string AJAX_JAVASCRIPT_TAG = "javascriptFile";
        public const string AJAX_HTML_TAG = "htmlContent";
        public const string AJAX_DOWNLOAD_FILE = "downloadFile";

        public static string SQL_SERVER_DOMAIN_NAME = Settings.SqlServerDomainName;

        //Tags on website (OZE)
        public const string MALE_TAG = "M";
        public const string FEMALE_TAG = "MME";
        public const int ID_FEMALE = 1;
        public const int ID_MALE = 0;
        public static List<string> month_list = new List<string>(new string[] { "janvier", "fevrier", "mars", "avril", "mai", "juin", "juillet", "aout", "septembre", "octobre", "novembre", "decembre" });

        //student state file (for csv files)
        public const char CSV_COLUMN_SEPARATOR = ';';

        public const char SEPARATOR_ARRAY_TO_TEXT = '/';

        public const string USE_FOR_PUNISHMENT_LIST = "Punishment_list";
        public const string USE_FOR_MAILS_LIST = "Mails_list";
        public const string USE_FOR_EXIT_BAN_REASONS_LIST = "Exit_ban_reasons_list";

        //xml files
        public const string XML_TAG_FOR_ITEM = "item";
        public const string ELEMENT_TAG_XML_FILE = "field";
        public const string SECTION_TAG_XML_FILE = "div";
        public const string ATTRIBUTE_ELEMENT_XML_FILE = "name";

        public const int NO_ERROR_INT_VALUE = 0;
        public const int ERROR_INT_VALUE = -1;

        public const int VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE = 1;
        public const int NO_VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE = 0;
        public const int EXIT_UNAUTHORIZED_VALUE = 0;
        public const int EXIT_AUTHORIZED_UNCONDITIONALLY_VALUE = 1;
        public const int EXIT_AUTHORIZED_UNDER_CONDITION_VALUE = 2;

        public const int DELETE_VALUE_DATABASE = 1;
        public const int DO_NOT_DELETE_VALUE_DATABASE = 0;

        public const int TYPE_OF_STUDENT_TABLE = 0;
        public const int TYPE_OF_AGENT_TABLE = 1;
        public const int TYPE_OF_DIVISION_TABLE = 2;

        public const int PRECISION_BEFORE_EXIT_TIME_MINUTES = 20;//minutes
        public static TimeSpan PRECISION_BEFORE_EXIT_TIME = TimeSpan.FromMinutes(PRECISION_BEFORE_EXIT_TIME_MINUTES);


        public const string MODIFICATION_STUDENT_PHOTO_MESSAGE = "Modification de la photo de l'élève";
        public const string MODIFICATION_STUDENT_RFID = "Modification de l'identifiant RFID de l'élève";
        public const string MODIFICATION_STUDENT_FUSION = "Fusion de plusieurs fiches élèves";

        public static string RFID_FORMAT_CONDITIONS = "L’identifiant RFID ne doit pas être nul, "
                                                   + "ne doit pas être composé uniquement d'espace, "
                                                   + "doit contenir " + Settings.LengthRfidId + " caractères";
        public static string INCORRECT_PASSWORD_FORMAT_TEXT = "Le mot de passe doit contenir au moins "
                                                            + SecurityManager.MIN_CHAR_IN_PASSWORD + " caractères, "
                                                            + "une lettre majuscule, une lettre minuscule et un chiffre";

        public static string[] VALID_STUDENT_FILE_EXTENSION = new string[]{".csv"};
        public static string[] VALID_AUDIO_EXTENSION = new string[] { ".mp3" };

        public const int ID_COMPUTER_AS_AGENT = 1;
        /*The computer agent is a fake agent store into the database with the right -1, no id and no password*/

        public const int NUMBER_DAYS_BEFORE_CHECKING_UNTREATED_STUDENT_INFORORMATIONS = 7;

        public const string DATE_FORMAT = "dd-MM-yyyy";
        public const string TIME_FORMAT = "HH:mm:ss:fff";
        public const string TIMESPAN_FORMAT = "hh\\:mm";
        public const string DATETIME_FORMAT = DATE_FORMAT + " " + TIME_FORMAT;

        public const string TAG_FOR_LUNCH_TIME = "DÉJEUNER";
        public const char TIMESLOT_SEPARATOR = '-';
        public static List<string> DAYS_OF_THE_WEEK = new List<string> (){ "Dimanche", "Lundi","Mardi","Mercredi","Jeudi","Vendredi","Samedi" };

        public static DateTime DEFAULT_DATETIME = new DateTime(1, 1, 1);
        public static TimeSpan DEFAULT_TIMESPAN = new TimeSpan(0, 0, 0);

        public static Tools.TimeSlot LUNCH_TIME = Settings.LunchTime;

        /* Contains all the parameters needed to constrcut a student photo name into the correct format
         * Since the read of a system file is slow and since we need to access those data multiple time
         * before each change of those parameters we also update the static value
        */
        public static Tools.ParmsStudentPhoto PARMS_STUDENT_PHOTO = Settings.StudentPhotoParameters;
        //all possible text format that can be used
        public const int TEXT_DEFAULT_FORMAT = -1;
        public const int TEXT_FORMAT_UPPER_CASE = 0;
        public const int TEXT_FORMAT_LOWER_CASE = 1;
        public const int TEXT_FORMAT_NORMALIZE = 2;

    }


    class Tools
    {
        /*Class is used to saved all kind of usefull method that are not relative to a specific namespace*/

        public static string getOpenDialogFilter(string[] valid_formats)
        {
            /* When user have to choose a file, we open a file selector
             * Then we specify which extension file we want, according to the type of file needed
             * 
             * To simplify a potentiel modification, all the valid extension for each type of file are saved into lists
             * So to get the correct string format for the file selector object we create a function to return that string 
             * according the given extension list given
             * 
            */

            //For the file selectro the filter string is "File|*.extension_name1, *.extension_name2,..."
            string res = "Files|";
            for(int i=0;i< valid_formats.Length;i++)
            {
                res += "*" + valid_formats[i] + ";";

            }

            return res;
        }

        public static List<String> getTextSchoolDays()
        {
            /*Return a list of string that represent all the school days set previously by the user*/

            List<string> list_res = new List<string>();

            List<int> list_index = Settings.SchoolDays;
            //days of the week are stored as integer wichich represent the position of the day in the week
            foreach(int index in list_index)
            {
                if(index!=-1)
                {
                    list_res.Add(Definition.DAYS_OF_THE_WEEK[index]);
                   /*from the retrieved integer we get the correct day as text by using a list of saved days :
                    * For example the list can be ["Monday", "Thuesday", ....] and if the integer is 0 then the return value is "Monday"
                    * Notice that integer are saved according to the order of that list
                   */

                }
            }

            return list_res;
        }


        public static string dateTimeToString(DateTime date_time)
        {
            /*Convert a dateTime object into a string representation of that dateTime as date only (not the time) and according to the set format*/
            return date_time.ToString(Definition.DATETIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public static string dateToStringFromDateTime(DateTime date_time)
        {
            /*Convert a dateTime object into a string representation of that dateTime as date and time and according to the set format*/
            return date_time.ToString(Definition.DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public static string timeToStringFromTimeSpan(TimeSpan time)
        {
            /*Convert a TimeSpan object into a string representation of that TimeSpan according to the set format*/

            return time.ToString(Definition.TIMESPAN_FORMAT, CultureInfo.InvariantCulture);
        }

        public static TimeSpan stringToTimeSpan(string text_time)
        {
            /*Convert a string, into the same format as the TimeSpan are converted into string, into TimeSpan object*/

            TimeSpan time = Definition.DEFAULT_TIMESPAN;

            TimeSpan.TryParseExact(text_time, Definition.TIMESPAN_FORMAT, CultureInfo.InvariantCulture, TimeSpanStyles.None, out time);

            return time;
        }


        public static DateTime stringToDateTime (string text_date)
        {
            /*Convert a string, into the same format as the DateTime are converted into string, into DateTime object*/

            DateTime date = Definition.DEFAULT_DATETIME;

            if (DateTime.TryParseExact(text_date, Definition.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {//if the string represent a dateTime object (date and time are saved in the string)

            }
            else if (DateTime.TryParseExact(text_date, Definition.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {//if the string represent a date only

            }
            else if (DateTime.TryParseExact(text_date, Definition.TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {//if the string represent a time only

            }

            return date;//return a DateTime even if the string was a date only (then return date + time set to 0) or for a time date set to 0 + time)
        }

        public static string getFormatedStringForSessionInfo(string last_name, string first_name, string job)
        {
            /*From the agent last name, first name and job return a string that gather all that information*/

            return last_name.ToUpper() + " " + first_name.ToLower() + " (" + job.ToUpper() + ")";
        }

        public static string getPathInPrivateOrPublicDirectoryFromFileName(string[] possible_path_directories_for_file, string file_name)
        {
            /* Client does not have to be connected to the database to see the start page (and some other non essential content)
             * In order to know which content need to be registered to be seen or not in each folder we seperate files between 
             * two folders : one for public content (that can be seen by all other) and another for private content
             * (that can be seen only if the client is registered)
             * 
             * So when we want to send back a file we have to get the path and we don't know if the file is private or public
             * So we check if the file is in the public folder or in the private folder and send back the path
             * (check if path_to_folder + private_folder + file_name exist or path_to_folder + public_folder + file_name exist)
            */

            if(possible_path_directories_for_file.Length==0)
            {
                return "";
            }

            int i = 0;
            string path_to_file = "";
            do
            {
                path_to_file = possible_path_directories_for_file[i] + Definition.PUBLIC_FOLDERS_NAME + Definition.SEPARATOR + file_name;
                if (!File.Exists(path_to_file))
                {
                    path_to_file = possible_path_directories_for_file[i] + Definition.PRIVATE_FOLDERS_NAME + Definition.SEPARATOR + file_name;
                }

                if (!File.Exists(path_to_file))
                {
                    path_to_file = "";
                }

                i++;
            } while ((i < possible_path_directories_for_file.Length) && (path_to_file == ""));


            return path_to_file;

            //!File.Exists(directories_list[index] + Definition.PUBLIC_FOLDERS_NAME + Definition.SEPARATOR + fileName_list[index])))
        }

        public static bool fileIsPublic(string path_to_file)
        {
            /* In each file we create two folder : one public and the other private
             * So when a file is in the folder "Image" in fact it can be in "Image/public_folder_name" or "Image/private_folder_name"
             * That function return true if the given file (through its path) is considered as public (in a public folder) 
             * or private (not in a public folder)
             * 
             * In fact the file can be in a folder which is not public (and not private) but in a public file. In that case the file has also 
             * to be considered as public. However if the file is in a public folder whiwh is in a private folder (for some reason), the file must
             * be considered as private. Private attribute must always override public attribute
             * Moreover if the file is an a folder which is not private or public, by default the folder is considered as private
             * 
             * For example :
             *  path_to_file = "WindowsFormsApplication1\\WindowsFormsApplication1\\bin\\Debug\\html\\javascript\\public\\door_script.js" return true;
             *  path_to_file = "WindowsFormsApplication1\\WindowsFormsApplication1\\bin\\Debug\\private\\html\\javascript\\public\\door_script.js"" return false
             *  path_to_file = "WindowsFormsApplication1\\WindowsFormsApplication1\\bin\\Debug\\html\\javascript\\door_script.js"" return fasle
            */

            if (string.IsNullOrEmpty(path_to_file))
            {
                return false;
            }

            bool public_found = false;
            bool private_found = false;
            do
            {
                path_to_file = Path.GetDirectoryName(path_to_file);
                string[] sections = path_to_file.Split(Path.DirectorySeparatorChar);

                if (sections.Length != 0)
                {
                    if (sections[sections.Length - 1] == Definition.PUBLIC_FOLDERS_NAME)
                    {
                        public_found = true;
                    } else if (sections[sections.Length - 1] == Definition.PRIVATE_FOLDERS_NAME)
                    {
                        private_found = true;
                    }
                }

            }while ( (private_found!=true) && (path_to_file != "") && (path_to_file != Definition.CURRENT_DIRECTORY));
            /* while the file is not in a private directory or subdirectory included in a private directory
             * or there is no more directory to find
             * or we reach the main folder which conatains the app
            */

            if ( (!private_found) && (public_found))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public static bool isRfidFormatCorrect(string rfid)
        {
            /*Check if the given RFID "rfid" is correct From :
             *  return true is the string is non null and not empty
             *  else return false
           */
            if (rfid==null || rfid.Replace(" ", "").Length==0 || rfid.Length != Settings.LengthRfidId)
            {
                return false;
            }else
            {
                return true;
            }
        }

       
        public static string arrayToText<T>(T [] array)
        {
            if(array.Length==0)
            {
                return "";
            }

            string res = "";
            for (int i=0; i<array.Length-1;i++)
            {
                res += array[i].ToString() + Definition.SEPARATOR_ARRAY_TO_TEXT;
            }

            res += array[array.Length - 1];

            return res;
        }

        public static T[] textToArray<T>(string text_array)
        {
            string[] split_text = text_array.Split(Definition.SEPARATOR_ARRAY_TO_TEXT);
            T[] array = new T[split_text.Length];

            for(int i=0; i< split_text.Length;i++)
            {
                array[i] = (T)Convert.ChangeType(split_text[i], typeof(T));
            }

            return array;
        }


        public static int [] getNumericHalfBoardDaysFromString(string half_board_days)
        {
            /*When we read the student state file that contains all the informations about student (as
             * last name, fisrt name, division, sex and half-board days), the half-board days are stored in a specific format
             * as a string "half_board_days" (for example half_board_days="Mon-Thu-Fri")
             * This method take that string and return a integer array that represent the 7 days of a week
             * If at the corresponding day the student must stay at school to eat at lunch, then the value put in that array 
             * will be 1, else 0
             * The first element of the array correspond to sunday
             * For example if half_board_days="Mon-Thu-Fri", then the function return [0,1,0,0,1,1,0]
            */
            int size = 7;
            int no_half_board_day_value = 0;//value when student is external for lunch
            int half_board_day_value = 1; //value when student eat at school for lunch

            int[] tab_days = new int[size];//array that represent all the days of a week

            for(int i=0;i< size;i++)
            {
                tab_days[i] = no_half_board_day_value; //set value to 0 (student is external)
            }

            if (half_board_days == null)//if the student is external
            {
                return tab_days;
            }

            ParmsStudentsStateFile parms = Settings.StudentsStateFileParameters;
            string[] split_text = half_board_days.Split(parms.separatorDays);

            for(int i=0; i<split_text.Length;i++)
            //get each day of the string "half board days" through the used separator (like ' ' or '-', or '\n') for delimiting days
            {
                string day = split_text[i];
                if (day == parms.sundayShortname)
                {
                    tab_days[0] = half_board_day_value;
                }else if (day == parms.mondayShortname)//get the format that represent monday in the string (for example "Mon")
                {
                    tab_days[1] = half_board_day_value;
                }else if(day == parms.tuesdayShortname)
                {
                    tab_days[2] = half_board_day_value;
                }else if (day == parms.tuesdayShortname)
                {
                    tab_days[2] = half_board_day_value;
                }else if (day == parms.wednesdayShortname)
                {
                    tab_days[3] = half_board_day_value;
                }else if (day == parms.thursdayShortname)
                {
                    tab_days[4] = half_board_day_value;
                }else if (day == parms.fridayShortname)
                {
                    tab_days[5] = half_board_day_value;
                }else if (day == parms.saturdayShortname)
                {
                    tab_days[6] = half_board_day_value;
                }

            }
            return tab_days;
        }

        public static int copyFile(string source_path, string destination_path)
        {
            /*
             * Copy file from sourcePath to destinationPath (erase data if the file in destinationpath exists)
            */
            try
            {
                File.Copy(source_path, destination_path, true);
                return Definition.NO_ERROR_INT_VALUE;

            }
            catch (Exception e)
            {
                Error.details = "chemin d'accès au fichier à copier : " + source_path + "\ndestination : " + destination_path + "\n\n\n" + e;
                Error.error = "CCF";
                return Definition.ERROR_INT_VALUE;
            };
        }


        public static int deleteFile(string path)
        {
            /*
             * delete file from path "path"
            */
            try
            {
                File.Delete(path);
                return Definition.NO_ERROR_INT_VALUE;

            }catch(Exception e)
            {
                Error.details = "chemin d'accès au fichier : " + path + "\n\n\n" + e;
                Error.error = "CDF";
                return Definition.ERROR_INT_VALUE;
            };
        }


        public static string getStudentPhotoNameWithoutExtension(string last_name, string first_name, string division)
        {
            /*Return the name of a student photo (without extension) that follows the format
             * of each student photo in the script (in order to be used lated)
             * For example return "last_name_toUpper-first_name_toLower-division_toUpper"
             * 
             * The format has been set by the user previously
            */

            string res = "";
            string element_value = null;
            var tuple = Tuple.Create(-1,-1);
            int format = -1;
            int index = -1;
            string[] list_elements = new string[Definition.PARMS_STUDENT_PHOTO.getNumberElements()];
            /* the list will contain all th element ordered by their poisition into the final string
             * for example : ["lastName","Division","FirstName"] or ["lastName","FirstName","Division"] or ...
            */

            //local method
            Action applyFormatOnString = () =>
            {
                /* user specified if he want to transform the element from database (like the student first name) before using it for the student 
                 * photo name
                 * For example we can set the element to lowercase or to uppercase ...
                 * 
                 * Moreoever, all the possible text format are enconding as integer
                */
                switch (format)
                {
                    case Definition.TEXT_FORMAT_UPPER_CASE://uppercase
                        element_value = element_value.ToUpper();
                        break;
                    case Definition.TEXT_FORMAT_LOWER_CASE://lowercase
                        element_value = element_value.ToLower();
                        break;
                    case Definition.TEXT_FORMAT_NORMALIZE://normalized
                        element_value = Tools.removeDiacritics(element_value);
                        break;
                }
            };

            //Since the following actions are the same, we just use a function to write less lines
            Action addElement = () =>
            {
                //the function put the element value in the correct format and at the correct position 
                index = tuple.Item1;//position of the element into the final string
                format = tuple.Item2;//format of the element
                applyFormatOnString();//change the element value with the desired value
                list_elements[index] = element_value;//set the element value at the correct position
            };

            //set element 1
            tuple = Definition.PARMS_STUDENT_PHOTO.LastName;
            element_value = last_name;//value of the current element
            addElement();//add element to the list
            //set element 2
            tuple = Definition.PARMS_STUDENT_PHOTO.FirstName;
            element_value = first_name;
            addElement();
            //set element 3
            tuple = Definition.PARMS_STUDENT_PHOTO.Division;
            element_value = division;
            addElement();

            //gather all the elements into one final string
            for (int i = 0; i < list_elements.Length; i++)
            {
                res += list_elements[i];
                if (i != list_elements.Length - 1)
                {
                    res += Definition.PARMS_STUDENT_PHOTO.separator;//add the desired separator until we reach the end of the list
                }
            }

            return res;
        }


        public static string searchFileFromRootDirectory(string directory, string file_name_to_find)
        {
            /* With the given root directory "directory" browse (recursively) all the file in that directory and in its subdirectories
             * and return the path the file that have the patern "file_name_to_find"
            */

            try
            {
                /* this statement coul be inside the loop but it's imply that we can not check if the file is inside the first given directory
                 * (at the first call of that function)
                 * By using this statement before the loop wa can check if the directory contains the file, and if no, go inside another directory
                */
                var files = from file in
                  Directory.EnumerateFiles(directory)
                            where file.Contains(file_name_to_find)//search for the file patern
                            select file;

                if (files.Count<string>() != 0)
                {
                    /* In fact if "file_name_to_find" is null or empty the files collection will be empty
                     * So if the file name to find is null or empty we will be here just after
                     * 
                     * We could check at the beging of the function if the file name is null or empty
                     * but we will have to check that statement after each call of the function even if we know the file is not null or empty
                     * (because if it was at the first call the function would have return an empty string
                    */
                    if (string.IsNullOrEmpty(file_name_to_find))
                    {
                        return "";
                    }
                    //the correct file have been found
                    return files.ElementAt(0);
                }

                string path_to_file = "";
                foreach (string d in Directory.EnumerateDirectories(directory))
                {
                    path_to_file = searchFileFromRootDirectory(d, file_name_to_find);
                    if (path_to_file != "")
                    {
                        return path_to_file;
                    }
                }

                return "";

            }
            catch { return ""; }
        }

        public static string getStudentPhotoName(string last_name, string first_name, string division)
        {
            string photo_name = getStudentPhotoNameWithoutExtension(last_name, first_name, division);

            string path_to_photo = searchFileFromRootDirectory(Definition.PATH_TO_FOLDER_STUDENTS_PHOTOS, photo_name);

            if(path_to_photo!="")
            {
                return Path.GetFileName(path_to_photo);
            }else
            {
                return "";
            }

        }

        public static string getStudentPhotoPath(string last_name, string first_name, string division)
        {
            /*
             * return the name with extension of a specific student photo define by last name "last_name" , first name "first_name" and his division "division"
            */

            string photo_name = getStudentPhotoNameWithoutExtension(last_name, first_name, division);
            return searchFileFromRootDirectory(Definition.PATH_TO_FOLDER_STUDENTS_PHOTOS, photo_name);

        }
    

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        
        public static int sendMail(string subject, object html_body, List<string> mail_list)
        {
            /*Allow srcipts to send mail with SecureString data or with string data "html_body" */

            try
            {

                if (mail_list != null)
                {
                    ParmsMailsSender parms = Settings.MailsSenderParameters; //previous set informations about the way to send mail

                    MailMessage message = new MailMessage();

                    foreach (string mail in mail_list)
                    {
                        if (mail != null)
                        {
                            message.Bcc.Add(mail); //add all the mail adress as hidden copies
                        }
                    }

                    message.Subject = subject;
                    message.From = new MailAddress(parms.adress);//get adress of the sender, here the script
                    message.IsBodyHtml = true;

                    /*If a have confidential data to send it is better to contains all that data into a secure string until we send the data
                     * Even if all the data will be convert into string in the mail
                     * But if we have no confidential data, in order to avoir useless conversion, we just use string rather than SecureString
                    */
                    if (html_body is SecureString)//confidential data
                    {
                        message.Body = parms.beforeMessage + "<br><br><br><section><p>" +
                                       DataProtection.ToInsecureString((SecureString)html_body) +
                                       "</p></section><br><br><br><br>" + parms.afterMessage;
                        /* In fact all the secure data is now in the memory of the computer because of the conversion SecureString-string
                         * ( ToInsecureString() )but use a secureString to transmit data is better
                       */
                        ((SecureString)html_body).Dispose();
                    }
                    else if (html_body is string)//no confidential data
                    {
                        message.Body = parms.beforeMessage + "<br><br><br><section><p>" + (string)html_body + "</p></section><br><br><br><br>" + parms.afterMessage;
                    }
                    //ParmsMailsSender.before_message = message saved by the user to be displayed before each mail body
                    //ParmsMailsSender.after_message = message saved by the user to be displayed after each mail body

                    SmtpClient smtp = new SmtpClient(); //set SMTP server in oder to send the mail
                    smtp.Port = parms.smtpPort;//save SMTP port (by user)
                    smtp.Host = parms.smtpSever;//save SMTP host (by user)
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(parms.adress, DataProtection.unprotect(parms.password));
                    //get the password that has been encrypted before the saved in the file

                    try
                    {
                        smtp.Send(message);//send mail
                        return Definition.NO_ERROR_INT_VALUE;
                    }
                    catch (Exception e)
                    {
                        Error.details = "Contenu du mail non envoyé : " + html_body + "\n\n" + e;
                        Error.error = "CSM";
                        return Definition.ERROR_INT_VALUE;
                    }
                    finally
                    {
                        smtp.Dispose();
                    }

                }

            }
            catch { }

            return Definition.ERROR_INT_VALUE;
        }


        public static string normalizeString(string input)
        {
            /*From the given string "input" return a string without space and to uppercase*/

            if (input !=null)
            {

                return input.Replace(" ", "").ToUpper();
            }else
            {
                return null;
            }
        }

        public static int SexIntFromString(string sex, string female_reference)
        {
            /* All sex informations are saved into the database as in integer
             * With a string reference of the sex (for exemple "F" or "Fe" ...)
             * return the correct integer value corresponding to that sex
            */

            if (normalizeString(sex) == normalizeString(female_reference))
            {
                return Definition.ID_FEMALE;
            }
            else
            {
                return Definition.ID_MALE;
            }
        }

        public static int getNumericMonthFromString(string month)
        {
            /*From plain text month return the numerical value of that month
             * For example for month="january" return 1, for month="juin" return 6
            */
            month = month.ToLower();
            month.Replace(" ", "");
            month = removeDiacritics(month);

            return Definition.month_list.LastIndexOf(month) + 1;
        }



        public static string removeDiacritics(string text)
        {
            /*The function take out the French accent marks in the letters while keeping the letter
             * For exemple : "é" becomes "e"
             * from : https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
            */

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


        public static string removeNonAlphaChar(string text)
        {
            string res = "";

            foreach (var c in text.ToLower())
            {
                if ( (c>='a') && (c<='z') )
                {
                    res += "" + c;
                }else
                {
                    res += " ";
                }
            }

            return res;
        }


        public static string normalizeText(string text)
        {
            return removeNonAlphaChar(removeDiacritics(text));
        }


        public static int readStudentsStateFile()
        {
            ParmsStudentsStateFile parms = Settings.StudentsStateFileParameters;
            try
            {
                DataBase database = new DataBase();

                string path_to_file = Definition.PATH_TO_FOLDER_DOCUMENT + Settings.StudentSateFileName;
                int count;
                int res;
                string line = "";
                string[] values;
                List<StudentData> list_student = new List<StudentData>();
                StudentData student = new StudentData();

                using (var reader = new StreamReader(path_to_file, Encoding.GetEncoding("iso-8859-1")))
                {
                    reader.ReadLine(); //drop out the first line of the file which is made for description

                    count = 1;
                    student.toDefault();
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        values = line.Split(Definition.CSV_COLUMN_SEPARATOR);


                        if (count%100 == 0)//we save student into the database block of n students by block of n students at once
                        {
                            res = database.updateStudentTable(list_student);
                            if(res != Definition.NO_ERROR_INT_VALUE)
                            {
                                Error.details = "Ligne du fichier correspondante : " + line;
                                Error.error = "CASITDB";
                                return res;
                            }
                            list_student.Clear();
                            count = 1;
                        }

                        student.toDefault();
                        student.lastName = values[parms.lastNameIndex];//Last name (string)
                        student.firstName = values[parms.firstNameIndex];// First name (string)
                        student.division = values[parms.divisionIndex]; // division (string)
                        student.sex = Tools.SexIntFromString(values[parms.sexIndex], parms.femaleShortname); //sex (int)
                        student.halfBoardDays = Tools.getNumericHalfBoardDaysFromString(values[parms.halfBoardDaysIndex]); //half-board regime (string)

                        list_student.Add(student);

                        count++;
                    }

                    if (list_student.Count != 0)//we save student into the database block of 200 students by block of 200 students at once
                    {
                        res = database.updateStudentTable(list_student);
                        if (res != Definition.NO_ERROR_INT_VALUE)
                        {
                            Error.details = "Ligne du fichier correspondante : " + line;
                            Error.error = "CASITDB";
                            return res;
                        }
                    }
                }
                return Definition.NO_ERROR_INT_VALUE;

            }
            catch (Exception e)
            {
                Error.details = "" + e;
                Error.error = "CATSSF";
            }

            return Definition.ERROR_INT_VALUE;

        }

        public static void writeUrlServerToFile(string url)
        {
            /*Since the server can change its ip adress or more often its port
             * local client have to know exactly the changes. 
             * So, we put the local url into an html page that will open the new url
             * The function just write html file that redirect the user to the stored (in the html file) url
            */

            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            try
            {
                string html_text = "<html><script>window.location.href = \"" + url + "\";</script></html>";
                File.WriteAllText(Settings.getPathToUrlFile(), html_text);

            }
            catch (Exception e)
            {
                Error.details = "" + e;
                Error.error = "CWIFFURLS";
            }
        }




        public static string getStringFromIntegerDay(int day_of_the_week)
        {
            if( (day_of_the_week>=0) && (day_of_the_week <= Definition.DAYS_OF_THE_WEEK.Count) )
            {
                return Definition.DAYS_OF_THE_WEEK[day_of_the_week];
            }
            else
            {
                return "";
            }
        }


        public static string getLocalWiredIPAddress()
        {
            /*Function that find the actual local ip adress (IPV4 for wired connexion)
             * If can't find any element return null 
            */

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //search for IPV4
                {
                    return ip.ToString();
                }
            }

            Error.error = "NNAWIPv4AIS";
            return "";
        }

        public static int findNewPort()
        {
            /*That function find an available port
             * If for some reasons the function can't find a port return 0.
             * 0 is a default value that asks windows to find a port for that application
            */

            try
            {
                TcpListener l = new TcpListener(IPAddress.Loopback, 0);
                l.Start();
                int port = ((IPEndPoint)l.LocalEndpoint).Port;
                l.Stop();

                return port;
            }
            catch { }

            return 0;
        }



        public struct StudentData
        {
            /*Structure used to easily manipulate the student information from the database*/

            public int tableId;
            //id of the student used as primary key in the corresponding table ("TABLE_ELEVES") in the database
            public string lastName; //las name
            public string firstName; //first name
            public string division; //division 
            public int sex; //sex as integer
            public string idRFID; //RFID id
            public int[] halfBoardDays;
            /*array that contains represent all the days of a working week and indicate at the corresponding index 
             * if the student eat for lunch at school (value equal to 1) or not (equal to 0)
            */
            public bool error;

            public void toDefault()
            {
                tableId = -1;
                lastName = "";
                firstName = "";
                division = "";
                sex = -1;
                idRFID = "";
                halfBoardDays = new int[] { };
                error = false;
            }

            public string getSexToString()//convert the integer of sex to string
            {
                if (sex == Definition.ID_FEMALE)
                {
                    return "féminin";//female
                }
                else if (sex == Definition.ID_MALE)
                {
                    return "masculin";//male
                }
                else
                {
                    return "";
                }
            }

            public string getFormatedStudentInfo()
            {
                string res = "";
                res += "Nom :" + lastName.ToUpper();
                res += "\nPrénom : " + firstName.ToLower();
                res += "\nClasse : " + division.ToUpper();
                res += "\nSexe : ";
                if (sex == Definition.ID_FEMALE)
                {
                    res += "SEXE FEMME";
                }
                else
                {
                    res += "SEXE HOMME";
                }
                res += "\nIdentifiant RFID : " + idRFID;

                return res;
            }

        }

        public struct Agent
        {
            public int tableId;//index into database
            public string lastName;
            public string firstName;
            public string job;
            public string mailAdress;
            private int right;//accreditation level
            public string id;
            public string hashedPassword;
            public string saltForHashe;
            public string shortSecureId;
            private int delete;
            /*If user want to delete an agent in fact we can not delete it from database because the index of that
             * agent into the database is used in other table to know which agent change value in the database
             * (for example if the agent associate with the index 12 allow a student to leave the school, then
             * in the corresponding table the row corresponding to that student will be associate with the index 12
             * to show that that agent let that student leave the school
             * 
             * So if we delete that agent from database the index 12 will no more corresponding to that agent
             * So if user want to delete an agent from database we set the value delete to DELETE_AGENT_VALUE
             * and update that value in the database.
             * Then when user will make somme research we will retrive all the agent that have not the delete value set to DELETE_AGENT_VALUE
             * So for the user that agent will be deleted
             * 
             * This is only when we remove all the log (which indicate that that agent let that student leave the school) that we delete from 
             * the database all agents which have the delete field set to DELETE_AGENT_VALUE
            */
            public string ipAdress;
            public string sessionId;
            public DateTime lastConnectionTime;
            public DateTime banishmentStartTime;
            public int numberOfConnectionAttemps;
            public bool banned;
            public bool error;


            public void toDefault()
            {
                tableId = -1;
                lastName = "";
                firstName = "";
                job = "";
                mailAdress = "";
                right = -1;
                id = "";
                hashedPassword = "";
                saltForHashe = "";
                shortSecureId = "";
                delete = Definition.DO_NOT_DELETE_VALUE_DATABASE;//default value is to not delete agent
                ipAdress = "";
                sessionId = "";
                banishmentStartTime = new DateTime();
                lastConnectionTime = new DateTime();
                numberOfConnectionAttemps = 0;
                banned = false;
                error = false;
            }

            public int getDeleteAgentValue()
            {
                return delete;
            }

            public void deleteAgent()
            {
                delete = Definition.DELETE_VALUE_DATABASE;
            }

            public void setRigth(int right_value)
            {
                right = right_value;
            }

            public string rightToString()
            {
                if( (right>=0) && (right< SecurityManager.RIGHTS_LIST.Count) )
                {
                    return SecurityManager.RIGHTS_LIST[right];
                }else
                {
                    return "";
                }
                
            }

            public void setIntRightFromString(string right_text)
            {
                right = SecurityManager.RIGHTS_LIST.LastIndexOf(right_text);
            }

            public int getRight()
            {
                return right;
            }

        }


        public struct StudyTime
        {
            /*Structure used to store the study time of the school*/

            public TimeSpan startTime;
            public TimeSpan endTime;
            public bool isLunchTime;
            public string ItemToText;//string that represent the study time slot
            public bool error;//represent if an error occures when the object was initialize

            public void toDefault()
            {
                startTime = new TimeSpan(0, 0, 0);
                endTime = new TimeSpan(0, 0, 0);
                isLunchTime = false;
                error = false;
                ItemToText = "";
            }

            public void setTextItem()
            {
                ItemToText = Tools.timeToStringFromTimeSpan(startTime) + " - " + Tools.timeToStringFromTimeSpan(endTime);
                if (isLunchTime)
                {
                    ItemToText += " (" + Definition.TAG_FOR_LUNCH_TIME + ")";
                }
            }
        }

        public struct TimeSlot
        {
            /*Structure that store time slot in general*/

            public int id;//could be used
            public DateTime startDate;
            public DateTime endDate;
            public int dayOfWeek;
            public TimeSpan startTime;
            public TimeSpan endTime;
            public string metaText;//could be used
            public bool error;//represent if an error occures when the object was initialize
            public bool readOnly;

            public void toDefault()
            {
                id = -1;
                startDate = new DateTime(1, 1, 1);
                endDate = new DateTime(1, 1, 1);
                dayOfWeek = 1;
                startTime = new TimeSpan(0, 0, 0);
                endTime = new TimeSpan(0, 0, 0);
                metaText = "";
                error = false;
                readOnly = false;
            }
        }

        public struct ParmsStudentPhoto
        {
            /*Structure use to deal with the element that compose each student photo name*/

            private int[] orderElements;
            /* 0 : last name
             * 1 : first name
             * 2 : division
            */
            private int[] formatElements;
            public char separator;
            public Tuple<int, int> LastName
            {
                get{return getElement(0);}
                set{ setElement(0, value.Item1, value.Item2);}
            }
            public Tuple<int, int> FirstName
            {
                get { return getElement(1); }
                set { setElement(1, value.Item1, value.Item2); }
            }
            public Tuple<int, int> Division
            {
                get { return getElement(2); }
                set { setElement(2, value.Item1, value.Item2); }
            }
            

            public void toDefault()
            {
                orderElements = new int[3];
                formatElements = new int[3];
                separator = ' ';
            }

            private void setElement(int index, int order, int format)
            {
                orderElements[index] = order;
                formatElements[index] = format;
            }

            private Tuple<int, int> getElement(int index)
            {
                return Tuple.Create(orderElements[index], formatElements[index]);
            }

            public int[] ElementsOrder
            {
                get { return orderElements; }
                set { orderElements = value; }
            }

            public int[] ElementsFormat
            {
                get { return formatElements; }
                set { formatElements = value; }
            }

            public int getNumberElements()
            {
                return orderElements.Length;
            }

        }


        public struct ParmsStudentsStateFile
        {
            /* Have a desired order for a list is not convinient for later use because we have to know that specific order
             * To avoid that problem we create a struct that deals with that order and let us just using the name of 
             * the desired element (as lastName, firstName, ...)
            */

            public int lastNameIndex;
            public int firstNameIndex;
            public int divisionIndex;
            public int sexIndex;
            public int halfBoardDaysIndex;
            public string femaleShortname;
            public string maleShortname;
            public string mondayShortname;
            public string tuesdayShortname;
            public string wednesdayShortname;
            public string thursdayShortname;
            public string fridayShortname;
            public string saturdayShortname;
            public string sundayShortname;
            public char separatorDays;


            public void toDefault()
            {
                lastNameIndex = -1;
                firstNameIndex = -1;
                divisionIndex = -1;
                sexIndex = -1;
                halfBoardDaysIndex = -1;
                femaleShortname = null;
                maleShortname = null;
                mondayShortname = null;
                tuesdayShortname = null;
                wednesdayShortname = null;
                thursdayShortname = null;
                fridayShortname = null;
                saturdayShortname = null;
                sundayShortname = null;
                separatorDays = ' ';
            }

        }



        public struct ParmsMailsSender
        {
            /* Have a desired order for a list is not convinient for later use because we have to know that specific order
             * To avoid that problem we create a struct that deals with that order and let us just using the name of 
             * the desired element (as lastName, firstName, ...)
            */

            public string adress;
            public string password;
            public string smtpSever;
            public int smtpPort;
            public string beforeMessage;
            public string afterMessage;

            public void toDefault()
            {
                adress = null;
                password = null;
                smtpSever = "";
                smtpPort = 0;
                beforeMessage = "";
                afterMessage = "";
            }

        }

    }


    class Settings
    {
        /* All needed data that have no plcae in database and which are related to the settings of the app like the Ip adress of the server, the port,...
         * are store in a single XML file
         * This class allow us to change a value in that file and to get a value from that file easily
         * 
         * In fact all the job are done in the "XmlHelper" class which read the file and make modification
         * But to get it correct we have to specify the type of XML element we have to create, the XML tag we want, ect
         * So all that kind of informations are stored inside that class and used to get or change value in the XML value
        */

        private const string XML_ATTRIBUTE_VALUE_IP = "Ip_server";
        //attribute of the corresponding XML element for ip adress : <field name="Ip_server" Ip_server=""></field>
        private const string XML_ATTRIBUTE_VALUE_PORT = "Port_server";
        //attribute of the corresponding XML element for port
        private const string XML_ATTRIBUTE_VALUE_PATH_URL_FILE = "Path_url_acess_file";
        private const string XML_ATTRIBUTE_VALUE_PUNISHMENT_WORDS_LIST = "Punishment_words_list";
        private const string XML_ATTRIBUTE_VALUE_MAILS_LIST = "Mails_list";
        private const string XML_ATTRIBUTE_VALUE_MAILS_SENDER_PARMS = "List_parameters_for_mails_sender";
        private const string XML_ATTRIBUTE_VALUE_COLOR_NON_CLOSED_PUNISHMENT = "Color_non_closed_punishment";
        private const string XML_ATTRIBUTE_VALUE_LENGTH_EXIT_BREAK = "Length_exit_break";
        public static TimeSpan EXIT_BREAK_NOT_ENABLED_TIMESPAN_VALUE = TimeSpan.Parse("24:00:00");
        private const string XML_ATRIBUTE_VALUE_STUDENTS_STATE_FILE_PARMS = "List_correspondance_columns_state_student_file";
        private const string XML_ATRIBUTE_VALUE_MAILS_PARMS = "List_parameters_for_sending_mails";
        private const string XML_ATTRIBUTE_VALUE_COLUMN_LAST_NAME = "Last_name";
        private const string XML_ATTRIBUTE_VALUE_COLUMN_FIRST_NAME = "First_name";
        private const string XML_ATTRIBUTE_VALUE_COLUMN_DIVISION = "Division";
        private const string XML_ATTRIBUTE_VALUE_COLUMN_SEX = "Sex";
        private const string XML_ATTRIBUTE_VALUE_COLUMN_HALF_BOARD_DAYS = "Half_board_days";
        private const string XML_ATTRIBUTE_VALUE_FEMALE_STUDENTS_STATE_FILE = "Female";
        private const string XML_ATTRIBUTE_VALUE_MALE_STUDENTS_STATE_FILE = "Male";
        private const string XML_ATTRIBUTE_VALUE_MONDAY_STUDENTS_STATE_FILE = "Mon";
        private const string XML_ATTRIBUTE_VALUE_TUESDAY_STUDENTS_STATE_FILE = "Tue";
        private const string XML_ATTRIBUTE_VALUE_WEDNESDAY_STUDENTS_STATE_FILE = "Wed";
        private const string XML_ATTRIBUTE_VALUE_THURSDAY_STUDENTS_STATE_FILE = "Thu";
        private const string XML_ATTRIBUTE_VALUE_FRIDAY_STUDENTS_STATE_FILE = "Fri";
        private const string XML_ATTRIBUTE_VALUE_SATURDAY_STUDENTS_STATE_FILE = "Sat";
        private const string XML_ATTRIBUTE_VALUE_SUNDAY_STUDENTS_STATE_FILE = "Sun";
        private const string XML_ATTRIBUTE_VALUE_SEPARATOR_DAYS_STUDENTS_STATE_FILE = "Separator_days";
        private const string XML_ATTRIBUTE_VALUE_MAIL_ADRESS = "Mail_adress";
        private const string XML_ATTRIBUTE_VALUE_MAIL_PASSWORD = "Password";
        private const string XML_ATTRIBUTE_VALUE_MAIL_SERVER_NAME_SMTP = "SMTP_sever";
        private const string XML_ATTRIBUTE_VALUE_MAIL_SMTP_PORT = "SMTP_port";
        private const string XML_ATTRIBUTE_VALUE_MAIL_BEFORE_MESSAGE = "Before_message";
        private const string XML_ATTRIBUTE_VALUE_MAIL_AFTER_MESSAGE = "After_message";
        private const string XML_ATTRIBUTE_VALUE_LENGTH_RFID_ID = "Length_rfid_id";
        private const string XML_ATTRIBUTE_VALUE_DATE_LAST_SUPPRESSION_DATABASE = "Date_last_suppression_from_database";
        private const string XML_ATTRIBUTE_VALUE_STUDY_TIME = "Study_time";
        private const string XML_ATTRIBUTE_VALUE_LUNCH_TIME = "Lunch_time";
        private const string XML_ATTRIBUTE_VALUE_SCHOOL_DAYS = "School_days";
        private const string XML_ATTRIBUTE_VALUE_FILE_NAME_ERROR_AUDIO = "Error_audio_name";
        private const string XML_ATTRIBUTE_VALUE_FILE_NAME_STUDENT_STATE_FILE = "Student_state_file";
        private const string XML_ATTRIBUTE_VALUE_SCHOOL_NAME = "School_name";
        private const string XML_ATTRIBUTE_VALUE_LIST_PARMS_STUDENT_PHOTO = "Student_photo_parms";
        private const string XML_ATTRIBUTE_VALUE_ORDER_ELEMENTS_STUDENT_PHOTO = "Order_element";
        private const string XML_ATTRIBUTE_VALUE_FORMAT_ELEMENTS_STUDENT_PHOTO = "Format_element";
        private const string XML_ATTRIBUTE_VALUE_SEPARATOR_STUDENT_PHOTO = "Separator";
        private const string XML_ATTRIBUTE_VALUE_DOMAIN_NAME_SQL_SERVER_FOR_DATABASE = "Sql_server_domain_name";
        private const string XML_ATTRIBUTE_VALUE_EXIT_BAN_REASONS_LIST = "Exit_ban_reasons_list";


        public static string ServerIpAdress
        {
            /*By using the path to the XML file define by the constant expression PATH_TO_TOOLS_FILE
             * and using the desired xml attribute (like XML_ATTRIBUTE_IP or XML_ATTRIBUTE_Port)
             * the function change the acutal value of the corresponding element by the new string value ipl
            */
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_IP);
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                       Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_IP, value);
            }

        }


        public static int ServerPort
        {
            /*By using the path to the XML file define by the constant expression PATH_TO_TOOLS_FILE
             * and using the desired xml attribute (like XML_ATTRIBUTE_IP or XML_ATTRIBUTE_Port)
             * the function change the acutal value of the corresponding element by the new object value port
             * In fact, port would be an integer but most of time we grab its value from TextBox through a string
             * So requeried no specific object type is more flexible
            */

            get
            {
                int port = -1;
                string text = XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_PORT);
                int.TryParse(text, out port);

                return port;
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                       Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_PORT, value);
            }
        }


        public static DateTime DateLastSuppressionFromDatabase
        {
            /*By using the path to the XML file define by the constant expression PATH_TO_TOOLS_FILE
             * and using the desired xml attribute (like XML_ATTRIBUTE_IP or XML_ATTRIBUTE_Port)
             * the function read the value of the correesponding XML element
             * 
             * If for some reasons the function cannot read the value of that eleent, return null
            */

            get
            {
                string text = XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_DATE_LAST_SUPPRESSION_DATABASE);

                return Tools.stringToDateTime(text);
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                            XML_ATTRIBUTE_VALUE_DATE_LAST_SUPPRESSION_DATABASE, Tools.dateTimeToString(value));
            }
            
        }



        public static string ColorNonClosedPunishment
        {
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_COLOR_NON_CLOSED_PUNISHMENT);
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                      Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_COLOR_NON_CLOSED_PUNISHMENT, value);
            }
           
        }


        public static string SqlServerDomainName
        {
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_DOMAIN_NAME_SQL_SERVER_FOR_DATABASE);
            }

            set
            {
                Definition.SQL_SERVER_DOMAIN_NAME = value; //update value into the app

                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                           Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_DOMAIN_NAME_SQL_SERVER_FOR_DATABASE, value);
            }
        }

        public static TimeSpan ExitBreak
        {
            get
            {
                string text = XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                                Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LENGTH_EXIT_BREAK);
                return Tools.stringToTimeSpan(text);
            }

            set
            {
                string text_time = Tools.timeToStringFromTimeSpan(value);
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LENGTH_EXIT_BREAK, text_time);
            }

        }

        public static bool isExitBreakEnabled(TimeSpan length_exit_break)
        {
            return length_exit_break != EXIT_BREAK_NOT_ENABLED_TIMESPAN_VALUE;
            //check if the timeSPan object is set to the defautl timeSpan
        }


        public static void setPathToUrlFile(string path_to_folder, string file_name)
        {
            /*The current acces to server (using ip adress and port) are stored in a shared .txt file
             * In that way, all client app just have to read that file to acces to server
             * The path to that shared file is stored in the XML settings file
             * 
             *That function change the path in the XML settings file
            */
            string path = "";
            if (!string.IsNullOrEmpty(path_to_folder))
            {
                path = path_to_folder + Path.DirectorySeparatorChar + file_name +".html";
            }

            XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                       XML_ATTRIBUTE_VALUE_PATH_URL_FILE, path);
        }

        public static string getPathToUrlFile()
        {
            /*The current acces to server (using ip adress and port) are stored in a shared .txt file
             * In that way, all client app just have to read that file to acces to server
             * The path to that shared file is stored in the XML settings file
             * 
             * Return null if we can't acces to the XML element with attribute XML_ATTRIBUTE_PATH_URL_FILE
            */

            return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_PATH_URL_FILE);
        }

        public static List<string> PunishmentWords
        {
            /* In the website all the punishments on a calendar are recognizabled by their name
             * Somes punishments need to be done in the school but not all, so we have to take care of
             * the type of these punishments. In other words, we have to know what type of 
             * punishments need to be done insise the school.
             * All the name oh these punishments (organized as a list) are store in an XML file, 
             * which will be used later
             *
             * So by the given list "table", the function change the old list with that punishments type
             * stored in the XML file by the new one
            */

            get
            {
                /* In the website all the punishments on a calendar are recognizabled by their name
             * Somes punishments need to be done in the school but not all, so we have to take care of
             * the type of these punishments. In other words, we have to know what type of 
             * punishments need to be done insise the school.
             * All the name oh these punishments (organized as a list) are store in an XML file, 
             * which will be used later
             *
             * So by the function return the list as List<string> of all the current punishments words stored in the XML file
             * Else, return null
            */


                return XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                                    XML_ATTRIBUTE_VALUE_PUNISHMENT_WORDS_LIST, Definition.XML_TAG_FOR_ITEM);
            }

            set
            {
                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                          XML_ATTRIBUTE_VALUE_PUNISHMENT_WORDS_LIST, Definition.XML_TAG_FOR_ITEM, value);
            }
           
        }




        public static List<int> SchoolDays
        {
            get
            {
                List<int> list_res = new List<int>();

                List<string> list_text_days = XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                               XML_ATTRIBUTE_VALUE_SCHOOL_DAYS, Definition.XML_TAG_FOR_ITEM);

                foreach (string day in list_text_days)
                {
                    int index = -1;
                    int.TryParse(day, out index);

                    if (index != -1)
                    {
                        list_res.Add(index);
                    }
                }

                return list_res;
            }

            set
            {
                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                           XML_ATTRIBUTE_VALUE_SCHOOL_DAYS, Definition.XML_TAG_FOR_ITEM, value);
            }
            
        }



        public static List<string> MailsList
        {
            get
            {
                return XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                                XML_ATTRIBUTE_VALUE_MAILS_LIST, Definition.XML_TAG_FOR_ITEM);
            }

            set
            {
                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                           XML_ATTRIBUTE_VALUE_MAILS_LIST, Definition.XML_TAG_FOR_ITEM, value);
            }
            
        }




        public static Tools.ParmsStudentsStateFile StudentsStateFileParameters
        {
            
            get
            {
                Tools.ParmsStudentsStateFile parms = new Tools.ParmsStudentsStateFile();
                List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_COLUMN_LAST_NAME,
                XML_ATTRIBUTE_VALUE_COLUMN_FIRST_NAME, XML_ATTRIBUTE_VALUE_COLUMN_DIVISION,XML_ATTRIBUTE_VALUE_COLUMN_SEX,
                XML_ATTRIBUTE_VALUE_COLUMN_HALF_BOARD_DAYS,XML_ATTRIBUTE_VALUE_FEMALE_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_MALE_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_MONDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_TUESDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_WEDNESDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_THURSDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_FRIDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_SATURDAY_STUDENTS_STATE_FILE, XML_ATTRIBUTE_VALUE_SUNDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_SEPARATOR_DAYS_STUDENTS_STATE_FILE });

                List<string> elements = XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                              XML_ATRIBUTE_VALUE_STUDENTS_STATE_FILE_PARMS, Definition.XML_TAG_FOR_ITEM,
                                              Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);

                try
                {
                    parms.lastNameIndex = Int32.Parse(elements[0]);
                    parms.firstNameIndex = Int32.Parse(elements[1]);
                    parms.divisionIndex = Int32.Parse(elements[2]);
                    parms.sexIndex = Int32.Parse(elements[3]);
                    parms.halfBoardDaysIndex = Int32.Parse(elements[4]);
                    parms.femaleShortname = elements[5];
                    parms.maleShortname = elements[6];
                    parms.mondayShortname = elements[7];
                    parms.tuesdayShortname = elements[8];
                    parms.wednesdayShortname = elements[9];
                    parms.thursdayShortname = elements[10];
                    parms.fridayShortname = elements[11];
                    parms.saturdayShortname = elements[12];
                    parms.sundayShortname = elements[13];
                    parms.separatorDays = (char)Int32.Parse(elements[14]);

                }
                catch(Exception e)
                {
                    parms.toDefault();
                }


                return parms;
            }

            set
            {
                /* All the informations about students in the school as their last names, first names, divisions, sexs and their half-board days
            * are saved in a local CSV file. Each columns of that file contains those informations. In order to grab the correct informations,
            * we have to know which column matches with which information (for example last name = column 0, first name=column 1, ...).
            * Important : the count of the columns start to 0 to be directly used after
            * 
            * All the half-board days are stored in the same column with a specific format for each day decrypted 
            * thanks to variables "shortname_*" (for example shortname_monday="Mon", shortname_tuesday"Tue",...) 
            * and all that days are separated by a specific char (for example : "Mon-Wed-Fri" or "Mon/Wed/Fri" ...) stored in "separator_days"
            * 
            * Sex of student are represented in a specific format decrypted by "shortname_female" and "shortname_male"
            * (for example shortname_female="F" and shortname_male="M")
           */
                List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_COLUMN_LAST_NAME,
                XML_ATTRIBUTE_VALUE_COLUMN_FIRST_NAME, XML_ATTRIBUTE_VALUE_COLUMN_DIVISION,XML_ATTRIBUTE_VALUE_COLUMN_SEX,
                XML_ATTRIBUTE_VALUE_COLUMN_HALF_BOARD_DAYS,XML_ATTRIBUTE_VALUE_FEMALE_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_MALE_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_MONDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_TUESDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_WEDNESDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_THURSDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_FRIDAY_STUDENTS_STATE_FILE,XML_ATTRIBUTE_VALUE_SATURDAY_STUDENTS_STATE_FILE,
                XML_ATTRIBUTE_VALUE_SUNDAY_STUDENTS_STATE_FILE, XML_ATTRIBUTE_VALUE_SEPARATOR_DAYS_STUDENTS_STATE_FILE });

                List<string> element = new List<string>(new string[] { "" + value.lastNameIndex, ""+value.firstNameIndex,
                ""+value.divisionIndex, ""+value.sexIndex, ""+value.halfBoardDaysIndex,value.femaleShortname,value.maleShortname,
                value.mondayShortname,value.tuesdayShortname,value.wednesdayShortname,value.thursdayShortname,value.fridayShortname,
                value.saturdayShortname,value.sundayShortname,""+(int)value.separatorDays});

                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                              XML_ATRIBUTE_VALUE_STUDENTS_STATE_FILE_PARMS, Definition.XML_TAG_FOR_ITEM,
                                              element, Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);
            }
        }


        public static Tools.ParmsStudentPhoto StudentPhotoParameters
        {

            get
            {
                Tools.ParmsStudentPhoto parmsStudentPhoto = new Tools.ParmsStudentPhoto();
                
                List<string> text_list = XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE,
                    Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LIST_PARMS_STUDENT_PHOTO, Definition.XML_TAG_FOR_ITEM);

                try
                {
                    parmsStudentPhoto.ElementsOrder = Tools.textToArray<int>(text_list[0]);
                    parmsStudentPhoto.ElementsFormat = Tools.textToArray<int>(text_list[1]);
                    parmsStudentPhoto.separator = (char)(Int32.Parse(text_list[2]));

                }catch
                {
                    parmsStudentPhoto.toDefault();
                }

                return parmsStudentPhoto;
            }

            set
            {
                //update the current value saved into the RAM memory
                Definition.PARMS_STUDENT_PHOTO = value;


                List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_ORDER_ELEMENTS_STUDENT_PHOTO,
                XML_ATTRIBUTE_VALUE_FORMAT_ELEMENTS_STUDENT_PHOTO, XML_ATTRIBUTE_VALUE_SEPARATOR_STUDENT_PHOTO });

                List<string> element = new List<string>(new string[] { Tools.arrayToText(value.ElementsOrder), Tools.arrayToText(value.ElementsFormat),  "" + (int)value.separator });

                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                              XML_ATTRIBUTE_VALUE_LIST_PARMS_STUDENT_PHOTO, Definition.XML_TAG_FOR_ITEM,
                                              element, Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);

            }
        }



        public static Tools.ParmsMailsSender MailsSenderParameters
        {

            get
            {

                Tools.ParmsMailsSender parms = new Tools.ParmsMailsSender();

                List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_MAIL_ADRESS,XML_ATTRIBUTE_VALUE_MAIL_PASSWORD,
                XML_ATTRIBUTE_VALUE_MAIL_SERVER_NAME_SMTP,XML_ATTRIBUTE_VALUE_MAIL_SMTP_PORT, XML_ATTRIBUTE_VALUE_MAIL_BEFORE_MESSAGE,
                XML_ATTRIBUTE_VALUE_MAIL_AFTER_MESSAGE});

                List<string> elements = XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                              XML_ATTRIBUTE_VALUE_MAILS_SENDER_PARMS, Definition.XML_TAG_FOR_ITEM, Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);

                try
                {
                    parms.adress = elements[0];
                    parms.password = elements[1];
                    parms.smtpSever = elements[2];
                    parms.smtpPort = Int32.Parse(elements[3]);
                    parms.beforeMessage = elements[4];
                    parms.afterMessage = elements[5];
                }catch
                {
                    parms.toDefault();
                }


                return parms;
            }

            set
            {
              /* All the informations about students in the school as their last names, first names, divisions, sexs and their half-board days
              * are saved in a local CSV file. Each columns of that file contains those informations. In order to grab the correct informations,
              * we have to know which column matches with which information (for example last name = column 0, first name=column 1, ...).
              * Important : the count of the columns start to 0 to be directly used after
              * 
              * All the half-board days are stored in the same column with a specific format for each day (for example "Mon", "Tue",...) 
              * and are separated by a char (for example : "Mon-Wed-Fri" or "Mon/Wed/Fri" ...)
             */

                List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_MAIL_ADRESS,XML_ATTRIBUTE_VALUE_MAIL_PASSWORD,
                XML_ATTRIBUTE_VALUE_MAIL_SERVER_NAME_SMTP,XML_ATTRIBUTE_VALUE_MAIL_SMTP_PORT, XML_ATTRIBUTE_VALUE_MAIL_BEFORE_MESSAGE,
                XML_ATTRIBUTE_VALUE_MAIL_AFTER_MESSAGE});

                List<string> element = new List<string>(new string[] { value.adress, value.password, value.smtpSever, "" + value.smtpPort,
                value.beforeMessage, value.afterMessage });

                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                              XML_ATTRIBUTE_VALUE_MAILS_SENDER_PARMS, Definition.XML_TAG_FOR_ITEM,
                                              element, Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);
            }
            
        }

        public static int LengthRfidId
        {
            get
            {
                string text = XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LENGTH_RFID_ID);
                int res = 0;
                int.TryParse(text, out res);

                return res;
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LENGTH_RFID_ID, value);
            }
        }



        public static string SchoolName
        {
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                             Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_SCHOOL_NAME);
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                             Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_SCHOOL_NAME, value);
            }
        }


        public static string ErrorAudioFileName
        {
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                             Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_FILE_NAME_ERROR_AUDIO);
            }
            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_FILE_NAME_ERROR_AUDIO, value);
            }
            
        }


        public static string StudentSateFileName
        {
            get
            {
                return XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                             Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_FILE_NAME_STUDENT_STATE_FILE);
            }

            set
            {
                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                            Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_FILE_NAME_STUDENT_STATE_FILE, value);
            } 
        }


        public static Tools.TimeSlot LunchTime
        {
            get
            {
                Tools.TimeSlot time = new Tools.TimeSlot();
                time.toDefault();

                string text = XmlHelper.readXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                                Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LUNCH_TIME);

                string[] hours = text.Split(Definition.TIMESLOT_SEPARATOR);
                if (hours.Length == 2)
                {
                    time.startTime = Tools.stringToTimeSpan(hours[0]);
                    time.endTime = Tools.stringToTimeSpan(hours[1]);
                }
                else
                {
                    time.error = true;
                }

                return time;
            }

            set
            {
                Definition.LUNCH_TIME = value;
                /* In fact we store lunch time in a separate array that other slo time (like study time) because we will need to use it very often
                 * and to access it very fast
                 * And To increase the access speed we store it in the RAM memory as a static member of a class
                 * It's avoid to parse and read a system file (which is slower that get an object in the RAM memory)
                 * The inconvenient is that once the memeber is initialize we can not update the value when the user change the value
                 * Thus, when the user change the value we store that new value in the file, as usual, but we also update the static member
                */
                string text_to_save = Tools.timeToStringFromTimeSpan(value.startTime) + Definition.TIMESLOT_SEPARATOR + Tools.timeToStringFromTimeSpan(value.endTime);

                XmlHelper.changeXMLElement(Definition.PATH_TO_TOOLS_FILE, Definition.ELEMENT_TAG_XML_FILE,
                                                Definition.ATTRIBUTE_ELEMENT_XML_FILE, XML_ATTRIBUTE_VALUE_LUNCH_TIME, text_to_save);
            }
        }


        public static List<Tools.StudyTime> StudyHours
        {
            get
            {
                List<Tools.StudyTime> list_res = new List<Tools.StudyTime>();
                List<string> temp_list = getStudyHoursToText();

                foreach (string item in temp_list)
                {
                    Tools.StudyTime time = new Tools.StudyTime();
                    time.toDefault();

                    if (item.Contains(Definition.TAG_FOR_LUNCH_TIME))
                    {
                        /* In fact lunch time are not stored inside the same XML element as the study hours, because the app will have to use it often
                         * So we save the lunch time in a separate object to have a faster acces to it
                        */
                        Tools.TimeSlot lunch_time = LunchTime;
                        time.startTime = lunch_time.startTime;
                        time.endTime = lunch_time.endTime;
                        time.isLunchTime = true;
                        time.setTextItem();

                    }
                    else
                    {
                        string[] hours = item.Split(Definition.TIMESLOT_SEPARATOR);
                        if (hours.Length == 2)
                        {
                            time.startTime = Tools.stringToTimeSpan(hours[0]);
                            time.endTime = Tools.stringToTimeSpan(hours[1]);
                        }
                        else
                        {
                            time.error = true;
                        }

                        time.setTextItem();
                    }

                    if (!time.error)
                    {
                        list_res.Add(time);
                    }
                }

                return list_res;
            }

            set
            {
                List<Tools.StudyTime> list_time = value.OrderBy(t => t.startTime).ToList();//sort by the first element (the start time) of each sub element

                List<string> list_res = new List<string>();

                foreach (Tools.StudyTime time in list_time)
                {
                    if (time.isLunchTime)
                    {
                        list_res.Add(Definition.TAG_FOR_LUNCH_TIME);

                        Tools.TimeSlot lunch_time = new Tools.TimeSlot();
                        lunch_time.toDefault();
                        lunch_time.startTime = time.startTime;
                        lunch_time.endTime = time.endTime;

                        LunchTime = lunch_time;
                    }
                    else
                    {
                        list_res.Add(Tools.timeToStringFromTimeSpan(time.startTime) + Definition.TIMESLOT_SEPARATOR + Tools.timeToStringFromTimeSpan(time.endTime));
                    }
                }


                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                                XML_ATTRIBUTE_VALUE_STUDY_TIME, Definition.XML_TAG_FOR_ITEM, list_res);
            }
            
        }



        public static List<string> getStudyHoursToText()
        {

            return XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                            XML_ATTRIBUTE_VALUE_STUDY_TIME, Definition.XML_TAG_FOR_ITEM);

        }



        public static List<string> ExitBanReasonsList
        {
            get
            {
                return XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                                XML_ATTRIBUTE_VALUE_EXIT_BAN_REASONS_LIST, Definition.XML_TAG_FOR_ITEM);
            }

            set
            {
                XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                           XML_ATTRIBUTE_VALUE_EXIT_BAN_REASONS_LIST, Definition.XML_TAG_FOR_ITEM, value);
            }
        }



        public static void changeErrorAudioFile(string path_new_file)
        {
            /*
             * Allow user to change the actual mp3 file that is played when student can't leave the school
             * The destination file will remain the same and espacially the name of the file stored in an XML file
            */

            string file_name = Definition.NAME_ERROR_AUDIO + Path.GetExtension(path_new_file);
            ErrorAudioFileName = file_name;

            Tools.copyFile(path_new_file, Definition.PATH_TO_FOLDER_AUDIO_FILES + Definition.PUBLIC_FOLDERS_NAME + Definition.SEPARATOR + file_name);
           
        }


        public static int changeStudentStateFile(string path_new_file)
        {
            string file_name = Definition.NAME_STUDENTS_STATE_FILE + Path.GetExtension(path_new_file);
            StudentSateFileName = file_name;

            Tools.copyFile(path_new_file, Definition.PATH_TO_FOLDER_DOCUMENT + file_name);

            return Tools.readStudentsStateFile(); //refresh the database
        }


        public static void changeStudentPhoto(int student_table_index, string path_new_photo, string formated_photo_name, string photo_extension)
        {
            /*
             * Allow user to change the actual photo of a specific student
             * "formated_photo_name" is not the name of the actual photo pointed by user but the
             * name (with extension) that follows the format of all other photo in the script
             * and then update the modification date of the student information into the database
            */

            Tools.copyFile(path_new_photo, Definition.PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS +  formated_photo_name + photo_extension);

            DataBase database = new DataBase();
            database.updateStudentModificationTime(student_table_index);
            database.writeToRegiter(Definition.TYPE_OF_STUDENT_TABLE, student_table_index, SecurityManager.getConnectedAgentTableIndex(), 
                Definition.MODIFICATION_STUDENT_PHOTO_MESSAGE);
           
        }


    }
    


}
