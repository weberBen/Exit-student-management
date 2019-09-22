using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolsClass;

namespace WindowsFormsApplication1.Forms
{
    /*
     * When user wants to sees and modifies server element as ip adress and communication port
    */

    partial class SettingsServerElements : Form
    {
        public SettingsServerElements()
        {
            InitializeComponent();
            this.ipAdressTextBox.Text = Settings.ServerIpAdress; //show the saved ip adress 
            this.portTextBox.Text= "" + Settings.ServerPort; //show the saved ip port
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

        private void save_Click(object sender, EventArgs e)
        {
            /*
             * When the user has finished all the modification and want to take ist in consideration
            */

            int port = -1;
            int.TryParse(portTextBox.Text, out port);

            if(port==-1)
            {
                MessageBox.Show("Port de communication non valide : le port de communication doit être un entier positif");
                return;
            }

            Settings.ServerIpAdress = ipAdressTextBox.Text; //change the ip adress the specific file
            Settings.ServerPort = port;//change the port in the specific file

            this.Close();
        }


        private void cancel_Click(object sender, EventArgs e)
        {
            /*
             * Don't take consideration of the evuentual consideration and quit 
            */
            this.Close();
        }

        private void findPortAuto_Click(object sender, EventArgs e)
        {
            /*
             * User asks to automatically find an available port
            */
            this.portTextBox.Text = Tools.findNewPort().ToString();
        }

        private void findIPAuto_Click(object sender, EventArgs e)
        {
            /*
             * User asks to automatically find the actual IPv4 wired adress 
            */
            this.ipAdressTextBox.Text = Tools.getLocalWiredIPAddress();
        }
    }
}
