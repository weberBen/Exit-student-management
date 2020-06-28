namespace WindowsFormsApplication1.Forms
{
    partial class DatabaseSettings
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
            this.driver_name_textBox = new System.Windows.Forms.TextBox();
            this.server_name_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.database_name_textBox = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 23);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(170, 29);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "*Nom du driver :";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(12, 177);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(170, 29);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "*Nom de l\'instance SQL server";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(44, 71);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(492, 71);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "Les noms des driver ODBC installé sur votre machine sont accéssible depuis le pan" +
    "naux d\'administration des sources de données ODBC sous windows";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(44, 229);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(492, 71);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "Le nom de votre serveur SQL est notamment disponible en utilisant Microsoft SQL S" +
    "erver Managment Studio";
            // 
            // driver_name_textBox
            // 
            this.driver_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driver_name_textBox.HideSelection = false;
            this.driver_name_textBox.Location = new System.Drawing.Point(202, 28);
            this.driver_name_textBox.Name = "driver_name_textBox";
            this.driver_name_textBox.Size = new System.Drawing.Size(334, 24);
            this.driver_name_textBox.TabIndex = 4;
            // 
            // server_name_textBox
            // 
            this.server_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.server_name_textBox.HideSelection = false;
            this.server_name_textBox.Location = new System.Drawing.Point(202, 182);
            this.server_name_textBox.Name = "server_name_textBox";
            this.server_name_textBox.Size = new System.Drawing.Size(334, 24);
            this.server_name_textBox.TabIndex = 5;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(419, 388);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(143, 36);
            this.save_button.TabIndex = 6;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(261, 388);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(143, 36);
            this.cancel_button.TabIndex = 7;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // database_name_textBox
            // 
            this.database_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.database_name_textBox.HideSelection = false;
            this.database_name_textBox.Location = new System.Drawing.Point(202, 336);
            this.database_name_textBox.Name = "database_name_textBox";
            this.database_name_textBox.Size = new System.Drawing.Size(334, 24);
            this.database_name_textBox.TabIndex = 9;
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.Location = new System.Drawing.Point(12, 331);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(170, 29);
            this.textBox6.TabIndex = 8;
            this.textBox6.Text = "*Nom base de données :";
            // 
            // DatabaseSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 435);
            this.Controls.Add(this.database_name_textBox);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.server_name_textBox);
            this.Controls.Add(this.driver_name_textBox);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.Name = "DatabaseSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paramètres de connexion à la base de données";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox driver_name_textBox;
        private System.Windows.Forms.TextBox server_name_textBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox database_name_textBox;
        private System.Windows.Forms.TextBox textBox6;
    }
}