using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInfo: MonoBehaviour {
    string word;
    string letter;

    int points = 10;

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

    public int Points { get { return points; } }

    private void OnTriggerEnter(Collider other)
    {
        points -= 5;
    }

    private void OnTriggerExit(Collider other)
    {
        points -= 5;
    }
}
