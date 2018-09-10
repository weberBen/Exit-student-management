--------------------------------- Pr�ambule ---------------------------------

Cette application permet de contr�ler la sortie des �l�ves en dehors d'un �tablissement. Pour ce faire, chaque �l�ve dispose d'un identifiant RFID int�gr� dans son carnet, pr�alablement enregistr�  sur cette application. Lorsque l'identifiant de l'�l�ve sera reconnu, l'application comparera son emploi du temps habituel, son r�gime de demi-pension, d'�ventuelles autorisations de sortie ou interdictions de sortie, les punitions � faire et celles non faites, pour en d�duire s'il peut sortir de l'�tablissement. 
L'application se d�compose en deux sous partie :
	- le cot� serveur (application Windows) permettant la manipulation d'une base de donn�es locale (et externe si besoin est)
	- le cot� client (page web) permettant d'interagir avec le serveur sur certaines fonctionnalit�s.

L'utilisation de l'application est r�gie par la licence de logiciel libre CeCILL et par des conditions d'utilisations annexes (disponible dans la documentation fournie).
Toutes les �tapes d'installation et de fonctionnement de l'application sont d�taill�es dans la documentation.


--------------------------------- Installation ---------------------------------

Avant de pouvoir utiliser le logiciel, il est n�cessaire d'installer les packages suivant :

	* Microsoft .NET Framework 4.5.1 ou sup�rieur (x86 ou x64 selon votre syst�me
	* La derni�re version de Microsoft Visual C++ - si besoin mettre � jour (x86 ou x64 selon votre syst�me)
Si aucun server SQL n'est install� sur votre machine :
	* SQL Server 2012 Express ou sup�rieur
		* S�lectionnez l'installation automatique (stand alone installation) d'un serveur SQL
		* Lors de l'installation ne pas oublier de s�lectionner le package "local database engine"
		* Conservez le nom de domaine de votre server SQL
	* SQL Server 2012 Express LocalDB ou sup�rieur (si vous n'avez pas pu l'installer avec SQL server 2012 Express).


--------------------------------- R�glages ---------------------------------

Une fois ces logiciels install�s il est n�cessaire de configurer l'application :

- Onglet R�glages :
	* Veuillez renseigner le nom de domaine de votre serveur SQL utilis� par la base de donn�es de l'application "Database_Students_Management"
Si vous ne connaissez pas le nom de votre serveur, utiliser le gestionnaire de configuration SQL et cliquer sur l'instance de serveur pr�sente (le nom du serveur est alors celui renseign� entre parenth�ses). Si vous avez choisi de laisser les param�tres par d�faut alors le nom de votre serveur est "(LocalDB)\MSSQLLocalDB" 
	* Il faut �galement sp�cifier l'adresse IP du serveur, ainsi que son port de communication (les r�seaux wifi sont � proscrire puisque le serveur communique avec les utilisateurs selon le protocole non s�curis� HTTP)
	* Il faut ensuite sp�cifier le chemin d'acc�s pour le fichier serveur
	* Par d�faut l'utilisateur ayant comme identifiant "admin" et comme mot de passe "admin" � tous les droits sur l'application. Il est obligatoire de changer cet identifiant et ce mot de passe � l'aide du gestionnaire des droits d'acc�s.


- Onglet Aide : ouvre l'explorateur de fichier vers le dossier Aide de l'application, dossier contenant de nombreuses informations sur son fonctionnement.
	
	
A noter qu'il faut obligatoirement se connecter avec un identifiant et un mot de passe pour acc�der aux onglets de l'application, sauf celui relatif au nom de domaine du serveur SQL.
