using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Type2 : BaseLevelManager {

    public PanelWord[] panelB;

    public PanelWord selectedButton;
    private List<WordInfo> words;


    // Use this for initialization
    void Start () {
        base.Start();
        string[] wordsFromXml = GameFunctions.GetTextXML(challengeTypeString, "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML(challengeTypeString, "LETTERS", "letter");

        int auxInt = 0;
        words = new List<WordInfo>();
        foreach (string s in wordsFromXml)
        {
            words.Add(new WordInfo(s, lettersFromXml[auxInt]));
            auxInt++;
        }

        foreach(PanelWord pw in panelB)
        {
            int random = Random.Range(0, words.Count - 1);
            pw.info = words[random];
            words.RemoveAt(random);
            pw.text.text = pw.info.Word;
        }
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}


    public override void ReceiveLetter(string letter)
    {
        if(selectedButton != null && selectedButton.info.Letter == letter)
        {
            //new word
            if (words.Count != 0)
            {
                int random = Random.Range(0, words.Count - 1);
                selectedButton.info = words[random];
                words.RemoveAt(random);
                selectedButton.text.text = selectedButton.info.Word;
            }
            else
            {
                selectedButton.text.text = "";
            }
        }
        else
        {
            selectedButton.Active = false;
        }
        selectedButton = null;
    }

}
