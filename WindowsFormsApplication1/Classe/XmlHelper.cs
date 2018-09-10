using System;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;


public static class XmlHelper
{
    /*This class contains methodes to change value of an existing XMl element in an existing XML file
     * The storage form of data is indicate in the Class ReglageAdresseIP in ToolsClass
    */



 
    public static void changeXMLElement(string path_to_file, string tag_to_to_find, string attribute, string attribute_value, Object content)
    {
        
        /*
         * Change the value of the XML element defined by its attribute (attribute=name) equal to attribute_value
         * in the file located in path_to_file. The actual value of the file is replaced by the object value
        */
        try
        {
            int res = 0;

            XDocument doc = XDocument.Load(path_to_file);

            /*<field <-- descendant
             *  name="Ip_adress" <--attribute
             *  "Ip_adress"="192.168.1."> <--value
             *</field> 
            */

            foreach (XElement element in doc.Descendants(tag_to_to_find)) //find all the XML elements that have the descendant "field"
            {
                if (element.Attribute(attribute).Value == attribute_value) //check for the element that have attribute equals to attribute_value 
                {
                    
                    element.SetAttributeValue(attribute_value, ""+content);
                    
                    res = 1;

                }
            }

            doc.Save(path_to_file); //save all the modification by erasing the ancient XML file

            if (res == 0) //can't find the corresponding XML element
            {
                Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n";
                Error.error = "CFAIXMLF";
            }
        }
        catch (Exception e) //can't find the XML file
        {
            Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n" + e;
            Error.error = "CLXMLF";
        }

    }



    public static string readXMLElement(string path_to_file, string tag_to_to_find, string attribute, string attribute_value)
    {
        /*
         * Read the value of the XML element defined by its attribute (attribute=name) equal to attribute_value
         * in the file located in path_to_file. 
         * 
         * If the function can't read the value, return null.
         * 
        */

        try
        {
            int res = 0;
            string value = "";

            XDocument doc = XDocument.Load(path_to_file);

            /*<field <-- descendant
             *  name="Ip_adress" <--attribute
             *  "Ip_adress"="192.168.1."> <--value
             *</field> 
            */

            foreach (XElement element in doc.Descendants(tag_to_to_find)) //find all the XML elements that have the descendant "descendant"
            {
                if (element.Attribute(attribute).Value == attribute_value) //check for the element that have attribute equals to attribute_value 
                {
                    value = element.Attribute(attribute_value).Value; //read the actual value

                    res = 1;

                }
            }

            if (res == 0) //can't find the corresponding XML element
            {
                Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n";
                Error.error = "CFAIXMLF";
                return null;
            }

            return value;
        }
        catch (Exception e) //can't find the XML file
        {
            Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n" + e;
            Error.error = "CLXMLF";
            return "";
        }


    }


    public static void changeXmlListElement<T>(string path_to_file, string tag_to_to_find, string attribute, string attribute_value, 
                                              string tag_element_list, List<T> list, string attribute_element_list="", List<string> list_attribute_element_value=null)
    {
        /*Replace all the children of an element by the element of the given list List<T>
         * 
         * The element is defined by its XML tag "tag_to_to_find" ("div", "span", "item" ...) and by the value "attribute_value" 
         * of its attribute ("name", "style", "class", ...)
         * 
         * All the children are defined by their XML tag "tag_element_list" and by their inside value given by each item of the list "list"
         * 
         * This method is a generic one to allow user to use generic List<> (for example List<string>, List<int>, List<object>, ...)
         * 
         *  For example, if : 
         *      tag_to_to_find = "div"
         *      attribute = "name"
         *      attribute_value = "name_of_div"
         *      tag_element_list = "item"
         *      list = {"item1", "item2", "item3"}
         *      
         * Then the methode change the current Xml element "<div name="name_of_div"></div>" from the file located in "path_to_file" by :
         * 
         * "<div name="name_of_div">
         *      <item>item1</item>
         *      <item>item2</item>
         *      <item>item3</item>
         * </div>"
         * 
         * if the variable "attribute_element_list" is non null then the user can set an the value of the XML attribute "attribute_element_list" 
         * for each items in the list "list". Those values are saved in the list "list_attribute_element_value" which must have the same
         * length as the list "list"
         * 
        */


        try
        {
            int res = 0;

            XmlDocument doc = new XmlDocument();
            doc.Load(path_to_file);


            bool is_attribute_element_to_set; //boolean that help to know if the user wants to set attribute for each item of the list "list"
            if (attribute_element_list != "" && list_attribute_element_value != null)
            {
                is_attribute_element_to_set = true;
            }
            else
            {
                is_attribute_element_to_set = false;
            }

            foreach (XmlElement element in doc.GetElementsByTagName(tag_to_to_find)) //find all the XML elements that have the descendant "tag_to_to_find"
            {
                if (element.GetAttribute(attribute) == attribute_value) //check for the element that have attribute equals to attribute_value 
                {
                    element.RemoveAll(); //also remove the name of the element
                    element.SetAttribute(attribute, attribute_value); //set the name again


                    for (int i=0; i<list.Count;i++)
                    {

                        XmlElement item = doc.CreateElement(tag_element_list); //create all the children of the given element

                        if(is_attribute_element_to_set)
                        {
                            item.SetAttribute(attribute_element_list, list_attribute_element_value[i]);
                        }

                        item.InnerText = ""+ list[i]; //set the value of each children

                        element.AppendChild(item); //add the child to the given element as descendant
                    }

                    res = 1;

                }
            }

            doc.Save(path_to_file); //save all the modification by erasing the ancient XML file


            if (res == 0) //can't find the corresponding XML element
            {
                Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n";
                Error.error = "CFAIXMLF";
            }

        }
        catch (Exception e) //can't find the XML file
        {
            Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n" + e;
            Error.error = "CLXMLF";
        }


    }



    public static List<string> readXmlListElement(string path_to_file, string tag_to_to_find, string attribute, string attribute_value, 
                            string tag_element_list, string attribute_element_list = "", List<string> list_attribute_element_value = null)
    {
        /*read all the value of the children of an element
         * 
         * The element is defined by its XML tag "tag_to_to_find" ("div", "span", "item" ...) and by the value "attribute_value" 
         * of its attribute ("name", "style", "class", ...)
         * 
         * All the children are defined by their XML tag "tag_element_list" and by their inside value 
         * 
         *  For example, if : 
         *      tag_to_to_find = "div"
         *      attribute = "name"
         *      attribute_value = "name_of_div"
         *      tag_element_list = "item"
         *      
         * Then the current Xml element loaded from the file located in "path_to_file" is  :
         * 
         * "<div name="name_of_div">
         *      <item>item1</item>
         *      <item>item2</item>
         *      <item>item3</item>
         * </div>"
         * 
         * So, the method retrun a list made of these elements child like : {"item1","item2","item3"}
         * Else return null
         * 
         * If the variable "attribute_element_list" is non null the user want to get the child item in a specific order
         * given throught the list "list_attribute_element_value". That list contains all the value of the list items attributes in 
         * the desired order. So the function return a list with order in the same way as the list "list_attribute_element_value"
         * 
        */


        try
        {
            int res = 0;
            List<string> list = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path_to_file);

            bool is_attribute_element_to_get; //boolean that help to know if the user wants to get attributes for each items of the list
            if (attribute_element_list != "" && list_attribute_element_value != null)
            {
                is_attribute_element_to_get = true;
            }
            else
            {
                is_attribute_element_to_get = false;
            }


            foreach (XmlElement element in doc.GetElementsByTagName(tag_to_to_find)) //find all the XML elements that have the descendant "tag_to_to_find"
            {
                if (element.GetAttribute(attribute) == attribute_value) //check for the element that have attribute equals to attribute_value 
                {
                    
                    
                    foreach (XmlElement child in element.GetElementsByTagName(tag_element_list))
                    {
                       

                        if (is_attribute_element_to_get)
                        {
                            int index = list_attribute_element_value.IndexOf(child.GetAttribute(attribute_element_list));
                            if (index != -1)
                            {
                                list_attribute_element_value[index] = child.InnerText; //ordering in the desired order
                            }else
                            {
                                Error.details = "L'attribut recherché était : " + attribute_element_list+ "\nayant pour valeur :"+ child.GetAttribute(attribute_element_list);
                                Error.error = "CFAXMLALE";
                                return null;
                            }
                        }else
                        {
                            list.Add(child.InnerText);
                        }
                    }


                    res = 1;

                }
            }

            

            if (res == 0) //can't find the corresponding XML element
            {
                Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n";
                Error.error = "CFAIXMLF";
                return null;
            }

            if (is_attribute_element_to_get)
            {
                return list_attribute_element_value;
            }
            else
            {
                return list;
            }

        }
        catch (Exception e) //can't find the XML file
        {
            Error.details = "Chemin vers le fichier XML : " + path_to_file + "\nElement cherché : name=" + attribute_value + "\n" + e;
            Error.error = "CLXMLF";
            return null;
        }



    }






}
 
 
 