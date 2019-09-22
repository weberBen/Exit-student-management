using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ParmsStudentPhoto = ToolsClass.Tools.ParmsStudentPhoto;

namespace WindowsFormsApplication1.Forms
{
    public partial class SettingsStudentPhoto : Form
    {
        /*Allow user to select the format of each student photo name
         * For exemple user can specify a name like "firstNameToLower_lastNameToUpper_DivisionToUpper" or 
         * "firstNameToLower lastNameToUpper DivisionToUpper" or "firstNameToUpper lastNameToULower DivisionToUpper" or ...
         * 
         * User choose the order of each element (which is basically the last name, first name and division) and choose the desired separator
         * like (' ', '-', '-', ...)
        */
        private const string TAG_SPACE = "SPACE";
        private static char actual_key_char = '_';
        private const string TAG_LAST_NAME = "Nom";
        private const string TAG_FIRST_NAME = "Prénom";
        private const string TAG_DIVISION = "Classe";
        private static string[] LIST_ELEMENT_TAG = new string[] { TAG_LAST_NAME, TAG_FIRST_NAME, TAG_DIVISION };
        private static TextBox[] LIST_TEXTBOXES_ELEMENTS_TAG;
        private static TextBox[] LIST_TEXTBOXES_ELEMENTS_ORDER;
        private static RadioButton[] LIST_MAJ_RADIOBUTONS;
        private static RadioButton[] LIST_MIN_RADIOBUTONS;
        private static RadioButton[] LIST_NORMALIZE_RADIOBUTONS;
        private static RadioButton[] LIST_NONE_RADIOBUTONS;
        

        public SettingsStudentPhoto()
        {
            InitializeComponent();

            LIST_TEXTBOXES_ELEMENTS_TAG = new TextBox[] { this.element_textBox_0, this.element_textBox_1, this.element_textBox_2 };
            LIST_TEXTBOXES_ELEMENTS_ORDER = new TextBox[] { this.order_element_textBox_0, this.order_element_textBox_1, this.order_element_textBox_2 };
            LIST_MAJ_RADIOBUTONS = new RadioButton[] { this.maj_radioButton_0, this.maj_radioButton_1, this.maj_radioButton_2 };
            LIST_MIN_RADIOBUTONS = new RadioButton[] { this.min_radioButton_0, this.min_radioButton_1, this.min_radioButton_2 };
            LIST_NORMALIZE_RADIOBUTONS = new RadioButton[] { this.normalize_radioButton_0, this.normalize_radioButton_1, this.normalize_radioButton_2 };
            LIST_NONE_RADIOBUTONS = new RadioButton[] { this.none_radioButton_0, this.none_radioButton_1, this.none_radioButton_2 };

            ParmsStudentPhoto parms = ToolsClass.Settings.StudentPhotoParameters;

            //set the view for user with the previous saved parms
            for (int i=0; i< LIST_ELEMENT_TAG.Length;i++)
            {
                //set name of element in NAME_ELEMENTS as text inside existing textboxes
                LIST_TEXTBOXES_ELEMENTS_TAG[i].Text = LIST_ELEMENT_TAG[i];

                //set the corresponding parms
                var tuple = Tuple.Create(-1,-1);
                RadioButton radioButton;
                int index;
                //get position of the element
                switch (LIST_ELEMENT_TAG[i])
                {
                    case TAG_LAST_NAME:
                        tuple = parms.LastName;
                        break;
                    case TAG_FIRST_NAME:
                        tuple = parms.FirstName;
                        break;
                    case TAG_DIVISION:
                        tuple = parms.Division;
                        break;
                }
                //get format of the element
                switch (tuple.Item2)
                {
                    case ToolsClass.Definition.TEXT_FORMAT_UPPER_CASE:
                        radioButton = LIST_MAJ_RADIOBUTONS[i];
                        break;
                    case ToolsClass.Definition.TEXT_FORMAT_LOWER_CASE:
                        radioButton = LIST_MIN_RADIOBUTONS[i];
                        break;
                    case ToolsClass.Definition.TEXT_FORMAT_NORMALIZE:
                        radioButton = LIST_NORMALIZE_RADIOBUTONS[i];
                        break;
                    default:
                        radioButton = LIST_NONE_RADIOBUTONS[i];
                        break;
                }
                //update view for user
                index = tuple.Item1;
                if(index <LIST_ELEMENT_TAG.Length)
                {
                    //for the user index start to 1 and 0 is used to specify that an element must not be used into the final format (so for the computer index >=-1)
                    LIST_TEXTBOXES_ELEMENTS_ORDER[i].Text = (index+1).ToString();
                    radioButton.Checked = true;
                }
            }

            actual_key_char = parms.separator;
            if (parms.separator==' ')
            {
                separator_textBox.Text = TAG_SPACE;
            }else
            {
                separator_textBox.Text = parms.separator.ToString();
            }

            display_result_textBox.Text = getFormatedString();//update the formated string



        }


        private void separator_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            actual_key_char = e.KeyChar;
            //allow only one char
            if (separator_textBox.Text.Length > 0)
            {
                separator_textBox.Text = "";
            }

            if (e.KeyChar == ' ') //space
            {
                e.Handled = true;
                separator_textBox.Text = TAG_SPACE;

            }
        }

        private ParmsStudentPhoto getFormatedParms()
        {
            //get order of each element
            int format_element;
            ParmsStudentPhoto parmsStudentPhoto = new ParmsStudentPhoto();
            parmsStudentPhoto.toDefault();

            parmsStudentPhoto.separator = actual_key_char;

            //gte the order of element set by user
            for (int i = 0; i < LIST_ELEMENT_TAG.Length; i++)
            {


                int index = 0;
                int.TryParse(LIST_TEXTBOXES_ELEMENTS_ORDER[i].Text.ToString(), out index);
                index = index - 1;//-1 because user have to enter index which start to 1
                if (index >= 0 && index < LIST_ELEMENT_TAG.Length)
                {
                    //set format
                    if (LIST_MAJ_RADIOBUTONS[i].Checked)//maj selected
                    {
                        format_element = ToolsClass.Definition.TEXT_FORMAT_UPPER_CASE;
                    }
                    else if (LIST_MIN_RADIOBUTONS[i].Checked)//min selected
                    {
                        format_element = ToolsClass.Definition.TEXT_FORMAT_LOWER_CASE;
                    }
                    else if (LIST_NORMALIZE_RADIOBUTONS[i].Checked)//remove all diacritics
                    {
                        format_element = ToolsClass.Definition.TEXT_FORMAT_NORMALIZE;
                    }
                    else
                    {
                        format_element = ToolsClass.Definition.TEXT_DEFAULT_FORMAT;
                    }

                }else//index = -1 => do not include the element
                {
                    format_element = ToolsClass.Definition.TEXT_FORMAT_INVISIBLE;
                }

                //set the corresponding parms
                switch (LIST_ELEMENT_TAG[i])
                {
                    case TAG_LAST_NAME:
                        parmsStudentPhoto.LastName = Tuple.Create(index, format_element);
                        break;
                    case TAG_FIRST_NAME:
                        parmsStudentPhoto.FirstName = Tuple.Create(index, format_element);
                        break;
                    case TAG_DIVISION:
                        parmsStudentPhoto.Division = Tuple.Create(index, format_element);
                        break;
                }

            }


            return parmsStudentPhoto;
        }


        private string getFormatedString()
        {
            ParmsStudentPhoto parms = getFormatedParms();
            /* all the needed parms has been set. Then we have to set all the element to the correct order and to the correc format*/

            string res = "";
            string element_value = null;
            var tuple = Tuple.Create(-1, -1);
            int format = -1;
            bool first_elem = true;
            int index = -1;
            string[] list_elements = new string[parms.getNumberElements()];
            //string that will contains all the element at the correct position, then we will have to gather all that element into one single string

            //initialize array
            for(int i=0; i< list_elements.Length; i++)
            {
                list_elements[i] = null;
            }

            //local method
            Action applyFormatOnString = () =>//set the correct format
            {
                switch (format)
                {
                    case ToolsClass.Definition.TEXT_FORMAT_UPPER_CASE:
                        element_value = element_value.ToUpper();
                        break;
                    case ToolsClass.Definition.TEXT_FORMAT_LOWER_CASE:
                        element_value = element_value.ToLower();
                        break;
                    case ToolsClass.Definition.TEXT_FORMAT_NORMALIZE:
                        element_value = ToolsClass.Tools.removeDiacritics(element_value).ToLower();
                        break;
                    case ToolsClass.Definition.TEXT_FORMAT_INVISIBLE:
                        element_value = null;
                        break;
                }
            };


            Action addElement = () =>//set element at the correct position
            {
                index = tuple.Item1;
                if (index < 0) //index = -1 to specify the current element must be included
                    return;
                format = tuple.Item2;
                applyFormatOnString();
                list_elements[index] = element_value;
            };

            //set element 1
            tuple = parms.LastName;
            element_value = TAG_LAST_NAME;
            addElement();
            //set element 2
            tuple = parms.FirstName;
            element_value = TAG_FIRST_NAME;
            addElement();
            //set element 3
            tuple = parms.Division;
            element_value = TAG_DIVISION;
            addElement();

            //gather all the elements
            for (int i = 0; i < list_elements.Length; i++)
            {
                if (list_elements[i] == null)//invisible element (must not be included)
                    continue;

                /* we add the separator char before the element in the loop to avoid having a space between a non null element follows by a null element
                 * For example, if we add the separator char after the element the following situation could happen with a list of elements set to [NAME, null, null] 
                 * then the output will be "NAME SEPARATOR_CHAR" instead of "NAME"
                */
                if (!first_elem)//because element at position 0 can be null, the fisrt element to display will not be for the index 0
                {
                    res += parms.separator;
                }else
                {
                    first_elem = false;
                }
                res += list_elements[i];
            }

            return res;

        }


        private void visualisation_button_Click(object sender, EventArgs e)
        {

            display_result_textBox.Text = getFormatedString();//display text

        }

        private Tuple<bool, String> valideParameters()
        {
            if(separator_textBox.Text.Length == 0)
            {
                return Tuple.Create(false, "Le caractère de séparation de champ n'a pas été saisi");
            }else
            {
                //check if each index is used at least one time
                bool not_null_array = false;
                int[] index_array = new int[LIST_ELEMENT_TAG.Length];
                for (int i = 0; i < index_array.Length; i++)
                {
                    index_array[i] = 0;
                }

                TextBox textBox;

                for(int i=0; i<LIST_TEXTBOXES_ELEMENTS_ORDER.Length; i++)
                {
                    textBox = LIST_TEXTBOXES_ELEMENTS_ORDER[i];


                    if (textBox.Text.Length==0)
                    {
                        return Tuple.Create(false, "La position de l'élément "+ LIST_ELEMENT_TAG[i]+" n'a pas été saisi");
                    }
                    else
                    {
                        int val = -1;
                        if(Int32.TryParse(textBox.Text, out val))
                        {
                            val--;//element start from 1 for user

                            if (val == -1)//element to not include
                                continue;    
                            if (val >= index_array.Length)
                                return Tuple.Create(false, "La position de l'élément " + LIST_ELEMENT_TAG[i] + " dépasse le maximun possible de "+ LIST_ELEMENT_TAG.Length);
                            if(val<-1)
                                return Tuple.Create(false, "La position de l'élément " + LIST_ELEMENT_TAG[i] + " doit être positive ");
                            if (index_array[val] != 0)
                                return Tuple.Create(false, "La position de l'élément " + LIST_ELEMENT_TAG[i] + " est déjà prise par un autre");

                            index_array[val]++;
                            not_null_array = true;
                        }
                        else
                        {
                            return Tuple.Create(false, "La position de l'élément " + LIST_ELEMENT_TAG[i] + " n'est pas valide (doit être comprise entre 0 et "+ LIST_ELEMENT_TAG.Length);
                        }
                    }
                }

                if (!not_null_array)
                {
                    return Tuple.Create(false, "Aucun élement n'a été positionné");
                }
            }

            return Tuple.Create(true, "");
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            var tuple = valideParameters();
            if (!tuple.Item1)
            {
                MessageBox.Show("ERREUR : certains paramètres ne sont pas valides :\n"+tuple.Item2);
                return;
            }

            ToolsClass.Settings.StudentPhotoParameters = getFormatedParms();
            this.Close();
        }


        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
