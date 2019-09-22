namespace WindowsFormsApplication1.Forms
{
    partial class SettingsList
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.display_textBox = new System.Windows.Forms.TextBox();
            this.item_textBox = new System.Windows.Forms.TextBox();
            this.add_item_button = new System.Windows.Forms.Button();
            this.del_item_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(66, 81);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(290, 109);
            this.checkedListBox1.TabIndex = 1;
            // 
            // display_textBox
            // 
            this.display_textBox.Enabled = false;
            this.display_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_textBox.ForeColor = System.Drawing.Color.DarkRed;
            this.display_textBox.Location = new System.Drawing.Point(12, 12);
            this.display_textBox.Multiline = true;
            this.display_textBox.Name = "display_textBox";
            this.display_textBox.Size = new System.Drawing.Size(412, 49);
            this.display_textBox.TabIndex = 2;
            // 
            // item_textBox
            // 
            this.item_textBox.Location = new System.Drawing.Point(6, 21);
            this.item_textBox.Name = "item_textBox";
            this.item_textBox.Size = new System.Drawing.Size(130, 20);
            this.item_textBox.TabIndex = 3;
            // 
            // add_item_button
            // 
            this.add_item_button.Location = new System.Drawing.Point(162, 21);
            this.add_item_button.Name = "add_item_button";
            this.add_item_button.Size = new System.Drawing.Size(75, 23);
            this.add_item_button.TabIndex = 4;
            this.add_item_button.Text = "Ajouter";
            this.add_item_button.UseVisualStyleBackColor = true;
            this.add_item_button.Click += new System.EventHandler(this.add_item_button_Click);
            // 
            // del_item_button
            // 
            this.del_item_button.Location = new System.Drawing.Point(6, 71);
            this.del_item_button.Name = "del_item_button";
            this.del_item_button.Size = new System.Drawing.Size(231, 23);
            this.del_item_button.TabIndex = 5;
            this.del_item_button.Text = "Supprimer les élèments selectionnés";
            this.del_item_button.UseVisualStyleBackColor = true;
            this.del_item_button.Click += new System.EventHandler(this.del_item_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.item_textBox);
            this.groupBox1.Controls.Add(this.del_item_button);
            this.groupBox1.Controls.Add(this.add_item_button);
            this.groupBox1.Location = new System.Drawing.Point(12, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(330, 351);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(90, 35);
            this.save_button.TabIndex = 7;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(225, 351);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(90, 35);
            this.cancel_button.TabIndex = 8;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // SettingsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 398);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.display_textBox);
            this.Controls.Add(this.checkedListBox1);
            this.MaximizeBox = false;
            this.Name = "SettingsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestion administrative";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.TextBox display_textBox;
        private System.Windows.Forms.TextBox item_textBox;
        private System.Windows.Forms.Button add_item_button;
        private System.Windows.Forms.Button del_item_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
    }
}