using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ToolsClass;
using StudentExitRegimeAuthorization = ToolsClass.Tools.StudentExitRegimeAuthorization;
using StudentExitRegime = ToolsClass.Tools.StudentExitRegime;

namespace WindowsFormsApplication1.Forms
{
    
    public partial class ExitRegime : Form
    {
        class Selection
        {
            public Selection()
            {
                this.function_get = default_function;
                this.function_set = default_function;
                this.selected_item = null;
            }

            private object selected_item;
            private Func<object, object> function_get;
            private Func<object, object> function_set;

            private object default_function(object x)
            {
                return x;
            }


            public object Item
            {

                get
                {
                    return function_get(selected_item);
                }

                set
                {
                    selected_item = function_set(value);
                }

            }

            public Func<object, object> HandlerSetFunction
            {
                set
                {
                    function_set = value;
                }

            }

            public Func<object, object> HandlerGetFunction
            {
                set
                {
                    function_get = value;
                }

            }
        }

        private List<int> list_deleted_authorization_id;
        private List<int> list_deleted_regime_id;
        private DateTimePicker dateTimePicker;
        private object value_before_edit_cell_authorization;
        private object value_before_edit_cell_regime;

        private readonly Color color;

        private Dictionary<string, List<string>> dict_relation_RA;

        private static Selection SelectedRegime;

        public ExitRegime()
        {
            InitializeComponent();
            list_deleted_authorization_id = new List<int>();
            list_deleted_regime_id = new List<int>();
            SelectedRegime = new Selection();
            dict_relation_RA = new Dictionary<string, List<string>>();
            color = selected_regime_textBox.BackColor;


            
            activate_checkBox.CheckedChanged += activate_checkBox_CheckedChanged;
            activate_checkBox.Checked = Settings.ExitRegimeActivationState;
            activationExitRegime();

            //--------------------------------- AUTHORIZATION DATAGRIDVIEW ---------------------------------
            Authorization_dataGridView.UserDeletingRow += Authorization_dataGridView_UserDeletingRow;
            Authorization_dataGridView.CellClick += Authorization_dataGridView_CellClick;
            Authorization_dataGridView.Scroll += Authorization_datagridview_Scroll;
            Authorization_dataGridView.RowLeave += Authorization_dataGridView_RowLeave;
            Authorization_dataGridView.RowsAdded += Authorization_dataGridView_RowsAdded;
            Authorization_dataGridView.CellEndEdit += Authorization_dataGridView_CellEndEdit;
            Authorization_dataGridView.CellBeginEdit += Authorization_dataGridView_CellBeginEdit;


            //set the datetime picker to only show time
            dateTimePicker = new DateTimePicker();
            dateTimePicker.Format = DateTimePickerFormat.Time;
            dateTimePicker.CustomFormat = ToolsClass.Definition.TIMESPAN_FORMAT;
            dateTimePicker.CloseUp += dateTimePicker_CloseUp;
            dateTimePicker.TextChanged += dateTimePicker_OnTextChange;
            dateTimePicker.Visible = false;
            Authorization_dataGridView.Controls.Add(dateTimePicker);

            //--------------------------------- REGIME DATAGRIDVIEW ---------------------------------
            Exit_regime_dataGridView.UserDeletingRow += Exit_regime_dataGridView_UserDeletingRow;
            Exit_regime_dataGridView.CellEndEdit += Exit_regime_dataGridView_CellEndEdit;
            Exit_regime_dataGridView.RowsAdded += Exit_regime_dataGridView_RowsAdded;
            Exit_regime_dataGridView.CellBeginEdit += Exit_regime_dataGridView_CellBeginEdit;
            Exit_regime_dataGridView.CellClick += Exit_regime_dataGridView_CellClick;



            //--------------------------------- RELATION DATAGRIDVIEW ---------------------------------


            //--------------------------------- BINDING DATA ---------------------------------

            DataBase database = new DataBase();
            var res = database.getAllExitRegime();
            if (res == null)
            {
                MessageBox.Show("Impossible d'accèder aux données");
                this.Close();
            }
            setRelations(res.Item2);

            InitializeAuthorizationDataGrid(res.Item1);
            InitializeRegimeDataGrid(res.Item2);

            //save the last selected exit regime by user
            SelectedRegime.HandlerSetFunction = updateSelectionSet;
            SelectedRegime.Item = null;
        }

        private void activationExitRegime()
        {
            if (activate_checkBox.Checked)
            {
                main_groupBox.Enabled = true;
            }
            else
            {
                main_groupBox.Enabled = false;
            }
        }

        private void activate_checkBox_CheckedChanged(Object sender, EventArgs e)
        {
            activationExitRegime();
        }


        /* --------------------------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * 
         *                                                              EXIT REGIME
         * 
         * 
          --------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        private void InitializeRegimeDataGrid(List<StudentExitRegime> list)
        {
            //populate the datagridview with exit regime label
            foreach (StudentExitRegime regime in list)
            {
                int row = Exit_regime_dataGridView.Rows.Add(regime.name);
                Exit_regime_dataGridView.Rows[row].Tag = regime.Id;//to track modification on existing element into the database
            }
        }


        private void Exit_regime_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //check if the current column of the cell is the one for the exit regime label
            int column_index_label = Exit_regime_dataGridView.Columns["label_regime"].Index;
            if (e.ColumnIndex == column_index_label)
            {
                //check that the edited label is unique and if it's not replace the value by the previous one (before modification was made)

                //get value of the current selected regime
                var tmp = Exit_regime_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
                if (tmp == null)//no value then do nothing
                    return;
                string current_label = tmp.ToString();

                //loop through all the exit regime labels
                for (int i = 0; i < Exit_regime_dataGridView.RowCount; i++)
                {
                    if (i == e.RowIndex)
                        continue;

                    tmp = Exit_regime_dataGridView.Rows[i].Cells[column_index_label].Value;
                    if (tmp == null)
                        continue;

                    if (current_label == tmp.ToString())//label has already been defined
                    {
                        MessageBox.Show("Erreur : le nom du régime de sortie est déjà existant");
                        Exit_regime_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value = value_before_edit_cell_regime;
                        return;
                    }
                }

                //modification are correct => update dictionary
                if (value_before_edit_cell_regime == null || current_label != value_before_edit_cell_regime.ToString())//new cell or modification of an existing cell
                {
                    if (value_before_edit_cell_regime == null)//new exit regime
                    {
                        try
                        {
                            dict_relation_RA.Add(current_label, new List<string>());
                        }
                        catch { }//label already exists into the database
                    }
                    else//modification of an existing exit regime
                    {
                        SelectedRegime.Item = current_label;
                        dict_relation_RA.Add(current_label, dict_relation_RA[value_before_edit_cell_regime.ToString()]);
                        dict_relation_RA.Remove(value_before_edit_cell_regime.ToString());
                    }
                }
            }
        }

        private void Exit_regime_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int column_index_label = Exit_regime_dataGridView.Columns["label_regime"].Index;
            if (e.ColumnIndex == column_index_label)
            {
                //for a cell inside the exit regime label column, save the content of that cell to be able to retablish that value after edition if needed
                value_before_edit_cell_regime = Exit_regime_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
            }
        }


        private void Exit_regime_dataGridView_RowsAdded(object sender, System.Windows.Forms.DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            Exit_regime_dataGridView.Rows[e.RowIndex].Tag = -1;
        }


        private void Exit_regime_dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

            DataGridViewRow row = e.Row;
            //check if it's a new item or a one of the database
            if (row.Tag == null)
                return;
            int regime_id = (int)row.Tag;

            if (regime_id != -1)//item into the database
            {
                list_deleted_regime_id.Add(regime_id);
            }

            //update the dictionary
            int column_index_label = Exit_regime_dataGridView.Columns["label_regime"].Index;
            var tmp = e.Row.Cells[column_index_label].Value;//get  label of the deleted exit regime
            if (tmp != null)
            {
                string label = tmp.ToString();
                dict_relation_RA.Remove(label);
            }

            SelectedRegime.Item = null;

            /*
            // Cancel the deletion 
            e.Cancel = true;
            */
        }

        private void Exit_regime_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //when user click on an exit regime label, then we display all the relation between that regime and the authorizations
            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;
            int column_index_relation = Authorization_dataGridView.Columns["relation_authorization"].Index;

            //get label of the selected regime
            SelectedRegime.Item = Exit_regime_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
            if (SelectedRegime.Item == null)//no selected regime
            {
                return;
            }
            string label_regime = (string)SelectedRegime.Item;

            //check if it already exists into the database
            if (!dict_relation_RA.ContainsKey(label_regime))
            {
                dict_relation_RA.Add(label_regime, new List<string>());
            }

            //get all the authorizations associated to that regime
            List<string> authorizations = dict_relation_RA[label_regime];

            //activate checkbox for each authorization associated with the current regime
            for (int row = 0; row < Authorization_dataGridView.RowCount; row++)//loop through all the authorization of the datagrid
            {
                DataGridViewCheckBoxCell cell_relation = (DataGridViewCheckBoxCell)Authorization_dataGridView.Rows[row].Cells[column_index_relation];

                //get label of the authorization
                var tmp = Authorization_dataGridView.Rows[row].Cells[column_index_label].Value;
                if (tmp == null)
                    continue;
                string label_authorization = tmp.ToString();

                if (authorizations.Contains(label_authorization))//the current authorization is associated with the regime
                {
                    cell_relation.Value = cell_relation.TrueValue;

                }
                else
                {
                    cell_relation.Value = cell_relation.FalseValue;
                }

            }
        }

        /* --------------------------------------------------------------------------------------------------------------------------------------------------------------
        * 
        * 
        *                                                              RELATIONS EXIT REGIMES AND PERMISSIONS
        * 
        * 
         --------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        private void setRelations(List<StudentExitRegime> list)
        {
            //all the relation between regime and authorization are temporarily store into a dictionary (regime label, list of associated authorization labels)
            foreach (StudentExitRegime regime in list)
            {
                dict_relation_RA.Add(regime.name, regime.authorizations);
            }
        }

        /* --------------------------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * 
         *                                                              AUTHORIZATION FOR EXIT REGIME
         * 
         * 
          --------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        private void InitializeAuthorizationDataGrid(List<StudentExitRegimeAuthorization> list)
        {
            foreach (StudentExitRegimeAuthorization authorization in list)
            {
                //populate the datagridview with authorization for exit regime
                int row = Authorization_dataGridView.Rows.Add(authorization.name, Tools.timeToStringFromTimeSpan(authorization.period),
                    Tools.timeToStringFromTimeSpan(authorization.start), Tools.timeToStringFromTimeSpan(authorization.end));
                Authorization_dataGridView.Rows[row].Tag = authorization.Id;//to track modification on existing element into the database
            }
        }

        private void Authorization_dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;
            if (e.ColumnIndex == column_index_label)
            { //for a cell inside the authorization label column, save the content of that cell to be able to retablish that value after edition if needed
                value_before_edit_cell_authorization = Authorization_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
            }
        }

        private void Authorization_dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;

            //check if the current column of the cell is the one for the authorization label
            if (e.ColumnIndex == column_index_label)
            {
                ////check that the edited label is unique and if it's not replace the value by the previous one (before modification was made)

                //get value of the current selected authorization
                var tmp = Authorization_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
                if (tmp == null)
                    return;
                string current_label = tmp.ToString();
                string new_label = "";

                //loop through all the authorization labels
                for (int i = 0; i < Authorization_dataGridView.RowCount; i++)
                {
                    if (i == e.RowIndex)
                        continue;

                    //label of the current authorization
                    tmp = Authorization_dataGridView.Rows[i].Cells[column_index_label].Value;
                    if (tmp == null)
                        continue;

                    new_label = tmp.ToString();
                    if (current_label == new_label)//label has already been defined
                    {
                        MessageBox.Show("Erreur : le nom de la permission est déjà existant");
                        Authorization_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value = value_before_edit_cell_authorization;
                        return;
                    }

                }
                //if(value_before_edit_cell_authorization==null) ==> new authorization, then do nothing

                //if the label of an existing authorization has been edited, then update the label for each relation regime-authorization
                if (value_before_edit_cell_authorization != null && current_label != value_before_edit_cell_authorization.ToString())
                {
                    foreach (KeyValuePair<string, List<string>> kvp in dict_relation_RA)//loop through all regime label
                    {
                        //remove (if exists) the old label of that authorization
                        bool res = kvp.Value.Remove(value_before_edit_cell_authorization.ToString());
                        if (res)//element was inside the list, then update the value be adding it
                            kvp.Value.Add(current_label);
                    }
                }
            }
        }


        private void Authorization_dataGridView_RowsAdded(object sender, System.Windows.Forms.DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            Authorization_dataGridView.Rows[e.RowIndex].Tag = -1;
        }


        private void Authorization_dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

            DataGridViewRow row = e.Row;

            //check if the deleted authorization was previously into the database
            if (row.Tag != null)
            {
                int authorization_id = (int)row.Tag;

                if (authorization_id != -1)//exists into the database
                {
                    list_deleted_authorization_id.Add(authorization_id);
                }
            }

            //update the dictionary be remove occurences of that authorization for each regime
            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;
            var tmp = e.Row.Cells[column_index_label].Value;//get label of the authorization
            if (tmp != null)
            {
                string label = tmp.ToString();

                foreach (KeyValuePair<string, List<string>> kvp in dict_relation_RA)//loop through regime
                {
                    kvp.Value.Remove(label);//remove (if exists) the authorization
                }
            }

            /*
            // Cancel the deletion 
            e.Cancel = true;
            */
        }

        private Tuple<bool, string> authorizationChecker(int row_index, bool show, bool extract_authoriation, ref StudentExitRegimeAuthorization authorization)
        {
            if (row_index < 0)
                return Tuple.Create(false, "Cellules non prise en compte");

            //check if label of the current authorization is not null
            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;

            var tmp = Authorization_dataGridView.Rows[row_index].Cells[column_index_label].Value;
            if (tmp == null)//label is null
            {
                return Tuple.Create(false, "Cellule ligne " + (row_index + 1) + " et colonne " + (column_index_label + 1) + " de la table des permissions vide");
            }
            string label_regime = tmp.ToString();


            //check if start>end and period < end-start after user leave the current row
            TimeSpan[] list_timespan = new TimeSpan[3];
            string[] list_col_name = new string[] { "period_authorization", "start_authorization", "end_authorization" };

            int column_index;
            string column_name;
            DataGridViewColumn column;

            //get all the timespan
            for (int i = 0; i < list_col_name.Length; i++)
            {
                string col_name = list_col_name[i];

                column = Authorization_dataGridView.Columns[col_name];
                column_index = column.Index;
                column_name = column.HeaderText;
                var temp = Authorization_dataGridView.Rows[row_index].Cells[column_index].Value;
                if (temp == null)
                {
                    string msg = "La cellule de la ligne " + (row_index + 1) + " et de la colonne <<" + column_name + ">> de la table des permissions est vide";
                    if (show)
                        MessageBox.Show(msg);

                    return Tuple.Create(false, msg);
                }
                string value = temp.ToString();

                list_timespan[i] = Tools.stringToTimeSpan(value);
            }

            TimeSpan period = list_timespan[0];
            TimeSpan start = list_timespan[1];
            TimeSpan end = list_timespan[2];


            //check the timespan value
            if (end <= start)
            {
                string msg = "La fin de la plage horraire ne peut pas être supérieur ou égale au début de la plage horraire (ligne " + (row_index + 1) + " dans la table des permissions)";
                if (show)
                    MessageBox.Show(msg);
                return Tuple.Create(false, msg);
            }
            else if ((end - start) <= period)
            {
                string msg = "La période fournie ne peut pas être plus grande que la plage horraire (ligne " + (row_index + 1) + " dans la table des permissions)";
                if (show)
                    MessageBox.Show(msg);
                return Tuple.Create(false, msg);
            }


            if (extract_authoriation)//return an authorization struct with the collected data about the current authorization
            {
                authorization.toDefault();

                authorization.name = label_regime;
                authorization.period = period;
                authorization.start = start;
                authorization.end = end;
            }

            return Tuple.Create(true, "");
        }

        private void Authorization_dataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //after user leave a row, check if all the parameters are correct

            //check for authorization label
            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;
            int column_index_relation = Authorization_dataGridView.Columns["relation_authorization"].Index;

            var tmp = Authorization_dataGridView.Rows[e.RowIndex].Cells[column_index_label].Value;
            if (tmp == null)
            {
                //if user activates the checkbox of an empty authorization, disactive it
                DataGridViewCheckBoxCell cell_relation = (DataGridViewCheckBoxCell)Authorization_dataGridView.Rows[e.RowIndex].Cells[column_index_relation];
                cell_relation.Value = cell_relation.FalseValue;

                return;
            }

            authorizationChecker(e.RowIndex, true, false, ref StudentExitRegimeAuthorization.Zero);//check all other parms

            dateTimePicker.Visible = false;
        }

        private void Authorization_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //display a timepicker onto the cell to allow users to choose a time (which will be write back into the cell as a string with the correct format)
            if (e.ColumnIndex == Authorization_dataGridView.Columns["period_authorization"].Index || e.ColumnIndex == Authorization_dataGridView.Columns["start_authorization"].Index
                || e.ColumnIndex == Authorization_dataGridView.Columns["end_authorization"].Index)
            {
                //set the timepicker value to the time written into the current cell
                var temp = Authorization_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                string cell_value;
                if (temp == null)
                    cell_value = null;
                else
                    cell_value = temp.ToString();

                //convert string to datetime
                DateTime date_time;
                if (cell_value != null)
                {
                    TimeSpan time = Tools.stringToTimeSpan(cell_value);
                    date_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
                }
                else
                {
                    date_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }
                dateTimePicker.Value = date_time;

                //display timepicker
                Rectangle Rectangle = Authorization_dataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dateTimePicker.Size = new Size(Rectangle.Width, Rectangle.Height);
                dateTimePicker.Location = new Point(Rectangle.X, Rectangle.Y);

                //show timepicker
                dateTimePicker.Visible = true;
            }
        }

        private void dateTimePicker_OnTextChange(object sender, EventArgs e)
        {
            //write back data from the timepicker to the cell of the datagridview
            TimeSpan time = Tools.TimeFromDateTimeToTimeSpan(dateTimePicker.Value);
            Authorization_dataGridView.CurrentCell.Value = Tools.timeToStringFromTimeSpan(time);
        }

        void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            //hide timepicker when user finish his edits
            dateTimePicker.Visible = false;
        }

        private void Authorization_datagridview_Scroll(object sender, ScrollEventArgs e)
        {
            dateTimePicker.Visible = false;
        }


        /* --------------------------------------------------------------------------------------------------------------------------------------------------------------
         * 
         * 
         *                                                              OPERATIONS
         * 
         * 
          --------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private object updateSelectionSet(object value_selection)
        {
            //function call each time the current selected exit regime is set
            Object output = null;

            if (value_selection != null)
            {
                output = value_selection.ToString();

                Authorization_dataGridView.Columns["relation_authorization"].Visible = true;
                selected_regime_textBox.Text = "Selection : <<" + (string)output+">>";

                selected_regime_textBox.BackColor = color;

            }
            else//no regime selected
            {
                Authorization_dataGridView.Columns["relation_authorization"].Visible = false;
                selected_regime_textBox.Text = "Aucune sélection";

                selected_regime_textBox.BackColor = Color.DarkGray;
            }

            return output;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //user wants to save the relation between an exit regime and its authorizations

            disactivateControl();//disable all the content during the process

            //check if the selected regime exists
            int column_index_label = Authorization_dataGridView.Columns["label_authorization"].Index;
            int column_index_relation = Authorization_dataGridView.Columns["relation_authorization"].Index;

            if (SelectedRegime.Item == null)//no regime selected
            {
                MessageBox.Show("Erreur : le nom du régime actuel est nul");
                activateControl();//active the previoulsy disabled content
                return;
            }
            string label_regime = (string)SelectedRegime.Item;

            //add the regime if it has not been done before
            if (!dict_relation_RA.ContainsKey(label_regime))
            {
                dict_relation_RA.Add(label_regime, new List<string>());
            }


            //collect all the authorizations associated to that regime
            List<string> associated_authorizations = new List<string>();

            for (int row = 0; row < Authorization_dataGridView.RowCount; row++)//loop through authorizations
            {
                DataGridViewCheckBoxCell cell_relation = (DataGridViewCheckBoxCell)Authorization_dataGridView.Rows[row].Cells[column_index_relation];
                var tmp = Authorization_dataGridView.Rows[row].Cells[column_index_label].Value;
                if (tmp == null)//undifined authorization label
                {
                    cell_relation.Value = cell_relation.FalseValue;
                    activateControl();

                    continue;
                }
                string authorization_label = tmp.ToString();

                //user associated that authorization with the current regime
                if (cell_relation.Value == cell_relation.TrueValue)//cell has been previously checked
                    associated_authorizations.Add(authorization_label);
            }

            //update dictionary
            dict_relation_RA[label_regime] = associated_authorizations;


            activateControl();//active the previoulsy disabled content
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //users wants to definitively save the changes
            disactivateControl();//disable all the content during the process

            if(activate_checkBox.Checked)
            {
                Settings.ExitRegimeActivationState = true;
            }else
            {
                Settings.ExitRegimeActivationState = false;
                this.Close();
                return;
            }

            StudentExitRegime regime;
            StudentExitRegimeAuthorization authorization;
            bool newItem;
            string msg_a = "";
            string msg_r = "";
            string msg = "";

            //---------------- update authorizations into the database ----------------

            List<StudentExitRegimeAuthorization> temp_list_authorization = new List<StudentExitRegimeAuthorization>();

            for (int row = 0; row < Authorization_dataGridView.RowCount; row++)//loop through authorizations of the datagrid
            {
                //check if the authorization is into the database
                object tag = Authorization_dataGridView.Rows[row].Tag;
                if (tag == null || (int)tag == -1)
                    newItem = true;//not previously into the database
                else//exists into the database
                    newItem = false;

                authorization = new StudentExitRegimeAuthorization();
                authorization.toDefault();

                //get the updated  or newauthorization
                var res = authorizationChecker(row, false, true, ref authorization);
                if (!(bool)res.Item1)
                {//the parameters of the authorization are not correct
                    if (row != Authorization_dataGridView.RowCount - 1)//datagrid automatically add a blank row to allow user to add new rows
                        msg_a += res.Item2 + "\n";
                    continue;

                }

                if (!newItem)//authorization already exists in the list
                {
                    authorization.Id = (int)tag;
                }

                temp_list_authorization.Add(authorization);
            }

            //---------------- update exit regime into the database ----------------

            List<StudentExitRegime> temp_list_regime = new List<StudentExitRegime>();

            int id;
            for (int row = 0; row < Exit_regime_dataGridView.RowCount; row++)//loop through exit regime of the datagrid
            {
                //check if the authorization is into the database
                object tag = Exit_regime_dataGridView.Rows[row].Tag;
                if (tag == null || (int)tag == -1)
                    id = -1;//not previously into the database
                else
                    id = (int)tag;//exists into the database

                //check if label of the current regime is correct
                int column_index_label = Exit_regime_dataGridView.Columns["label_regime"].Index;
                var tmp = Exit_regime_dataGridView.Rows[row].Cells[column_index_label].Value;
                if (tmp == null)//parameters are not correct
                {
                    if (row != Exit_regime_dataGridView.RowCount - 1)//datagrid automatically add a blank row to allow user to add new rows
                        msg_r += "La cellule de la ligne " + (row + 1) + " colonne " + (column_index_label + 1) + " dans la table des régimes de sortie est vide\n";
                    continue;
                }

                string label_regime = tmp.ToString();

                //check if it already exists into the database
                if (!dict_relation_RA.ContainsKey(label_regime))
                {
                    dict_relation_RA.Add(label_regime, new List<string>());//new regime
                }

                //update or create new regime
                regime = new StudentExitRegime();
                regime.toDefault();

                regime.Id = id;
                regime.name = label_regime;
                regime.authorizations = dict_relation_RA[label_regime];


                temp_list_regime.Add(regime);

            }

            //---------------- solving incorrect format problems ----------------
            if (msg_a.Length > 0)
            {
                msg += "--------------- PERMISSIONS ---------------" + "\n\n" + msg_a + "\n";
            }
            if (msg_r.Length > 0)
            {
                msg += "--------------- REGIMES DE SORTIE ---------------" + "\n\n" + msg_r;
            }
            if (msg.Length > 0)//error
            {
                //ask user if he wants to ignore problemes
                if (MessageBox.Show("Certaines modifications ne seront pas prises en compte pour les raisons suivantes :\n" + msg + "\n\nVoulez vous continuer quand même ?", "Valider les changements", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    activateControl();//active the previoulsy disabled content
                    return;
                }
            }


            //---------------- update database ----------------

            DataBase database = new DataBase();
            int errCode;

            //remove deleted authorization (that previously were into the database)
            foreach (int table_id in list_deleted_authorization_id)
            {
                errCode = database.removeAuthorizationExitRegime(table_id);
                if (errCode != Definition.NO_ERROR_INT_VALUE)
                {
                    MessageBox.Show("Une erreur est survenue : impossible de mettre à jour la base de données");
                    activateControl();//active the previoulsy disabled content
                    this.Close();
                }
            }
            //update (or add) the others
            errCode = database.updateAuthorizationExitRegime(temp_list_authorization);
            if(errCode!=Definition.NO_ERROR_INT_VALUE)
            {
                MessageBox.Show("Une erreur est survenue : impossible de mettre à jour la base de données");
                activateControl();//active the previoulsy disabled content
                this.Close();
            }


            //remove deleted regime (that previously were into the database)
            foreach (int table_id in list_deleted_regime_id)
            {
                errCode = database.removeExitRegime(table_id);
                if (errCode != Definition.NO_ERROR_INT_VALUE)
                {
                    MessageBox.Show("Une erreur est survenue : impossible de mettre à jour la base de données");
                    activateControl();//active the previoulsy disabled content
                    this.Close();
                }
            }
            //update (or add) the others
            errCode = database.updateExitRegime(temp_list_regime);
            if (errCode != Definition.NO_ERROR_INT_VALUE)
            {
                MessageBox.Show("Une erreur est survenue : impossible de mettre à jour la base de données");
                activateControl();//active the previoulsy disabled content
                this.Close();
            }


            activateControl();//active the previoulsy disabled content
            this.Close();//close windows
        }

        private void activateControl()
        {
            bool value = true;
            Authorization_dataGridView.Enabled = value;
            Exit_regime_dataGridView.Enabled = value;
            save_relations__button.Enabled = value;
        }

        private void disactivateControl()
        {
            bool value = false;
            Authorization_dataGridView.Enabled = value;
            Exit_regime_dataGridView.Enabled = value;
            save_relations__button.Enabled = value;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
