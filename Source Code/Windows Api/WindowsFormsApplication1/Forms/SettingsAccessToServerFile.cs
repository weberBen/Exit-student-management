using System;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1.Forms
{
    public partial class SettingsAccessToServerFile : Form
    {
        private string path_to_folder;

        public SettingsAccessToServerFile()
        {
            InitializeComponent();
            path_to_folder = "";


            string path_to_file = ToolsClass.Settings.getPathToUrlFile();
            if(string.IsNullOrEmpty(path_to_file))
            {
                activate_checkBox.Checked = false;
                display_resukt_textBox.Text = "";
                file_name_textBox.Text = "";
                path_to_folder = "";
                groupBox1.Enabled = false;
            }else
            {
                activate_checkBox.Checked = true;
                path_to_folder = Path.GetDirectoryName(path_to_file);
                display_resukt_textBox.Text = path_to_folder;
                file_name_textBox.Text = Path.GetFileNameWithoutExtension(path_to_file);
                groupBox1.Enabled = true;
            }
        }

        private void activate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!activate_checkBox.Checked)
            {
                display_resukt_textBox.Text = "";
                file_name_textBox.Text = "";
                path_to_folder = "";
                groupBox1.Enabled = false;
            }else
            {
                groupBox1.Enabled = true;
            }
        }

        private void change_folder_button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    path_to_folder = fbd.SelectedPath;
                    display_resukt_textBox.Text = path_to_folder;
                }
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if(activate_checkBox.Checked && (path_to_folder=="" || file_name_textBox.Text==""))
            {
                MessageBox.Show("Veuillez sélectionner un dossier et renseigner le nom du fichier avant de poursuivre");
                return;
            }

            ToolsClass.Settings.setPathToUrlFile(path_to_folder, file_name_textBox.Text);

            this.Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
