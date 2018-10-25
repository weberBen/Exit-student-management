using System;
using System.Globalization;
using System.Collections.Generic;

using ToolsClass;
using StudentData = ToolsClass.Tools.StudentData;
using TimeSlot = ToolsClass.Tools.TimeSlot;

/* The website use lot of javascript to built dynamic element
 * So this can be hard to get all the require information only with post and get request
 * In other words, if you want you can use a webbrowser control which can be include in a form
 * In this case, the form must have the name : "OnlineDatabaseForm" and user must not be able to interect with the form
 * (or precisely with the webbrowser control)
 * 
 * If possible avoid using SendKeys.Send() but if it's necessarily the form must open in full screen and user must not be able to rezize it (just leave
 * the close option)
*/
namespace ExtractDataFromWebSite
{
    public class UpdateStudentInfoFromOnlineDataBase
    {
        public bool process_error;
        //when all the process if finished set the value to false if there was no error, else set to true (by default true)
        public string message_to_send;
        public TimeSpan exit_break_timespan;
        private const int TIMEOUT_PROCESS_MINUTES = 60;

        public UpdateStudentInfoFromOnlineDataBase()
        {
            process_error = false;
            message_to_send = "";
            exit_break_timespan = Settings.ExitBreak;
            DataBase database = new DataBase();

            database.purgeRegularExitsTable();

            //<-------- insert all the require function to run automatically all the procees of data scraping
        }

        public void function()
        {
            //----------------------------------------- INFO -----------------------------------------
            /* If all the process take too much time the methodes need to be terminated
             * The maximum time accorded to that process is 80% of the timer time which start that process
             * For example if that process is started every hour, then it have to be done before the app start a new instance of that process
             * so the current process need to be terminated after 0.8*1 hour = 48 minutes
             * Here the maximum time accorded is "TIMEOUT_PROCESS_MINUTES"
            */

            if (false)//if timeout
            {
                process_error = true;
                message_to_send += "La mise à jour de la base de données à partir de la source en ligne a pris trop de temps. Le processus a dû être arrêté prématurement.";
                return;
            }

            //url website https://enc.hauts-de-seine.fr/fr/dashboard

            //-------------------------------------- connexion to the website
            var secureUserDataForWebSite = DataProtection.readSecureDataConnexionWebsite();//return Tuple<SecureString,SecureString)
            string id = DataProtection.ToInsecureString(secureUserDataForWebSite.Item1);//get plain text ID
            string password = DataProtection.ToInsecureString(secureUserDataForWebSite.Item2);//get plain text password
            /* the variable "id" and "password" are just set to explain the result be should not use in the code
             * Try to use directly the statement DataProtection.ToInsecureString(...) inside the needed function
             * And try to limitate the use of thoses statements
            */

            //<----------------------- do something

            secureUserDataForWebSite.Item1.Dispose();
            secureUserDataForWebSite.Item2.Dispose();

            if (false)//if connexion to the website fail
            {
                process_error = true;
                message_to_send += "Impossible de se connecter au site web pour récolter des doonées";
                return;
            }

            //-------------------------------------- Browse website information about student


            /* In fact there is there are few ways to execute the process but one is more secure for our app :
             * get all the student from our own database and then find that student on the website
            */

            /* "StudentData" is the strcuture use to store data linked to a student:
             *      * public parameters (int) : tableId -> return the table index of the student
             *      * public method : getSexToString() -> return a string that return the sex of the student
             *      * public method : getFormatedStudentInfo() -> gather all information about the student into a string
             *      * public parameters (string) : lastName -> student last name
             *      * public parameters (string) : firstName -> student first name
             *      * public parameters (string) : division -> student division
             *      * public parameters (int)    : sex -> student sex as integer
             *      
             *      
             * About the student sex :
             *      * integer value of female sex : Definition.ID_FEMALE (int)
             *      * integer value of male sex : Definition.ID_MALE (int)
             *      * string value of female sex (which is used in the website) : Definition.FEMALE_TAG (string)
             *      * string value of male sex (which is used in the website) : Definition.MALE_TAG (string)
            */

            /* To get all the student from tha database we have few options. One could be to load all the student into a list (in the RAM)
             * but this method is way too much ressouces and time consuming (because all the student are already saved into the disk)
             * So we will retrieve all the student one by one. To do that, we have to open the database, ask for the good request and then read the read the result
             * 
             * The first method "void startStudentEnumeration(void)" open the database (as a private paramaters of the class DataBase)
             * Then to get the next student "bool getNextStudentFromEnumeration(ref StudentData)" which set the object StudentData with the value 
             * of the following student. The method return false when we reach the "end" of the database
             * At least, to close the database use "void endStudentEnumeration(void)"
             * Example :
             *      Database db = new Database();
             *      db.startStudentEnumeration();
             *      StudentData student = new Tools.StudentData();
             *      while (db.getNextStudentFromEnumeration(ref student))
             *      {
             *          Console.WriteLine("last name : " + student.lastName + " - first name : " + student.firstName + " - division : " + student.division);
             *      }
             *      db.endStudentEnumeration();
             *      
             * The instance of the class Database used to open the database with "startStudentEnumeration()" must be the same that is used to the other method
             * because the data are local to that instance.
            */


            List<StudentData> list_student_not_updated = new List<StudentData>();
            //save all the student object corresponding to the students that can not have been updated from the website


            DataBase database = new DataBase();
            StudentData student = new StudentData();

            database.startStudentEnumeration();

            while (database.getNextStudentFromEnumeration(ref student))
            {
                if(student.tableId == -1)//something wrong happened
                {
                    continue;
                }
                TimeSpan start_exit_break = new TimeSpan(0, 0, 0);
                TimeSpan end_exit_break = new TimeSpan(0, 0, 0);


                string last_name = student.lastName;
                string first_name = student.firstName;
                string division = student.division;
                string sex = student.getSexToString();
                //All division, last name, first name, sex (as string) are in theory the same as on the website

                //---------------------> search the student on the website with the needed data

                if (false)//if the student is not found on the website
                {
                    process_error = true;
                    list_student_not_updated.Add(student);
                    continue;//start agin the loop with the next object in the list
                }


                for (int k = 0; k < 0; k++)//for each hour in the student schedule
                {
                    /* The student schedule need to be browse here
                     * (I will refer to a time slot as an "hour" to simplify the probleme. But in fact this is not a real hour composed 
                     * of 60 minutes, it can be 1h30 or 2h,...)
                     * 
                     * For each hour into that schedule we need to check : 
                     *  - if the is a regular study hour (-> we do nothing for that case)
                     *  - if this is a free hour (a non study hour)
                     *  - if this is a cancel hour (the teacher is absent)
                     *  - if this is an unjustified absence
                     *  - if this is a planned absence (the school was aware of that absence)
                     *  - if this is an non validate punishment (a punishment planned where the student did not came)
                     *  - if this a an actual punishement (for example at 3PM student need to come in school to validate is punishement)
                     *  
                     *  According to the online database (the website) all those informations can be usable on the student schedule
                     *  or it can be in separates section of the website. Then the process can change but the action made
                     *  for each case (previously listed) must be the same.
                     *  
                     *  All hour that is not a study hour must be process like an study hour (espacially for punishements in the actual day)
                     *  
                     *  There are differents types of punishment. But only few require the student to stay at school. Those type of 
                     *  punishement can be retrieved with Settings.getPunishmentWords() (List<string>). The name of punishements are stricly 
                     *  the same as on the website.
                     *  Also the non validate punishements are visible on the schedule because its used a specific color (something likre red)
                     *  which can be retieved with Settings.getColorNonClosedPunishment() (string)
                    */

                    //-------------------------------------- Free hour -------------------------------------------------------- 
                    if (Settings.isExitBreakEnabled(exit_break_timespan))//check if the school enable the functionality
                    {
                        /* I cannot write a general method for this action because its depend on how you organize the whole process
                         * The aim is to detect if the student have a BLOCK (+/- Defintion.PRECISION_BEFORE_EXIT_TIME_MINUTES minutes between each hour) 
                         * of couple free hours (hours when student does not have 
                         * lesson or does not have punishment). 
                         * The "couple free hours" is exactly the TimeSpan object "meridan_break_timespan"
                         * 
                         * For exemple if meridan_break_timespan <=> 01h30 and PRECISION_BEFORE_EXIT_TIME_MINUTES=20 
                         * and if the student have those free hours :
                         *  - 14:05 - 14:55
                         *  - 15:05 - 16:00
                         *  
                         *  Then when we sum all those free hours is equal to 01h45 whiwh is superior to 01h30
                         *  So student can leave the school from at 14:05
                         *  
                         *  But if the free hours was :
                         *   - 14:05 - 14:55
                         *   - 15:20 - 16:15
                         * Then the student does not have a break where they can leave the school (a break that we called an exit break) 
                         * and so can not leave the school at 14:05 (or 15:20) because the are 25 minutes from 14:55 to 15:20 
                         * which is strictly superior to Defintion.PRECISION_BEFORE_EXIT_TIME_MINUTES the comparaison between 25 minutes 
                         * and Defintion.PRECISION_BEFORE_EXIT_TIME_MINUTES must be strcit 
                         * ( if(25 minutes > Definition.PRECISION_BEFORE_EXIT_TIME_MINUTES) and not if(25 minutes >= Definition.PRECISION_BEFORE_EXIT_TIME_MINUTES) )
                         * (if you need to create timeSpan from Definition.PRECISION_BEFORE_EXIT_TIME_MINUTES, we can use 
                         * Definition.PRECISION_BEFORE_EXIT_TIME (timeSpan))
                         * 
                         * If there the block of free hours potentially contains multiple exti break, all the exit breaks must be considered 
                         * as multiple exit breaks (and not a unique long exit break)
                         * 
                         * In theory if the teacher is absent or delete his class it would be considered as an free hour but it's must be check with the school
                         */


                        /* To get the actual time slot of the exit break we set two timeSpan "start_exit_break", which 
                         * represent the start in the time of the exit break and "end_exit_break", which represent the end
                         * in the time of the exit break
                         * 
                         * At the first loop into the student schedule, we initialize the two variable at the same value ("00:00:00")
                         * which means that for the moment the exit break has not been set
                         * Then when we find the first free hour in the student schedule we set "start_exit_break" to the start of that free hour
                         * and "end_exit_break" to the end of that free hour.
                         * 
                         * Then when a new free hour has been identified we check if that free hour is close enough to the previous one in order 
                         * to form a "block", close enough to consider the two hours as a continuous time sequence. If this is the case,
                         * the we set "end_exit_break" to the end of the actual free hour
                         * 
                         * Then we check if the length of the time sequence (which is made by subtract the end of the potential exit break
                         * by its start) is long enough to be consider as a exit break (the length of the exit break is determined by the 
                         * school). If this is the case, then we create a timeSlot object of those time points and add it to a list "list_free_hours"
                         * Then, we reset the whole thing by comming back to the start point : the start of the exit break is equal to
                         * the end of the exit break.
                         * In other case we do nothing
                         */

                        TimeSpan start_time = new TimeSpan(); //must be the start of the current time slot on the schedule
                        TimeSpan end_time = new TimeSpan(); //must be the end of the current time slot on the schedul

                        if (start_exit_break == end_exit_break)//no free hour has been set yet
                        {
                            start_exit_break = start_time;
                            end_exit_break = end_time;
                        }
                        else if (false)
                        {//if the actual time slot is a free hour

                            if (Math.Abs(start_time.Subtract(end_exit_break).TotalMinutes) <= Definition.PRECISION_BEFORE_EXIT_TIME_MINUTES)
                            {
                                /* we check here if the current time slot (which is a free hour for the student) is close enough (in the time) 
                                 * to the previous free hour
                                */
                                end_exit_break = end_time;
                            }
                        }
                        if ((end_exit_break.Subtract(start_exit_break) >= exit_break_timespan)
                            && !(start_exit_break >= Definition.LUNCH_TIME.startTime && end_exit_break <= Definition.LUNCH_TIME.startTime)
                           )
                        {//check if the break is not exclusively included in the regular exit break
                            //save the data
                            database.setDailyExitForStudent(student.tableId, start_exit_break,
                                start_exit_break.Add(TimeSpan.FromMinutes(30)), Definition.VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE);

                            //reset the previous exit break start
                            start_exit_break = end_exit_break;
                        }
                    }


                    //------------------------------- Last study hour of the day -------------------------------------
                    if (false)
                    {
                        /*If this is the last study hour (which include all hour dedicate to punishement or other stuff)*/
                        TimeSpan start_time = new TimeSpan();//start of the last study hour of the student
                        TimeSpan end_time = TimeSpan.ParseExact("23:59:59", "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                        //don't change the value
                        int verification_value;
                        if (false)//if this is the regular last hour on the student schedule
                        {
                            verification_value = Definition.NO_VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE;
                        }
                        else//if the last study hour is a delete study hour or because a teacher is absente or something else (need to be seen with the school)
                        {
                            verification_value = Definition.VERIFICATION_REQUIRED_FOR_STUDENT_EXIT_VALUE;
                        }
                        database.setDailyExitForStudent(student.tableId, start_time, end_time, verification_value);
                    }


                    //---------------------------- Unjustified absence or non validate punishement ---------------------------------------------

                    /* Unjustified absence means all absence that as not been terminated by an agent (if the absence has a reason like 
                     * being sick this day but has not been validate by an agent it must be process like an unjustified absence
                     * 
                     * We take in consideration only absences that is start few days from today (to let the school time to validate and check the thing)
                     * Same thing for non validate punishement
                     * 
                   */

                    DateTime date = new DateTime(); //must be the date of the unjustified absence or non validate punishement
                    if (date.AddDays(Definition.NUMBER_DAYS_BEFORE_CHECKING_UNTREATED_STUDENT_INFORORMATIONS) < DateTime.Now)
                    {
                        string reason = "";
                        if (false)//unjustified absence
                        {
                            reason = "BLOCAGE AUTOMATIQUE : absence non cloturée datant du : " + Tools.dateToStringFromDateTime(date);
                        }
                        else //non validate punishement
                        {
                            reason = "BLOCAGE AUTOMATIQUE : punition non cloturée datant du : " + Tools.dateToStringFromDateTime(date);
                        }

                        //transform the unjustified absence or non validate punishement into an exit ban for the whole current day
                        int res = database.setStopForStudent(Definition.TYPE_OF_STUDENT_TABLE, student.tableId, Definition.ID_COMPUTER_AS_AGENT, reason);
                        if (res != Definition.NO_ERROR_INT_VALUE)
                        {
                            process_error = true;
                            message_to_send += "ERREUR - ELEVE : " + student.getFormatedStudentInfo();
                            message_to_send += "Impossible d'enregistrer dans la base de donnée l'object ayant pour sujet : \n" + reason;
                        }
                    }
                }//end loop hours student Schedule

            }//end loop throught database
            database.endStudentEnumeration();

            if (list_student_not_updated.Count != 0)
            {
                message_to_send += "Les élèves suivant n'ont pas pu être trouvé sur la base de donnée en ligne :\n";
                foreach (StudentData stud in list_student_not_updated)
                {
                    message_to_send += "          ";
                    message_to_send += stud.getFormatedStudentInfo();
                }

                return;
            }

        }

    }

}