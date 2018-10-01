using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Type1 : MonoBehaviour {

    public GameObject wordPrefab;
    public float timeWait = 2.0f;
    public GameObject[] buttonGroups;

    private GameManager gameManager;
    private List<WordInfo> wordObjects;
    private Queue<WordInfo> wordsInScreen;
    private float timer;
    private int currentWordIndex = 0;
    private int totalScore = 0;
    private string challengeTypeString;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetChallengeType();
        GetAndShuffleWords();
    }

    // Update is called once per frame
    void Update()
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
    void SetChallengeType()
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

    /// <summary>
    /// 
    /// </summary>
    void GetAndShuffleWords()
    {
        string[] wordsFromXml = GameFunctions.GetTextXML(challengeTypeString, "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML(challengeTypeString, "LETTERS", "letter");

        int auxInt = 0;
        wordObjects = new List<WordInfo>();
        wordsInScreen = new Queue<WordInfo>(4);
        foreach (string s in wordsFromXml)
        {
            WordInfo aux = Instantiate(wordPrefab).GetComponent<WordInfo>();
            aux.Word = s;
            aux.Letter = lettersFromXml[auxInt];
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
    public void ReceiveLetter(string letter)
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
