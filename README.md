# Automatic checkup for daily student exit

## Intoduction

When middle-school (and sometimes high-school) students go out of the school supervisory staff need to check their authorizations to leave the school (for example during the lunch or when a professor is absent). At rush hours hundreds of students can leave the school at the same time. Then the staff needs to speed up the checking to reduce the flow of student. 

At these times, mistakes can easily be done and the job of the staff gets harder. Moreover, only a few members of the staff are trained for the checking, all the administrative staff (which for small structure are important figures for students) can not proceed to the checkup because of its relative complexity (lot of information rely on the comprehension of the supervisory staff).

And the tracking of student punishment is really hard to sustain. Each day staff need to check if students of the last day have done their punishment, if not they have to get them during the lectures, which is often not possible. And students can in general not catch at the exit of the school because the staff need to exactly know the student. It ends up with a list of students that have not done their punishment reported from day to day by the staff give up on them in order to not accumulate the delay.

For all of its reason a modular system which can be adapted to the rules of a specific structure and that will do the checkup (under the supervision of a human) is a good point: no more training is needed to do the checkup because all the procedure will end up with a simple message "Authorized" or "Unauthorized" and for the specific cases student just has to be oriented to a trained person).

### The checkup procedure

The procedure is made up of two components: 

- Verify if the student has no lectures at that time (that imply dynamically checking canceled lectures)
- Verify if the student has been authorized by its legal representative to leave the school in case of a canceled lecture
- Verify if the student has been punished and then need to stay at school or if he skip a previous punishment
- Verify other custom rules that are school specific

# The main characteristic

<p align="center">
<img src="/assets/system_design.png?" alt="Overview of the system design" width="550"/>
</p>

## Web scraping

Lot of school use an external provider for handeling student timetable and cancled lectures, then the system 







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
