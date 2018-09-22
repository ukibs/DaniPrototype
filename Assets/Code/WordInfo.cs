using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInfo: MonoBehaviour {
    string word;
    string letter;

    int points = 10;
    bool solved = false;

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

    private void Update()
    {
        if (!solved)
        {
            transform.position += new Vector3(0, -1 * Time.deltaTime, 0);
        }
        else
        {
            transform.position += new Vector3(50 * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        points -= 5;
    }

    private void OnTriggerExit(Collider other)
    {
        points -= 5;
    }

    public void Resolve()
    {
        solved = true;
    }
}
