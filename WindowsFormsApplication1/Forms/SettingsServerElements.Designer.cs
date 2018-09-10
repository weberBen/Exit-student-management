namespace WindowsFormsApplication1.Forms
{
    partial class SettingsServerElements
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
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.ipAdressTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.findPortAuto = new System.Windows.Forms.Button();
            this.findIPAuto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(304, 168);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(119, 38);
            this.save.TabIndex = 0;
            this.save.Text = "Valider les changements";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(168, 168);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(119, 38);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Annuler";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ipAdressTextBox
            // 
            this.ipAdressTextBox.Location = new System.Drawing.Point(55, 50);
            this.ipAdressTextBox.Name = "ipAdressTextBox";
            this.ipAdressTextBox.Size = new System.Drawing.Size(207, 20);
            this.ipAdressTextBox.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(180, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Adresse IP locale (IPv4)  actuelle :";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(12, 85);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(180, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Port de communication actuel : ";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(55, 123);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(120, 20);
            this.portTextBox.TabIndex = 4;
            // 
            // findPortAuto
            // 
            this.findPortAuto.Location = new System.Drawing.Point(197, 123);
            this.findPortAuto.Name = "findPortAuto";
            this.findPortAuto.Size = new System.Drawing.Size(160, 23);
            this.findPortAuto.TabIndex = 6;
            this.findPortAuto.Text = "Trouver automatiquement";
            this.findPortAuto.UseVisualStyleBackColor = true;
            this.findPortAuto.Click += new System.EventHandler(this.findPortAuto_Click);
            // 
            // findIPAuto
            // 
            this.findIPAuto.Location = new System.Drawing.Point(268, 48);
            this.findIPAuto.Name = "findIPAuto";
            this.findIPAuto.Size = new System.Drawing.Size(89, 23);
            this.findIPAuto.TabIndex = 7;
            this.findIPAuto.Text = "Trouver";
            this.findIPAuto.UseVisualStyleBackColor = true;
            this.findIPAuto.Click += new System.EventHandler(this.findIPAuto_Click);
            // 
            // SettingsServerElements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 218);
            this.Controls.Add(this.findIPAuto);
            this.Controls.Add(this.findPortAuto);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.ipAdressTextBox);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsServerElements";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Réglage des paramètres du serveur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox ipAdressTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Button findPortAuto;
        private System.Windows.Forms.Button findIPAuto;
    }
}
