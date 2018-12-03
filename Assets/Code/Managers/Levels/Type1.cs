using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Type1 : BaseLevelManager {

    public GameObject wordPrefab;
    public float timeWait = 2.0f;

    private List<WordInfo> wordObjects;
    private Queue<WordInfo> wordsInScreen;
    private float timer;
    //private int currentWordIndex = 0;
    private int totalScore = 0;

    // Use this for initialization
    protected void Start()
    {
        base.Start();
        
        GetAndCreateWords();
    }

    // Update is called once per frame
    protected void Update()
    {
        timer += Time.deltaTime;
        //foreach (WordInfo t in wordsInScreen)
        //{
        //    t.transform.position += new Vector3(0, -1 * Time.deltaTime, 0);
        //}
        if (timer > timeWait)
        {
            timer = 0;
            AddWordInScreen();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void GetAndCreateWords()
    {
        //string[] wordsFromXml = GameFunctions.GetTextXML(challengeTypeString, "WORDS", "word");
        //string[] lettersFromXml = GameFunctions.GetTextXML(challengeTypeString, "LETTERS", "letter");

        char[] lettersToFilter = challengeTypeString.ToCharArray();
        string[] lettersToFilterString = new string[lettersToFilter.Length];
        for (int i = 0; i < lettersToFilterString.Length; i++)
            lettersToFilterString[i] = lettersToFilter[i].ToString();
        Debug.Log(lettersToFilterString[0]);
        //WordsWithLetters wordsWithLetters = GetWords(lettersToFilterString);
        //WordsWithLetters wordsWithLetters = GetWords2(lettersToFilterString, (int)gameManager.infoType[ChallengeType.ZCS].Difficulty);
        WordsWithLetters wordsWithLetters = GetWords2(lettersToFilterString, 10);

        string[] wordsToUse = wordsWithLetters.words;

        string[] lettersToUse = wordsWithLetters.letters;

        int auxInt = 0;
        wordObjects = new List<WordInfo>();
        wordsInScreen = new Queue<WordInfo>(4);
        foreach (string s in wordsToUse)
        {
            WordInfo aux = Instantiate(wordPrefab).GetComponent<WordInfo>();
            aux.Word = s;
            aux.Letter = lettersToUse[auxInt];
            aux.GetComponent<TextMesh>().text = aux.Word;
            aux.gameObject.SetActive(false);
            wordObjects.Add(aux);
            auxInt++;
        }

        AddWordInScreen();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="letter"></param>
    public override void ReceiveLetter(string letter)
    {
        if (letter.Equals(wordsInScreen.Peek().Letter))
        {
            Debug.Log("Bien, sabes leer!!!");
            if (wordsInScreen.Count > 1 || wordObjects.Count != 0)
            {
                WordInfo nextWord = wordsInScreen.Dequeue();
                totalScore += nextWord.Points;
                Debug.Log(totalScore);
                //nextWord.gameObject.SetActive(false);
                nextWord.Resolve();
            }
            else
                SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Burro!!!");
            totalScore -= 5;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void AddWordInScreen()
    {
        int indexToGet = Random.Range(0, wordObjects.Count - 1);
        WordInfo addText = wordObjects[indexToGet];
        wordObjects.RemoveAt(indexToGet);
        addText.gameObject.SetActive(true);
        wordsInScreen.Enqueue(addText);
    }
}
