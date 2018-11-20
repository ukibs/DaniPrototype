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
    private float currentTime = 0;
    private int buttonComplete;

    private float failureFactor = 2f;
    private float wordsPoints = 0;

    private float points;

    // Use this for initialization
    void Start () {
        base.Start();
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        
        if((panelB.Length == panelsBloqued || buttonComplete == panelB.Length || panelB.Length == (panelsBloqued + buttonComplete) || currentTime < 0))
        {
            pointsPanel.SetActive(true);
            totalPoints.text = CalculatePoints() + "";
            gameManager.RestTimeLastLevel = levelTime - currentTime;
            gameManager.NextLevel(points);
        }
        else if (pointsPanel.activeSelf == false)
        {
            dt += Time.deltaTime;
            currentTime = (levelTime - dt);
            timer.text = ((int)currentTime).ToString();
        }
	}

    private string CalculatePoints()
    {
        string total = "";
        float x = -(panelsBloqued * failureFactor);
        float y = levelTime / 2 + wordsPoints;
        points = x + y;
        total = "Total:  " + (x + y) + "\nFallos: " + x + "\nPalabras: " + wordsPoints + "\nTiempo: " + (levelTime/2);

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
            words.Add(new WordInfo(s, lettersFromXml[auxInt], gameManager.infoType[gameManager.Challenge_Type].Difficulty));
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
            panel.text.text = "✓";
            panel.Active = WordStates.COMPLETE;
            buttonComplete++;
        }
    }
}
