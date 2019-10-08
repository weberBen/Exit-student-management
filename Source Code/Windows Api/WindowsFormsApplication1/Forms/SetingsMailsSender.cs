using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parms = ToolsClass.Tools.ParmsMailsSender;
using System.Web;
using System.Security;

namespace WindowsFormsApplication1.Forms
{
    
    public partial class SetingsMailsSender : Form
    {

        private const string DISPLAY_PASSWORD = "password_saved";
        //avoid to set the real password in the textbox (even if it'is set for password input)
        private int scroll_bar_index;
        private bool first_time;


        public SetingsMailsSender()
        {
            InitializeComponent();
            scroll_bar_index = 0;
            first_time = true;


            Parms parms = ToolsClass.Settings.MailsSenderParameters;

            mail_adress_textBox.Text = parms.adress;
            if (parms.password == "")
            {
                password_textBox.Text = "";
            }
            else
            {
                password_textBox.Text = DISPLAY_PASSWORD;
            }
            smtp_server_textBox.Text = parms.smtpSever;
            smtp_port_textBox.Text = "" + parms.smtpPort;
            before_message_textBox.Text = parms.beforeMessage.Replace("<br>",Environment.NewLine);
            after_message_textBox.Text = parms.afterMessage.Replace("<br>", Environment.NewLine);
        }


        private void save_button_Click(object sender, EventArgs e)
        {
            if(mail_adress_textBox.Text=="" || password_textBox.Text==""  || smtp_port_textBox.Text=="" || smtp_port_textBox.Text=="")
            {
                MessageBox.Show("ERREUR : tous les champs doivent être renseignés !");
                return;
            }

            Parms parms = new Parms();
            parms.toDefault();

            parms.adress = mail_adress_textBox.Text;
            parms.smtpSever = smtp_server_textBox.Text;
            try
            {
                parms.smtpPort = Int32.Parse(smtp_port_textBox.Text);
            }
            catch
            {
                MessageBox.Show("ERREUR : format du port SMTP non reconnu");
                return;
            }

            if (password_textBox.Text != DISPLAY_PASSWORD) //new password, so have to change it
            {
                SecureString password = new SecureString();
                foreach (char c in password_textBox.Text)
                {
                    password.AppendChar(c);
                }
                password.MakeReadOnly();

                try
                {

                    parms.password = DataProtection.protect(password);
                }
                catch
                {
                    MessageBox.Show("ERREUR : Impossible de sécuriser le mot de passe avant de l'enregistrer");
                    return;
                }finally
                {
                    password.Dispose();
                }
            }//else password reamin the same (already saved in the sruct ParmsMailsSender)
            parms.beforeMessage = removeAllVoidAtTheExtremaOfString(before_message_textBox.Text).Replace(Environment.NewLine, "<br>");
            parms.afterMessage = removeAllVoidAtTheExtremaOfString(after_message_textBox.Text).Replace(Environment.NewLine, "<br>");


            ToolsClass.Settings.MailsSenderParameters = parms;

            this.Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {

            this.Close();
        }


        private void smtp_port_textBoxe_KeyPress(object sender, KeyPressEventArgs e)
        {
            //allow only numbers in that textbox
            if ( (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }


        private void password_textBox_Click (object sender, EventArgs e)
        {
            password_textBox.SelectAll();
        }


        private string removeAllVoidAtTheExtremaOfString(string input)
        {
            /*Remove all the spaces and the newlines found at the start and at the end before 
             * finding "real" text
            */
            input = input.Replace(Environment.NewLine, ""+'\n');

            int index_start, index_end;
            int exit;


            index_start = 0;
            index_end = input.Length - 1;
            exit = 0;
            while ((index_end >=0) && (exit == 0)) //remove space at the end of the string
            {
                exit = 1;
               
                if (input[index_start] == ' ' || input[index_start] == '\n')
                {
                    input = input.Remove(index_start, 1);
                    index_end--;
                    exit = 0;
                }
                if (input[index_end] == ' ' || input[index_end] == '\n')
                {
                    input = input.Remove(index_end, 1);
                    exit = 0;
                    index_end--;
                }

            }

            
            return input.Replace("" + '\n', Environment.NewLine);
        }


        private void update_webbrowsers()
        {
            /*take the content of textboxes to display it as html in the webbrowser*/
            try
            {
                scroll_bar_index = webBrowser_message.Document.Body.ScrollTop;
                //get index of the scrollbar in order to set the position view of the new document (when it will finish to load)
            }catch{}

            webBrowser_message.DocumentText = before_message_textBox.Text.Replace(Environment.NewLine, "<br>")
                    + "<br><br><br>----------------------<br><br><br>" + after_message_textBox.Text.Replace(Environment.NewLine, "<br>"); ;

        }


        private void setHtmlText(string tag)
        {
            /*When user have selected a text section and he click on a control buttun (like italic, bold, underlign)
             * If the section has already the corresponding HTML tag (for example if the section is already underliged)
             * This method remove the tag.
             * Else set the correct tag
             * 
             * If the select section is null do nothing
            */

            TextBox textBox = new TextBox();
            string select_text;
            string start_tag = "<" + tag + ">";
            string end_tag = "</" + tag + ">";

            if (before_message_textBox.SelectedText != "")
            {
                select_text = before_message_textBox.SelectedText;
                textBox = before_message_textBox;
            }
            else if(after_message_textBox.SelectedText !="")
            {
                select_text = after_message_textBox.SelectedText;
                textBox = after_message_textBox;
            }else
            {
                MessageBox.Show("Veuillez sélectionner une partie du texte pour opérer la transformation souhaitée.\nSi la section est déjà transformée " +
                    "Cette opération aura pour effet d'annuler la transformation effective");
                return;
            }


            if (select_text.Contains(start_tag) && select_text.Contains(end_tag))
            {
                select_text = select_text.Replace(start_tag, "");
                select_text = select_text.Replace(end_tag, "");

                textBox.SelectedText = select_text;
            }
            else
            {
                textBox.SelectedText = start_tag + select_text + end_tag;
            }

        }

        private void italic_button_Click(object sender, EventArgs e)
        {

            setHtmlText("i");
        }

        private void bold_button_Click(object sender, EventArgs e)
        {
            setHtmlText("b");
        }

        private void underline_button_Click(object sender, EventArgs e)
        {
            setHtmlText("u");
        }

        private void highligth_button_Click(object sender, EventArgs e)
        {
            setHtmlText("mark");
        }

        private void textChangedEventHandler(object sender, EventArgs args)
        {
            update_webbrowsers();
        } 


        private void upate_webBrowsers_button_Click(object sender, EventArgs e)
        {
            update_webbrowsers();
        }

        private void webBrowserMessageDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            //When new document is loaded, set the scrollbar index
            webBrowser_message.Document.Body.ScrollTop = scroll_bar_index;

            if(first_time)//display content when user open the form
            {
                first_time = false;
                update_webbrowsers();
            }
        }
        
    }
}
