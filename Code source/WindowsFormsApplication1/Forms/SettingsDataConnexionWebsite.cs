using System;
using System.Windows.Forms;
using System.Security;

namespace WindowsFormsApplication1.Forms
{
    public partial class SettingsDataConnexionWebsite : Form
    {
        public SettingsDataConnexionWebsite()
        {
            InitializeComponent();
        }

        
        private void save_button_Click(object sender, EventArgs e)
        {
            if(password_textBox.Text!="" && id_textBox.Text!="")
            {
                SecureString id = new SecureString();
                foreach (char c in id_textBox.Text)
                {
                    id.AppendChar(c);
                }
                id.MakeReadOnly();


                SecureString password = new SecureString();
                foreach (char c in password_textBox.Text)
                {
                    password.AppendChar(c);
                }
                password.MakeReadOnly();

                DataProtection.saveSecureDataConnexionWebsite(id,password);

                id.Dispose();
                password.Dispose();

                this.Close();
            }
            else
            {
                MessageBox.Show("ERREUR : il est indispensable de renseigner tous les champs !");
            }
            
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
