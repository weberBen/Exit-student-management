--------------------------------- Préambule ---------------------------------

Cette application permet de contrôler la sortie des élèves en dehors d'un établissement. Pour ce faire, chaque élève dispose d'un identifiant RFID intégré dans son carnet, préalablement enregistré  sur cette application. Lorsque l'identifiant de l'élève sera reconnu, l'application comparera son emploi du temps habituel, son régime de demi-pension, d'éventuelles autorisations de sortie ou interdictions de sortie, les punitions à faire et celles non faites, pour en déduire s'il peut sortir de l'établissement. 
L'application se décompose en deux sous partie :
	- le coté serveur (application Windows) permettant la manipulation d'une base de données locale (et externe si besoin est)
	- le coté client (page web) permettant d'interagir avec le serveur sur certaines fonctionnalités.

L'utilisation de l'application est régie par la licence de logiciel libre CeCILL et par des conditions d'utilisations annexes (disponible dans la documentation fournie).
Toutes les étapes d'installation et de fonctionnement de l'application sont détaillées dans la documentation.


--------------------------------- Installation ---------------------------------

Avant de pouvoir utiliser le logiciel, il est nécessaire d'installer les packages suivant :

	* Microsoft .NET Framework 4.5.1 ou supérieur (x86 ou x64 selon votre système
	* La dernière version de Microsoft Visual C++ - si besoin mettre à jour (x86 ou x64 selon votre système)
Si aucun server SQL n'est installé sur votre machine :
	* SQL Server 2012 Express ou supérieur
		* Sélectionnez l'installation automatique (stand alone installation) d'un serveur SQL
		* Lors de l'installation ne pas oublier de sélectionner le package "local database engine"
		* Conservez le nom de domaine de votre server SQL
	* SQL Server 2012 Express LocalDB ou supérieur (si vous n'avez pas pu l'installer avec SQL server 2012 Express).


--------------------------------- Réglages ---------------------------------

Une fois ces logiciels installés il est nécessaire de configurer l'application :

- Onglet Réglages :
	* Veuillez renseigner le nom de domaine de votre serveur SQL utilisé par la base de données de l'application "Database_Students_Management"
Si vous ne connaissez pas le nom de votre serveur, utiliser le gestionnaire de configuration SQL et cliquer sur l'instance de serveur présente (le nom du serveur est alors celui renseigné entre parenthèses). Si vous avez choisi de laisser les paramètres par défaut alors le nom de votre serveur est "(LocalDB)\MSSQLLocalDB" 
	* Il faut également spécifier l'adresse IP du serveur, ainsi que son port de communication (les réseaux wifi sont à proscrire puisque le serveur communique avec les utilisateurs selon le protocole non sécurisé HTTP)
	* Il faut ensuite spécifier le chemin d'accès pour le fichier serveur
	* Par défaut l'utilisateur ayant comme identifiant "admin" et comme mot de passe "admin" à tous les droits sur l'application. Il est obligatoire de changer cet identifiant et ce mot de passe à l'aide du gestionnaire des droits d'accès.


- Onglet Aide : ouvre l'explorateur de fichier vers le dossier Aide de l'application, dossier contenant de nombreuses informations sur son fonctionnement.
	
	
A noter qu'il faut obligatoirement se connecter avec un identifiant et un mot de passe pour accéder aux onglets de l'application, sauf celui relatif au nom de domaine du serveur SQL.
