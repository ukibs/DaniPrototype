using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<string> wordList;
    public List<string> missingCharList;

    private int wordIndex = 0;
    private string currentWord;

	// Use this for initialization
	void Start () {
        currentWord = wordList[wordIndex].Replace(missingCharList[wordIndex].ToCharArray()[0], '_');
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2 - 100, Screen.height*1/3, 200, 40), currentWord);
    }

    public void ReceiveLetter(string letter)
    {
        if (letter.Equals(missingCharList[wordIndex])){
            Debug.Log("Bien, sabes leer!!!");
            wordIndex++;
            if (wordIndex < wordList.Count)
                currentWord = wordList[wordIndex].Replace(missingCharList[wordIndex].ToCharArray()[0], '_');
            else
                SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Burro!!!");
        }
    }
}
