using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Type2 : BaseLevelManager {

    public PanelWord[] panelB;
    public PanelWord selectedButton;
    public Text timer;
    public GameObject pointsPanel;
    public Text totalPoints;

    private List<WordInfo> words;
    private int panelsBloqued;
    private float dt;
    private float levelTime = 30;
    private int buttonComplete;

    private float failureFactor = 0.5f;
    private float wordsPoints = 0;

    // Use this for initialization
    void Start () {
        base.Start();
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        
        if(panelB.Length == panelsBloqued || buttonComplete == panelB.Length)
        {
            pointsPanel.SetActive(true);
            totalPoints.text = CalculatePoints() + "";
        }
        else
        {
            dt += Time.deltaTime;
            timer.text = ((int)(levelTime - dt)).ToString();
        }
	}

    private float CalculatePoints()
    {
        float total = 0;

        total -= (panelsBloqued * failureFactor);

        total += levelTime + wordsPoints;

        return total;
    }

    private void Init()
    {
        string[] wordsFromXml = GameFunctions.GetTextXML(challengeTypeString, "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML(challengeTypeString, "LETTERS", "letter");

        int auxInt = 0;
        words = new List<WordInfo>();
        foreach (string s in wordsFromXml)
        {
            words.Add(new WordInfo(s, lettersFromXml[auxInt], Random.Range(1,10)));
            auxInt++;
        }

        foreach (PanelWord pw in panelB)
        {
            NewWordInPanel(pw);
        }
        panelsBloqued = 0;
    }

    public override void ReceiveLetter(string letter)
    {
        if (selectedButton != null)
        {
            if (selectedButton.info.Letter == letter)
            {
                NewWordInPanel(selectedButton);
                wordsPoints += selectedButton.NewWord();
            }
            else
            {
                selectedButton.Active = WordStates.BLOCK;
                panelsBloqued++;
            }
            selectedButton = null;
        }
    }

    private void NewWordInPanel(PanelWord panel)
    {
        if (words.Count != 0)
        {
            int random = Random.Range(0, words.Count - 1);
            panel.info = words[random];
            words.RemoveAt(random);
            panel.text.text = panel.info.Word;
            panel.Active = WordStates.AVAILABLE;
        }
        else
        {
            selectedButton.text.text = "✓";
            selectedButton.Active = WordStates.COMPLETE;
            buttonComplete++;
        }
    }
}
