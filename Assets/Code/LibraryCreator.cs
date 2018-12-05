using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
public class LibraryCreator : EditorWindow
{
    private enum Letters
    {
        C, S, Z, B, V, G, J
    }

    private enum DifficultyCriteria
    {
        Invalid = -1,

        NumLetters,
        HowMuchUsed,
        Frequency,
        //Syllabes,

        Count
    }

    Vector2 scrollPos;
    DictionaryFile dialogueFile = new DictionaryFile();
    string importFilePathLbl = string.Empty;

    //
    private int maxWords = 0;
    private string lettersToUse = "";
    private bool[] difficultyCriterias;   // TODO: Apply this with textboxes
    private bool showLines = false;

    [MenuItem("Window/LibraryCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LibraryCreator));
    }

    void Init()
    {
        dialogueFile = new DictionaryFile();
        importFilePathLbl = string.Empty;
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(30));
        AddImportButton();
        AddImportAndExportButton();
        AddExportButton();
        AddMaxWordsField();
        EditorGUILayout.EndHorizontal();

        if (showLines == true)
        {
            foreach (int sentenceLine in dialogueFile.Lines.Keys)
            {
                AddLine(sentenceLine, (WordLine)dialogueFile.Lines[sentenceLine]);
            }
        }
        EditorGUILayout.EndScrollView();
    }
    
    private void AddLine(int _currentLine, WordLine newLine)
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(20));

        //Dialogue line
        EditorGUILayout.LabelField(_currentLine.ToString(), GUILayout.MaxWidth(30));

        //Dialogue Text
        string typeOfDialogue = "Word: ";
        EditorGUILayout.LabelField(typeOfDialogue, GUILayout.MaxWidth(50));
        newLine.Word = EditorGUILayout.TextField("", newLine.Word, GUILayout.MaxWidth(300));

        // NOTA: De querer mantenerlo podemos hacer que genere el ENUM al cargar las palabras

        //Letter to skip
        //EditorGUILayout.LabelField("Letter: ", GUILayout.MaxWidth(40));
        //newLine.letter = (Letters)EditorGUILayout.EnumPopup(newLine.letter, options: GUILayout.MaxWidth(80));

        //Points
        EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(40));
        newLine.Value = EditorGUILayout.IntSlider(newLine.Value, 1, 100, GUILayout.MaxWidth(300));

        //AddRemoveWordButton(_currentLine);

        EditorGUILayout.EndHorizontal();
        AddVerticalSeparator();
        EditorGUILayout.EndVertical();
    }

    private void AddHorizontalSeparator()
    {
        EditorGUILayout.LabelField("      ", GUILayout.MaxWidth(40));
    }

    private void AddVerticalSeparator()
    {
        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------", GUILayout.MaxHeight(20));
    }

    private void AddImportButton()
    {
        if (GUILayout.Button("IMPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            ImportFile();
    }

    private void ImportFile()
    {
        dialogueFile = new DictionaryFile();
        dialogueFile.ImportFile(maxWords, lettersToUse);
        showLines = true;
    }

    private void AddExportButton()
    {
        if (GUILayout.Button("EXPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            ExportFile();
    }
    
    private void ExportFile()
    {
        // Llamaremos al nuevo
        dialogueFile.ExportFile(lettersToUse);
    }

    private void AddImportAndExportButton()
    {
        if (GUILayout.Button("IMPORT & EXPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            ImportAndExportFile();
    }

    private void ImportAndExportFile()
    {
        //
        showLines = false;
        // Import the data
        dialogueFile = new DictionaryFile();
        dialogueFile.ImportFile(maxWords, lettersToUse);
        // And export the files
        dialogueFile.ExportFile(lettersToUse);
    }

    // De momento metemos aqui maximo de palabras y letras a usar
    private void AddMaxWordsField()
    {
        // Se usa antes de importar
        GUILayout.Label("Max words:", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        maxWords = Int32.Parse(EditorGUILayout.TextField(maxWords.ToString(), GUILayout.MaxWidth(100), GUILayout.MaxHeight(20)));

        // Este en principio también
        // TODO: Hacerlo
        GUILayout.Label("Letters to use:", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        lettersToUse = EditorGUILayout.TextField(lettersToUse, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));

        //
        //GUILayoutOption[] options = new GUILayoutOption[1];
        //options[1] = new GUILayoutOption
        //difficultyCriterias[0] = (DifficultyCriteria)EditorGUILayout.SelectableLabel(difficultyCriterias[0], options: GUILayout.MaxWidth(80));

        //(Letters)EditorGUILayout.EnumPopup(newLine.letter, options: GUILayout.MaxWidth(80));
    }

    private void AddStartNewDialogueButton()
    {
        if (GUILayout.Button("NEW", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
        {
            dialogueFile = new DictionaryFile();
            dialogueFile.AddNewWord(1, new WordLine());
            importFilePathLbl = string.Empty;
        }
    }

    #region Aux Classes

    private class DictionaryFile
    {
        Hashtable lines;
        private bool[] difficultyCriterias;

        //private int maxWords;

        public Hashtable Lines { get { return lines; } }

        public DictionaryFile()
        {
            lines = new Hashtable();
        }

        //public void ImportFile(string _filePath)
        public void ImportFile(int _numWords, string _lettersToUse)
        {
            // TODO: Poner aqui para coger los nuevos objetos
            // Poner un tope de palabras que queremos coger (para aligerar cargas)
            FreqWord[] freqWords = GameFunctions.GetWordsAndFreqsJson(_numWords, _lettersToUse);
            

            for (int i = 0; i < freqWords.Length; i++)
            {
                WordLine nextWord = new WordLine(freqWords[i].word);
                nextWord.charLetter = freqWords[i].keyLetter;
                nextWord.Value = SetWordDifficulty(difficultyCriterias, nextWord.Word, Int32.Parse(freqWords[i].frequency));
                lines.Add(i, nextWord);
            }

            //dictionaryFile.Close();
        }

        //
        public int SetWordDifficulty(bool[] difficultyCriterias, string word, int frequency)
        {
            int finalValue = 0;

            // TODO: Revisar como manejamos esto
            // if (difficultyCriterias[(int)DifficultyCriteria.NumLetters]) {}
            finalValue += word.Length;

            // TODO: Revisar como manejamos esto
            // if (difficultyCriterias[(int)DifficultyCriteria.Frequency]) {}
            if (frequency > 14000)  // +- 1000 mas comunes
                finalValue += 1;
            else if (frequency > 30000)   // +- 5000 mas comunes
                finalValue += 2;
            else if (frequency > 1200)      // +- 10000 mas comunes
                finalValue += 3;
            else
                finalValue += 4;

            return finalValue;
        }

        //
        public int GetMaxDifficulty()
        {
            int maxDifficulty = 0;

            for(int i = 0; i < lines.Count; i++)
            {
                if( ((WordLine)lines[i]).Value > maxDifficulty)
                {
                    maxDifficulty = ((WordLine)lines[i]).Value;
                }
            }

            return maxDifficulty;
        }

        // The old one with a xml
        // public void ExportFile(string _filePath)
        public void ExportFile(string _lettersToUse)
        {
            //
            int maxDifficulty = GetMaxDifficulty();
            //Debug.Log(maxDifficulty);
            //List<FreqWord>[] wordLists = new List<FreqWord>[maxDifficulty];
            List<string>[] wordLists = new List<string>[maxDifficulty];
            for (int i = 0; i < maxDifficulty; i++)
                wordLists[i] = new List<string>();
            //
            for (int i = 0; i < lines.Count; i++)
            {
                WordLine nextWord = (WordLine)lines[i];
                //FreqWord newFreqWord = new FreqWord(nextWord.Word, nextWord.charLetter);
                string wordWithLetter = nextWord.Word + nextWord.charLetter;
                wordLists[nextWord.Value - 1].Add(wordWithLetter);
            }
            //
            TextObject[] textObjects = new TextObject[maxDifficulty];
            //FreqWordsObject[] textObjects = new FreqWordsObject[maxDifficulty];
            for (int i = 0; i < maxDifficulty; i++)
            {
                textObjects[i] = new TextObject();
                textObjects[i].entries = wordLists[i].ToArray();
            }
            //
            if(!AssetDatabase.IsValidFolder("Assets/Resources/" + _lettersToUse))
            {
                string guid = AssetDatabase.CreateFolder("Assets/Resources", _lettersToUse);
                string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
            }            
            //
            for (int i = 0; i < maxDifficulty; i++)
            {
                string fileName = _lettersToUse + i.ToString();
                //
                string jsonList = JsonUtility.ToJson(textObjects[i]);
                //
                string path = Application.dataPath + "/Resources/" + _lettersToUse + "/" + fileName + ".json";
                //Debug.Log(path);
                Debug.Log(jsonList);
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(jsonList);
                }
            }
            
        }

        public void AddNewWord(int _line, WordLine _ds)
        {
            if (lines.ContainsKey(_line))
            {
                IncreaseSentenceLineNumber(_line);
                lines[_line] = _ds;
            }
            else
            {
                lines.Add(_line, _ds);
            }
        }

        public void RemoveDialogueSentence(int _line)
        {
            if (lines.ContainsKey(_line))
            {
                lines.Remove(_line);
                DecreaseSentenceLineNumber(_line);
            }
        }

        private void IncreaseSentenceLineNumber(int _line, int _toIncrease = 1)
        {
            for (int i = 1; i <= _toIncrease; i++)
            {
                if (lines.ContainsKey(_line + i))
                {
                    IncreaseSentenceLineNumber(_line + i, _toIncrease);
                    lines.Remove(_line + i);
                }
            }
            lines.Add(_line + _toIncrease, lines[_line]);
            //ChangeSentenceLineReferences(_line, _line + _toIncrease);
        }

        private void DecreaseSentenceLineNumber(int _line, int _toDecrease = 1)
        {
            for (int i = _line; i <= lines.Count; i++)
            {
                lines.Add(i, lines[i + _toDecrease]);
                lines.Remove(i + _toDecrease);
                //ChangeSentenceLineReferences(i + _toDecrease, i);
            }
        }
    }

    private class WordLine
    {
        string word;
        public string Word { get { return word; } set { word = value; } }

        public Letters letter;
        public Letters Letter { get { return letter; } set { letter = value; } }

        public char charLetter;
        public char CharLetter { get { return charLetter; } set { charLetter = value; } }

        private int wordValue;
        public int Value { get { return wordValue; } set { wordValue = value; } } 

        public WordLine() { }

        public WordLine(string word, string l, int value)
        {
            this.word = word;
            letter = (Letters)Enum.Parse(typeof(Letters), l);
            wordValue = value;
        }

        public WordLine(string word)
        {
            this.word = word;
            // freq
        }
    }

    #endregion
}

#endif
