using System;
using System.Windows.Forms;
using System.Security;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}