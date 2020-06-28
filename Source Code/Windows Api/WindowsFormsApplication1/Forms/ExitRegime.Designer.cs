namespace WindowsFormsApplication1.Forms
{
    partial class ExitRegime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitRegime));
            this.Exit_regime_dataGridView = new System.Windows.Forms.DataGridView();
            this.Authorization_dataGridView = new System.Windows.Forms.DataGridView();
            this.label_authorization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.period_authorization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start_authorization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end_authorization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relation_authorization = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.save_relations__button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.main_groupBox = new System.Windows.Forms.GroupBox();
            this.default_regime_checkBox = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.selected_regime_textBox = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label_regime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exitEndOfDay_regime = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Exit_regime_dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Authorization_dataGridView)).BeginInit();
            this.main_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Exit_regime_dataGridView
            // 
            this.Exit_regime_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Exit_regime_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.label_regime,
            this.exitEndOfDay_regime});
            this.Exit_regime_dataGridView.Location = new System.Drawing.Point(20, 60);
            this.Exit_regime_dataGridView.Name = "Exit_regime_dataGridView";
            this.Exit_regime_dataGridView.Size = new System.Drawing.Size(259, 262);
            this.Exit_regime_dataGridView.TabIndex = 0;
            // 
            // Authorization_dataGridView
            // 
            this.Authorization_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Authorization_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.label_authorization,
            this.period_authorization,
            this.start_authorization,
            this.end_authorization,
            this.relation_authorization});
            this.Authorization_dataGridView.Location = new System.Drawing.Point(369, 60);
            this.Authorization_dataGridView.Name = "Authorization_dataGridView";
            this.Authorization_dataGridView.Size = new System.Drawing.Size(565, 262);
            this.Authorization_dataGridView.TabIndex = 3;
            // 
            // label_authorization
            // 
            this.label_authorization.HeaderText = "Nom";
            this.label_authorization.Name = "label_authorization";
            this.label_authorization.ToolTipText = "Nom associé à la permission";
            // 
            // period_authorization
            // 
            this.period_authorization.HeaderText = "Période";
            this.period_authorization.Name = "period_authorization";
            this.period_authorization.ReadOnly = true;
            this.period_authorization.ToolTipText = "Période de temps au bout de laquelle un élève peut sortir selon la plage horraire" +
    " spécifiée";
            // 
            // start_authorization
            // 
            this.start_authorization.HeaderText = "Début plage";
            this.start_authorization.Name = "start_authorization";
            this.start_authorization.ReadOnly = true;
            this.start_authorization.ToolTipText = "Début de la plage horraire";
            // 
            // end_authorization
            // 
            this.end_authorization.HeaderText = "Fin plage";
            this.end_authorization.Name = "end_authorization";
            this.end_authorization.ReadOnly = true;
            this.end_authorization.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.end_authorization.ToolTipText = "Fin de la plage horraire";
            // 
            // relation_authorization
            // 
            this.relation_authorization.FalseValue = "0";
            this.relation_authorization.HeaderText = "Associé";
            this.relation_authorization.Name = "relation_authorization";
            this.relation_authorization.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.relation_authorization.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.relation_authorization.ToolTipText = "Associer la permission avec le régime de sortie sélectionné";
            this.relation_authorization.TrueValue = "1";
            // 
            // save_relations__button
            // 
            this.save_relations__button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_relations__button.Location = new System.Drawing.Point(286, 335);
            this.save_relations__button.Name = "save_relations__button";
            this.save_relations__button.Size = new System.Drawing.Size(75, 32);
            this.save_relations__button.TabIndex = 5;
            this.save_relations__button.Text = "Asscoier";
            this.save_relations__button.UseVisualStyleBackColor = true;
            this.save_relations__button.Click += new System.EventHandler(this.button2_Click);
            // 
            // save_button
            // 
            this.save_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_button.Location = new System.Drawing.Point(889, 595);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(85, 37);
            this.save_button.TabIndex = 7;
            this.save_button.Text = "Valider";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.button3_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_button.Location = new System.Drawing.Point(789, 595);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(85, 37);
            this.cancel_button.TabIndex = 8;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // main_groupBox
            // 
            this.main_groupBox.Controls.Add(this.default_regime_checkBox);
            this.main_groupBox.Controls.Add(this.textBox2);
            this.main_groupBox.Controls.Add(this.textBox1);
            this.main_groupBox.Controls.Add(this.Exit_regime_dataGridView);
            this.main_groupBox.Controls.Add(this.Authorization_dataGridView);
            this.main_groupBox.Controls.Add(this.save_relations__button);
            this.main_groupBox.Controls.Add(this.selected_regime_textBox);
            this.main_groupBox.Location = new System.Drawing.Point(12, 173);
            this.main_groupBox.Name = "main_groupBox";
            this.main_groupBox.Size = new System.Drawing.Size(962, 376);
            this.main_groupBox.TabIndex = 9;
            this.main_groupBox.TabStop = false;
            this.main_groupBox.Text = "Gestion";
            // 
            // default_regime_checkBox
            // 
            this.default_regime_checkBox.AutoSize = true;
            this.default_regime_checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.default_regime_checkBox.Location = new System.Drawing.Point(740, 335);
            this.default_regime_checkBox.Name = "default_regime_checkBox";
            this.default_regime_checkBox.Size = new System.Drawing.Size(139, 21);
            this.default_regime_checkBox.TabIndex = 9;
            this.default_regime_checkBox.Text = "régime par défaut";
            this.default_regime_checkBox.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(369, 34);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(565, 20);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "Permissions";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(20, 34);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(259, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "Régimes de sortie";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // selected_regime_textBox
            // 
            this.selected_regime_textBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.selected_regime_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.selected_regime_textBox.Cursor = System.Windows.Forms.Cursors.No;
            this.selected_regime_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selected_regime_textBox.Location = new System.Drawing.Point(490, 328);
            this.selected_regime_textBox.Name = "selected_regime_textBox";
            this.selected_regime_textBox.ReadOnly = true;
            this.selected_regime_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.selected_regime_textBox.Size = new System.Drawing.Size(244, 26);
            this.selected_regime_textBox.TabIndex = 6;
            this.selected_regime_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Window;
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(962, 134);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // label_regime
            // 
            this.label_regime.HeaderText = "Nom";
            this.label_regime.Name = "label_regime";
            this.label_regime.ToolTipText = "Nom associé au régime de sortie";
            // 
            // exitEndOfDay_regime
            // 
            this.exitEndOfDay_regime.FalseValue = "0";
            this.exitEndOfDay_regime.HeaderText = "Sortie fin de journée";
            this.exitEndOfDay_regime.Name = "exitEndOfDay_regime";
            this.exitEndOfDay_regime.TrueValue = "1";
            // 
            // ExitRegime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 643);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.main_groupBox);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Name = "ExitRegime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Paramètres des régimes de sortie";
            ((System.ComponentModel.ISupportInitialize)(this.Exit_regime_dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Authorization_dataGridView)).EndInit();
            this.main_groupBox.ResumeLayout(false);
            this.main_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView Authorization_dataGridView;
        private System.Windows.Forms.DataGridView Exit_regime_dataGridView;
        private System.Windows.Forms.Button save_relations__button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.GroupBox main_groupBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox selected_regime_textBox;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn label_authorization;
        private System.Windows.Forms.DataGridViewTextBoxColumn period_authorization;
        private System.Windows.Forms.DataGridViewTextBoxColumn start_authorization;
        private System.Windows.Forms.DataGridViewTextBoxColumn end_authorization;
        private System.Windows.Forms.DataGridViewCheckBoxColumn relation_authorization;
        private System.Windows.Forms.CheckBox default_regime_checkBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn label_regime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn exitEndOfDay_regime;
    }
}