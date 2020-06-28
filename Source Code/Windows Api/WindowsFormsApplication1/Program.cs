﻿using System;
using System.Windows.Forms;
using System.Security;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace WindowsFormsApplication1
{

    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>


        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Une instance du porgramme est déjà lancée");
            }
        }

    }
}
 