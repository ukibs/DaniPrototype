using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<string> wordList;
    public List<string> missingCharList;
    public TextMesh word;

    private int wordIndex = 0;
    private string currentWord;

	// Use this for initialization
	void Start () {
        currentWord = wordList[wordIndex].Replace(missingCharList[wordIndex].ToCharArray()[0], '_');
        word.text = currentWord;
	}
	
	// Update is called once per frame
	void Update () {
        word.transform.position += new Vector3(0, -1*Time.deltaTime, 0);
	}

    public void ReceiveLetter(string letter)
    {
        if (letter.Equals(missingCharList[wordIndex])){
            Debug.Log("Bien, sabes leer!!!");
            wordIndex++;
            if (wordIndex < wordList.Count)
            {
                currentWord = wordList[wordIndex].Replace(missingCharList[wordIndex].ToCharArray()[0], '_');
                word.text = currentWord;
            }
            else
                SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Burro!!!");
        }
    }
}
