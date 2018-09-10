namespace WindowsFormsApplication1.Forms
{
    partial class SettingsLengthExitBreak
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsLengthExitBreak));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.minute_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hour_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.enable_pause_checkBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.minute_textBox);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.hour_textBox);
            this.groupBox.Location = new System.Drawing.Point(12, 145);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(350, 89);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "min";
            // 
            // minute_textBox
            // 
            this.minute_textBox.Location = new System.Drawing.Point(158, 36);
            this.minute_textBox.Name = "minute_textBox";
            this.minute_textBox.Size = new System.Drawing.Size(32, 20);
            this.minute_textBox.TabIndex = 2;
            this.minute_textBox.TextChanged += new System.EventHandler(this.minute_textBox_TextChanged);
            this.minute_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.minute_textBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "h";
            // 
            // hour_textBox
            // 
            this.hour_textBox.Location = new System.Drawing.Point(104, 36);
            this.hour_textBox.Name = "hour_textBox";
            this.hour_textBox.Size = new System.Drawing.Size(32, 20);
            this.hour_textBox.TabIndex = 0;
            this.hour_textBox.TextChanged += new System.EventHandler(this.hour_textBox_TextChanged);
            this.hour_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.hour_textBox_KeyPress);
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(364, 258);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(87, 35);
            this.save_button.TabIndex = 2;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(260, 258);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(87, 35);
            this.cancel_button.TabIndex = 3;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // enable_pause_checkBox
            // 
            this.enable_pause_checkBox.AutoSize = true;
            this.enable_pause_checkBox.Location = new System.Drawing.Point(12, 122);
            this.enable_pause_checkBox.Name = "enable_pause_checkBox";
            this.enable_pause_checkBox.Size = new System.Drawing.Size(351, 17);
            this.enable_pause_checkBox.TabIndex = 4;
            this.enable_pause_checkBox.Text = "Activer la détection automatique des pauses autorisant l\'élève à sortir";
            this.enable_pause_checkBox.UseVisualStyleBackColor = true;
            this.enable_pause_checkBox.CheckedChanged += new System.EventHandler(this.enable_pause_checkBox_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(439, 88);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // SettingsLengthExitBreak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 301);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.enable_pause_checkBox);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.groupBox);
            this.Name = "SettingsLengthExitBreak";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pause autorisation de sortie";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox minute_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hour_textBox;
        private System.Windows.Forms.CheckBox enable_pause_checkBox;
        private System.Windows.Forms.TextBox textBox1;
    }
}