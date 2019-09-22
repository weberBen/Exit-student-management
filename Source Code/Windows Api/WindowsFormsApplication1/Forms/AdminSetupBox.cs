using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Forms
{
    partial class AdminSetupBox : Form
    {
        public String admin_lastName = null;
        public String admin_firstName = null;
        public String admin_id = null;
        public SecureString admin_password = null;

        public AdminSetupBox()
        {
            InitializeComponent();
            info_password_textBox.Text = ToolsClass.Definition.INCORRECT_PASSWORD_FORMAT_TEXT;
        }

        #region Accesseurs d'attribut de l'assembly

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void textbox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                save_button.PerformClick();
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            String last_name = lastName_textBox.Text;
            String first_name = firstName_textBox.Text;
            String id = id_textBox.Text;

            if(last_name.Length==0 || first_name.Length==0 || id.Length==0 || password1_textBox.Text.Length ==0 || password2_textBox.Text.Length ==0)
            {
                MessageBox.Show("Tous les champ doivent être remplis !");
                return;
            }
            if (!SecurityManager.isIdFree(id_textBox.Text, -1))
            {
                MessageBox.Show("L'identifiant est déjà pris");//check if the given agent id is free into the database
                return;
            }

            if (password1_textBox.Text != password2_textBox.Text)
            {
                MessageBox.Show("les mots de passe ne correspondent pas");
                return;
            }

            SecureString password = new SecureString();
            foreach (char c in password1_textBox.Text)
            {
                password.AppendChar(c);
            }
            password.MakeReadOnly();

            if (!SecurityManager.formatPasswordCorrect(password))
            {
                MessageBox.Show(ToolsClass.Definition.INCORRECT_PASSWORD_FORMAT_TEXT);
                password.Dispose();
                return;
            }

            this.admin_lastName = last_name;
            this.admin_firstName = first_name;
            this.admin_id = id;
            this.admin_password = password;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
