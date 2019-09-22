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
    public partial class SettingsList : Form
    {
        /*Show a listBow to the user in order to let him enter informations as list */
        private List<string> data = new List<string>();
        private string use = "";

        public SettingsList(string useFor="")
        {
            InitializeComponent();

            use = useFor;

            //the same structure is used for multiple type of list
            if(use == ToolsClass.Definition.USE_FOR_MAILS_LIST)
            {
                display_textBox.Text = "Veuillez saisir les adresses emails souhaitées pour recevoir les éventuels messages du système en cas de problème";
                data = ToolsClass.Settings.MailsList;
            }else if(use == ToolsClass.Definition.USE_FOR_EXIT_BAN_REASONS_LIST)
            {
                display_textBox.Text = "Veuillez saisir les différents motifs les plus courants liés à une interdiction de sortie";
                data = ToolsClass.Settings.ExitBanReasonsList;
            }

            checkedListBox1.DataSource = new BindingSource(data, null);
            



        }

        private void add_item_button_Click(object sender, EventArgs e)
        {
            if (item_textBox.Text != "" && item_textBox.Text != null)
            {
                data.Add(item_textBox.Text);
                item_textBox.Text = "";
                checkedListBox1.DataSource = new BindingSource(data, null);
                item_textBox.Focus();
            }
        }

        private void del_item_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    data.Remove(checkedListBox1.Items[i].ToString());
                }
            }

            checkedListBox1.DataSource = new BindingSource(data, null);
        }



        private void save_button_Click(object sender, EventArgs e)
        {
            if (use == ToolsClass.Definition.USE_FOR_MAILS_LIST)
            {
                ToolsClass.Settings.MailsList = data;
            }else if (use == ToolsClass.Definition.USE_FOR_EXIT_BAN_REASONS_LIST)
            {
                ToolsClass.Settings.ExitBanReasonsList = data;
            }

            this.Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
