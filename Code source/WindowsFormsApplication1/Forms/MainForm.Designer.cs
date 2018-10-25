namespace WindowsFormsApplication1
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.update = new System.Windows.Forms.ToolStripMenuItem();
            this.rafraichirLaBaseDeDonnées = new System.Windows.Forms.ToolStripMenuItem();
            this.purgerLaBaseDeDonnées = new System.Windows.Forms.ToolStripMenuItem();
            this.redémarrerLeServer = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.nomDeDomainDuServerSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.adresseIpDuServer = new System.Windows.Forms.ToolStripMenuItem();
            this.fichierDaccèsAuServeur = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDesAdressesMails = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDuServiceDenvoiDeCourriel = new System.Windows.Forms.ToolStripMenuItem();
            this.donnéesDeConnexionSiteWeb = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDesDroitsDaccès = new System.Windows.Forms.ToolStripMenuItem();
            this.Administration = new System.Windows.Forms.ToolStripMenuItem();
            this.SchoolData = new System.Windows.Forms.ToolStripMenuItem();
            this.formatNomDesPhotosÉtudiantes = new System.Windows.Forms.ToolStripMenuItem();
            this.fichierDétatsDesÉtudiants = new System.Windows.Forms.ToolStripMenuItem();
            this.duréeDeLaPauseAutorisantLesSorties = new System.Windows.Forms.ToolStripMenuItem();
            this.longueurIdentifiantRFID = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDesDonnéesÉlèves = new System.Windows.Forms.ToolStripMenuItem();
            this.SchoolLifeOffice = new System.Windows.Forms.ToolStripMenuItem();
            this.sonSortieNonAutorisé = new System.Windows.Forms.ToolStripMenuItem();
            this.motifsDinterdictionDeSortie = new System.Windows.Forms.ToolStripMenuItem();
            this.appelationPunitionEnPrésence = new System.Windows.Forms.ToolStripMenuItem();
            this.couleurPunitionNonClôturée = new System.Windows.Forms.ToolStripMenuItem();
            this.aideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayEventTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connexion_button = new System.Windows.Forms.Button();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.id_textBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.disconnection_button = new System.Windows.Forms.Button();
            this.display_session_textBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.update,
            this.Settings,
            this.Administration,
            this.SchoolLifeOffice,
            this.aideToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(476, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // update
            // 
            this.update.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rafraichirLaBaseDeDonnées,
            this.purgerLaBaseDeDonnées,
            this.redémarrerLeServer});
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(140, 20);
            this.update.Text = "Mis à jour des données";
            // 
            // rafraichirLaBaseDeDonnées
            // 
            this.rafraichirLaBaseDeDonnées.Name = "rafraichirLaBaseDeDonnées";
            this.rafraichirLaBaseDeDonnées.Size = new System.Drawing.Size(227, 22);
            this.rafraichirLaBaseDeDonnées.Text = "Rafraîchir la base de données";
            // 
            // purgerLaBaseDeDonnées
            // 
            this.purgerLaBaseDeDonnées.Name = "purgerLaBaseDeDonnées";
            this.purgerLaBaseDeDonnées.Size = new System.Drawing.Size(227, 22);
            this.purgerLaBaseDeDonnées.Text = "Purger la base de données";
            // 
            // redémarrerLeServer
            // 
            this.redémarrerLeServer.Name = "redémarrerLeServer";
            this.redémarrerLeServer.Size = new System.Drawing.Size(227, 22);
            this.redémarrerLeServer.Text = "Redémarrer le server";
            // 
            // Settings
            // 
            this.Settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nomDeDomainDuServerSQL,
            this.adresseIpDuServer,
            this.fichierDaccèsAuServeur,
            this.gestionDesAdressesMails,
            this.gestionDuServiceDenvoiDeCourriel,
            this.donnéesDeConnexionSiteWeb,
            this.gestionDesDroitsDaccès});
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(66, 20);
            this.Settings.Text = "Régalges";
            // 
            // nomDeDomainDuServerSQL
            // 
            this.nomDeDomainDuServerSQL.Name = "nomDeDomainDuServerSQL";
            this.nomDeDomainDuServerSQL.Size = new System.Drawing.Size(271, 22);
            this.nomDeDomainDuServerSQL.Text = "Nom de domain du server SQL";
            // 
            // adresseIpDuServer
            // 
            this.adresseIpDuServer.Name = "adresseIpDuServer";
            this.adresseIpDuServer.Size = new System.Drawing.Size(271, 22);
            this.adresseIpDuServer.Tag = "";
            this.adresseIpDuServer.Text = "Adresse Ip du server";
            // 
            // fichierDaccèsAuServeur
            // 
            this.fichierDaccèsAuServeur.Name = "fichierDaccèsAuServeur";
            this.fichierDaccèsAuServeur.Size = new System.Drawing.Size(271, 22);
            this.fichierDaccèsAuServeur.Tag = "";
            this.fichierDaccèsAuServeur.Text = "Fichier d\'accès au serveur";
            // 
            // gestionDesAdressesMails
            // 
            this.gestionDesAdressesMails.Name = "gestionDesAdressesMails";
            this.gestionDesAdressesMails.Size = new System.Drawing.Size(271, 22);
            this.gestionDesAdressesMails.Tag = "";
            this.gestionDesAdressesMails.Text = "Gestion des adresses mails";
            // 
            // gestionDuServiceDenvoiDeCourriel
            // 
            this.gestionDuServiceDenvoiDeCourriel.Name = "gestionDuServiceDenvoiDeCourriel";
            this.gestionDuServiceDenvoiDeCourriel.Size = new System.Drawing.Size(271, 22);
            this.gestionDuServiceDenvoiDeCourriel.Tag = "";
            this.gestionDuServiceDenvoiDeCourriel.Text = "Gestion du service d\'envoi de courriel";
            // 
            // donnéesDeConnexionSiteWeb
            // 
            this.donnéesDeConnexionSiteWeb.Name = "donnéesDeConnexionSiteWeb";
            this.donnéesDeConnexionSiteWeb.Size = new System.Drawing.Size(271, 22);
            this.donnéesDeConnexionSiteWeb.Tag = "";
            this.donnéesDeConnexionSiteWeb.Text = "Données de connexion site web";
            // 
            // gestionDesDroitsDaccès
            // 
            this.gestionDesDroitsDaccès.Name = "gestionDesDroitsDaccès";
            this.gestionDesDroitsDaccès.Size = new System.Drawing.Size(271, 22);
            this.gestionDesDroitsDaccès.Tag = "";
            this.gestionDesDroitsDaccès.Text = "Gestion des droits d\'accès";
            // 
            // Administration
            // 
            this.Administration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SchoolData,
            this.formatNomDesPhotosÉtudiantes,
            this.fichierDétatsDesÉtudiants,
            this.duréeDeLaPauseAutorisantLesSorties,
            this.longueurIdentifiantRFID,
            this.gestionDesDonnéesÉlèves});
            this.Administration.Name = "Administration";
            this.Administration.Size = new System.Drawing.Size(88, 20);
            this.Administration.Text = "Administratif";
            // 
            // SchoolData
            // 
            this.SchoolData.Name = "SchoolData";
            this.SchoolData.Size = new System.Drawing.Size(277, 22);
            this.SchoolData.Text = "Information sur l\'établissement";
            // 
            // formatNomDesPhotosÉtudiantes
            // 
            this.formatNomDesPhotosÉtudiantes.Name = "formatNomDesPhotosÉtudiantes";
            this.formatNomDesPhotosÉtudiantes.Size = new System.Drawing.Size(277, 22);
            this.formatNomDesPhotosÉtudiantes.Text = "Format nom des photos étudiantes";
            // 
            // fichierDétatsDesÉtudiants
            // 
            this.fichierDétatsDesÉtudiants.Name = "fichierDétatsDesÉtudiants";
            this.fichierDétatsDesÉtudiants.Size = new System.Drawing.Size(277, 22);
            this.fichierDétatsDesÉtudiants.Text = "Fichier d\'états des étudiants";
            // 
            // duréeDeLaPauseAutorisantLesSorties
            // 
            this.duréeDeLaPauseAutorisantLesSorties.Name = "duréeDeLaPauseAutorisantLesSorties";
            this.duréeDeLaPauseAutorisantLesSorties.Size = new System.Drawing.Size(277, 22);
            this.duréeDeLaPauseAutorisantLesSorties.Text = "Durée de la pause autorisant les sorties";
            // 
            // longueurIdentifiantRFID
            // 
            this.longueurIdentifiantRFID.Name = "longueurIdentifiantRFID";
            this.longueurIdentifiantRFID.Size = new System.Drawing.Size(277, 22);
            this.longueurIdentifiantRFID.Text = "Longueur identifiant RFID";
            // 
            // gestionDesDonnéesÉlèves
            // 
            this.gestionDesDonnéesÉlèves.Name = "gestionDesDonnéesÉlèves";
            this.gestionDesDonnéesÉlèves.Size = new System.Drawing.Size(277, 22);
            this.gestionDesDonnéesÉlèves.Text = "Gestion des données élèves";
            // 
            // SchoolLifeOffice
            // 
            this.SchoolLifeOffice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sonSortieNonAutorisé,
            this.motifsDinterdictionDeSortie,
            this.appelationPunitionEnPrésence,
            this.couleurPunitionNonClôturée});
            this.SchoolLifeOffice.Name = "SchoolLifeOffice";
            this.SchoolLifeOffice.ShowShortcutKeys = false;
            this.SchoolLifeOffice.Size = new System.Drawing.Size(78, 20);
            this.SchoolLifeOffice.Text = "Vie scolaire";
            // 
            // sonSortieNonAutorisé
            // 
            this.sonSortieNonAutorisé.Name = "sonSortieNonAutorisé";
            this.sonSortieNonAutorisé.Size = new System.Drawing.Size(251, 22);
            this.sonSortieNonAutorisé.Text = "Son sortie non autorisé";
            // 
            // motifsDinterdictionDeSortie
            // 
            this.motifsDinterdictionDeSortie.Name = "motifsDinterdictionDeSortie";
            this.motifsDinterdictionDeSortie.Size = new System.Drawing.Size(251, 22);
            this.motifsDinterdictionDeSortie.Text = "Motifs d\'interdiction de sortie";
            // 
            // appelationPunitionEnPrésence
            // 
            this.appelationPunitionEnPrésence.Name = "appelationPunitionEnPrésence";
            this.appelationPunitionEnPrésence.Size = new System.Drawing.Size(251, 22);
            this.appelationPunitionEnPrésence.Text = "Appelation punitions en présence";
            // 
            // couleurPunitionNonClôturée
            // 
            this.couleurPunitionNonClôturée.Name = "couleurPunitionNonClôturée";
            this.couleurPunitionNonClôturée.Size = new System.Drawing.Size(251, 22);
            this.couleurPunitionNonClôturée.Text = "Couleur punition non clôturée";
            // 
            // aideToolStripMenuItem
            // 
            this.aideToolStripMenuItem.Name = "aideToolStripMenuItem";
            this.aideToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.aideToolStripMenuItem.Text = "Aide";
            this.aideToolStripMenuItem.Click += new System.EventHandler(this.aideToolStripMenuItem_Click);
            // 
            // displayEventTextBox
            // 
            this.displayEventTextBox.Enabled = false;
            this.displayEventTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayEventTextBox.Location = new System.Drawing.Point(9, 27);
            this.displayEventTextBox.Multiline = true;
            this.displayEventTextBox.Name = "displayEventTextBox";
            this.displayEventTextBox.Size = new System.Drawing.Size(459, 53);
            this.displayEventTextBox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connexion_button);
            this.groupBox1.Controls.Add(this.password_textBox);
            this.groupBox1.Controls.Add(this.id_textBox);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(9, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 159);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Session";
            // 
            // connexion_button
            // 
            this.connexion_button.Location = new System.Drawing.Point(251, 119);
            this.connexion_button.Name = "connexion_button";
            this.connexion_button.Size = new System.Drawing.Size(89, 34);
            this.connexion_button.TabIndex = 4;
            this.connexion_button.Text = "Connexion";
            this.connexion_button.UseVisualStyleBackColor = true;
            this.connexion_button.Click += new System.EventHandler(this.connexion_button_Click);
            // 
            // password_textBox
            // 
            this.password_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_textBox.Location = new System.Drawing.Point(136, 76);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(204, 23);
            this.password_textBox.TabIndex = 3;
            this.password_textBox.UseSystemPasswordChar = true;
            this.password_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_password_KeyDown);
            // 
            // id_textBox
            // 
            this.id_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.id_textBox.Location = new System.Drawing.Point(136, 33);
            this.id_textBox.Name = "id_textBox";
            this.id_textBox.Size = new System.Drawing.Size(204, 23);
            this.id_textBox.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(24, 76);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 24);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "*Mot de passe :";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(24, 33);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 24);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "*Identifiant :";
            // 
            // disconnection_button
            // 
            this.disconnection_button.BackColor = System.Drawing.Color.LightCoral;
            this.disconnection_button.Location = new System.Drawing.Point(363, 137);
            this.disconnection_button.Name = "disconnection_button";
            this.disconnection_button.Size = new System.Drawing.Size(105, 159);
            this.disconnection_button.TabIndex = 7;
            this.disconnection_button.Text = "Déconnexion";
            this.disconnection_button.UseVisualStyleBackColor = false;
            this.disconnection_button.Click += new System.EventHandler(this.disconnection_button_Click);
            // 
            // display_session_textBox
            // 
            this.display_session_textBox.Enabled = false;
            this.display_session_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_session_textBox.Location = new System.Drawing.Point(9, 107);
            this.display_session_textBox.Multiline = true;
            this.display_session_textBox.Name = "display_session_textBox";
            this.display_session_textBox.Size = new System.Drawing.Size(346, 24);
            this.display_session_textBox.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 303);
            this.Controls.Add(this.display_session_textBox);
            this.Controls.Add(this.disconnection_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.displayEventTextBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem update;
        private System.Windows.Forms.ToolStripMenuItem Settings;
        private System.Windows.Forms.ToolStripMenuItem adresseIpDuServer;
        private System.Windows.Forms.TextBox displayEventTextBox;
        private System.Windows.Forms.ToolStripMenuItem fichierDaccèsAuServeur;
        private System.Windows.Forms.ToolStripMenuItem gestionDesAdressesMails;
        private System.Windows.Forms.ToolStripMenuItem donnéesDeConnexionSiteWeb;
        private System.Windows.Forms.ToolStripMenuItem gestionDuServiceDenvoiDeCourriel;
        private System.Windows.Forms.ToolStripMenuItem rafraichirLaBaseDeDonnées;
        private System.Windows.Forms.ToolStripMenuItem purgerLaBaseDeDonnées;
        private System.Windows.Forms.ToolStripMenuItem gestionDesDroitsDaccès;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button connexion_button;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.TextBox id_textBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button disconnection_button;
        private System.Windows.Forms.TextBox display_session_textBox;
        private System.Windows.Forms.ToolStripMenuItem Administration;
        private System.Windows.Forms.ToolStripMenuItem SchoolLifeOffice;
        private System.Windows.Forms.ToolStripMenuItem sonSortieNonAutorisé;
        private System.Windows.Forms.ToolStripMenuItem appelationPunitionEnPrésence;
        private System.Windows.Forms.ToolStripMenuItem fichierDétatsDesÉtudiants;
        private System.Windows.Forms.ToolStripMenuItem duréeDeLaPauseAutorisantLesSorties;
        private System.Windows.Forms.ToolStripMenuItem couleurPunitionNonClôturée;
        private System.Windows.Forms.ToolStripMenuItem gestionDesDonnéesÉlèves;
        private System.Windows.Forms.ToolStripMenuItem redémarrerLeServer;
        private System.Windows.Forms.ToolStripMenuItem longueurIdentifiantRFID;
        private System.Windows.Forms.ToolStripMenuItem SchoolData;
        private System.Windows.Forms.ToolStripMenuItem formatNomDesPhotosÉtudiantes;
        private System.Windows.Forms.ToolStripMenuItem nomDeDomainDuServerSQL;
        private System.Windows.Forms.ToolStripMenuItem aideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motifsDinterdictionDeSortie;
    }
}

