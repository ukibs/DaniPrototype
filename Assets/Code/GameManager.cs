using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject wordPrefab;
    public float timeWait = 2.0f;

    private Queue<WordInfo> wordObjects;
    private Queue<WordInfo> wordsInScreen;
    private float timer;

	// Use this for initialization
	void Start () {
        string[] wordsFromXml = GameFunctions.GetTextXML("ZCS", "WORDS", "word");
        string[] lettersFromXml = GameFunctions.GetTextXML("ZCS", "LETTERS", "letter");

        int auxInt = 0;
        wordObjects = new Queue<WordInfo>();
        wordsInScreen = new Queue<WordInfo>(4);
        foreach(string s in wordsFromXml)
        {
            WordInfo aux = Instantiate(wordPrefab).GetComponent<WordInfo>();
            aux.Word = s;
            aux.Letter = lettersFromXml[auxInt];
            aux.gameObject.SetActive(false);
            wordObjects.Enqueue(aux);
            auxInt++;
        }

        AddWordInScreen();
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

    public void ReceiveLetter(string letter)
    {
        if (letter.Equals(wordsInScreen.Peek().Letter)){
            Debug.Log("Bien, sabes leer!!!");
            if (wordsInScreen.Count > 1 || wordObjects.Count != 0)
            {
                wordsInScreen.Dequeue().gameObject.SetActive(false);
            }
            else
                SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Burro!!!");
        }
    }

    private void AddWordInScreen()
    {
        WordInfo addText = wordObjects.Dequeue().GetComponent<WordInfo>();
        addText.gameObject.SetActive(true);
        wordsInScreen.Enqueue(addText);
    }
}
