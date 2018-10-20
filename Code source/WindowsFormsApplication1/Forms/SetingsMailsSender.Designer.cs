namespace WindowsFormsApplication1.Forms
{
    partial class SetingsMailsSender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetingsMailsSender));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.mail_adress_textBox = new System.Windows.Forms.TextBox();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.smtp_server_textBox = new System.Windows.Forms.TextBox();
            this.smtp_port_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.upate_webBrowsers_button = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.highligth_button = new System.Windows.Forms.Button();
            this.italic_button = new System.Windows.Forms.Button();
            this.underline_button = new System.Windows.Forms.Button();
            this.bold_button = new System.Windows.Forms.Button();
            this.webBrowser_message = new System.Windows.Forms.WebBrowser();
            this.after_message_textBox = new System.Windows.Forms.TextBox();
            this.before_message_textBox = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(16, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(184, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "*Adresse email : ";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(16, 59);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(184, 23);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "*Mot de passe : ";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(16, 102);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(184, 60);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "*Nom du server SMTP : \r\n(ex : \"smtp-mail.outlook.com\", \"smtp.gmail.com\")";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(16, 178);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(184, 24);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "*Port SMTP :";
            // 
            // mail_adress_textBox
            // 
            this.mail_adress_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mail_adress_textBox.Location = new System.Drawing.Point(228, 19);
            this.mail_adress_textBox.Name = "mail_adress_textBox";
            this.mail_adress_textBox.Size = new System.Drawing.Size(184, 23);
            this.mail_adress_textBox.TabIndex = 4;
            // 
            // password_textBox
            // 
            this.password_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_textBox.Location = new System.Drawing.Point(228, 59);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(184, 23);
            this.password_textBox.TabIndex = 5;
            this.password_textBox.UseSystemPasswordChar = true;
            this.password_textBox.Click += new System.EventHandler(this.password_textBox_Click);
            // 
            // smtp_server_textBox
            // 
            this.smtp_server_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.smtp_server_textBox.Location = new System.Drawing.Point(228, 120);
            this.smtp_server_textBox.Name = "smtp_server_textBox";
            this.smtp_server_textBox.Size = new System.Drawing.Size(184, 23);
            this.smtp_server_textBox.TabIndex = 6;
            // 
            // smtp_port_textBox
            // 
            this.smtp_port_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.smtp_port_textBox.Location = new System.Drawing.Point(228, 179);
            this.smtp_port_textBox.Name = "smtp_port_textBox";
            this.smtp_port_textBox.Size = new System.Drawing.Size(184, 23);
            this.smtp_port_textBox.TabIndex = 7;
            this.smtp_port_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.smtp_port_textBoxe_KeyPress);
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(1334, 574);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(89, 35);
            this.save_button.TabIndex = 8;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(1230, 574);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(89, 35);
            this.cancel_button.TabIndex = 9;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.Color.DarkRed;
            this.textBox5.Location = new System.Drawing.Point(12, 12);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(1411, 85);
            this.textBox5.TabIndex = 10;
            this.textBox5.Text = resources.GetString("textBox5.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.smtp_port_textBox);
            this.groupBox1.Controls.Add(this.mail_adress_textBox);
            this.groupBox1.Controls.Add(this.smtp_server_textBox);
            this.groupBox1.Controls.Add(this.password_textBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 220);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paramètres du serveur d\'envoi";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.upate_webBrowsers_button);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.webBrowser_message);
            this.groupBox2.Controls.Add(this.after_message_textBox);
            this.groupBox2.Controls.Add(this.before_message_textBox);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Location = new System.Drawing.Point(467, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(956, 442);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Paramètres des courriels";
            // 
            // upate_webBrowsers_button
            // 
            this.upate_webBrowsers_button.Location = new System.Drawing.Point(493, 364);
            this.upate_webBrowsers_button.Name = "upate_webBrowsers_button";
            this.upate_webBrowsers_button.Size = new System.Drawing.Size(446, 31);
            this.upate_webBrowsers_button.TabIndex = 15;
            this.upate_webBrowsers_button.Text = "Mettre à jour le contenu";
            this.upate_webBrowsers_button.UseVisualStyleBackColor = true;
            this.upate_webBrowsers_button.Click += new System.EventHandler(this.upate_webBrowsers_button_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.highligth_button);
            this.groupBox3.Controls.Add(this.italic_button);
            this.groupBox3.Controls.Add(this.underline_button);
            this.groupBox3.Controls.Add(this.bold_button);
            this.groupBox3.Location = new System.Drawing.Point(22, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(446, 88);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controle en HTML";
            // 
            // highligth_button
            // 
            this.highligth_button.Location = new System.Drawing.Point(304, 36);
            this.highligth_button.Name = "highligth_button";
            this.highligth_button.Size = new System.Drawing.Size(75, 31);
            this.highligth_button.TabIndex = 11;
            this.highligth_button.Text = "Surligner";
            this.highligth_button.UseVisualStyleBackColor = true;
            this.highligth_button.Click += new System.EventHandler(this.highligth_button_Click);
            // 
            // italic_button
            // 
            this.italic_button.Location = new System.Drawing.Point(61, 36);
            this.italic_button.Name = "italic_button";
            this.italic_button.Size = new System.Drawing.Size(75, 31);
            this.italic_button.TabIndex = 8;
            this.italic_button.Text = "Italique";
            this.italic_button.UseVisualStyleBackColor = true;
            this.italic_button.Click += new System.EventHandler(this.italic_button_Click);
            // 
            // underline_button
            // 
            this.underline_button.Location = new System.Drawing.Point(223, 36);
            this.underline_button.Name = "underline_button";
            this.underline_button.Size = new System.Drawing.Size(75, 31);
            this.underline_button.TabIndex = 10;
            this.underline_button.Text = "Souligner";
            this.underline_button.UseVisualStyleBackColor = true;
            this.underline_button.Click += new System.EventHandler(this.underline_button_Click);
            // 
            // bold_button
            // 
            this.bold_button.Location = new System.Drawing.Point(142, 36);
            this.bold_button.Name = "bold_button";
            this.bold_button.Size = new System.Drawing.Size(75, 31);
            this.bold_button.TabIndex = 9;
            this.bold_button.Text = "Gras";
            this.bold_button.UseVisualStyleBackColor = true;
            this.bold_button.Click += new System.EventHandler(this.bold_button_Click);
            // 
            // webBrowser_message
            // 
            this.webBrowser_message.Location = new System.Drawing.Point(493, 62);
            this.webBrowser_message.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_message.Name = "webBrowser_message";
            this.webBrowser_message.Size = new System.Drawing.Size(446, 246);
            this.webBrowser_message.TabIndex = 13;
            this.webBrowser_message.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserMessageDocumentCompleted);
            // 
            // after_message_textBox
            // 
            this.after_message_textBox.AcceptsReturn = true;
            this.after_message_textBox.AcceptsTab = true;
            this.after_message_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.after_message_textBox.Location = new System.Drawing.Point(22, 208);
            this.after_message_textBox.Multiline = true;
            this.after_message_textBox.Name = "after_message_textBox";
            this.after_message_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.after_message_textBox.Size = new System.Drawing.Size(446, 100);
            this.after_message_textBox.TabIndex = 6;
            this.after_message_textBox.TextChanged += new System.EventHandler(this.textChangedEventHandler);
            // 
            // before_message_textBox
            // 
            this.before_message_textBox.AcceptsReturn = true;
            this.before_message_textBox.AcceptsTab = true;
            this.before_message_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.before_message_textBox.Location = new System.Drawing.Point(22, 62);
            this.before_message_textBox.Multiline = true;
            this.before_message_textBox.Name = "before_message_textBox";
            this.before_message_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.before_message_textBox.Size = new System.Drawing.Size(446, 100);
            this.before_message_textBox.TabIndex = 5;
            this.before_message_textBox.TextChanged += new System.EventHandler(this.textChangedEventHandler);
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.Location = new System.Drawing.Point(6, 30);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(257, 23);
            this.textBox6.TabIndex = 2;
            this.textBox6.Text = "Message à afficher en début de courriel :";
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(6, 179);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(257, 23);
            this.textBox7.TabIndex = 3;
            this.textBox7.Text = "Message à afficher en fin de couriel : ";
            // 
            // SetingsMailsSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 618);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Name = "SetingsMailsSender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestion de la structure des mails";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox mail_adress_textBox;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.TextBox smtp_server_textBox;
        private System.Windows.Forms.TextBox smtp_port_textBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox after_message_textBox;
        private System.Windows.Forms.TextBox before_message_textBox;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button highligth_button;
        private System.Windows.Forms.Button italic_button;
        private System.Windows.Forms.Button underline_button;
        private System.Windows.Forms.Button bold_button;
        private System.Windows.Forms.WebBrowser webBrowser_message;
        private System.Windows.Forms.Button upate_webBrowsers_button;
    }
}