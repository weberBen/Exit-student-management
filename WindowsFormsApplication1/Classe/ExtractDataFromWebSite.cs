using System;
using System.Globalization;
using System.Collections.Generic;

using ToolsClass;
using StudentData = ToolsClass.Tools.StudentData;
using TimeSlot = ToolsClass.Tools.TimeSlot;

namespace ExtractDataFromWebSite
{
    public class UpdateStudentInfoFromOnlineDataBase
    {
        public bool process_error;
        //when all the process if finished set the value to false if there was no error, else set to true (by default true)
        public string message_to_send;
        public TimeSpan exit_break_timespan;
        private DataBase database;

        public UpdateStudentInfoFromOnlineDataBase()
        {
            process_error = false;
            message_to_send = "";
            exit_break_timespan = Settings.ExitBreak;
            database = new DataBase();

            database.purgeRegularExitsTable();


            //<-------- insert all the require function to run automatically all the procees of data scraping
        }
    
    }

}