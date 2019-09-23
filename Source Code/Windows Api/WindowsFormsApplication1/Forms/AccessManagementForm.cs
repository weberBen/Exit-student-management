using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Agent = ToolsClass.Tools.Agent;
using System.Text.RegularExpressions;
using System.Net;
using System.Security;
using System.Web;

namespace WindowsFormsApplication1.Forms
{
    public partial class AccessManagementForm : Form
    {
        private const string PASSWORD_TAG = "password_tag";
        private const string SHORT_ID_TAG = "0000";
        private Color CUSTOM_RED = Color.FromArgb(255, 179, 179);
        private Color CUSTOM_GREEN = Color.FromArgb(179, 255, 179);
        private Color DEFAULT_COLOR; 

        private List<int> list_ids = new List<int>();
        private List<string> list_results = new List<string>();

        private DataBase database = new DataBase();
        private Agent agent;
        private string old_mail_adress;

        private const int NUMBER_CHAR_PASSWORD = 10;

        public AccessManagementForm()
        {
            InitializeComponent();

            DEFAULT_COLOR = display_password_state_textBox.BackColor;
            agent.toDefault();

            setListBox();

            right_checkedListBox.DataSource = new BindingSource(SecurityManager.RIGHTS_LIST, null);
            //Add to the checkedlistbox all the right (accreditation level) transcripted to text
        }

        private void setListBox()
        {
            /*set list of all current registered agent*/
            listBox.DataSource = new List<string>();

            var res = database.getAllAgents();
            list_ids = res.Item1;//list of all agent into the database (that are not delete)
            list_results = res.Item2;//list of informations about the corresponding agent

            listBox.DataSource = list_results;//show all the agent into the listbox
        }


        private bool isDataCorrect()
        {
            /*If the user correctly filled all the mandatory*/
            if (last_name_textBox.Text == "" || first_name_textBox.Text == "" || job_textBox.Text == ""
               || !ToolsClass.Tools.IsValidEmail(mail_adress_textBox.Text) || right_checkedListBox.CheckedItems.Count != 1
               || (agent.tableId == -1 && password_textBox.Text.Replace(" ", "").Length == 0 ))
            {
                MessageBox.Show("Les champs obligatoire (*) doivent être correctement rempli !");
                return false;
            }

            //new password for already registered agent
            if(password_textBox.Text.Replace(" ", "").Length != 0)
            {
                    SecureString password = new SecureString();
                    foreach (char c in password_textBox.Text)
                    {
                        password.AppendChar(c);
                    }
                    password.MakeReadOnly();

                    if (password_textBox.Text != password_confirmation_textBox.Text)
                    {//If the two password don't matches
                        MessageBox.Show("Les mots de passe ne correspondent pas");
                        return false;
                    }
                    else if (!SecurityManager.formatPasswordCorrect(password))
                    {//if the password format is not correct
                        MessageBox.Show(ToolsClass.Definition.INCORRECT_PASSWORD_FORMAT_TEXT);
                        return false;
                    }
                    else if (!SecurityManager.isIdFree(id_textBox.Text, agent.tableId))
                    {
                        MessageBox.Show("L'identifiant est déjà pris");//check if the given agent id is free into the database
                        return false;
                    }

                    if (password != null)
                    {
                        password.Dispose();
                    }
                }
            

            return true;
        }


        private void supp_item_button_Click(object sender, EventArgs e)
        {
            /*Delete the slected agent from the database*/
            if (agent.tableId == -1)//no selected agent
            {
                MessageBox.Show("Veuillez sélectionner un agent avant de pouvoir modifier ses informations");
                return;
            }

            if(agent.tableId == SecurityManager.getConnectedAgentTableIndex())
            {
                MessageBox.Show("Vous ne pouvez pas supprimer votre propre compte");
                return;
            }

            if (SecurityManager.getConnectedAgentRight()!= SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_DEUS))
            {
                //an user cannot delete an account with higer or equal priviledges
                if(SecurityManager.getConnectedAgentRight() == agent.getRight() || !SecurityManager.rightLevelEnoughToAccesSystem(SecurityManager.getConnectedAgentRight(), agent.getRight()))
                {
                    MessageBox.Show("Vous ne disposez pas des droits necessaires pour supprimer un compte de plus haut (ou égal) privilège");
                    return;
                }
            }

            agent.deleteAgent();//delete agent
            if(database.updateAgent(agent)==ToolsClass.Definition.NO_ERROR_INT_VALUE)
            {
                int index = list_ids.LastIndexOf(agent.tableId);
                if (index != -1)
                {
                    //update the listbox
                    listBox.DataSource = new List<string>();

                    list_ids.RemoveAt(index);
                    list_results.RemoveAt(index);

                    listBox.DataSource = list_results;

                    setDefault();
                }

                MessageBox.Show("Les informations de l'agent ne sont désormais plus accessibles mais elles resteront en mémoire jusqu'à la prochaine purge");

            }else//an error occured
            {
                MessageBox.Show("Un problème est survenu : impossible de mettre à jour les informations");
            }

        }
        
        private void password_textBox_Click(object sender, EventArgs e)
        {
            password_textBox.SelectAll();
            
        }


        private void save_change_button_Click(object sender, EventArgs e)
        {
            if (!isDataCorrect())//if all the given data is correct
            {
                return;
            }


            int res;
            //save all the current information into a strcture
            agent.lastName = last_name_textBox.Text;
            agent.firstName = first_name_textBox.Text;
            agent.job = job_textBox.Text;
            agent.mailAdress = mail_adress_textBox.Text;
            agent.setIntRightFromString(right_checkedListBox.CheckedItems[0].ToString());
            agent.id = id_textBox.Text; 

            if (password_textBox.Text.Replace(" ","").Length!=0)//password has been changed
            {
                SecureString password = new SecureString();
                foreach (char c in password_textBox.Text)
                {
                    password.AppendChar(c);
                }
                password.MakeReadOnly();

                //create hash for the password
                string salt = DataProtection.getSaltForHashes();
                agent.hashedPassword = DataProtection.getSaltHashedString(password, salt);
                agent.saltForHashe = salt;
                //hashed the password
                if (password != null)
                {
                    password.Dispose();
                }
            }

            //send mail to the agent about all his confidential data
            string subject;
            string body ="";

            if (agent.tableId==-1)//if user add a new agent we automatically set a new short id for him
            {
                //user cannot create an account with higher priviledges
                if (!SecurityManager.rightLevelEnoughToAccesSystem(SecurityManager.getConnectedAgentRight(), agent.getRight()))
                {
                    MessageBox.Show("Vous ne pouvez pas créer un compte de plus haut privilege que le votre");
                    return;
                }

                agent.shortSecureId = SecurityManager.setAgentShortSecureId();
                res = database.addNewAgent(agent);

                subject = "Création de votre compte";

                body = agent.lastName.ToUpper()+" "+agent.firstName.ToLower()+" ("+agent.job.ToUpper()+") : "
                                    +"<br> L'agent " + SecurityManager.getFormatedSessionInfo() + " vient de créer votre compte"
                                    + "<br><br>Identifiant : " + id_textBox.Text
                                    + "<br>Mot de passe : " + password_textBox.Text
                                    + "<br>Code d'identification : " + DataProtection.ToInsecureString(DataProtection.unprotect(agent.shortSecureId))
                                    + "<br><br>Le code d'identification est unique et ne peut être changé pour des raisons de sécurité.";
                

            }else
            {
                //user cannot edit an account with higher or equal priviledges (except for the maximun privildge right user)
                if (SecurityManager.getConnectedAgentRight() != SecurityManager.RIGHTS_LIST.LastIndexOf(SecurityManager.RIGHT_DEUS))
                {
                    //an user cannot delete an account with higer or equal priviledges
                    if (SecurityManager.getConnectedAgentRight() == agent.getRight() || !SecurityManager.rightLevelEnoughToAccesSystem(SecurityManager.getConnectedAgentRight(), agent.getRight()))
                    {
                        MessageBox.Show("Vous ne disposez pas des droits éditer un compte de plus haut (ou égal) privilège");
                        return;
                    }
                }

                res = database.updateAgent(agent);

                //send mail to the user in order to notify him about the actual changes
                subject = "Modification de votre compte";

                body = agent.lastName.ToUpper() + " " + agent.firstName.ToLower() + " (" + agent.job.ToUpper() + ") : "
                                    + "<br> L'agent " + SecurityManager.getFormatedSessionInfo() + " vient de modifier votre compte"
                                    + "<br><br>Identifiant : " + id_textBox.Text
                                    + "<br>Mot de passe : " + password_textBox.Text
                                    + "<br>Code d'identification : " + DataProtection.ToInsecureString(DataProtection.unprotect(agent.shortSecureId))
                                    + "<br>Le code d'identification est unique et ne peut être changé pour des raisons de sécurité.";

            }


            if (res == ToolsClass.Definition.NO_ERROR_INT_VALUE)//no error
            {
                /*if the user change the mail adress of an agent, we send mail to the old adress "old_mail_adress" to notify him
                 * that we change his mail adress
                 * Then we send mail to the new adress with the data that changed
                */
                if ((old_mail_adress!=null) && (old_mail_adress!="") && (old_mail_adress != agent.mailAdress))
                {
                    string text_body = agent.lastName.ToUpper() + " " + agent.firstName.ToLower() + " (" + agent.job.ToUpper() + ") : "
                                    + "<br> L'agent " + SecurityManager.getFormatedSessionInfo() + " vient de modifier votre compte"
                                    + "<br>Cette adresse mail n'est désormais plus assiocié à votre compte."
                                    + "En conséquence, vous ne receverez plus de notification à cette adresse";
                    ToolsClass.Tools.sendMail(subject, text_body, new List<string>(new string[] { old_mail_adress }));
                }

                //send mail to the current mail adress
                int res_mail = ToolsClass.Tools.sendMail(subject, body, new List<string>(new string[] { agent.mailAdress }));

                string message = "Les modifications ont été enregistrées avec succès";
                if(res_mail!=ToolsClass.Definition.NO_ERROR_INT_VALUE)//fail to send data
                {
                    HtmlMessageBox box = new HtmlMessageBox("Information sur le compte", ref body);
                    box.ShowDialog();

                    message += "\nLe mail n'a pas pu être envoyé";
                }
                else
                {
                    message += "\nLe mail a correctement été envoyé";
                }
                MessageBox.Show(message);

                setListBox();//update listbox
                setDefault();//reset view

            }
            else//error
            {
                MessageBox.Show("Une erreur est survenue !");
            }


        }

        private void selectAgentInTextBox()
        {
            //select the current agent given by the structure Agent into the listbox
            for(int i=0;i< listBox.Items.Count;i++)
            {
                int index = list_results.LastIndexOf(listBox.Items[i].ToString());
                if(index!=-1)
                {
                    if(list_ids[index]==agent.tableId)//agent find into the listbox
                    {
                        listBox.SetSelected(i, true);//select
                    }else
                    {
                        listBox.SetSelected(i, false);//unselect
                    }
                }
            }
        }

        private void end_button_Click(object sender, EventArgs e)
        {
            /*exit*/
            this.Close();
        }



        private void uncheckItemCheckBox()
        {
            /*uncheck all item in the checkedlistbox*/
            foreach (int i in right_checkedListBox.CheckedIndices)
            {
                right_checkedListBox.SetItemCheckState(i, CheckState.Unchecked);
            }

        }


        private void setDefault()
        {
            /*Set default value to fields*/
            last_name_textBox.Text = "";
            first_name_textBox.Text = "";
            job_textBox.Text = "";
            mail_adress_textBox.Text = "";

            uncheckItemCheckBox();

            id_textBox.Text = "";
            password_textBox.Text = "";
            short_id_textBox.Text = "";

            password_textBox.Text = "";
            password_confirmation_textBox.Text = "";
            short_id_textBox.Text = "";

            display_password_state_textBox.Text = "";
            display_password_state_textBox.BackColor = DEFAULT_COLOR;

            agent.toDefault();
        }


        private void new_item_button_Click(object sender, EventArgs e)
        {
            /*Add a new agent*/
            setDefault();
        }

        

        private void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*When user double click on an item we display all the informations about the 
             * student represented by that item
            */
            listBox.ClearSelected();

            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                int i = list_results.IndexOf(listBox.Items[index].ToString());
                //get the index of the desired student in the list of resultas search
                //Then use that index to get the database id of that student with the list of ids
                if (i != -1)
                {
                    agent = database.getAgentByIndex(list_ids[i]);
                    setAgent(agent);
                }
                listBox.SetSelected(index, true);
            }


        }


        private void setAgent(Agent agent)
        {
            /*Set all the fields with the stored informations about the corresponding agent*/
            last_name_textBox.Text = agent.lastName.ToUpper();
            first_name_textBox.Text = agent.firstName.ToLower();
            job_textBox.Text = agent.job;
            mail_adress_textBox.Text = agent.mailAdress;

            setCheckedItemCheckBox();

            id_textBox.Text = agent.id;
            //never show password and short id
            password_textBox.Text = "";
            password_confirmation_textBox.Text = "";
            short_id_textBox.Text = "";

            display_password_state_textBox.Text = "";
            display_password_state_textBox.BackColor = DEFAULT_COLOR;

            old_mail_adress = agent.mailAdress;

        }


        private void setCheckedItemCheckBox()
        {
            /*check the correct right according to the current agent*/
            string rigth_to_text = agent.rightToString();

            for (int x = 0; x <= right_checkedListBox.Items.Count - 1; x++)
            {
                if(right_checkedListBox.Items[x].ToString()  == rigth_to_text)
                {
                    right_checkedListBox.SetItemChecked(x, true);
                    break;
                }else
                {
                    right_checkedListBox.SetItemChecked(x, false);
                }
                
            }
        }


        private void new_short_id_button_Click(object sender, EventArgs e)
        {
            /*give a new short id*/
            agent.shortSecureId = SecurityManager.setAgentShortSecureId();
            short_id_textBox.Text = SHORT_ID_TAG ;
        }

        private void find_id_button_Click(object sender, EventArgs e)
        {
            /*give a new id*/
            agent.lastName = last_name_textBox.Text;
            agent.firstName = first_name_textBox.Text;

            SecurityManager.setAgentId(ref agent);

            id_textBox.Text = agent.id;
        }


        private void right_checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            /* We can only choose one accreditation level 
             * So when user check one item (one accreditation level) we uncheck all the other option
            */
            for (int i = 0; i < right_checkedListBox.Items.Count; ++i)
            {
                if (i != e.Index)
                {
                    right_checkedListBox.SetItemChecked(i, false);
                }
            }
        }


        private void password_textBoxes_TextChanged(object sender, EventArgs e)
        {
            /* When the user change the password
             * The order of the conditions is important
             * Fist id the password don't matches we display a warning
             * Then if the format password is not correct we display another warning
             */
            if (password_textBox.Text != password_confirmation_textBox.Text)
            {//if the two password field don't matches
                display_password_state_textBox.BackColor = CUSTOM_RED;
                display_password_state_textBox.Text = "Les mots de passe ne correspondent pas";
            }else
            {
                SecureString password = new SecureString();
                foreach (char c in password_textBox.Text)
                {
                    password.AppendChar(c);
                }
                password.MakeReadOnly();
                if (SecurityManager.formatPasswordCorrect(password))
                {//if the format of the password is correct and the two password matches (the password and its confirmation)
                    display_password_state_textBox.Text = "Le format du mot de passe est correct";
                    display_password_state_textBox.BackColor = CUSTOM_GREEN;

                }else
                {//if the format of the password is not correct
                    display_password_state_textBox.Text = ToolsClass.Definition.INCORRECT_PASSWORD_FORMAT_TEXT;
                    display_password_state_textBox.BackColor = CUSTOM_RED;
                }

                if (password != null)
                {
                    password.Dispose();
                }
            }
        }


        private void id_textBox_TextChanged(object sender, EventArgs e)
        {
            /*When user change his id*/
            if(!SecurityManager.isIdFree(id_textBox.Text,agent.tableId))
            {//the given id is already taken
                display_password_state_textBox.Text = "L'identifiant est déjà pris";
                display_password_state_textBox.BackColor = CUSTOM_RED;
            }else
            {
                display_password_state_textBox.Text = "L'identifiant est correct";
                display_password_state_textBox.BackColor = CUSTOM_GREEN;
            }
        }
    }
}
