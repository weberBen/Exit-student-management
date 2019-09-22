namespace WindowsFormsApplication1.Forms
{
    partial class SettingsAccessToServerFile
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
            this.activate_checkBox = new System.Windows.Forms.CheckBox();
            this.change_folder_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.file_name_textBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.display_resukt_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(437, 69);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Lorsque cette fonctionnalité est activée, à chaque fois que le serveur démarre l\'" +
    "adresse IP et le port de communication sont sauvegardés dans ce fichier sous for" +
    "me d\'un lien internet";
            // 
            // activate_checkBox
            // 
            this.activate_checkBox.AutoSize = true;
            this.activate_checkBox.Location = new System.Drawing.Point(12, 111);
            this.activate_checkBox.Name = "activate_checkBox";
            this.activate_checkBox.Size = new System.Drawing.Size(59, 17);
            this.activate_checkBox.TabIndex = 1;
            this.activate_checkBox.Text = "Activer";
            this.activate_checkBox.UseVisualStyleBackColor = true;
            this.activate_checkBox.CheckedChanged += new System.EventHandler(this.activate_checkBox_CheckedChanged);
            // 
            // change_folder_button
            // 
            this.change_folder_button.Location = new System.Drawing.Point(6, 71);
            this.change_folder_button.Name = "change_folder_button";
            this.change_folder_button.Size = new System.Drawing.Size(104, 26);
            this.change_folder_button.TabIndex = 2;
            this.change_folder_button.Text = "Sélectionner";
            this.change_folder_button.UseVisualStyleBackColor = true;
            this.change_folder_button.Click += new System.EventHandler(this.change_folder_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.file_name_textBox);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.display_resukt_textBox);
            this.groupBox1.Controls.Add(this.change_folder_button);
            this.groupBox1.Location = new System.Drawing.Point(12, 134);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 154);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // file_name_textBox
            // 
            this.file_name_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_name_textBox.Location = new System.Drawing.Point(129, 115);
            this.file_name_textBox.Name = "file_name_textBox";
            this.file_name_textBox.Size = new System.Drawing.Size(302, 24);
            this.file_name_textBox.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(6, 115);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(117, 26);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Nom du fichier : ";
            // 
            // display_resukt_textBox
            // 
            this.display_resukt_textBox.Enabled = false;
            this.display_resukt_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_resukt_textBox.Location = new System.Drawing.Point(6, 19);
            this.display_resukt_textBox.Multiline = true;
            this.display_resukt_textBox.Name = "display_resukt_textBox";
            this.display_resukt_textBox.Size = new System.Drawing.Size(425, 46);
            this.display_resukt_textBox.TabIndex = 4;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(349, 307);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(100, 45);
            this.save_button.TabIndex = 5;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(234, 307);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(100, 45);
            this.cancel_button.TabIndex = 6;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // SettingsAccessToServerFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 359);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.activate_checkBox);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.Name = "SettingsAccessToServerFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fichier d\'accès au serveur";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox activate_checkBox;
        private System.Windows.Forms.Button change_folder_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox display_resukt_textBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox file_name_textBox;
        private System.Windows.Forms.TextBox textBox2;
    }
}