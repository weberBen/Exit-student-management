# Automatic checkup for daily student exit

## Motivations

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

## External provider

A lot of school uses an external provider for handling student timetable, canceled lectures and other information on students. Then the system needs to scrap all the data of this provider to populated a local database that will store the time slots when each student can exit the school. Because that part is school specific, the choice has been made to exclude the scrapping script from the main app to keep it modular as possible. The script will act on the database and then when needed the modifications will be seen by the main app which will decide according to the time and the database if a student can leave the school.

Real time modification is not possible here because of the fluctuation of the network and the stability of the provider which are not being made in response to hundreds of requests in a short period of time. Then the script will update the database in the middle of each hour and a pipe between the main application and the script will notify the main application if the update has been interrupted (because of an error, of the network,...). In that case, the main application will notify all the client to proceed manually to the checkup (to avoid having students exit the school because of an old and inaccurate information).

That script also updates "exit ban" which will override all other authorizations to leave the school for a student (or for a whole class). The configuration of the exit ban is up to the school because the script has to be rewritten for each school.

## Server side

The server responses to the main requests :

- if a student can leave the school
- add an exit ban manually (that will be stored in a different table than the ban set by the script in the database)
- add an authorization manually
- change user parameters

Because all computers in a school are locally connected the use a of server/client design has been bright to the light quickly. It allow a member of the staff to access the system anywhere in the school.

Each user has a specific role associated with rights (than can be edited in a custom way). For example, no registration is needed to check if a student can leave the school, supervisory staff can add an exit ban, chef of the supervisory staff can add a new authorization but not other members of that staff. Because in small team session are used by multiple person, the add of a ban or an authorization has to be validated by a unique 4 character password (and is always asked). In other words, each ban or authorization is associated with a unique user that is not necessarily the one connected to the opened session.

All the client and server side can be adapted to the school rules without modifying their source code.

## Database

The database has first need to be populated with the basic information on student (their first name, last name, section and their half board days). That information can be added directly through the main application from a csv file (the order of the column can be edited in the main application). Be aware that if the first and last name of students does not exactly match the ones of the external provider students will not be updated (in other words, they will always be unauthorized to leave the school until the official end of the school or if they are allowed to leave the school for lunch).

For each student a unique id is associated and can/has to be manually added.

## Client

No need to install a software, the clients just access the server through a web browser.


# Real word application

<p align="center">
<img src="/assets/technical_solution_id.png?" alt="Real world implementation of the unique id" width="450"/>
</p>

Since each student owns a liaison dairy the unique id will be added to that diary through an RFID (as hardware read only to avoid modification of the data by a student) sticker.

<p align="center">
<img src="/assets/diary_rfid.jpg?" alt="RFID stciker on a diary" width="400"/>
</p>


The front end is composed of a TV that will display a photo of the student, his first and last name, his section, and the result of the request "can he leaves the school" (notice that in case of an unauthorized exit a relatively loud song is played, and can be custom).

<p align="center">
<img src="/assets/front_end.jpg?" alt="Overview of the front end" width="700"/>
</p>


<p align="center">
<img src="/assets/exit_client.jpg?" alt="Zoom in the request result" width="550"/>
</p>

The default ban reasons are set through the main application


<p align="center">
<img src="/assets/ban_client.png?" alt="Ban page on a client" width="550"/>
</p>

The timetable of the authorization section is dynamically created based on the hours given to the main application

<p align="center">
<img src="/assets/authorization_client.png?" alt="Authorization page on a client" width="550"/>
</p>

Finally a log file can be viewed and download for each student/section

<p align="center">
<img src="/assets/journal_client.png?" alt="Log page on a client" width="550"/>
</p>

All the modification (ban/authorization) can be made for a single student or for the whole section

<p align="center">
<img src="/assets/search_client.png?" alt="Search page on a client" width="550"/>
</p>

# Application spécification

## Server side

### Main application

Windows app (*C#*)

#### Requirements :

* Microsoft .NET Framework 4.5.1 ou greater (x86 or x64)
* SQL Server 2012 Express LocalDB

### Web srcapping app

Python 3 script

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
