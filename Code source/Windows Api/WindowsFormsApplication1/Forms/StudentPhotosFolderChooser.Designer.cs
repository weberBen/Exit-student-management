namespace WindowsFormsApplication1.Forms
{
    partial class StudentPhotosFolderChooser
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delete_radioButton = new System.Windows.Forms.RadioButton();
            this.merge_radioButton = new System.Windows.Forms.RadioButton();
            this.import_button = new System.Windows.Forms.Button();
            this.folder_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.delete_radioButton);
            this.groupBox1.Controls.Add(this.merge_radioButton);
            this.groupBox1.Location = new System.Drawing.Point(23, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action lors de l\'importation";
            // 
            // delete_radioButton
            // 
            this.delete_radioButton.AutoSize = true;
            this.delete_radioButton.Location = new System.Drawing.Point(19, 64);
            this.delete_radioButton.Name = "delete_radioButton";
            this.delete_radioButton.Size = new System.Drawing.Size(174, 17);
            this.delete_radioButton.TabIndex = 2;
            this.delete_radioButton.TabStop = true;
            this.delete_radioButton.Text = "Supprimer les images existantes";
            this.delete_radioButton.UseVisualStyleBackColor = true;
            // 
            // merge_radioButton
            // 
            this.merge_radioButton.AutoSize = true;
            this.merge_radioButton.Location = new System.Drawing.Point(19, 29);
            this.merge_radioButton.Name = "merge_radioButton";
            this.merge_radioButton.Size = new System.Drawing.Size(200, 17);
            this.merge_radioButton.TabIndex = 1;
            this.merge_radioButton.TabStop = true;
            this.merge_radioButton.Text = "Fusionner avec les images existantes";
            this.merge_radioButton.UseVisualStyleBackColor = true;
            // 
            // import_button
            // 
            this.import_button.Location = new System.Drawing.Point(42, 206);
            this.import_button.Name = "import_button";
            this.import_button.Size = new System.Drawing.Size(95, 27);
            this.import_button.TabIndex = 1;
            this.import_button.Text = "Importer";
            this.import_button.UseVisualStyleBackColor = true;
            this.import_button.Click += new System.EventHandler(this.import_button_Click);
            // 
            // folder_textBox
            // 
            this.folder_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folder_textBox.Location = new System.Drawing.Point(23, 134);
            this.folder_textBox.Multiline = true;
            this.folder_textBox.Name = "folder_textBox";
            this.folder_textBox.ReadOnly = true;
            this.folder_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.folder_textBox.Size = new System.Drawing.Size(400, 66);
            this.folder_textBox.TabIndex = 2;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(324, 224);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(99, 35);
            this.save_button.TabIndex = 3;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // StudentPhotosFolderChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 267);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.folder_textBox);
            this.Controls.Add(this.import_button);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StudentPhotosFolderChooser";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton delete_radioButton;
        private System.Windows.Forms.RadioButton merge_radioButton;
        private System.Windows.Forms.Button import_button;
        private System.Windows.Forms.TextBox folder_textBox;
        private System.Windows.Forms.Button save_button;
    }
}
