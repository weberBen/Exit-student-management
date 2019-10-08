using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolsClass;
using StudyTime = ToolsClass.Tools.StudyTime;


namespace WindowsFormsApplication1.Forms
{
    public partial class SchoolDataForm : Form
    {
        private int index_lunch_time;
        private int index_viewed_item;
        private List<StudyTime> list_time;

        public SchoolDataForm()
        {
            InitializeComponent();
            index_lunch_time = -1;
            index_viewed_item = -1;
            list_time = new List<StudyTime>();


            //set data source for the study hours
            list_time = Settings.StudyHours;

            int i;

            i=0;
            while ( (i<list_time.Count) && (list_time[i].isLunchTime==false) )
            {
                i++;
            }

            if(i<list_time.Count)
            {
                index_lunch_time = i;
            }

            resetItemBox();
            updateListBox();

            //set data source for the days of the week
            checkedListBox.DataSource = new BindingSource(Definition.DAYS_OF_THE_WEEK, null);
            //set items selected or not
            List<int> list_selected_day_index = Settings.SchoolDays;
            for (int j = 0; j < checkedListBox.Items.Count; j++)
            {
                string item_value = (string)checkedListBox.Items[j];
                int index = Definition.DAYS_OF_THE_WEEK.LastIndexOf(item_value);
                if(index!=-1)
                {
                    index = list_selected_day_index.LastIndexOf(index);
                    if(index != -1)
                    {
                        checkedListBox.SetItemChecked(j, true);
                    }
                }
            }

            school_name_textBox.Text = ToolsClass.Settings.SchoolName;
        }




        private void textBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            //allow only number into the textbox
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void resetItemBox()
        {
            start_hour_textBox.Text = "";
            end_hour_textBox.Text = "";
            start_minutes_textBox.Text = "";
            end_minutes_textBox.Text = "";

            lunch_hour_checkBox.Checked = false;

            start_hour_textBox.Focus();
        }


        private void updateListBox()
        {
            listBox.DataSource = null;//reset data source

            list_time = list_time.OrderBy(t => t.startTime).ToList();

            listBox.DataSource = list_time.Select(time => time.ItemToText).ToList();//update data source

            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        private StudyTime createItem()
        {
            StudyTime time = new StudyTime();
            time.toDefault();

            int start_hour = -1;
            int start_minutes = -1;
            int end_hour = -1;
            int end_minutes = -1;

            int.TryParse(start_hour_textBox.Text, out start_hour);
            int.TryParse(start_minutes_textBox.Text, out start_minutes);
            int.TryParse(end_hour_textBox.Text, out end_hour);
            int.TryParse(end_minutes_textBox.Text, out end_minutes);

            if (start_hour == -1 || end_hour == -1 || start_minutes == -1 || end_minutes == -1)
            {
                MessageBox.Show("ERREUR : le format des créneaux horaires spécifiés n'est pas valide");
                time.error = true;
                return time;
            }

            TimeSpan start_time = new TimeSpan(start_hour, start_minutes, 0);
            TimeSpan end_time = new TimeSpan(end_hour, end_minutes, 0);

            if(start_time>=end_time)
            {
                MessageBox.Show("ERREUR : Le créneau horaire ne peut pas débuter après avoir pris fin");
                time.error = true;
                return time;
            }

            time.startTime = start_time;
            time.endTime = end_time;
            time.isLunchTime = lunch_hour_checkBox.Checked;
            time.setTextItem();

            return time;
        }


        private void add_item_button_Click(object sender, EventArgs e)
        {

            StudyTime time = createItem();
            if(time.error)
            {
                return;
            }

            //add item to list
            list_time.Add(time);

            if (time.isLunchTime)
            {
                if (index_lunch_time != -1)
                {
                    StudyTime temp_time = list_time[index_lunch_time];
                    temp_time.isLunchTime = false;
                    temp_time.setTextItem();

                    list_time[index_lunch_time] = temp_time;
                }

                index_lunch_time = list_time.Count - 1;
            }

            updateListBox();
            resetItemBox();
        }

        private void remove_item_button_Click(object sender, EventArgs e)
        {
            int index = listBox.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                index_viewed_item = (list_time.Select(time => time.ItemToText).ToList()).LastIndexOf(listBox.Items[index].ToString());

                if (index_viewed_item != -1)
                {
                    if (list_time[index_viewed_item].isLunchTime)
                    {
                        index_lunch_time = -1;
                    }

                    list_time.RemoveAt(index_viewed_item);

                    updateListBox();
                }
            }

            index_viewed_item = -1;
        }



        private void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*When user double click on an item we display all the informations about the 
             * student represented by that item
            */
           
            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                index_viewed_item = (list_time.Select(time => time.ItemToText).ToList()).LastIndexOf(listBox.Items[index].ToString());

                if(index_viewed_item != -1)
                {
                    start_hour_textBox.Text = ""+list_time[index_viewed_item].startTime.Hours;
                    start_minutes_textBox.Text = "" + list_time[index_viewed_item].startTime.Minutes;

                    end_hour_textBox.Text = "" + list_time[index_viewed_item].endTime.Hours;
                    end_minutes_textBox.Text = "" + list_time[index_viewed_item].endTime.Minutes;

                    if (list_time[index_viewed_item].isLunchTime)
                    {
                        lunch_hour_checkBox.Checked = true;
                    }else
                    {
                        lunch_hour_checkBox.Checked = false;
                    }
                }
            }

        }

        private void edit_item_button_Click(object sender, EventArgs e)
        {
            if (index_viewed_item != -1)
            {
                StudyTime time = createItem();
                if (time.error)
                {
                    return;
                }

                if ((index_viewed_item == index_lunch_time) && (!lunch_hour_checkBox.Checked))
                { //the actual lunch time is no longer lunch time
                    index_lunch_time = -1;
                }else if (time.isLunchTime)
                {
                    if (index_lunch_time != -1)
                    {
                        StudyTime temp_time = list_time[index_lunch_time];
                        temp_time.isLunchTime = false;
                        temp_time.setTextItem();

                        list_time[index_lunch_time] = temp_time;
                    }

                    index_lunch_time = index_viewed_item;
                }

                list_time[index_viewed_item] = time;

                updateListBox();
                resetItemBox();
            }

            index_viewed_item = -1;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if(index_lunch_time==-1)
            {
                MessageBox.Show("ERREUR : veuillez définir un créneau horaire comme étant celui du déjeuner");
                return;
            }

            //save study hours
            Settings.StudyHours = list_time;

            //save school days
            List<int> list_index_days = new List<int>();
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    string item_value = (string)checkedListBox.Items[i];
                    int index = Definition.DAYS_OF_THE_WEEK.LastIndexOf(item_value);
                    if(index!=-1)
                    {
                        list_index_days.Add(index);
                    }
                }
            }
            Settings.SchoolDays = list_index_days;
            Settings.SchoolName = school_name_textBox.Text;


            this.Close();
        }
    }
}
