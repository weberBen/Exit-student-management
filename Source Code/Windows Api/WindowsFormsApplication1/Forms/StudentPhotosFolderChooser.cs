using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Forms
{
    partial class StudentPhotosFolderChooser : Form
    {
        public StudentPhotosFolderChooser()
        {
            InitializeComponent();
            this.Text = String.Format("À propos de {0}", AssemblyTitle);
            
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

        private String path_folder = null;

        private void save_button_Click(object sender, EventArgs e)
        {
            if(path_folder==null)
            {
                MessageBox.Show("Aucun dossier sélectionné");
                return;
            }

            if(merge_radioButton.Checked)
            {
                //copy all the files
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(path_folder, ToolsClass.Definition.PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs);
                }
                catch(Exception ioE)
                {
                    MessageBox.Show("Erreur : \n" + ioE);
                    return;
                }
            }else if(delete_radioButton.Checked)
            {
                //remove all file and folder
                System.IO.DirectoryInfo di = new DirectoryInfo(ToolsClass.Definition.PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS);
                foreach(FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                //copy all the files
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(path_folder, ToolsClass.Definition.PATH_TO_FOLDER_STUDENTS_PUBLIC_PHOTOS, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs);
                }
                catch (Exception ioE)
                {
                    MessageBox.Show("Erreur : \n" + ioE);
                    return;
                }
            }

            MessageBox.Show("L'importation s'est terminée avec succès");
            this.Close();
        }

        private void import_button_Click(object sender, EventArgs e)
        {
            using (var fdb = new FolderBrowserDialog())
            {
                DialogResult result = fdb.ShowDialog();
                if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fdb.SelectedPath))
                {
                    path_folder = fdb.SelectedPath;
                    folder_textBox.Text = path_folder;
                }
            }
        }
    }
}
