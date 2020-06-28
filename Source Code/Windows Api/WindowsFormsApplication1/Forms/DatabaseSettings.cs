using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParmsDatabase = ToolsClass.Tools.ParmsDatabase;

namespace WindowsFormsApplication1.Forms
{
    public partial class DatabaseSettings : Form
    {
        public DatabaseSettings()
        {
            InitializeComponent();

            ParmsDatabase parms_db = ToolsClass.Settings.Database;

            driver_name_textBox.Text = parms_db.DriverName;
            server_name_textBox.Text = parms_db.ServerName;
            database_name_textBox.Text = parms_db.DatabaseName;
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if(driver_name_textBox.Text.Length==0 || server_name_textBox.Text.Length == 0 || database_name_textBox.Text.Length == 0)
            {
                MessageBox.Show("Veuillez remplir les champs obligatoires (*)");
                return;
            }

            ParmsDatabase parms_db = ToolsClass.Settings.Database;
            parms_db.toDefault();

            parms_db.DriverName = driver_name_textBox.Text;
            parms_db.ServerName = server_name_textBox.Text;
            parms_db.DatabaseName = database_name_textBox.Text;

            ToolsClass.Settings.Database = parms_db;
            MessageBox.Show("Les modifications ont été enregistré mais elles ne seront prise en compte qu'au prochain démarrage de l'application");

            this.Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
