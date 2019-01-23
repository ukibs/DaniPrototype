using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsWithLetters
{
    public string[] words;
    public string[] letters;

    public WordsWithLetters(string[] preparedWords, string[] preparedLetters)
    {
        words = preparedWords;
        letters = preparedLetters;
    }

}

public abstract class BaseLevelManager : MonoBehaviour {
    public GameObject[] buttonGroups;
    protected GameManager gameManager;
    protected string challengeTypeString = "ZCS";

    // Use this for initialization
    protected void Start () {
        gameManager = FindObjectOfType<GameManager>();
        SetChallengeType();
    }
	
	// Update is called once per frame
	protected void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    protected void SetChallengeType()
    {
        for (int i = 0; i < buttonGroups.Length; i++)
            buttonGroups[i].SetActive(false);

        switch (gameManager.Challenge_Type)
        {
            case ChallengeType.ZCS:
                challengeTypeString = "ZCS";
                buttonGroups[0].SetActive(true);
                break;
            case ChallengeType.BV:
                challengeTypeString = "BV";
                buttonGroups[1].SetActive(true);
                break;
            case ChallengeType.GJ:
                challengeTypeString = "GJ";
                buttonGroups[2].SetActive(true);
                break;
        }
    }

    public abstract void ReceiveLetter(string letter);

    protected WordsWithLetters GetWords(string[] keyChars)
    {
        List<string> wordsToUse = new List<string>(10000);
        List<string> lettersToUse = new List<string>(10000);
        // Get the alphabetical lists
        for (int i = 97; i < 123; i++)
        {
            char nextChar = (char)i;
            string[] nextSubList = GameFunctions.GetTextJson(nextChar.ToString());
            // Recorring the list (nexSublist)
            for (int j = 0; j < nextSubList.Length; j++)
            {
                // Check with the key chars
                for(int k = 0; k < keyChars.Length; k++)
                {
                    
                    if (nextSubList[j].Contains(keyChars[k]) || nextSubList[j].Contains(keyChars[k].ToLower()))
                    {
                        wordsToUse.Add(nextSubList[j]);
                        lettersToUse.Add(keyChars[k]);
                    }
                }                
            }
        }

        // TODO: Añadir filtro de las palabras que puedan causar confusión
        //for(int i = 0; i < wordsToUse.Count; i++)
        //{
        //    for (int j = 1; j < wordsToUse.Count; j++)
        //    {

        //    }
        //}

        string[] preparedWords = wordsToUse.ToArray();
        string[] preparedLetters = lettersToUse.ToArray();

        WordsWithLetters wordsWithLetters = new WordsWithLetters(preparedWords, preparedLetters);

        return wordsWithLetters;
    }

    protected WordsWithLetters GetWords2(string[] keyChars, int currentLevel)
    {
        string fileName = "";
        for (int i = 0; i < keyChars.Length; i++) fileName += keyChars[i];
        fileName += "/" + fileName.ToLower() + currentLevel;

        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        //
        string jsonList = jsonFile.text;
        //
        TextObject freqWordsObject = JsonUtility.FromJson<TextObject>(jsonList);
        //Debug.Log(jsonList);

        string[] preparedWords = new string[freqWordsObject.entries.Length];
        string[] preparedLetters = new string[freqWordsObject.entries.Length];

        for (int i = 0; i < freqWordsObject.entries.Length; i++)
        {
            string unproccesedWord = freqWordsObject.entries[i];
            preparedWords[i] = unproccesedWord.Substring(0, unproccesedWord.Length - 1);
            preparedLetters[i] = unproccesedWord.Substring(unproccesedWord.Length - 1);
            //preparedLetters[i] = freqWordsObject.entries[i].keyLetter.ToString();
        }

        WordsWithLetters wordsWithLetters = new WordsWithLetters(preparedWords, preparedLetters);

        return wordsWithLetters;
    }
}
