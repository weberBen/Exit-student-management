/* Open source software under the CeCILL (Copyleft) designed by Benjamin WEBER
 * (05/09/2018)
 * 
 * The software uses the newtonsoft json librairy which includes the following licences :
 * 
 * <<The MIT License (MIT)
 * Copyright (c) 2007 James Newton-King
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.>>
 * 
*/

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using WindowsFormsApplication1.Forms;
using System.Timers;
using System.Security;

using ToolsClass;


//4e47cf6kp4XP5zuw1dQ6epCoMjOPgMvc42qgT
/*fonction a modifier pour le site web :
 *  DisposeAllNonCriticalForms() -> SecurityManager
 *  Timer_Tick() -> SecurityManager
*/

namespace WindowsFormsApplication1
{


public partial class MainForm : Form
{
    
        LocalServer.HttpServer LocalServer = new LocalServer.HttpServer(); //class that deal with the local server

        private const int TIME_OUT_VALUE = 30000; //ms
        private Color CUSTOM_RED = Color.FromArgb(251, 80, 80);
        private Color CUSTOM_GREEN = Color.FromArgb(179, 255, 179);
        private Color CUSTOM_ORANGE = Color.FromArgb(255, 140, 26);
        private static System.Timers.Timer timer_collect_data;
        private const int TIMER_BEFORE_COLLECT_DATA_MIN = 60;//minutes
        private const int TIMER_INTERVAL_COLLECT_DATE = TIMER_BEFORE_COLLECT_DATA_MIN * 60000;//ms


        public MainForm()
        {
            InitializeComponent();

            this.displayEventTextBox.TextAlign = HorizontalAlignment.Center;
            this.FormClosing += this.form_OnClosing;//event handler on form closing

            if (LocalServer.Start() != Definition.NO_ERROR_INT_VALUE) //can't start the local server
            {
                changeBanner("Impossible de démarrer le serveur", CUSTOM_RED);
            }else
            {
                changeBanner("Serveur démarré avec succès", CUSTOM_GREEN);
            }

            /* User has to been connected in order to use the app physically
             * And each user have an accreditation level that does not cover all the possible action
             * Thus for each allowed action we register the need accreditation level
           */
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                switch (item.Name.ToLower()) 
                {
                    case "update":
                        item.Tag = SecurityManager.RIGHTS_LIST[0];
                        break;
                    case "settings":
                        item.Tag = SecurityManager.RIGHTS_LIST[0];
                        break;
                    case "administration":
                        item.Tag = SecurityManager.RIGHTS_LIST[1];
                        break;
                    case "schoollifeoffice":
                        item.Tag = SecurityManager.RIGHTS_LIST[2];
                        break;
                }

                foreach (ToolStripDropDownItem sub_item in item.DropDownItems)
                {
                    //set event handler on click for subitem in the item of the menu strip
                    sub_item.Click += new System.EventHandler(this.submenuItem_Click);
                }
            }


            SecurityManager.setConnection("", null);

            daily_tasks();

            /*DELETE ----------------------------------------------------------------------------------------------------------------------------*/
            /*
            SecureString password = new SecureString();
                foreach(char c in "admin")
                {
                    password.AppendChar(c);
                }
                password.MakeReadOnly();

                SecureString short_secure_id = new SecureString();
                foreach (char c in "0000")
                {
                    short_secure_id.AppendChar(c);
                }
                short_secure_id.MakeReadOnly();

                string salt = DataProtection.getSaltForHashes();
                string hash_password = DataProtection.getSaltHashedString(password, salt);
                string encrypted_short_id = DataProtection.protect(short_secure_id);

                DataBase dataBase = new DataBase();
                Tools.Agent agent = new Tools.Agent();

                agent.toDefault();
                agent.lastName = "Poste";
                agent.firstName = "Informatisé";
                agent.firstName = "ordinateur";
                agent.setRigth(-1);
                dataBase.addNewAgent(agent);

                agent.toDefault();

                agent.firstName = "admin";
                agent.lastName = "admin";
                agent.job = "DEUS EX";
                agent.setRigth(0);
                agent.mailAdress = "none";
                agent.id = "admin";
                agent.hashedPassword = hash_password;
                agent.saltForHashe = salt;
                agent.shortSecureId = encrypted_short_id;

                dataBase.addNewAgent(agent);

                password.Dispose();
                short_secure_id.Dispose();
            /*DELETE ----------------------------------------------------------------------------------------------------------------------------*/

        }


        private void form_OnClosing(Object sender, FormClosingEventArgs e)
        {
            changeBanner("Fermeture du serveur en cours", CUSTOM_ORANGE);

            Task t = Task.Run(() => LocalServer.Dispose()); //wait for the closing of the local server
            t.Wait(TimeSpan.FromMilliseconds(TIME_OUT_VALUE));
        }

        private void daily_tasks()
        {
            /*task that have to be made at the start of the app*/

            DataBase database = new DataBase();

            //set timer
            timer_collect_data = new System.Timers.Timer();
            timer_collect_data.Interval = TIMER_INTERVAL_COLLECT_DATE;
            timer_collect_data.Elapsed += new ElapsedEventHandler(collectData); ; //event timer tick
            timer_collect_data.Start();

            DateTime date = ToolsClass.Settings.DateLastSuppressionFromDatabase;

            database.purgeRegularExitsTable();//remove regular exit for all student

            //check if the database was not clean for a while
            if (date.Add(TimeSpan.FromDays(SecurityManager.TIME_BEFORE_SUPPRESSION_OF_DATA_FROM_DATABASE_DAYS)) < DateTime.Now)
            {
                File.WriteAllText(Definition.PATH_TO_LOG_ERROR_FILE, ""); //reset file where error log were saved
                database.clearDatabase();

                ToolsClass.Settings.DateLastSuppressionFromDatabase = DateTime.Now;
            }
        }

        private void collectData(object source, ElapsedEventArgs e)
        {
            /* Populate the database with onlide data about student
             * The process must be repeat frequently
            */

            ExtractDataFromWebSite.UpdateStudentInfoFromOnlineDataBase CollectData = new ExtractDataFromWebSite.UpdateStudentInfoFromOnlineDataBase();
            if(CollectData.process_error)//an error occurs during the collect of the data
            {
                string subject = "Erreur critique (collecte des données en ligne)";
                Tools.sendMail(subject, CollectData.message_to_send, ToolsClass.Settings.MailsList);
                //send information to all agent who wanted to be alerted
                Error.details = CollectData.message_to_send;
                Error.error = "EWCOD";
            }

            timer_collect_data.Start();
            //restart the timer
        }


        private void restartLocalServer()
        {
            /*Restart the app if user want to restart the server 
                * (Notthe best way to deal with that but avoid long quit script)
            */
            Process.Start(Application.ExecutablePath); // to start new instance of application
            this.Close(); //to turn off current app
        }


        private void changeBanner(string text_to_display, Color color)
        {
            this.displayEventTextBox.Text = text_to_display;
            this.displayEventTextBox.BackColor = color;
        }

        public void endPreviousSession()
        {
            display_session_textBox.Text = "";
            id_textBox.Text = "";
            password_textBox.Text = "";

        }


        private void disconnection_button_Click(object sender, EventArgs e)
        {
            SecurityManager.disconnection();
            endPreviousSession();
        }


        private void connexion_button_Click(object sender, EventArgs e)
        {
            SecureString password = new SecureString();
            foreach (char c in password_textBox.Text)
            {
                password.AppendChar(c);
            }
            password.MakeReadOnly();


            if (SecurityManager.setConnection(id_textBox.Text, password) == Definition.NO_ERROR_INT_VALUE)
            {
                MessageBox.Show("Bienvenu " + SecurityManager.getFormatedSessionInfo());
            }else
            {
                MessageBox.Show("Vous n'avez pas les droits nécessaire pour accéder à cette application");
            }

            if (password != null)
            {
                password.Dispose();
            }

            endPreviousSession();
            display_session_textBox.Text = SecurityManager.getFormatedSessionInfo();
        }




        private void submenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clicked_menu_item = sender as ToolStripMenuItem;

            if (clicked_menu_item.Name != "nomDeDomainDuServerSQL")
            { /*In fact if the user want to change the domaine name of the sql server he could not anymore connect to the database
               *because the server name has changed (and users need to access database when they try to set a connection with the database
               */
                int needed_right = SecurityManager.RIGHTS_LIST.LastIndexOf(clicked_menu_item.OwnerItem.Tag.ToString());
                if (!SecurityManager.rightLevelEnoughToAccesSystem(SecurityManager.getConnectedAgentRight(), needed_right))
                {
                    if (SecurityManager.getFormatedSessionInfo() == "")
                    {
                        endPreviousSession();
                    }
                    MessageBox.Show("Vous n'avez pas les droits nécessaires pour accéder à cette option");
                    return;
                }
            }


            switch (clicked_menu_item.Name)
            {
                /*----------------------------------  Update  ---------------------------------- */
                case "rafraichirLaBaseDeDonnées":
                    {
                        string message;
                        if (Tools.readStudentsStateFile() == Definition.NO_ERROR_INT_VALUE)
                        {
                            message = "Mis à jour effectuée";
                        }
                        else
                        {
                            message = "La mise à jour a été interrompu suite à une erreur\n"
                                    + "Vérifier que le fichier n'a pas utilisé par un autre processus "
                                    + "et que les paramètres enregistrés sont valides";
                        }

                        MessageBox.Show(message);

                    }
                    break;

                case "purgerLaBaseDeDonnées":
                    {
                        string title = "Demande de confirmation";
                        string body = "Etes-vous certain(e) de vouloir purger la base de données ?\nAttention : cette action est irréversible!";

                        //check if user realy want to delete a student from database
                        if (MessageBox.Show(body, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        // user clicked yes
                        {
                            //send mail to notice that the database has been clean
                            string mail_title = "Purge de la base de données";
                            string mail_body = "L'agent " + SecurityManager.getFormatedSessionInfo() + " vient d'ordonner la purge de la base de données. "
                                        + "Cette action est irréversible et aura pour effet de supprimer l'intégralité des données sauvegardées, "
                                        + "hormis celle concernant les agents.";
                            Tools.sendMail(mail_title, mail_body, ToolsClass.Settings.MailsList);
                            
                            //purge the whole database
                            DataBase database = new DataBase();
                            if(database.purgeAllData()==Definition.NO_ERROR_INT_VALUE)
                            {
                                MessageBox.Show("La base de données a été effacée avec succès");
                            }else
                            {
                                MessageBox.Show("Une partie ou la totalité de la base de données n'a pas pu être effacée !");
                            }
                        }
                    }
                    break;
                case "redémarrerLeServer":
                    {
                        restartLocalServer();
                    }
                    break;
                /*----------------------------------  Settings  ---------------------------------- */
                case "nomDeDomainDuServerSQL":
                    {
                        string title = "Serveur SQL";
                        string message = "Veuillez le nom du domaine du server SQL auquel la base de données est liée";
                        string default_response = "" + ToolsClass.Settings.SqlServerDomainName;
                        string user_answer = Microsoft.VisualBasic.Interaction.InputBox(message, title, default_response);

                        if (user_answer != "" && user_answer != null)
                        {
                            ToolsClass.Settings.SqlServerDomainName = user_answer;
                        }
                    }
                    break;
                case "adresseIpDuServer":
                    {
                        /* Handler to the click event on the menu item "adresse Ip du server". 
                            * Allow user to change element of server like ip adress and cimmunication port
                            */
                        SettingsServerElements Form = new SettingsServerElements();
                        Form.ShowDialog();
                    }
                    break;
                case "fichierDaccèsAuServeur":
                    {
                        /* Handler to the click event on the menu item "fichier d'accès au serveur". 
                        * Allow user to select the location of file that contains the actual urlof the server 
                        */
                        SettingsAccessToServerFile Form = new SettingsAccessToServerFile();
                        Form.ShowDialog();
                    }
                    break;
                case "gestionDesAdressesMails":
                    {
                        SettingsList Form = new SettingsList(Definition.USE_FOR_MAILS_LIST);
                        Form.ShowDialog();
                    }
                    break;
                case "gestionDuServiceDenvoiDeCourriel":
                    {
                        SetingsMailsSender Form = new SetingsMailsSender();
                        Form.ShowDialog();
                    }
                    break;
                case "donnéesDeConnexionSiteWeb":
                    {
                        SettingsDataConnexionWebsite Form = new SettingsDataConnexionWebsite();
                        Form.ShowDialog();
                    }
                    break;
                case "gestionDesDroitsDaccès":
                    {
                        AccessManagementForm Form = new AccessManagementForm();
                        Form.ShowDialog();
                    }
                    break;
                /*----------------------------------  Administration  ---------------------------------- */
                case "SchoolData":
                    {
                        SchoolDataForm Form = new SchoolDataForm();
                        Form.ShowDialog();
                    }
                    break;
                case "formatNomDesPhotosÉtudiantes":
                    {
                        SettingsStudentPhoto Form = new SettingsStudentPhoto();
                        Form.ShowDialog();
                    }
                    break;
                case "fichierDétatsDesÉtudiants":
                    {
                        SettingsStudentsStateFile Form = new SettingsStudentsStateFile();
                        Form.ShowDialog();
                    }
                    break;
                case "duréeDeLaPauseAutorisantLesSorties":
                    {
                        SettingsLengthExitBreak Form = new SettingsLengthExitBreak();
                        Form.ShowDialog();
                    }
                    break;
                case "longueurIdentifiantRFID":
                    {
                        string title = "Longueur des identifiants RFID";
                        string message = "Veuillez indiquer le nombre de caractère composant les identifiant RFID utilisés";
                        string default_response = "" + ToolsClass.Settings.LengthRfidId;
                        string user_answer = Microsoft.VisualBasic.Interaction.InputBox(message, title, default_response);

                        if (user_answer != "" && user_answer != null)
                        {
                            int length_rfid_id = 10;
                            int.TryParse(user_answer, out length_rfid_id);

                            ToolsClass.Settings.LengthRfidId = length_rfid_id;
                        }
                    }
                    break;
                case "gestionDesDonnéesÉlèves":
                    {
                        BrowseStudentData Form = new BrowseStudentData();
                        Form.ShowDialog();
                    }
                    break;
                /*----------------------------------  School Life Office  ---------------------------------- */
                case "sonSortieNonAutorisé":
                    {
                        /* Handler to the click event on the menu item "son sortie non autorisée". 
                        * Allow user to select the new mp3 file that will be played when student can't 
                        * leave the school 
                        */


                        OpenFileDialog openFileDialog = new OpenFileDialog();

                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = Tools.getOpenDialogFilter(Definition.VALID_AUDIO_EXTENSION);
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            ToolsClass.Settings.changeErrorAudioFile(openFileDialog.FileName);
                        }
                    }
                    break;
                case "motifsDinterdictionDeSortie":
                    {
                        SettingsList Form = new SettingsList(Definition.USE_FOR_EXIT_BAN_REASONS_LIST);
                        Form.ShowDialog();
                    }
                    break;
                case "appelationPunitionEnPrésence":
                    {
                        SettingsList Form = new SettingsList(Definition.USE_FOR_PUNISHMENT_LIST);
                        Form.ShowDialog();
                    }
                    break;

                case "couleurPunitionNonClôturée":
                    {
                        string title = "Couleur de l'icône des punitions non clôturées";
                        string message = "Veuillez indiquer la couleur utilisée pour représenter l'icône des punitions non clôturées\n(instruction HTML ou rgb() ou rgba() ou autre)";
                        string default_response = ToolsClass.Settings.ColorNonClosedPunishment;
                        string user_answer = Microsoft.VisualBasic.Interaction.InputBox(message, title, default_response);

                        if (user_answer != "" && user_answer != null)
                        {
                            ToolsClass.Settings.ColorNonClosedPunishment = user_answer;
                        }
                    }
                    break; 

                default:
                    break;
                
            }
        }

        private void aideToolStripMenuItem_Click(object sender, EventArgs e)//help button
        {
            //open folder in the file exploxer
            try
            {
                Process.Start("explorer.exe", Definition.PATH_TO_FOLDER_HELP);
            }catch
            {
                MessageBox.Show("Impossible d'atteindre le dossier : " + Definition.PATH_TO_FOLDER_HELP);
            }
        }
    }

    
}



