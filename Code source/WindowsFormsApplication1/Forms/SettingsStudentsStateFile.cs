using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parms = ToolsClass.Tools.ParmsStudentsStateFile;

namespace WindowsFormsApplication1.Forms
{
    public partial class SettingsStudentsStateFile : Form
    {
        private const string TAG_SPACE="SPACE";
        private const string TAG_RETURN = "RETURN";
        private static char actual_key_char=' ';
        private static string path_to_new_file = "";

        public SettingsStudentsStateFile()
        {
            InitializeComponent();

            Parms parms = ToolsClass.Settings.StudentsStateFileParameters;

            //add 1 for each result because we store columns index from 0 to ... (and not from 1 to ...)
            last_name_textBox.Text = ""+ (parms.lastNameIndex + 1);
            first_name_textBox.Text = ""+ (parms.firstNameIndex + 1);
            division_textBox.Text = ""+ (parms.divisionIndex + 1);
            sex_textBox.Text = ""+ (parms.sexIndex + 1);
            half_board_days_textBox.Text = ""+ (parms.halfBoardDaysIndex + 1);
            shortname_female_textBox.Text = ""+ parms.femaleShortname;
            shortname_male_textBox.Text = ""+ parms.maleShortname;
            mon_textBox.Text = "" + parms.mondayShortname;
            tue_textBox.Text = "" + parms.tuesdayShortname;
            wed_textBox.Text = "" + parms.wednesdayShortname;
            thu_textBox.Text = "" + parms.thursdayShortname;
            fri_textBox.Text = "" + parms.fridayShortname;
            sat_textBox.Text = "" + parms.saturdayShortname;
            sun_textBox.Text = "" + parms.sundayShortname;

            if (parms.separatorDays == ' ') //space
            { 
                separator_days_textBox.Text = TAG_SPACE;
            }
            else if (parms.separatorDays == (char)13) //enter
            {
                separator_days_textBox.Text = TAG_RETURN;
            }
            else
            {
                separator_days_textBox.Text = "" + parms.separatorDays;
            }

            

        }
        

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void save_button_Click(object sender, EventArgs e)
        {
            if(last_name_textBox.Text=="" || first_name_textBox.Text=="" || division_textBox.Text==""
                || sex_textBox.Text=="" || half_board_days_textBox.Text=="" || shortname_female_textBox.Text==""
                || shortname_male_textBox.Text=="" || separator_days_textBox.Text == "")
            {
                MessageBox.Show("ATTENTION : les champs surmontés d’un astérisque « * » doivent obligatoirement être renseignés");
                return;
            }


            try
            {
                Parms parms = new Parms();
                parms.toDefault();
                //take out one for each result because we ask the user to start the counting at 1 (and not 0)
                parms.lastNameIndex = Int32.Parse(last_name_textBox.Text) - 1;
                parms.firstNameIndex = Int32.Parse(first_name_textBox.Text) - 1;
                parms.divisionIndex = Int32.Parse(division_textBox.Text) - 1;
                parms.sexIndex = Int32.Parse(sex_textBox.Text) - 1;
                parms.halfBoardDaysIndex = Int32.Parse(half_board_days_textBox.Text) - 1;
                parms.femaleShortname = shortname_female_textBox.Text;
                parms.maleShortname = shortname_male_textBox.Text;
                parms.mondayShortname = mon_textBox.Text;
                parms.tuesdayShortname = tue_textBox.Text;
                parms.wednesdayShortname = wed_textBox.Text;
                parms.thursdayShortname = thu_textBox.Text;
                parms.fridayShortname = fri_textBox.Text;
                parms.saturdayShortname = sat_textBox.Text;
                parms.sundayShortname = sun_textBox.Text;
                parms.separatorDays = actual_key_char;

                ToolsClass.Settings.StudentsStateFileParameters = parms;

                if(path_to_new_file!="")
                {
                    ToolsClass.Settings.changeStudentStateFile(path_to_new_file);
                }

            }
            catch
            {
                MessageBox.Show("ERREUR : certains paramètres ne sont pas valides ! ");
                return;
            }

            this.Close();
        }

        
        private void textBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            //allow only numbers (start at 1 and not 0) in the textboxes
            if (e.KeyChar == '0' || ( !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) ) )
            {
                e.Handled = true;
            }
        }


        private void separator_days_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            actual_key_char = e.KeyChar;
            //allow only one char
            if (separator_days_textBox.Text.Length>0)
            {
                separator_days_textBox.Text = "";
            }

            if (e.KeyChar==' ') //space
            {
                e.Handled = true;
                separator_days_textBox.Text = TAG_SPACE;

            }else if(e.KeyChar == (char)13) //enter
            {
                e.Handled = true;
                separator_days_textBox.Text = TAG_RETURN;
            }
        }


        private void separator_days_textBox_Click(object sender, EventArgs e)
        {
            separator_days_textBox.SelectAll();
        }

        private void browse_files_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = ToolsClass.Tools.getOpenDialogFilter(ToolsClass.Definition.VALID_STUDENT_FILE_EXTENSION);
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path_to_new_file = openFileDialog.FileName;
                display_path_textBox.Text = path_to_new_file;
            }
        }

        private void remove_selected_file_button_Click(object sender, EventArgs e)
        {
            path_to_new_file = "";
            display_path_textBox.Text = path_to_new_file;
        }
    }
}
