namespace WindowsFormsApplication1.Forms
{
    partial class AccessManagementForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.new_item_button = new System.Windows.Forms.Button();
            this.supp_item_button = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox_short_id = new System.Windows.Forms.GroupBox();
            this.new_short_id_button = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.short_id_textBox = new System.Windows.Forms.TextBox();
            this.groupBox_password_and_id = new System.Windows.Forms.GroupBox();
            this.display_password_state_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.password_confirmation_textBox = new System.Windows.Forms.TextBox();
            this.find_id_button = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.id_textBox = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.mail_adress_textBox = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.right_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.job_textBox = new System.Windows.Forms.TextBox();
            this.first_name_textBox = new System.Windows.Forms.TextBox();
            this.last_name_textBox = new System.Windows.Forms.TextBox();
            this.save_change_button = new System.Windows.Forms.Button();
            this.end_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox_short_id.SuspendLayout();
            this.groupBox_password_and_id.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(18, 70);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 26);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "*Nom agent :";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(18, 30);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 26);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "*Prénom agent :";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(18, 115);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(145, 26);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "*Travail :";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(18, 209);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(145, 26);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "*Droit d\'administration :";
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(6, 30);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(292, 381);
            this.listBox.TabIndex = 4;
            this.listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.new_item_button);
            this.groupBox1.Controls.Add(this.listBox);
            this.groupBox1.Location = new System.Drawing.Point(23, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 469);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agent enregistré";
            // 
            // new_item_button
            // 
            this.new_item_button.Location = new System.Drawing.Point(197, 432);
            this.new_item_button.Name = "new_item_button";
            this.new_item_button.Size = new System.Drawing.Size(101, 31);
            this.new_item_button.TabIndex = 17;
            this.new_item_button.Text = "Ajouter";
            this.new_item_button.UseVisualStyleBackColor = true;
            this.new_item_button.Click += new System.EventHandler(this.new_item_button_Click);
            // 
            // supp_item_button
            // 
            this.supp_item_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.supp_item_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.supp_item_button.Location = new System.Drawing.Point(18, 432);
            this.supp_item_button.Name = "supp_item_button";
            this.supp_item_button.Size = new System.Drawing.Size(101, 31);
            this.supp_item_button.TabIndex = 7;
            this.supp_item_button.Text = "Supprimer";
            this.supp_item_button.UseVisualStyleBackColor = false;
            this.supp_item_button.Click += new System.EventHandler(this.supp_item_button_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox_short_id);
            this.groupBox3.Controls.Add(this.groupBox_password_and_id);
            this.groupBox3.Controls.Add(this.mail_adress_textBox);
            this.groupBox3.Controls.Add(this.textBox7);
            this.groupBox3.Controls.Add(this.right_checkedListBox);
            this.groupBox3.Controls.Add(this.supp_item_button);
            this.groupBox3.Controls.Add(this.job_textBox);
            this.groupBox3.Controls.Add(this.first_name_textBox);
            this.groupBox3.Controls.Add(this.last_name_textBox);
            this.groupBox3.Controls.Add(this.save_change_button);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Location = new System.Drawing.Point(366, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(761, 469);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fiche agent";
            // 
            // groupBox_short_id
            // 
            this.groupBox_short_id.Controls.Add(this.new_short_id_button);
            this.groupBox_short_id.Controls.Add(this.textBox9);
            this.groupBox_short_id.Controls.Add(this.short_id_textBox);
            this.groupBox_short_id.Location = new System.Drawing.Point(415, 19);
            this.groupBox_short_id.Name = "groupBox_short_id";
            this.groupBox_short_id.Size = new System.Drawing.Size(340, 90);
            this.groupBox_short_id.TabIndex = 23;
            this.groupBox_short_id.TabStop = false;
            this.groupBox_short_id.Text = "Authentification accès limité";
            // 
            // new_short_id_button
            // 
            this.new_short_id_button.Location = new System.Drawing.Point(253, 51);
            this.new_short_id_button.Name = "new_short_id_button";
            this.new_short_id_button.Size = new System.Drawing.Size(81, 23);
            this.new_short_id_button.TabIndex = 22;
            this.new_short_id_button.Text = "Nouveau";
            this.new_short_id_button.UseVisualStyleBackColor = true;
            this.new_short_id_button.Click += new System.EventHandler(this.new_short_id_button_Click);
            // 
            // textBox9
            // 
            this.textBox9.Enabled = false;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(6, 19);
            this.textBox9.Multiline = true;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(145, 26);
            this.textBox9.TabIndex = 20;
            this.textBox9.Text = "Code d\'identification :";
            this.textBox9.UseSystemPasswordChar = true;
            // 
            // short_id_textBox
            // 
            this.short_id_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.short_id_textBox.Location = new System.Drawing.Point(16, 51);
            this.short_id_textBox.Name = "short_id_textBox";
            this.short_id_textBox.ReadOnly = true;
            this.short_id_textBox.Size = new System.Drawing.Size(231, 23);
            this.short_id_textBox.TabIndex = 21;
            this.short_id_textBox.UseSystemPasswordChar = true;
            // 
            // groupBox_password_and_id
            // 
            this.groupBox_password_and_id.Controls.Add(this.display_password_state_textBox);
            this.groupBox_password_and_id.Controls.Add(this.label1);
            this.groupBox_password_and_id.Controls.Add(this.password_confirmation_textBox);
            this.groupBox_password_and_id.Controls.Add(this.find_id_button);
            this.groupBox_password_and_id.Controls.Add(this.textBox5);
            this.groupBox_password_and_id.Controls.Add(this.password_textBox);
            this.groupBox_password_and_id.Controls.Add(this.id_textBox);
            this.groupBox_password_and_id.Controls.Add(this.textBox6);
            this.groupBox_password_and_id.Location = new System.Drawing.Point(415, 115);
            this.groupBox_password_and_id.Name = "groupBox_password_and_id";
            this.groupBox_password_and_id.Size = new System.Drawing.Size(340, 311);
            this.groupBox_password_and_id.TabIndex = 22;
            this.groupBox_password_and_id.TabStop = false;
            this.groupBox_password_and_id.Text = "Authentification accès aux contrôles système";
            // 
            // display_password_state_textBox
            // 
            this.display_password_state_textBox.Enabled = false;
            this.display_password_state_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_password_state_textBox.Location = new System.Drawing.Point(6, 202);
            this.display_password_state_textBox.Multiline = true;
            this.display_password_state_textBox.Name = "display_password_state_textBox";
            this.display_password_state_textBox.Size = new System.Drawing.Size(334, 93);
            this.display_password_state_textBox.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Confirmation";
            // 
            // password_confirmation_textBox
            // 
            this.password_confirmation_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_confirmation_textBox.Location = new System.Drawing.Point(16, 164);
            this.password_confirmation_textBox.Name = "password_confirmation_textBox";
            this.password_confirmation_textBox.Size = new System.Drawing.Size(302, 23);
            this.password_confirmation_textBox.TabIndex = 7;
            this.password_confirmation_textBox.UseSystemPasswordChar = true;
            this.password_confirmation_textBox.TextChanged += new System.EventHandler(this.password_textBoxes_TextChanged);
            // 
            // find_id_button
            // 
            this.find_id_button.Location = new System.Drawing.Point(166, 19);
            this.find_id_button.Name = "find_id_button";
            this.find_id_button.Size = new System.Drawing.Size(81, 23);
            this.find_id_button.TabIndex = 23;
            this.find_id_button.Text = "Suggestion";
            this.find_id_button.UseVisualStyleBackColor = true;
            this.find_id_button.Click += new System.EventHandler(this.find_id_button_Click);
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.Location = new System.Drawing.Point(6, 19);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(145, 26);
            this.textBox5.TabIndex = 8;
            this.textBox5.Text = "Identifiant :";
            // 
            // password_textBox
            // 
            this.password_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_textBox.Location = new System.Drawing.Point(16, 126);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(302, 23);
            this.password_textBox.TabIndex = 6;
            this.password_textBox.UseSystemPasswordChar = true;
            this.password_textBox.Click += new System.EventHandler(this.password_textBox_Click);
            this.password_textBox.TextChanged += new System.EventHandler(this.password_textBoxes_TextChanged);
            // 
            // id_textBox
            // 
            this.id_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.id_textBox.Location = new System.Drawing.Point(16, 51);
            this.id_textBox.Multiline = true;
            this.id_textBox.Name = "id_textBox";
            this.id_textBox.Size = new System.Drawing.Size(302, 37);
            this.id_textBox.TabIndex = 5;
            this.id_textBox.TextChanged += new System.EventHandler(this.id_textBox_TextChanged);
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.Location = new System.Drawing.Point(6, 94);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(145, 26);
            this.textBox6.TabIndex = 9;
            this.textBox6.Text = "*Mot de passe :";
            // 
            // mail_adress_textBox
            // 
            this.mail_adress_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mail_adress_textBox.Location = new System.Drawing.Point(184, 156);
            this.mail_adress_textBox.Name = "mail_adress_textBox";
            this.mail_adress_textBox.Size = new System.Drawing.Size(199, 23);
            this.mail_adress_textBox.TabIndex = 3;
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(18, 156);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(145, 26);
            this.textBox7.TabIndex = 18;
            this.textBox7.Text = "*Adresse mail :";
            // 
            // right_checkedListBox
            // 
            this.right_checkedListBox.CheckOnClick = true;
            this.right_checkedListBox.FormattingEnabled = true;
            this.right_checkedListBox.Location = new System.Drawing.Point(192, 212);
            this.right_checkedListBox.Name = "right_checkedListBox";
            this.right_checkedListBox.Size = new System.Drawing.Size(191, 94);
            this.right_checkedListBox.TabIndex = 4;
            this.right_checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.right_checkedListBox_ItemCheck);
            // 
            // job_textBox
            // 
            this.job_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.job_textBox.Location = new System.Drawing.Point(184, 115);
            this.job_textBox.Name = "job_textBox";
            this.job_textBox.Size = new System.Drawing.Size(199, 23);
            this.job_textBox.TabIndex = 2;
            // 
            // first_name_textBox
            // 
            this.first_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.first_name_textBox.Location = new System.Drawing.Point(184, 30);
            this.first_name_textBox.Name = "first_name_textBox";
            this.first_name_textBox.Size = new System.Drawing.Size(199, 23);
            this.first_name_textBox.TabIndex = 0;
            // 
            // last_name_textBox
            // 
            this.last_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.last_name_textBox.Location = new System.Drawing.Point(184, 70);
            this.last_name_textBox.Name = "last_name_textBox";
            this.last_name_textBox.Size = new System.Drawing.Size(199, 23);
            this.last_name_textBox.TabIndex = 1;
            // 
            // save_change_button
            // 
            this.save_change_button.Location = new System.Drawing.Point(654, 432);
            this.save_change_button.Name = "save_change_button";
            this.save_change_button.Size = new System.Drawing.Size(101, 31);
            this.save_change_button.TabIndex = 8;
            this.save_change_button.Text = "Valider";
            this.save_change_button.UseVisualStyleBackColor = true;
            this.save_change_button.Click += new System.EventHandler(this.save_change_button_Click);
            // 
            // end_button
            // 
            this.end_button.Location = new System.Drawing.Point(1052, 517);
            this.end_button.Name = "end_button";
            this.end_button.Size = new System.Drawing.Size(101, 31);
            this.end_button.TabIndex = 8;
            this.end_button.Text = "Fin";
            this.end_button.UseVisualStyleBackColor = true;
            this.end_button.Click += new System.EventHandler(this.end_button_Click);
            // 
            // AccessManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 551);
            this.Controls.Add(this.end_button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "AccessManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestion des droits d\'accès";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox_short_id.ResumeLayout(false);
            this.groupBox_short_id.PerformLayout();
            this.groupBox_password_and_id.ResumeLayout(false);
            this.groupBox_password_and_id.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button supp_item_button;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.TextBox id_textBox;
        private System.Windows.Forms.TextBox job_textBox;
        private System.Windows.Forms.TextBox first_name_textBox;
        private System.Windows.Forms.TextBox last_name_textBox;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button save_change_button;
        private System.Windows.Forms.Button end_button;
        private System.Windows.Forms.CheckedListBox right_checkedListBox;
        private System.Windows.Forms.TextBox mail_adress_textBox;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.GroupBox groupBox_short_id;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox short_id_textBox;
        private System.Windows.Forms.GroupBox groupBox_password_and_id;
        private System.Windows.Forms.Button new_item_button;
        private System.Windows.Forms.Button new_short_id_button;
        private System.Windows.Forms.Button find_id_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox password_confirmation_textBox;
        private System.Windows.Forms.TextBox display_password_state_textBox;
    }
}