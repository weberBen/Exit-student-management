namespace WindowsFormsApplication1.Forms
{
    partial class BrowseStudentData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseStudentData));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.sex_textBox = new System.Windows.Forms.TextBox();
            this.division_textBox = new System.Windows.Forms.TextBox();
            this.first_name_textBox = new System.Windows.Forms.TextBox();
            this.last_name_textBox = new System.Windows.Forms.TextBox();
            this.change_photo_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sunday_textBox = new System.Windows.Forms.TextBox();
            this.delete_student_button = new System.Windows.Forms.Button();
            this.saturday_textBox = new System.Windows.Forms.TextBox();
            this.friday_textBox = new System.Windows.Forms.TextBox();
            this.thursday_textBox = new System.Windows.Forms.TextBox();
            this.wednesday_textBox = new System.Windows.Forms.TextBox();
            this.tuesday_textBox = new System.Windows.Forms.TextBox();
            this.monday_textBox = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.cancel_student_data_button = new System.Windows.Forms.Button();
            this.save_student_data_button = new System.Windows.Forms.Button();
            this.rfid_id_textBox = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.search_textBox = new System.Windows.Forms.TextBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fuson_button = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.end_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(21, 19);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(130, 170);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(171, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(82, 26);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "Nom :";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(171, 61);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(82, 26);
            this.textBox2.TabIndex = 22;
            this.textBox2.Text = "Prénom :";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(171, 102);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(82, 26);
            this.textBox3.TabIndex = 23;
            this.textBox3.Text = "Classe :";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(171, 144);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(82, 26);
            this.textBox4.TabIndex = 24;
            this.textBox4.Text = "Sexe :";
            // 
            // sex_textBox
            // 
            this.sex_textBox.Enabled = false;
            this.sex_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sex_textBox.Location = new System.Drawing.Point(274, 144);
            this.sex_textBox.Name = "sex_textBox";
            this.sex_textBox.Size = new System.Drawing.Size(295, 23);
            this.sex_textBox.TabIndex = 8;
            this.sex_textBox.Click += new System.EventHandler(this.textboxes_Click);
            // 
            // division_textBox
            // 
            this.division_textBox.Enabled = false;
            this.division_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.division_textBox.Location = new System.Drawing.Point(274, 102);
            this.division_textBox.Name = "division_textBox";
            this.division_textBox.Size = new System.Drawing.Size(295, 23);
            this.division_textBox.TabIndex = 7;
            this.division_textBox.Click += new System.EventHandler(this.textboxes_Click);
            // 
            // first_name_textBox
            // 
            this.first_name_textBox.Enabled = false;
            this.first_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.first_name_textBox.Location = new System.Drawing.Point(274, 61);
            this.first_name_textBox.Name = "first_name_textBox";
            this.first_name_textBox.Size = new System.Drawing.Size(295, 23);
            this.first_name_textBox.TabIndex = 6;
            this.first_name_textBox.Click += new System.EventHandler(this.textboxes_Click);
            // 
            // last_name_textBox
            // 
            this.last_name_textBox.Enabled = false;
            this.last_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.last_name_textBox.Location = new System.Drawing.Point(274, 19);
            this.last_name_textBox.Name = "last_name_textBox";
            this.last_name_textBox.Size = new System.Drawing.Size(295, 23);
            this.last_name_textBox.TabIndex = 5;
            this.last_name_textBox.Click += new System.EventHandler(this.textboxes_Click);
            // 
            // change_photo_button
            // 
            this.change_photo_button.Location = new System.Drawing.Point(21, 198);
            this.change_photo_button.Name = "change_photo_button";
            this.change_photo_button.Size = new System.Drawing.Size(130, 29);
            this.change_photo_button.TabIndex = 9;
            this.change_photo_button.Text = "Modifier la photo";
            this.change_photo_button.UseVisualStyleBackColor = true;
            this.change_photo_button.Click += new System.EventHandler(this.change_photo_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sunday_textBox);
            this.groupBox1.Controls.Add(this.delete_student_button);
            this.groupBox1.Controls.Add(this.saturday_textBox);
            this.groupBox1.Controls.Add(this.friday_textBox);
            this.groupBox1.Controls.Add(this.thursday_textBox);
            this.groupBox1.Controls.Add(this.wednesday_textBox);
            this.groupBox1.Controls.Add(this.tuesday_textBox);
            this.groupBox1.Controls.Add(this.monday_textBox);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.cancel_student_data_button);
            this.groupBox1.Controls.Add(this.save_student_data_button);
            this.groupBox1.Controls.Add(this.rfid_id_textBox);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.pictureBox);
            this.groupBox1.Controls.Add(this.change_photo_button);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.sex_textBox);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.division_textBox);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.first_name_textBox);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.last_name_textBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 337);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elève";
            // 
            // sunday_textBox
            // 
            this.sunday_textBox.Enabled = false;
            this.sunday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sunday_textBox.Location = new System.Drawing.Point(434, 212);
            this.sunday_textBox.Name = "sunday_textBox";
            this.sunday_textBox.Size = new System.Drawing.Size(74, 23);
            this.sunday_textBox.TabIndex = 28;
            this.sunday_textBox.Text = "Dimanche";
            // 
            // delete_student_button
            // 
            this.delete_student_button.BackColor = System.Drawing.Color.LightCoral;
            this.delete_student_button.Location = new System.Drawing.Point(21, 295);
            this.delete_student_button.Name = "delete_student_button";
            this.delete_student_button.Size = new System.Drawing.Size(118, 36);
            this.delete_student_button.TabIndex = 27;
            this.delete_student_button.Text = "Supprimer l\'élève";
            this.delete_student_button.UseVisualStyleBackColor = false;
            this.delete_student_button.Click += new System.EventHandler(this.delete_student_button_Click);
            // 
            // saturday_textBox
            // 
            this.saturday_textBox.Enabled = false;
            this.saturday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saturday_textBox.Location = new System.Drawing.Point(354, 212);
            this.saturday_textBox.Name = "saturday_textBox";
            this.saturday_textBox.Size = new System.Drawing.Size(74, 23);
            this.saturday_textBox.TabIndex = 21;
            this.saturday_textBox.Text = "Samedi";
            // 
            // friday_textBox
            // 
            this.friday_textBox.Enabled = false;
            this.friday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friday_textBox.Location = new System.Drawing.Point(274, 212);
            this.friday_textBox.Name = "friday_textBox";
            this.friday_textBox.Size = new System.Drawing.Size(74, 23);
            this.friday_textBox.TabIndex = 20;
            this.friday_textBox.Text = "Vendredi";
            // 
            // thursday_textBox
            // 
            this.thursday_textBox.Enabled = false;
            this.thursday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thursday_textBox.Location = new System.Drawing.Point(514, 183);
            this.thursday_textBox.Name = "thursday_textBox";
            this.thursday_textBox.Size = new System.Drawing.Size(74, 23);
            this.thursday_textBox.TabIndex = 19;
            this.thursday_textBox.Text = "Jeudi";
            // 
            // wednesday_textBox
            // 
            this.wednesday_textBox.Enabled = false;
            this.wednesday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wednesday_textBox.Location = new System.Drawing.Point(434, 183);
            this.wednesday_textBox.Name = "wednesday_textBox";
            this.wednesday_textBox.Size = new System.Drawing.Size(74, 23);
            this.wednesday_textBox.TabIndex = 16;
            this.wednesday_textBox.Text = "Mercredi";
            // 
            // tuesday_textBox
            // 
            this.tuesday_textBox.Enabled = false;
            this.tuesday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tuesday_textBox.Location = new System.Drawing.Point(354, 183);
            this.tuesday_textBox.Name = "tuesday_textBox";
            this.tuesday_textBox.Size = new System.Drawing.Size(74, 23);
            this.tuesday_textBox.TabIndex = 18;
            this.tuesday_textBox.Text = "Mardi";
            // 
            // monday_textBox
            // 
            this.monday_textBox.Enabled = false;
            this.monday_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monday_textBox.Location = new System.Drawing.Point(274, 183);
            this.monday_textBox.Name = "monday_textBox";
            this.monday_textBox.Size = new System.Drawing.Size(74, 23);
            this.monday_textBox.TabIndex = 17;
            this.monday_textBox.Text = "Lundi";
            // 
            // textBox8
            // 
            this.textBox8.Enabled = false;
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.Location = new System.Drawing.Point(171, 183);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(82, 26);
            this.textBox8.TabIndex = 14;
            this.textBox8.Text = "Régime :";
            // 
            // cancel_student_data_button
            // 
            this.cancel_student_data_button.Location = new System.Drawing.Point(340, 295);
            this.cancel_student_data_button.Name = "cancel_student_data_button";
            this.cancel_student_data_button.Size = new System.Drawing.Size(118, 36);
            this.cancel_student_data_button.TabIndex = 13;
            this.cancel_student_data_button.Text = "Annuler";
            this.cancel_student_data_button.UseVisualStyleBackColor = true;
            this.cancel_student_data_button.Click += new System.EventHandler(this.cancel_student_data_button_Click);
            // 
            // save_student_data_button
            // 
            this.save_student_data_button.Location = new System.Drawing.Point(487, 295);
            this.save_student_data_button.Name = "save_student_data_button";
            this.save_student_data_button.Size = new System.Drawing.Size(118, 36);
            this.save_student_data_button.TabIndex = 12;
            this.save_student_data_button.Text = "Valider";
            this.save_student_data_button.UseVisualStyleBackColor = true;
            this.save_student_data_button.Click += new System.EventHandler(this.save_student_data_button_Click);
            // 
            // rfid_id_textBox
            // 
            this.rfid_id_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rfid_id_textBox.Location = new System.Drawing.Point(274, 253);
            this.rfid_id_textBox.Name = "rfid_id_textBox";
            this.rfid_id_textBox.Size = new System.Drawing.Size(295, 23);
            this.rfid_id_textBox.TabIndex = 1;
            this.rfid_id_textBox.Click += new System.EventHandler(this.textboxes_Click);
            this.rfid_id_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxes_RFID_input_KeyDown);
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.Location = new System.Drawing.Point(171, 253);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(82, 26);
            this.textBox5.TabIndex = 10;
            this.textBox5.Text = "Code RFID :";
            // 
            // search_textBox
            // 
            this.search_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.search_textBox.Location = new System.Drawing.Point(252, 35);
            this.search_textBox.Name = "search_textBox";
            this.search_textBox.Size = new System.Drawing.Size(353, 23);
            this.search_textBox.TabIndex = 0;
            this.search_textBox.Click += new System.EventHandler(this.textboxes_Click);
            this.search_textBox.TextChanged += new System.EventHandler(this.search_textBox_TextChanged);
            this.search_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxes_RFID_input_KeyDown);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(252, 74);
            this.listBox.Name = "listBox";
            this.listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox.Size = new System.Drawing.Size(353, 212);
            this.listBox.TabIndex = 13;
            this.listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fuson_button);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.search_textBox);
            this.groupBox2.Controls.Add(this.listBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 355);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(619, 300);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recherche";
            // 
            // fuson_button
            // 
            this.fuson_button.Location = new System.Drawing.Point(6, 250);
            this.fuson_button.Name = "fuson_button";
            this.fuson_button.Size = new System.Drawing.Size(118, 36);
            this.fuson_button.TabIndex = 16;
            this.fuson_button.Text = "Fusionner les fiches";
            this.fuson_button.UseVisualStyleBackColor = true;
            this.fuson_button.Click += new System.EventHandler(this.fusion_button_Click);
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(6, 35);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(209, 176);
            this.textBox7.TabIndex = 14;
            this.textBox7.Text = resources.GetString("textBox7.Text");
            // 
            // end_button
            // 
            this.end_button.Location = new System.Drawing.Point(577, 673);
            this.end_button.Name = "end_button";
            this.end_button.Size = new System.Drawing.Size(118, 36);
            this.end_button.TabIndex = 15;
            this.end_button.Text = "Fin";
            this.end_button.UseVisualStyleBackColor = true;
            this.end_button.Click += new System.EventHandler(this.end_button_Click);
            // 
            // BrowseStudentData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 721);
            this.Controls.Add(this.end_button);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BrowseStudentData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestion des données étudiantes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox sex_textBox;
        private System.Windows.Forms.TextBox division_textBox;
        private System.Windows.Forms.TextBox first_name_textBox;
        private System.Windows.Forms.TextBox last_name_textBox;
        private System.Windows.Forms.Button change_photo_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button save_student_data_button;
        private System.Windows.Forms.TextBox rfid_id_textBox;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox search_textBox;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button end_button;
        private System.Windows.Forms.Button cancel_student_data_button;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox saturday_textBox;
        private System.Windows.Forms.TextBox friday_textBox;
        private System.Windows.Forms.TextBox thursday_textBox;
        private System.Windows.Forms.TextBox wednesday_textBox;
        private System.Windows.Forms.TextBox tuesday_textBox;
        private System.Windows.Forms.TextBox monday_textBox;
        private System.Windows.Forms.Button delete_student_button;
        private System.Windows.Forms.Button fuson_button;
        private System.Windows.Forms.TextBox sunday_textBox;
    }
}