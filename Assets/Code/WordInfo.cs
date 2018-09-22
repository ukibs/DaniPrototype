using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInfo: MonoBehaviour {
    string word;
    string letter;

	public string Word
    {
        get { return word; }
        set { word = value;}
    }

    public string Letter
    {
        get { return letter; }
        set { letter = value;
            GetComponent<TextMesh>().text = word.Replace(letter.ToCharArray()[0], '_'); ;
        }
    }
}
