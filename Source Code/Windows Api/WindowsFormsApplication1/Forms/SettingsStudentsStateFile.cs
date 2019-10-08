using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private static string path_to_new_file;

        public SettingsStudentsStateFile()
        {
            InitializeComponent();
            path_to_new_file = "";



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

            if (ToolsClass.Settings.ExitRegimeActivationState)
            {
                exit_regime_textBox.Text = "" + (parms.exitRegimeIndex + 1);
            }
            else
            {
                exit_regime_textBox.Text = "-1";
                exit_regime_textBox.Enabled = false;
            }

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


            if (parms.separatorCsv == ' ') //space
            {
                separator_csv_textBox.Text = TAG_SPACE;
            }
            else if (parms.separatorCsv == (char)13) //enter
            {
                separator_csv_textBox.Text = TAG_RETURN;
            }
            else
            {
                separator_csv_textBox.Text = "" + parms.separatorCsv;
            }

            if(parms.encapsulationByQuote)
            {
                encapsulation_checkBox.Checked = true;
            }else
            {
                encapsulation_checkBox.Checked = false;
            }



        }
        
        private Tuple<bool, char> getCharFromTextBox(TextBox textBox)
        {
            string text = textBox.Text;

            if(text.Length==1)
            {
                return Tuple.Create(true, text[0]);
            }
            else
            {
                if (text == TAG_SPACE)
                    return Tuple.Create(true, ' ');
                if (text == TAG_RETURN)
                    return Tuple.Create(true, (char)13);
            }

            return Tuple.Create(false, ' ');
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void save_button_Click(object sender, EventArgs e)
        {
            TextBox[] list = new TextBox[] { last_name_textBox, first_name_textBox, division_textBox, sex_textBox, half_board_days_textBox, exit_regime_textBox };
            bool check_regime = exit_regime_textBox.Enabled;

            if (shortname_female_textBox.Text=="" || shortname_male_textBox.Text=="" || separator_days_textBox.Text == "" || separator_csv_textBox.Text=="")
            {
                MessageBox.Show("ATTENTION : les champs surmontés d’un astérisque « * » doivent obligatoirement être renseignés");
                return;
            }

            List<int> index = new List<int>();
            for (int i=0; i<list.Length; i++)
            {
                TextBox textBox = list[i];

                if(textBox.Text=="")
                {
                    MessageBox.Show("ATTENTION : les champs surmontés d’un astérisque « * » doivent obligatoirement être renseignés");
                    return;
                }

                if (textBox == exit_regime_textBox && !check_regime)
                    continue;

                int tmp = -1;
                Int32.TryParse(textBox.Text, out tmp);
                if(tmp==-1)
                {
                    MessageBox.Show("ATTENTION : seuls les caractères numériques sont acceptés comme indice des colonnes");
                    return;
                }
                if(tmp<=0)
                {
                    MessageBox.Show("ATTENTION : L'indexation débute à 1 et non à 0");
                    return;
                }

                if(index.Contains(tmp))
                {
                    MessageBox.Show("ATTENTION : L'indice "+tmp+" est utilisé à plusieurs reprises");
                    return;
                }
                index.Add(tmp);
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
                parms.exitRegimeIndex = Int32.Parse(exit_regime_textBox.Text) - 1;
                parms.mondayShortname = mon_textBox.Text;
                parms.tuesdayShortname = tue_textBox.Text;
                parms.wednesdayShortname = wed_textBox.Text;
                parms.thursdayShortname = thu_textBox.Text;
                parms.fridayShortname = fri_textBox.Text;
                parms.saturdayShortname = sat_textBox.Text;
                parms.sundayShortname = sun_textBox.Text;

                var tmp = getCharFromTextBox(separator_days_textBox);
                if (!tmp.Item1)
                {
                    MessageBox.Show("Erreur : valeur du séparateur de jours invalide");
                    return;
                }
                parms.separatorDays = tmp.Item2;


                tmp = getCharFromTextBox(separator_csv_textBox);
                if (!tmp.Item1)
                {
                    MessageBox.Show("Erreur : valeur du séparateur CSV invalide");
                    return;
                }
                parms.separatorCsv = tmp.Item2;


                if (encapsulation_checkBox.Checked)
                {
                    parms.encapsulationByQuote = true;
                }
                else
                {
                    parms.encapsulationByQuote = false;
                }

                ToolsClass.Settings.StudentsStateFileParameters = parms;

                if(path_to_new_file!="")
                {
                    try
                    {
                        DataBase database = new DataBase();

                        //copy file
                        string path_source_file = ToolsClass.Definition.PATH_TO_TMP_FOLDER + ToolsClass.Definition.SEPARATOR + Path.GetRandomFileName();
                        File.Copy(path_to_new_file, path_source_file, true);

                        string path_error_file = ToolsClass.Definition.PATH_TO_TMP_FOLDER + ToolsClass.Definition.SEPARATOR + Path.GetRandomFileName();

                        //update database
                        int res = database.updateStudentTableFromFile(path_source_file, path_error_file);
                        if(res!=ToolsClass.Definition.NO_ERROR_INT_VALUE)//ask user if he wants to see logs
                        {
                            string msg = "Une erreur est survenue lors de la mise à jour.\n"
                                + "Certaines informations sont peut-être incomplètes ou incorrectes. Dans ce cas, elles n'ont pas été prise en compte.\n"
                                + "\nVoulez vous consulter le fichier des logs ?\n";

                            
                            DialogResult dialogResult = MessageBox.Show(msg, "Erreur", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {//export error file to the selected directory
                                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                                saveFileDialog1.Filter = "CSV Files(*.csv) | *.csv";
                                saveFileDialog1.FilterIndex = 2;
                                saveFileDialog1.RestoreDirectory = true;

                                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    File.Copy(path_error_file, saveFileDialog1.FileName + ".csv", true);

                                }
                            }
                        }


                        //remove file
                        File.Delete(path_source_file);
                        File.Delete(path_error_file);

                    } catch(Exception ef)
                    { 
                        MessageBox.Show("Une erreur est survenue : impossible de mettre à jour la base de données à partir du fichier source selectionné !\n"+ef);
                        return;
                    }
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


        private void separator_csv_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //allow only one char
            if (separator_csv_textBox.Text.Length > 0)
            {
                separator_csv_textBox.Text = "";
            }

            if (e.KeyChar == ' ') //space
            {
                e.Handled = true;
                separator_csv_textBox.Text = TAG_SPACE;

            }
            else if (e.KeyChar == (char)13) //enter
            {
                e.Handled = true;
                separator_csv_textBox.Text = TAG_RETURN;
            }
        }


        private void separator_csv_textBox_Click(object sender, EventArgs e)
        {
            separator_csv_textBox.SelectAll();
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
