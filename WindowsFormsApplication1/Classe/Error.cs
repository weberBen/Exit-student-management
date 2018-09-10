using System;
using System.Windows.Forms;
using System.IO;

public class Error
{
    /*When an error occurs somewhere in the code, we change the value of the public property of that class 
     * to execute a new script
    */

    private static string s_isError="";
    private static string s_details = "";
    private static bool display_message = false;
    private static bool send_mail = false;

    public static string details
    {
        get { return s_details; }
        set{ s_details = value;}
    }


    public static string error
    {
        get { return s_isError; }
        set
        {
            s_isError = value;
            if (s_isError != "")
            {
                /*Write all error into a file*/
                string header = "\n---------------------------------------------------------------"+"\nDATE : "+DateTime.Now.ToString("dd-MM-yyyy HH:mm")+ "\n\n";
                string text = header + correspondanceError();
                string [] split_text = text.Split('\n');

                //append text to file
                using (StreamWriter stream = File.AppendText(ToolsClass.Definition.PATH_TO_LOG_ERROR_FILE))
                {
                    foreach(string line in split_text)
                    {
                        stream.WriteLine(line);
                    }
                }

                if(display_message)
                {
                    MessageBox.Show(text);
                }
            }

            display_message = false;
            send_mail = false;
        }
    }


    private static string correspondanceError()
    {
        string correspondance="Code erreur : "+ s_isError +"\nSignification : ";

        switch(s_isError)
        {
            case "NNAWIPv4AIS": //No Network Adapters With IPv4 Address In System
                correspondance += "La recherche automatique d'adresse IP pour le serveur locale n'a pas pu aboutir";
                break;
            case "CFIPAIF": //Cannot Find IP Adress In File
                correspondance += "Impossible de trouver l'adresse IP du server dans le fichier correspondant";
                break;
            case "COFFIPA": //Cannot Open File For IP Adresss
                correspondance += "Impossible d'ouvrir le fichier contenant l'adresse Ip du server";
                break;
            case "CFAIXMLF": //Cannot Find Attribute In XML File
                correspondance += "Impossible de trouver un élément d'un fichier XML";
                break;
            case "CLXMLF": //Cannot Load XML File
                correspondance += "Impossible d'ouvrir un fichier XML";
                break;
            case "FTLLSBRAWAP": //Fail To Lunch Local Server Because Run App Without Administrator Privileges
                correspondance += "L'application a été lancé sans les droits d'administrateur nécessaire au lancement du serveur local";
                break;
            case "FTSLS": //Fail To Start Local
                correspondance += "Impossible de lancer le serveur même avec les droits d'administrateur";
                break;
            case "CCLS": //Fail To Start Local
                correspondance += "Impossible de fermer le server";
                break;
            case "CGPTFFFE": //Cannot Get Path To File From File Explorer
                correspondance += "Impossible de lire le fichier sélectionné";
                break;
            case "CWIFFURLS": //Cannot Write Into File For URL Server
                correspondance += "Impossible d'écrire dans le fichier correspondant l'URL d'accès au server local";
                break;
            case "TTLMIOTS": //Try To Launch Multiple Instances Of The Server
                correspondance += "Impossible de lancer plusieurs instances du serveur en même temps !";
                break;
            case "CGCOCR": //Cannot Get Context Of the Client Request
                correspondance += "Impossible d'obtenir les informations d'une requête web faite par un ordinateur client";
                break;
            case "CCTLS": //Cannot Close The Local Server
                correspondance += "Impossible de fermer le serveur";
                break;
            case "CFAXMLALE": //Cannot Find An XML Attribute of an List Element
                correspondance += "Impossible de trouver l'attribut souhaité de l'élément d'une liste lors de la lecture d'un fichier XML";
                break;
            case "CSDCWTF": //Cannot Save Data Connexion to Website To File
                correspondance += "Impossible de sauvegarder les données de connexion au site web";
                break;
            case "CRDCTWFF": //Cannot Read Data Connexion to Website From File
                correspondance += "Impossible de lire les données de connexion au site web";
                break;
            case "CSM": //Cannot Send Mail
                correspondance += "Envoi de courriers électroniques impossible";
                break;
            case "CODB": //Cannot Open DataBase
                correspondance += "Impossible d'ouvrir la base de données contenant toutes les informations sur les élèves";
                break;
            case "DBNOFER": //Data Base is Not Opened For Executing Request
                correspondance += "Impossible d'envoyer une requête à la base de données contenant les informations sur les élèves car celle-ci ne semble pas être active";
                break;
            case "CERTDB": //Cannot Execute Request To DataBase
                correspondance += "Une requête pour la base de données contenant les informations des élèves n'a pas pu aboutir";
                break;
            case "CSDFSSFTDB": //Cannot Save Data From Students State File To DataBase
                correspondance += "Impossible de sauvegarder les données des fiches élèves";
                break;
            case "CCF": //Cannot Copy File
                correspondance += "Impossible de copier le fichier";
                break;
            case "CDF": //Cannot Delete File
                correspondance += "Impossible de supprimer le fichier";
                break;
            case "CATSSF": //Cannot Access to Student State File
                correspondance += "Impossible de lire le fichier contenant les informations des élèves";
                break;
            case "FTANRFIDTDB": //Fail To Add New RFID To DataBase
                correspondance += "Le fichier contenant les identifiants RFID n'est pas conforme";
                break;
            case "NERFIDIDB": //No Enough RFID Into DataBase
                correspondance += "Le fichier contenant les identifiants RFID n'est pas conforme";
                break;
            case "CATRFIDF": //Cannot Access To RFID File
                correspondance += "Impossible d'accéder au fichier contenant les identifiants RFID";
                break;
            case "EWCOD"://Error when collecting online data
                correspondance += "Une erreur est survenue lors de la collecte des données en ligne";
                break;
            case "ICRWS"://Invalide Client Request on the Web Server
                correspondance += "Une erreur lors d'une requête client est survenue";
                break;
            default:
                correspondance += "Erreur non répertoriée";
                break;
        }

        correspondance += "\nDétails : " + s_details;
        return correspondance;
    }


}

