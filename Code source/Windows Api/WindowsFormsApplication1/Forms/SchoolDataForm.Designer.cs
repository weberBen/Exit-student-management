namespace WindowsFormsApplication1.Forms
{
    partial class SchoolDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.start_hour_textBox = new System.Windows.Forms.TextBox();
            this.start_minutes_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lunch_hour_checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.end_hour_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.end_minutes_textBox = new System.Windows.Forms.TextBox();
            this.add_item_button = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.remove_item_button = new System.Windows.Forms.Button();
            this.edit_item_button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.school_name_textBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // start_hour_textBox
            // 
            this.start_hour_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_hour_textBox.Location = new System.Drawing.Point(22, 54);
            this.start_hour_textBox.Name = "start_hour_textBox";
            this.start_hour_textBox.Size = new System.Drawing.Size(51, 24);
            this.start_hour_textBox.TabIndex = 0;
            this.start_hour_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
            // 
            // start_minutes_textBox
            // 
            this.start_minutes_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_minutes_textBox.Location = new System.Drawing.Point(105, 55);
            this.start_minutes_textBox.Name = "start_minutes_textBox";
            this.start_minutes_textBox.Size = new System.Drawing.Size(51, 24);
            this.start_minutes_textBox.TabIndex = 1;
            this.start_minutes_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(79, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = " : ";
            // 
            // lunch_hour_checkBox
            // 
            this.lunch_hour_checkBox.AutoSize = true;
            this.lunch_hour_checkBox.Location = new System.Drawing.Point(6, 171);
            this.lunch_hour_checkBox.Name = "lunch_hour_checkBox";
            this.lunch_hour_checkBox.Size = new System.Drawing.Size(100, 17);
            this.lunch_hour_checkBox.TabIndex = 3;
            this.lunch_hour_checkBox.Text = "Pause déjeuner";
            this.lunch_hour_checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.end_hour_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.end_minutes_textBox);
            this.groupBox1.Controls.Add(this.lunch_hour_checkBox);
            this.groupBox1.Controls.Add(this.start_hour_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.start_minutes_textBox);
            this.groupBox1.Location = new System.Drawing.Point(17, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 194);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paramètres";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(6, 93);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(71, 24);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Fin : ";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(6, 19);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(71, 24);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Début : ";
            // 
            // end_hour_textBox
            // 
            this.end_hour_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.end_hour_textBox.Location = new System.Drawing.Point(22, 123);
            this.end_hour_textBox.Name = "end_hour_textBox";
            this.end_hour_textBox.Size = new System.Drawing.Size(51, 24);
            this.end_hour_textBox.TabIndex = 2;
            this.end_hour_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(79, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = " : ";
            // 
            // end_minutes_textBox
            // 
            this.end_minutes_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.end_minutes_textBox.Location = new System.Drawing.Point(105, 124);
            this.end_minutes_textBox.Name = "end_minutes_textBox";
            this.end_minutes_textBox.Size = new System.Drawing.Size(51, 24);
            this.end_minutes_textBox.TabIndex = 3;
            this.end_minutes_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
            // 
            // add_item_button
            // 
            this.add_item_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add_item_button.Location = new System.Drawing.Point(129, 271);
            this.add_item_button.Name = "add_item_button";
            this.add_item_button.Size = new System.Drawing.Size(108, 33);
            this.add_item_button.TabIndex = 4;
            this.add_item_button.Text = "ajouter";
            this.add_item_button.UseVisualStyleBackColor = true;
            this.add_item_button.Click += new System.EventHandler(this.add_item_button_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(274, 26);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(262, 290);
            this.listBox.TabIndex = 6;
            this.listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDoubleClick);
            // 
            // remove_item_button
            // 
            this.remove_item_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remove_item_button.Location = new System.Drawing.Point(274, 322);
            this.remove_item_button.Name = "remove_item_button";
            this.remove_item_button.Size = new System.Drawing.Size(262, 33);
            this.remove_item_button.TabIndex = 7;
            this.remove_item_button.Text = "Supprimer";
            this.remove_item_button.UseVisualStyleBackColor = true;
            this.remove_item_button.Click += new System.EventHandler(this.remove_item_button_Click);
            // 
            // edit_item_button
            // 
            this.edit_item_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edit_item_button.Location = new System.Drawing.Point(17, 271);
            this.edit_item_button.Name = "edit_item_button";
            this.edit_item_button.Size = new System.Drawing.Size(106, 33);
            this.edit_item_button.TabIndex = 8;
            this.edit_item_button.Text = "modifier";
            this.edit_item_button.UseVisualStyleBackColor = true;
            this.edit_item_button.Click += new System.EventHandler(this.edit_item_button_Click);
            // 
            // save_button
            // 
            this.save_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_button.Location = new System.Drawing.Point(779, 488);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(133, 33);
            this.save_button.TabIndex = 9;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_button.Location = new System.Drawing.Point(640, 488);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(133, 33);
            this.cancel_button.TabIndex = 10;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.add_item_button);
            this.groupBox2.Controls.Add(this.listBox);
            this.groupBox2.Controls.Add(this.edit_item_button);
            this.groupBox2.Controls.Add(this.remove_item_button);
            this.groupBox2.Location = new System.Drawing.Point(15, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 373);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Heures de cours";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkedListBox);
            this.groupBox3.Location = new System.Drawing.Point(599, 97);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(294, 372);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Jours de cours";
            // 
            // checkedListBox
            // 
            this.checkedListBox.CheckOnClick = true;
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Location = new System.Drawing.Point(17, 27);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(245, 289);
            this.checkedListBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.school_name_textBox);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Location = new System.Drawing.Point(14, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(569, 82);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Informations légales";
            // 
            // school_name_textBox
            // 
            this.school_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.school_name_textBox.Location = new System.Drawing.Point(205, 33);
            this.school_name_textBox.Name = "school_name_textBox";
            this.school_name_textBox.Size = new System.Drawing.Size(332, 24);
            this.school_name_textBox.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(18, 33);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(181, 24);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Nom de l\'établissement :";
            // 
            // SchoolDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 533);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.MaximizeBox = false;
            this.Name = "SchoolDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestion des informations légales";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox start_hour_textBox;
        private System.Windows.Forms.TextBox start_minutes_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox lunch_hour_checkBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button add_item_button;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button remove_item_button;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox end_hour_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox end_minutes_textBox;
        private System.Windows.Forms.Button edit_item_button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox school_name_textBox;
        private System.Windows.Forms.TextBox textBox1;
    }
}