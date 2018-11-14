using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="category"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
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

    public static string[] GetTextJson(string jsonName)
    {
        string[] textToReturn = new string[1];
        string text = System.IO.File.ReadAllText("Assets/Resources/StartBy/" + jsonName + ".json");
        TextObject newObject = JsonUtility.FromJson<TextObject>(text);
        //textToReturn = JsonUtility.FromJson<string[]>(text);
        textToReturn = newObject.entries;

        return textToReturn;
    }

    public static FreqWord[] GetWordsWithFreqJson(string jsonName)
    {
        FreqWord[] textToReturn = new FreqWord[1];
        string text = System.IO.File.ReadAllText("Assets/Resources/" + jsonName + ".json");
        FreqWordsObject freqWordsObject = JsonUtility.FromJson<FreqWordsObject>(text);
        //textToReturn = JsonUtility.FromJson<string[]>(text);

        textToReturn = freqWordsObject.entries;

        return textToReturn;
    }

    class TextObject
    {
        public string[] entries;
    }

    class FreqWordsObject
    {
        public FreqWord[] entries;
    }
    #endregion
}

//public static User ReadToObject(string json)
//{
//    User deserializedUser = new User();
//    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
//    DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
//    deserializedUser = ser.ReadObject(ms) as User;
//    ms.Close();
//    return deserializedUser;
//}
