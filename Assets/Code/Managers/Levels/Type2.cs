using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Type2 : BaseLevelManager {

    public PanelWord[] panelB;
    public PanelWord selectedButton;
    public Text timer;

    private List<WordInfo> words;
    private int panelsBloqued;
    private float dt;
    private float levelTime = 30;


    // Use this for initialization
    void Start () {
        base.Start();
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        dt += Time.deltaTime;
        timer.text = ((int)(levelTime - dt)).ToString();
        if(panelB.Length == panelsBloqued)
        {
            Init();
        }
	}

    private void Init()
    {
        string[] wordsFromXml = GameFunctions.GetTextXML(challengeTypeString, "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML(challengeTypeString, "LETTERS", "letter");

        int auxInt = 0;
        words = new List<WordInfo>();
        foreach (string s in wordsFromXml)
        {
            words.Add(new WordInfo(s, lettersFromXml[auxInt]));
            auxInt++;
        }

        foreach (PanelWord pw in panelB)
        {
            int random = Random.Range(0, words.Count - 1);
            pw.info = words[random];
            words.RemoveAt(random);
            pw.text.text = pw.info.Word;
            pw.Active = true;
        }
        panelsBloqued = 0;
    }

    public override void ReceiveLetter(string letter)
    {
        if (selectedButton != null)
        {
            if (selectedButton.info.Letter == letter)
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
                panelsBloqued++;
            }
            selectedButton = null;
        }
    }

}
