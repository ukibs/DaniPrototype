using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqWord {

    public string word;
    public string frequency;
    public char keyLetter;

    public FreqWord() { }

    //public FreqWord(string word, string frequency)
    //{
    //    this.word = word;
    //    this.frequency = frequency;
    //}

    public FreqWord(string word, char keyLetter)
    {
        this.word = word;
        this.keyLetter = keyLetter;
    }
}
