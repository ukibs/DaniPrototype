using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject wordPrefab;
    public float timeWait = 2.0f;

    private List<WordInfo> wordObjects;
    private Queue<WordInfo> wordsInScreen;
    private float timer;
    private int currentWordIndex = 0;
    private int totalScore = 0;

	// Use this for initialization
	void Start () {
        GetAndShuffleWords();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        foreach (WordInfo t in wordsInScreen)
        {
            t.transform.position += new Vector3(0, -1 * Time.deltaTime, 0);
        }
        if(timer > timeWait)
        {
            timer = 0;
            AddWordInScreen();
        }
	}

    /// <summary>
    /// 
    /// </summary>
    void GetAndShuffleWords()
    {
        string[] wordsFromXml = GameFunctions.GetTextXML("ZCS", "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML("ZCS", "LETTERS", "letter");

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
        if (letter.Equals(wordsInScreen.Peek().Letter)){
            Debug.Log("Bien, sabes leer!!!");
            if (wordsInScreen.Count > 1 || wordObjects.Count != 0)
            {
                WordInfo nextWord = wordsInScreen.Dequeue();
                totalScore += nextWord.Points;
                Debug.Log(totalScore);
                nextWord.gameObject.SetActive(false);
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
        int indexToGet = Random.Range(0, wordObjects.Count-1);
        WordInfo addText = wordObjects[indexToGet];
        wordObjects.RemoveAt(indexToGet);
        addText.gameObject.SetActive(true);
        wordsInScreen.Enqueue(addText);
    }
}
