namespace WindowsFormsApplication1.Forms
{
    partial class AdminSetupBox
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.save_button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lastName_textBox = new System.Windows.Forms.TextBox();
            this.firstName_textBox = new System.Windows.Forms.TextBox();
            this.id_textBox = new System.Windows.Forms.TextBox();
            this.password1_textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.info_password_textBox = new System.Windows.Forms.TextBox();
            this.password2_textBox = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // save_button
            // 
            this.save_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_button.Location = new System.Drawing.Point(347, 448);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(81, 38);
            this.save_button.TabIndex = 7;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(18, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(90, 26);
            this.textBox1.TabIndex = 11;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Nom : ";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(18, 76);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(90, 26);
            this.textBox2.TabIndex = 21;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Prénom : ";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(18, 119);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(90, 26);
            this.textBox3.TabIndex = 31;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "Identifiant : ";
            // 
            // lastName_textBox
            // 
            this.lastName_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastName_textBox.Location = new System.Drawing.Point(137, 30);
            this.lastName_textBox.Name = "lastName_textBox";
            this.lastName_textBox.Size = new System.Drawing.Size(168, 26);
            this.lastName_textBox.TabIndex = 1;
            // 
            // firstName_textBox
            // 
            this.firstName_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstName_textBox.Location = new System.Drawing.Point(137, 76);
            this.firstName_textBox.Name = "firstName_textBox";
            this.firstName_textBox.Size = new System.Drawing.Size(168, 26);
            this.firstName_textBox.TabIndex = 2;
            // 
            // id_textBox
            // 
            this.id_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.id_textBox.Location = new System.Drawing.Point(137, 119);
            this.id_textBox.Name = "id_textBox";
            this.id_textBox.Size = new System.Drawing.Size(168, 26);
            this.id_textBox.TabIndex = 3;
            // 
            // password1_textBox
            // 
            this.password1_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password1_textBox.Location = new System.Drawing.Point(18, 121);
            this.password1_textBox.Name = "password1_textBox";
            this.password1_textBox.PasswordChar = '*';
            this.password1_textBox.Size = new System.Drawing.Size(264, 26);
            this.password1_textBox.TabIndex = 5;
            this.password1_textBox.UseSystemPasswordChar = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.info_password_textBox);
            this.groupBox1.Controls.Add(this.password2_textBox);
            this.groupBox1.Controls.Add(this.password1_textBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 237);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 192);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mot de passe";
            // 
            // info_password_textBox
            // 
            this.info_password_textBox.Enabled = false;
            this.info_password_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.info_password_textBox.Location = new System.Drawing.Point(18, 31);
            this.info_password_textBox.Multiline = true;
            this.info_password_textBox.Name = "info_password_textBox";
            this.info_password_textBox.ReadOnly = true;
            this.info_password_textBox.Size = new System.Drawing.Size(392, 74);
            this.info_password_textBox.TabIndex = 10;
            this.info_password_textBox.TabStop = false;
            this.info_password_textBox.Text = " ";
            // 
            // password2_textBox
            // 
            this.password2_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password2_textBox.Location = new System.Drawing.Point(18, 153);
            this.password2_textBox.Name = "password2_textBox";
            this.password2_textBox.PasswordChar = '*';
            this.password2_textBox.Size = new System.Drawing.Size(264, 26);
            this.password2_textBox.TabIndex = 6;
            this.password2_textBox.UseSystemPasswordChar = true;
            this.password2_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_password_KeyDown);
            // 
            // textBox9
            // 
            this.textBox9.Enabled = false;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(12, 15);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(416, 26);
            this.textBox9.TabIndex = 10;
            this.textBox9.TabStop = false;
            this.textBox9.Text = "Saisir les informations relatives au compte administrateur";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.id_textBox);
            this.groupBox2.Controls.Add(this.lastName_textBox);
            this.groupBox2.Controls.Add(this.firstName_textBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 161);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information compte";
            // 
            // AdminSetupBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 497);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.save_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminSetupBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paramètrage du compte administrateur";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox lastName_textBox;
        private System.Windows.Forms.TextBox firstName_textBox;
        private System.Windows.Forms.TextBox id_textBox;
        private System.Windows.Forms.TextBox password1_textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox password2_textBox;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox info_password_textBox;
    }
}
