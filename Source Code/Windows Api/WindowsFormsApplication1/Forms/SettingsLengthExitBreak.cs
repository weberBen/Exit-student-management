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
    public partial class SettingsLengthExitBreak : Form
    {
        private const int MAX_HOURS = 9;
        private const int MAX_MINUTES = 60;

        public SettingsLengthExitBreak()
        {
            InitializeComponent();

            TimeSpan time =ToolsClass.Settings.ExitBreak;

            if (ToolsClass.Settings.isExitBreakEnabled(time))
            {
                hour_textBox.Text = time.Hours.ToString();
                minute_textBox.Text = time.Minutes.ToString();

                enable_pause_checkBox.Checked = true;
                groupBox.Enabled = true;
            }
            else
            {
                enable_pause_checkBox.Checked = false;
                groupBox.Enabled = false;
            }
        }

        private void enable_pause_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if(enable_pause_checkBox.Checked)
            {
                groupBox.Enabled = true;
            }else
            {
                groupBox.Enabled = false;
            }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            
            TimeSpan time = new TimeSpan();
            TimeSpan.TryParse(hour_textBox.Text + ":" + minute_textBox.Text + ":00", out time);

            if(!enable_pause_checkBox.Checked)
            {
                time = ToolsClass.Settings.EXIT_BREAK_NOT_ENABLED_TIMESPAN_VALUE;
            }
            ToolsClass.Settings.ExitBreak = time;

            this.Close();
        }

        private void hour_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void minute_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        private void hour_textBox_TextChanged(object sender, EventArgs e)
        {
            int hour = 0;
            Int32.TryParse(hour_textBox.Text, out hour);

            if(hour>MAX_HOURS)
            {
                hour_textBox.Text = "";
                MessageBox.Show("L'heure doit être comprise entre 0h et "+ MAX_HOURS+"h (inclus)");
            }
        }

        private void minute_textBox_TextChanged(object sender, EventArgs e)
        {
            int minute = 0;
            Int32.TryParse(minute_textBox.Text, out minute);

            if (minute > MAX_MINUTES)
            {
                minute_textBox.Text = "";
                MessageBox.Show("Les minutes doivent être comprises entre 0min et "+ MAX_MINUTES+"min (inclus)");
            }
        }
    }
}
