using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1.Forms
{
    public partial class BrowseStudentData : Form
    {

        private static System.Windows.Forms.Timer timer;
        private const int TIME_TIMER = 500; //ms
        private List<string> list_results; //list of all results from the search
        private List<int> list_id;
        //list of all the corresponding id from the database in oder to retireve the correct student from the search later
        private ToolsClass.Tools.StudentData student;
        private PhotoHandler photoHandler;
        private DataBase dataBase;
        private Color defaulft_textBoxes_color;
        private Font defaulft_textBoxes_font;
        private TextBox[] tab_textBox;


        private struct PhotoHandler
        {
            /*
             * Structure that manage student photo
            */

            public string photo_path; //path to the current student photo
            public bool new_photo; //if user decided to change the current photo
            public string temp_file_path;
            /* When user want to change the current student photo, we create a tempoary file
             * that is replace by the desired photo. It's only when the user confirms that change
             * that the photo is replace in the correct folder with all other student photos.
             * So the variable "temp_file_path" contains the path to the temporary file
            */
            public string photo_extension;
            /* The temporary file has .tmp extension. So before copy the desired photo to that temporary
             * file we copy the file extension (png, jpg, ...) of the photo for a later use 
            */
            

            public void setDefaultValues(string path="")
            {
                photo_path = path;
                new_photo = false;
                temp_file_path = "";
                photo_extension = "";
            }


            public void setNewPhoto(string new_path)
            {
                /* 
                 * When the user want to change the current student photo
                */
 
                new_photo = true;
                temp_file_path = Path.GetTempFileName(); //create a unique temporary file
                photo_extension = Path.GetExtension(new_path); //get extension of the photo

                ToolsClass.Tools.copyFile(new_path, temp_file_path); //copy pointed photo by user to the temporary file
            }

            public int saveNewPhoto(int student_table_index, string last_name, string first_name, string division, int sex)
            {
                /*When the user confirms the change, we save the photo into the correct folder
                 * with the correct name (which is not the current name of the file)
               */

                if (temp_file_path == "" || photo_extension == "") //confirm there is a new photo to copy
                {
                    return ToolsClass.Definition.ERROR_INT_VALUE;
                }

                try
                {
                    ToolsClass.Settings.changeStudentPhoto(student_table_index, temp_file_path,
                                ToolsClass.Tools.getStudentPhotoNameWithoutExtension(last_name, first_name, division), photo_extension);
                    //copy file into the correct folder with the correct name and update the modification date of the student information in the database
                }
                catch
                {
                    return ToolsClass.Definition.ERROR_INT_VALUE;
                }

                return ToolsClass.Definition.NO_ERROR_INT_VALUE;

                
            }


            public string getActualPhotoPath(string last_name, string first_name, string division)
            {
                /* Get the path of the current student photo
                 * If there is no photo to found, return the path to a default image
                */

                photo_path = ToolsClass.Tools.getStudentPhotoPath(last_name, first_name, division);
                //get formated photo name with extension
                if (photo_path == "")//student photo found
                {
                    photo_path = getDefaultPhotoPath();
                }
                
                setDefaultValues(photo_path);
                //reset all the current variable (especially a possible path to a temporary file)

                return photo_path;
            }

            public string getDefaultPhotoPath()
            {
                /*When there is no photo that correspond to the pointed student we set a default image
                 * This method return the path to that default image
                */

                setDefaultValues(ToolsClass.Definition.PATH_TO_DEFAULT_STUDENT_PHOTO);
                return photo_path;
            }
        }


        
        //---------------------------------------  INITIALIZATION  ----------------------------------
        public BrowseStudentData() 
        {
            InitializeComponent();
            dataBase = new DataBase();
            list_results = new List<string>();
            list_id = new List<int>();
            student = new ToolsClass.Tools.StudentData();

            //set default values for structures
            student.toDefault();
            /* When user double click on an item all the informations about that student (grab with he corresponding student id in the table)
             * are stored in the struct student
            */
            photoHandler.setDefaultValues();

            //set timer (used later)
            timer = new Timer();
            timer.Interval = TIME_TIMER;
            timer.Tick += Timer_Tick; //event timer tick


            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; //fit image to pictureBox
            pictureBox.Load(photoHandler.getDefaultPhotoPath()); //set default image
         
            defaulft_textBoxes_color = monday_textBox.BackColor;
            defaulft_textBoxes_font = monday_textBox.Font;
            tab_textBox = tab_textBox = new TextBox[]{sunday_textBox, monday_textBox, tuesday_textBox, wednesday_textBox,thursday_textBox,friday_textBox,saturday_textBox };

            this.ActiveControl = search_textBox;
        }


        private void textBoxes_RFID_input_KeyDown(object sender, KeyEventArgs e)
        {
            /* Since the RFID reader is detect as an HI (humain interface) like keyboard, when the reader send data to computer
             * all the char are send are considered as numerci key. So the corresponding keyCode will be between D0 and D9
             * But because of the HI reader we have to press cap key in order to get input char as numerci key
             * else all we get is non numeric key (For example in AZERTY keyboard wihtout numerci pad, the key for '0' is also the key for '&' 
             * when the cap key is not pressed, so when the keycode to '0' will be detected the result will be '&' and not '0')
             * 
             * To avoid having to switch between cap lock and unlock we intercept all key down (from the textboxes that will deal with the RFID reader)
             * and if the key code is for a numerci key we suppress the key press event, convert that char as numeric char, 
             * and write it into the corresponding textBox
            */

            char keyChar = (char)e.KeyData;
            if (char.IsDigit(keyChar))//input from the HI RFID reader
            {
                /* since each char is just added to the existing text into the textbox
                 * when user select text and next want to add digit char, then the seleted text will not be removed
                 * So to avoid that issue we check if text is selected. If so we replace that selection by a blank
                */

                TextBox textBox = (TextBox)sender;
                if (textBox.SelectionLength !=0)
                {
                    textBox.Text = textBox.Text.Replace(textBox.SelectedText, "");//remove selected text
                }
                textBox.AppendText(keyChar.ToString());//add the char to the existing text (at the cursor position)

                e.SuppressKeyPress = true;//remove the key press event
                e.Handled = true;
            }

            
        }
        

        private void onFormClosing(object sender, CancelEventArgs e)
        {
            /* In fact if a photo was loaded into the pictureBox then util the app will remain active (the app and not just that child window),
             * the loaded image will remain inaccessible for other programs
             * So whatever an image was loaded or not when that child window will be closing we release the image
            */
            pictureBox.Dispose();//here "pictureBox.Dispose()" and not "pictureBox.Image.Dispose()"
        }


        private void save_student_data_button_Click(object sender, EventArgs e)
        {
            /*When the user confirms all the change we saved them into the database
             * and eventually change the current student photo
            */
            if (student.tableId == -1) //no student has been choose
            {
                MessageBox.Show("Il faut sélectionner un élève pour pouvoir modifier ses informations");
                return;
            }else if(!ToolsClass.Tools.isRfidFormatCorrect(rfid_id_textBox.Text))
            {
                MessageBox.Show(ToolsClass.Definition.RFID_FORMAT_CONDITIONS);
                return;
            }else if(!dataBase.isRfidFree(rfid_id_textBox.Text, student.tableId))
            {
                MessageBox.Show("L'identifiant RFDI est déjà pris !");
                return;
            }


            pictureBox.Image.Dispose();
            /* at that step image are manipulate so if one of the target image is loaded by the pictureBox this will cause an error
             * so we have to release all images loaded by the pictureBox
            */
            if ( dataBase.changeStudentRfid(student.tableId, rfid_id_textBox.Text) == ToolsClass.Definition.NO_ERROR_INT_VALUE)
            //save change to database (the RFID id)
            {
                if (student.idRFID != rfid_id_textBox.Text)
                {
                    string modification = ToolsClass.Definition.MODIFICATION_STUDENT_RFID
                                         + "\n\tAncien RFID : " + student.idRFID + "\n\tNouveau RFID : " + rfid_id_textBox.Text;
                    dataBase.writeToRegiter(ToolsClass.Definition.TYPE_OF_STUDENT_TABLE, student.tableId, SecurityManager.getConnectedAgentTableIndex(), modification);

                }
                if ((!photoHandler.new_photo) ||
                    (photoHandler.saveNewPhoto(student.tableId, student.lastName, student.firstName, student.division, student.sex) == ToolsClass.Definition.NO_ERROR_INT_VALUE) )
                //If there is no new photo or if there is a new photo and the copy has succeed
                {
                    MessageBox.Show("Les modifications ont été enregistrées !"); //display modifications saved 
                }
                   
                   
            }else
            {
                return;
            }

            setStudentData(student.tableId);
            //get again all the data from that student and displayed them

            search_textBox.Text = "";
            search_textBox.Focus();
            

        }


        private void cancel_student_data_button_Click(object sender, EventArgs e)
        {
            /* If the user does not want to conserve changes
             * we display all previous informations about the student
            */
            pictureBox.Image.Dispose();
            if (student.tableId != -1) //there is student that has been set
            { 
                rfid_id_textBox.Text = student.idRFID;
                pictureBox.Load(photoHandler.getActualPhotoPath(student.lastName, student.firstName, student.division));
            }else
            {
                pictureBox.Load(photoHandler.getDefaultPhotoPath());
            }

            rfid_id_textBox.Focus();
            rfid_id_textBox.SelectAll();
            
        }

        private void textboxes_Click(object sender, EventArgs e)
        {
            /*
             * When click on textbox select all the content
            */
            TextBox clicked_textbox = (TextBox)sender;
            clicked_textbox.SelectAll();
        }



        private void end_button_Click(object sender, EventArgs e)
        {
            /*exit button*/
            this.Close();
        }


        private void search_textBox_TextChanged(object sender, EventArgs e)
        {
            /* There is no search button just a textbox where user can type informations to find.
             * So to launch the search into database without button we have to wait 
             * for user to finish typing.
             * When the user changes the current text (he is typing) we restart a timer
             * When the user stops typing the timer is not reset and eventually goes to its end
             * 
             * That is only when the timer finish event is triggered that the search is launched 
           */

            //restart timer
            timer.Stop();
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e) //handler when the time counter is finished
        {
            //when arrived here the user are not typing for 500 ms
            timer.Stop();

            if(search_textBox.Text!="")
            {
                //launch the search to database
                var lists = dataBase.getStudentDataBySearch(search_textBox.Text);
                list_id = lists.Item1; //list of all the student id found who correspond to the search
                list_results = lists.Item2; // list of all the corresponding informations about theses students

                if(list_id.Count==0)//no result
                {
                    list_results = new List<string>() { "Aucun résultat" };
                }

                listBox.DataSource = list_results;//populate listbox

            }
            
        }



        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*When user double click on an item we display all the informations about the 
             * student represented by that item
            */
            if(list_id.Count==0)
            {
                return;
            }

            listBox.ClearSelected();

            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                int i = list_results.IndexOf(listBox.Items[index].ToString());
                //get the index of the desired student in the list of resultas search
                //Then use that index to get the database id of that student with the list of ids
                if(i!=-1)
                {
                    setStudentData(list_id[i]); //display informations about that student
                }
                listBox.SetSelected(index, true);
            }
            
            
        }


        private void setStudentData(int table_id_student)
        {
            /*Change the value of all field for user using the informations stored a the structure*/

            student = dataBase.getStudentByIndex(table_id_student);//get informations about student in a structure

            last_name_textBox.Text = student.lastName;
            first_name_textBox.Text = student.firstName;
            division_textBox.Text = student.division;
            sex_textBox.Text = student.getSexToString();
            rfid_id_textBox.Text = student.idRFID;
            exit_regime_textBox.Text = student.labelRegime;

            for (int i=0; i< student.halfBoardDays.Length;i++)
            {
                /*All day of the working week is represented by a textbox
                 * When student eat at a specific day we set the color to a custom green
                 * else we set the color to the default color and use a lin through the text
                 * All the related textBoxes are stored in an array to acces to it with index of the corresponding day
                 * (start at 0 for sunday)
                */
                if (tab_textBox[i] != null)
                {
                    if (student.halfBoardDays[i] == 1)//student must eat at school at lunch time
                    {
                        tab_textBox[i].BackColor = Color.FromArgb(255, 166, 77);
                        tab_textBox[i].Font = defaulft_textBoxes_font;//default font without the line through

                    }
                    else//student is external
                    {
                        tab_textBox[i].BackColor = defaulft_textBoxes_color;
                        tab_textBox[i].Font = new Font(tab_textBox[i].Font, FontStyle.Strikeout);//the line through
                    }
                }
            }

            pictureBox.Image.Dispose();
            pictureBox.Load(photoHandler.getActualPhotoPath(student.lastName, student.firstName, student.division));

            rfid_id_textBox.Focus();
            rfid_id_textBox.SelectAll();

        }

        private void change_photo_button_Click(object sender, EventArgs e)
        {
            /*
             * allow user to change the student photo.
             * Actually this function just store (and display) the desired image in a temporary file.
             * The change will be operated when the user confirm that change
            */
            if(student.tableId == -1)//no student has been selected
            {
                MessageBox.Show("ERREUR : il faut sélectionner un élève avant de pouvoir modifier ses informations");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = ToolsClass.Definition.PATH_TO_FOLDER_STUDENTS_PHOTOS;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                photoHandler.setNewPhoto(openFileDialog.FileName);//save photo to a temporary file
                pictureBox.Image.Dispose();
                pictureBox.Load(openFileDialog.FileName);//just diplay that image in the picture box 
            }
        }

        private void delete_student_button_Click(object sender, EventArgs e)
        {
            /*Allow user to delete all information of a student from database and delete the associate photo from folder*/


            if (student.tableId == -1)
            {
                MessageBox.Show("ERREUR : il faut sélectionner un élève avant de pouvoir modifier ses informations");
                return;
            }
            string title = "Demande de confirmation";
            string body = "Etes-vous certain(e) de vouloir supprimer cet élève de la base de données (y compris la photo associée) ?\nAttention : cette action est irréversible !";
            
            //check if user realy want to delete a student from database
            if (MessageBox.Show(body, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            // user clicked yes
            {
                pictureBox.Image.Dispose();
                pictureBox.Load(photoHandler.getDefaultPhotoPath());
                /*When picture is set in an pictureBox the image is currently not availabe 
                 * for other operation because the pictureBox use the source file of that image
                 * In other words, if we want to delete the set picture in that pictureBox (the student photo)
                 * we have to release the source file (so set a new image)
                */
                
                if (dataBase.setStudentDeletedFromIdTable(student.tableId) == ToolsClass.Definition.NO_ERROR_INT_VALUE)
                //photo are delete inside the function deleteStudentFromIdTable()
                {
                    /*If the delete of student from data base succeed  and if there no photo to delete or 
                     * there is one and the delete of that file succeed
                    */
                    int index = list_id.IndexOf(student.tableId); //remove the student from the search results
                    if (index != -1)
                    {
                        list_id.RemoveAt(index);
                        list_results.RemoveAt(index);
                        listBox.DataSource = new List<string>();
                        //populate listbox because when changes are made to the list set as dataSource for the lisbox
                        //that changes are not update. So we have to set an empty list and after reload the old list (with changes)
                        listBox.DataSource = list_results;//reload datasource

                    }

                    setStudentData(student.tableId);
                    MessageBox.Show("Cet élève vient d'être supprimé de la base de données");
                }else
                {
                    MessageBox.Show("Une erreur est survenue, impossible de supprimer l'élève de la base de données");
                }

            }
        }



        private void fusion_button_Click(object sender, EventArgs e)
        {
            /*Allow user to gather specific information about selected student into one student in the database
             * The student that will gather all the informations is automatically detected as the one which have the 
             * latest modifications date
             * Then remove all the other student from the database and the associate photos
            */

            pictureBox.Image.Dispose(); //photo will be deleted so we have to release the photo from the pictureBox
            pictureBox.Load(photoHandler.getDefaultPhotoPath());
            

            var list_selected_item = listBox.SelectedItems;//get selected student

            if(list_selected_item.Count<2)//need at least two students
            {
                MessageBox.Show("ERREUR : pour fusionner des informations il faut sélectionner au moins deux élèves !");
                return;
            }

            List<int> list_id_for_fusion = new List<int>();//list that will conatin the id from database of all selected student

            foreach (var item in listBox.SelectedItems)
            {

                int i = list_results.IndexOf(item.ToString());
                //get the index of the desired student in the list of resultas search
                //Then use that index to get the database id of that student with the list of ids
                if (i != -1)
                {
                    list_id_for_fusion.Add(list_id[i]); //display informations about that student
                }
                
            }

            int res = dataBase.FusionStudentsFromIds(list_id);
            //gather all informations and delete the other and return the id (from database) of the student which gather all the information

            if (res==ToolsClass.Definition.ERROR_INT_VALUE)
            {
                string message = "Une erreur est survenue lors de l'opération de fusion. Il se peut qu'une partie des informations soient définitivement perdues ou incohérentes"
                    + "\nUne opération manuelle est nécessaire : il faut supprimer manuellement tous les éléments de la fusion, puis ajouter manuellement les informations voulues";
                MessageBox.Show(message);
            }else//no error
            {

                foreach (int id in list_id_for_fusion)//delete the student that have been remove from the database
                {
                    if (id != res)//keep the studet that gather all information in the list
                    {
                        int index = list_id.LastIndexOf(id);
                        if (index != -1)
                        {

                            list_id.RemoveAt(index);
                            list_results.RemoveAt(index);
                        }
                    }
                }

                listBox.DataSource = new List<string>();
                //populate listbox because when changes are made to the list set as dataSource for the lisbox
                //that changes are not update. So we have to set an empty list and after reload the old list (with changes)
                listBox.DataSource = list_results;//reload datasource

                //set the item, that represent the student which gather all infromation, selected
                string final_student = list_results[list_id.LastIndexOf(res)];
                int index_item = listBox.FindString(final_student);
                // Determine if a valid index is returned. Select the item if it is valid.
                if (index_item != -1)
                {
                    listBox.SetSelected(index_item, true);
                }

                setStudentData(res);//display new updated information about the selected student
            }

        }

    }
}
