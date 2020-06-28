namespace WindowsFormsApplication1.Forms
{
    partial class HtmlMessageBox
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
            this.ok_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.close_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_button.Location = new System.Drawing.Point(298, 192);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(69, 32);
            this.ok_button.TabIndex = 2;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.webBrowser);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 184);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(378, 184);
            this.webBrowser.TabIndex = 4;
            // 
            // close_button
            // 
            this.close_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.close_button.Location = new System.Drawing.Point(226, 192);
            this.close_button.Name = "close_button";
            this.close_button.Size = new System.Drawing.Size(66, 32);
            this.close_button.TabIndex = 4;
            this.close_button.Text = "Fermer";
            this.close_button.UseVisualStyleBackColor = true;
            // 
            // HtmlMessageBox
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_button;
            this.ClientSize = new System.Drawing.Size(379, 228);
            this.Controls.Add(this.close_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ok_button);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HtmlMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Button close_button;
    }
}