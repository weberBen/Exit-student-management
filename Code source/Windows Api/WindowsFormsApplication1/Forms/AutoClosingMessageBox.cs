using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Forms
{
    public partial class AutoClosingMessageBox : Form
    {
        private static Timer timer;

        private bool answer = false;

        public AutoClosingMessageBox(string title, string message, int timeout, string text_button_ok="Oui", string text_button_cancel="Quiter")
        {
            InitializeComponent();
            this.Text = title;
            this.message_textBox.Text = message;
            this.ok_button.Text = text_button_ok;
            this.cancel_button.Text = text_button_cancel;


            timer = new System.Windows.Forms.Timer();
            timer.Interval = timeout;
            timer.Tick += Timer_Tick; //event timer tick

            timer.Start();

        }


        private void Timer_Tick(object sender, EventArgs e) //handler when the time counter is finished
        {
            answer = false;
            this.Close();
        }



        private void cancel_button_Click(object sender, EventArgs e)
        {
            answer = false;
            this.Close();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            answer = true;
            this.Close();
        }

        public bool getDialogResult()
        {
            return answer;
        }
    }
}
