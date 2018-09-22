using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class GameFunctions
{

    #region Public Attributes
    #endregion

    #region Private Attributes
    #endregion

    #region MonoDevelop Methods
    #endregion

    #region User Methods
    //
    public static string[] GetTextXML(string name, string category, string tag)
    {
        string[] textToReturn;
        XmlDocument xml_d;
        XmlNode objectToUse;
        XmlNodeList xmlDescription;
        TextAsset textasset = (TextAsset)Resources.Load(name, typeof(TextAsset));
        xml_d = new XmlDocument();
        xml_d.LoadXml(textasset.text);
        //Search if it is the correct ID or name
        string route = "MAIN/" + category;
        objectToUse = xml_d.SelectSingleNode(route);
        if (objectToUse != null)
        {
            //Debug.Log(objectToUse);
            xmlDescription = ((XmlElement)objectToUse).GetElementsByTagName(tag);
            textToReturn = new string[xmlDescription.Count];
            int j = 0;
            foreach (XmlNode node in xmlDescription)
            {
                textToReturn[j] = node.InnerText;
                j++;
            }
            return textToReturn;
        }
        return null;
    }
    #endregion
}
