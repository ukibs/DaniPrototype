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
    //fails
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
        if (gameManager.bonusList[0].active) { levelTime += 5; gameManager.bonusList[0].amount--; gameManager.bonusList[0].active = false; }
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        
        if((panelB.Length == panelsBloqued || buttonComplete == panelB.Length || panelB.Length == (panelsBloqued + buttonComplete) || currentTime < 0) && pointsPanel.activeSelf == false)
        {
            pointsPanel.SetActive(true);
            totalPoints.text = CalculatePoints() + "";
            gameManager.RestTimeLastLevel = currentTime;
            gameManager.NextLevel(points>=0?points:0);
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
        float y = (currentTime) / 2 + wordsPoints;
        points = x + y;
        total = "Total:  " + (x + y) + "\nFallos: " + x + "\nPalabras: " + wordsPoints + "\nTiempo: " + (currentTime / 2);

        return total;
    }

    private void Init()
    {
        char[] lettersToFilter = challengeTypeString.ToCharArray();
        string[] lettersToFilterString = new string[lettersToFilter.Length];
        for (int i = 0; i < lettersToFilterString.Length; i++)
            lettersToFilterString[i] = lettersToFilter[i].ToString();
        
        WordsWithLetters wordsWithLetters = GetWords2(lettersToFilterString, (int)gameManager.LevelSelectedData.difficulty);
       
        string[] wordsFromXml = wordsWithLetters.words;
        string[] lettersFromXml = wordsWithLetters.letters;

        int auxInt = 0;
        words = new List<WordInfo>();
        foreach (string s in wordsFromXml)
        {
            if (auxInt < gameManager.LevelSelectedData.amountWords)
            {
                words.Add(new WordInfo(s, lettersFromXml[auxInt], gameManager.LevelSelectedData.difficulty));
                auxInt++;
            }
            else break;
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
            // TODO: Revisar palabras con mayúscula
            if (selectedButton.info.Letter == letter.ToLower())
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
