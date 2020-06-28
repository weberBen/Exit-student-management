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
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHA NTABILITY, FITNESS
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
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;

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
    private System.Windows.Forms.Timer timer;
    private static System.Windows.Forms.Timer timer_connection;
    private const int TIMER_TIME_MINUTES = 2880; //48h
    private const int TIME_TIMER_PHYSICAL_CONNECTION = SecurityManager.TIMEOUT_PHYSICAL_SESSION_MIN * 60000; //ms

    private TextBox banner;
    private string actual_text_banner;
    private Color actual_color_banner;

    private bool close_named_piped = false;


    private void closeNamedPipe()
    {
        this.close_named_piped = true;
    }

    private void startNamedPipe()
    {
            //start new thread
            Task.Factory.StartNew(() =>
            {

                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(Definition.NAMED_PIPE, PipeDirection.InOut))
                {
                    // Wait for a client to connect
                    pipeServer.WaitForConnection();
                    //client connected

                    try
                    {
                        // Read user input and send that to the client process.
                        using (BinaryReader br = new BinaryReader(pipeServer))
                        {
                            Boolean exit = false;
                            while (!exit)
                            {
                                if (this.close_named_piped)
                                {
                                    exit = true;
                                    break;
                                }

                                var len = (int)br.ReadUInt32();            // Read string length
                                var str = new string(br.ReadChars(len));    // Read string

                                switch(str)
                                {
                                    case "error":
                                        Definition.EXTERNAL_DATABASE_OFFLINE = true;
                                        break;
                                    case "online":
                                        Definition.EXTERNAL_DATABASE_OFFLINE = false;
                                        break;
                                    case "exit":
                                        exit = true;
                                        break;
                                }
                            }
                        }
                    }
                    // Catch the IOException that is raised if the pipe is broken
                    // or disconnected.
                    catch (IOException e)
                    {
                        Definition.EXTERNAL_DATABASE_OFFLINE = true;
                        startNamedPipe();//if the server has interrupted the pipe then start listening to it again
                    }
                }

            });
        }


    public MainForm()
        {

            InitializeComponent();
            banner = this.displayEventTextBox;
            timer = null;


            updateComputerTableId();
            EmptyAdmin(); //check if it's needed to set a new admi
            startNamedPipe();

            this.displayEventTextBox.TextAlign = HorizontalAlignment.Center;
            this.FormClosing += this.form_OnClosing;//event handler on form closing

            if (LocalServer.Start() != Definition.NO_ERROR_INT_VALUE) //can't start the local server
            {
                changeBanner("Impossible de démarrer le serveur", CUSTOM_RED);
            }else
            {
                changeBanner("Serveur démarré avec succès", CUSTOM_GREEN);
            }

            actual_text_banner = this.displayEventTextBox.Text;
            actual_color_banner = this.displayEventTextBox.BackColor;

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
                        item.Tag = SecurityManager.RIGHTS_LIST[1];
                        break;
                    case "administration":
                        item.Tag = SecurityManager.RIGHTS_LIST[2];
                        break;
                    case "schoollifeoffice":
                        item.Tag = SecurityManager.RIGHTS_LIST[3];
                        break;
                }

                foreach (ToolStripDropDownItem sub_item in item.DropDownItems)
                {
                    //set event handler on click for subitem in the item of the menu strip
                    sub_item.Click += new System.EventHandler(this.submenuItem_Click);

                    foreach (ToolStripDropDownItem sub_sub_item in sub_item.DropDownItems)
                    {
                        sub_sub_item.Click += new System.EventHandler(this.submenuItem_Click);
                    }
                }
            }

            SecurityManager.setConnection("", null);
            
            dailyTask();
            //start timer
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = TIMER_TIME_MINUTES*60*1000;//into ms
            timer.Start();

            timer_connection = new System.Windows.Forms.Timer();
            timer_connection.Tick += new EventHandler(timerConnectionTick);
            timer_connection.Interval = TIME_TIMER_PHYSICAL_CONNECTION;// ms

        }

        private void timerTick(object source, EventArgs e)
        {
            timer.Stop();
            dailyTask();
            timer.Start();
        }

        private void timerConnectionTick(object source, EventArgs e)
        {
             /*When a agent is physically connected to the app we start a timer that represent the resting time before disconnection
             * At this step there is no more time left
             * First we ask the user if we want to stay connected by a autoclosing message form (that close after a specific time if there is no answer)
             * If there is no answer or the agent does not want the stay connected we close the session
             * Else we restart a new timer
            */
            timer_connection.Stop();


            string title = "Demande de confirmation";
            string message = "Utilisez vous encore l'application ?";
            string text_button_ok = "Oui";
            string text_button_cancel = "Quiter";
            int timeout = 10000;//ms

            AutoClosingMessageBox Form = new AutoClosingMessageBox(title, message, timeout, text_button_ok, text_button_cancel);
            Form.ShowDialog();
            if (Form.getDialogResult() == true)//user confirm we want the keep using the app
            {
                timer_connection.Start();
                return;
            }

            terminateSession();
        }


        private void disposeObject()
        {
            closeNamedPipe();
            LocalServer.Dispose();
        }

        private void form_OnClosing(Object sender, FormClosingEventArgs e)
        {
            changeBanner("Fermeture du serveur en cours", CUSTOM_ORANGE);

            timer.Stop();

            Task t = Task.Run(() => disposeObject()); //wait for the closing of the local server
            t.Wait(TimeSpan.FromMilliseconds(TIME_OUT_VALUE));
        }
        
        private void updateComputerTableId()
        {
            DataBase dataBase = new DataBase();

            //update database table id for the computer agent (if its exists)
            Definition.COMPUTER_TABLE_ID_DATABASE = dataBase.getComputerTableId();
        }

        private void EmptyAdmin()
        {
            DataBase dataBase = new DataBase();

            //check if there is no agents (because it's the first use of the app or the database has been reset
            int numberAgent = dataBase.getNumberAgent();
            if (numberAgent == -1)
            { 
                MessageBox.Show("Vérifier les paramètres de connexion à la base de données");
                return;
            }
            if (numberAgent != 0)// get the number of not deleted agent (computer agent excluded)
                return;

            //add default user for automatic task and admin
            using (var form = new AdminSetupBox())//get admin info
            {
                var result = form.ShowDialog();
                if(result == DialogResult.OK)
                {
                    //set admin short secure id
                    SecureString short_secure_id_secureString = new SecureString();
                    String short_secure_id = SecurityManager.setAgentShortSecureId();
                    foreach (char c in short_secure_id)
                    {
                        short_secure_id_secureString.AppendChar(c);
                    }
                    short_secure_id_secureString.MakeReadOnly();


                    //get encrypted value
                    string salt = DataProtection.getSaltForHashes();
                    string hash_password = DataProtection.getSaltHashedString(form.admin_password, salt);
                    string encrypted_short_id = DataProtection.protect(short_secure_id_secureString);
                    form.admin_password.Dispose();
                    short_secure_id_secureString.Dispose();


                    //add agents into the database
                    Tools.Agent agent = new Tools.Agent();

                    //default agent for automatic use only
                    agent.toDefault();
                    Random rand = new Random();
                    agent.lastName = "Poste";
                    agent.firstName = "Informatisé";
                    agent.id = Definition.COMPUTER_AUTHENTIFICATION_ID_DATABASE;
                    agent.setRigth(-1);
                    //add agent
                    if (dataBase.addNewAgent(agent) != Definition.NO_ERROR_INT_VALUE)
                    {
                        MessageBox.Show("Impossible d'ajouter un nouvel agent");
                        return;
                    }
                    //update database table id for the computer agent
                    Definition.COMPUTER_TABLE_ID_DATABASE = dataBase.getComputerTableId();

                    //add admin
                    agent.toDefault();

                    agent.firstName = form.admin_firstName;
                    agent.lastName = form.admin_lastName;
                    agent.job = "DEUS EX";
                    agent.setRigth(0);
                    agent.mailAdress = "none";
                    agent.id = form.admin_id;
                    agent.hashedPassword = hash_password;
                    agent.saltForHashe = salt;
                    agent.shortSecureId = encrypted_short_id;
                    //add agent
                    if (dataBase.addNewAgent(agent) != Definition.NO_ERROR_INT_VALUE)
                    {
                        MessageBox.Show("Impossible d'ajouter un nouvel agent");
                        return;
                    }

                    MessageBox.Show("La création du compte administrateur s'est terminée avec succès.\nVotre identifiant est : "+ form.admin_id);
                }else
                {
                    MessageBox.Show("la création du compte administrateur a echouée");
                    return;
                }
            }
            
        }

        private void clearData()
        {
            changeBanner("Nettoyage de la base de données" + Environment.NewLine + "Veuillez patienter...", Color.DarkOrange);
            //clear database
            DataBase dataBase = new DataBase();
            dataBase.clearDatabase();
            ToolsClass.Settings.DateLastSuppressionFromDatabase = DateTime.Now;


            bannerToPreviousState();
            changeBanner("Nettoyage des log" + Environment.NewLine + "Veuillez patienter...", Color.DarkOrange);
            //clear log file
            File.WriteAllText(Definition.PATH_TO_LOG_ERROR_FILE, "");

            DirectoryInfo di = new DirectoryInfo(Definition.PATH_TO_TMP_FOLDER);
            foreach (FileInfo file in di.GetFiles())
            {
                try{ file.Delete(); }catch { }//delete file if not used by another process
            }


            bannerToPreviousState();
        }

        private void dailyTask()
        {
            DateTime date = ToolsClass.Settings.DateLastSuppressionFromDatabase;

            //check if the database was not clean for a while
            if (date.Add(TimeSpan.FromDays(SecurityManager.TIME_BEFORE_SUPPRESSION_OF_DATA_FROM_DATABASE_DAYS)) < DateTime.Now)
            {
                clearData();
            }
        }

        private void bannerToPreviousState()
        {
            if ( (actual_text_banner != banner.Text) || (actual_color_banner != banner.BackColor) )
            {
                banner.Text = actual_text_banner;
                banner.BackColor = actual_color_banner;

                this.Refresh();
            }
        }

        private void changeBanner(string text_to_display, Color color)
        {
            actual_text_banner = banner.Text;
            actual_color_banner = banner.BackColor;

            banner.Text = text_to_display;
            banner.BackColor = color;

            this.Refresh();
        }

        private void resetSecureData()
        {
            this.id_textBox.Text = "";
            this.password_textBox.Text = "";

            this.id_textBox.Focus();
        }

        public void endPreviousSession()
        {
            this.display_session_textBox.BackColor = DefaultBackColor;
            this.display_session_textBox.Text = "";

            this.Refresh();
        }

        public void startSession(string session_info)
        {
            //Set a timer
            timer_connection.Stop();
            timer_connection.Start();

            resetSecureData();

            this.display_session_textBox.BackColor = Color.LightGreen;
            this.display_session_textBox.Text = session_info;

            this.Refresh();
        
        }

        private void clearDisplaySession()
        {
            resetSecureData();
            endPreviousSession();
        }

        private void terminateSession()
        {
            timer_connection.Stop();

            SecurityManager.disconnection();
            clearDisplaySession();
        }

        private void disconnection_button_Click(object sender, EventArgs e)
        {
            terminateSession();
        }

        private void textbox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode ==  Keys.Enter)
            {
                connexion_button.PerformClick();
            }
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
                startSession(SecurityManager.getFormatedSessionInfo());
            }
            else
            {
                terminateSession();
                MessageBox.Show("Vous n'avez pas les droits nécessaire pour accéder à cette application");
            }


            if (password != null)
            {
                password.Dispose();
            }
        }

        


        private void submenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clicked_menu_item = sender as ToolStripMenuItem;
            if (clicked_menu_item.Name != "nomDeDomainDuServerSQL")
            { /*In fact if the user want to change the domaine name of the sql server he could not anymore connect to the database
               *because the server name has changed (and users need to access database when they try to set a connection with the database
               */

                //get the first parent of the submenu

                if (!Error.debug)
                {
                    ToolStripItem source_item = clicked_menu_item.OwnerItem;
                    while (source_item.OwnerItem != null)
                    {
                        source_item = source_item.OwnerItem;
                    }

                    int needed_right = SecurityManager.RIGHTS_LIST.LastIndexOf(source_item.Tag.ToString());
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
            }

            switch (clicked_menu_item.Name)
            {
                /*----------------------------------  Update  ---------------------------------- */
                case "nettoyerBaseDeDonnées":
                    {
                        clearData();
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
                            changeBanner("Purge de la base de données" + Environment.NewLine + "Veuillez patienter...", Color.DarkOrange);
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
                /*----------------------------------  Settings  ---------------------------------- */
                case "nomDeDomainDuServerSQL":
                    {
                        DatabaseSettings Form = new DatabaseSettings();
                        Form.ShowDialog();
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
                case "formatDesPhotos":
                    {
                        SettingsStudentPhoto Form = new SettingsStudentPhoto();
                        Form.ShowDialog();
                    }
                    break;
                case "importerDesPhotos":
                    {
                        StudentPhotosFolderChooser Form = new StudentPhotosFolderChooser();
                        Form.ShowDialog();
                    }
                    break;
                case "MAJBaseDeDonnées":
                    {
                        SettingsStudentsStateFile Form = new SettingsStudentsStateFile();
                        Form.ShowDialog();
                    }
                    break;
                case "régimesDeSortie":
                    {
                        ExitRegime Form = new ExitRegime();
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
                        Form.Show();
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
                default:
                    break;
                
            }

            bannerToPreviousState();
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



