using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;


public class SpeechRecognitionEngine : MonoBehaviour
{
    //public string[] keywords = new string[] { "tree", "three", "left", "ass" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public Text results;
    //public Image target;

    protected PhraseRecognizer recognizer;
    //protected KeywordRecognizer
    protected string word = "derecha";

    private void Start()
    {
        // Json testing
        //string[] testText = GameFunctions.GetTextJson("t");
        List<string> wordsToUse = new List<string>();
        for(int i = 97; i < 123; i++)
        {
            char nextChar = (char)i;
            string[] nextSubList = GameFunctions.GetTextJson(nextChar.ToString());
            for(int j = 0; j < nextSubList.Length; j++)
            {
                wordsToUse.Add(nextSubList[j]);
            }
        }

        string[] testText = wordsToUse.ToArray();

        //if (keywords != null)
        //{
            recognizer = new KeywordRecognizer(testText, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        //}
        
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    private void Update()
    {
        //var x = target.transform.position.x;
        //var y = target.transform.position.y;

        //switch (word)
        //{
        //    case "arriba":
        //        y += speed;
        //        break;
        //    case "abajo":
        //        y -= speed;
        //        break;
        //    case "izquierda":
        //        x -= speed;
        //        break;
        //    case "derecha":
        //        x += speed;
        //        break;
        //}

        //target.transform.position = new Vector3(x, y, 0);
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}

#endif