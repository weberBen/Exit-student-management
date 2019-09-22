using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Forms
{
    public partial class HtmlMessageBox : Form
    {
        public HtmlMessageBox(String Title, ref String htmlText)
        {
            InitializeComponent();
            this.Text = Title;
            this.webBrowser.DocumentText = htmlText;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
