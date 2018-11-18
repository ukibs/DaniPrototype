using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Reflection;
using System;

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
    private DifficultyCriteria[] difficultyCriterias;   // TODO: Apply this with textboxes

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
        AddExportButton();
        AddMaxWordsField();
        EditorGUILayout.EndHorizontal();

        foreach (int sentenceLine in dialogueFile.Lines.Keys)
        {
            AddLine(sentenceLine, (WordLine)dialogueFile.Lines[sentenceLine]);
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
        //EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(40));
        //newLine.Value = EditorGUILayout.IntSlider(newLine.Value, 1, 100, GUILayout.MaxWidth(300));

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
        //string importFilePath = string.Empty;

        if (GUILayout.Button("IMPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            //importFilePath = EditorUtility.OpenFilePanel("filepanel", "C:\\Users\\USUARIO\\Documents\\InfiniteGames\\InGitHub\\Educacion\\Assets\\Resources", "");
            ImportFile();

        //if (importFilePath.Length > 0)
        //{
        //    ImportFile(ref importFilePath);
        //}
    }

    //private void ImportFile(ref string _importFilePath)
    private void ImportFile()
    {
        //string tmpPath = _importFilePath;
        //_importFilePath = string.Empty;
        dialogueFile = new DictionaryFile();
        //dialogueFile.ImportFile(tmpPath);
        dialogueFile.ImportFile(maxWords, lettersToUse);
    }

    private void AddExportButton()
    {
        string exportFilePath = string.Empty;

        if (GUILayout.Button("EXPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            exportFilePath = EditorUtility.SaveFilePanel("filepanel", "C:\\Users\\USUARIO\\Documents\\InfiniteGames\\InGitHub\\Educacion\\Assets\\Resources", "", "");

        if (exportFilePath.Length > 0)
        {
            ExportFile(ref exportFilePath);
        }
    }

    private void ExportFile(ref string _exportFilePath)
    {
        string tmpPath = _exportFilePath;
        _exportFilePath = string.Empty;
        // Llamaremos al nuevo
        dialogueFile.ExportFiles(tmpPath);
    }

    // De momento metemos aqui maximo de palabras y letras a usar
    private void AddMaxWordsField()
    {
        // Se usa antes de importar
        GUILayout.Label("Max words:", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        maxWords = Int32.Parse(EditorGUILayout.TextField(maxWords.ToString(), GUILayout.MaxWidth(100), GUILayout.MaxHeight(20)));
        //if (maxWords > 0) Debug.Log(maxWords);
        // Este en principio también
        // TODO: Hacerlo
        GUILayout.Label("Letters to use:", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
        lettersToUse = EditorGUILayout.TextField(lettersToUse, GUILayout.MaxWidth(100), GUILayout.MaxHeight(20));
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

    //private void AddNewLineButton()
    //{
    //    if (GUILayout.Button("ADD NEW Word", GUILayout.MaxWidth(100)))
    //        dialogueFile.AddNewWord(0, new WordLine());
    //}

    //private void AddRemoveWordButton(int _currentLine)
    //{
    //    if (GUILayout.Button("REMOVE LINE", GUILayout.MaxWidth(100)))
    //        dialogueFile.RemoveDialogueSentence(_currentLine);
    //}

    #region Aux Classes

    private class DictionaryFile
    {
        Hashtable lines;
        //private int maxWords;

        public Hashtable Lines { get { return lines; } }

        public DictionaryFile()
        {
            lines = new Hashtable();
        }

        //public void ImportFile(string _filePath)
        public void ImportFile(int _numWords, string _lettersToUse)
        {
            //if (!File.Exists(_filePath))
            //{
            //    Debug.LogError(this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " ERROR: not exists file _filePath=" + _filePath);
            //    return;
            //}

            //StreamReader dictionaryFile = new StreamReader(_filePath, System.Text.Encoding.UTF8);

            //string[] wordsFromXml = GameFunctions.GetTextXML("ZCS", "WORDS", "word");
            //string[] lettersFromXml = GameFunctions.GetTextXML("ZCS", "LETTERS", "letter");
            //string[] valuesFromXml = GameFunctions.GetTextXML("ZCS", "VALUE", "value");

            // TODO: Poner aqui para coger los nuevos objetos
            // Poner un tope de palabras que queremos coger (para aligerar cargas)
            FreqWord[] freqWords = GameFunctions.GetWordsAndFreqsJson(_numWords, _lettersToUse);

            //for (int i = 0; i < wordsFromXml.Length; i++)
            //{
            //    lines.Add(i, new WordLine(wordsFromXml[i], lettersFromXml[i], Int32.Parse(valuesFromXml[i])));
            //}

            for (int i = 0; i < freqWords.Length; i++)
            {
                lines.Add(i, new WordLine(freqWords[i].word));
            }

            //dictionaryFile.Close();
        }

        // The old one with a xml
        public void ExportFile(string _filePath)
        {
            using (StreamWriter sw = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("<MAIN>");
                sw.WriteLine("  <WORDS>");
                foreach (int sentenceLine in lines.Keys)
                {
                    sw.WriteLine("    <word>" + ((WordLine)lines[sentenceLine]).Word + "</word>");
                }
                sw.WriteLine("  </WORDS>");
                sw.WriteLine("  <LETTERS>");
                foreach (int sentenceLine in lines.Keys)
                {
                    sw.WriteLine("    <letter>" + ((WordLine)lines[sentenceLine]).letter + "</letter>");
                }
                sw.WriteLine("  </LETTERS>");
                sw.WriteLine("  <VALUE>");
                foreach (int sentenceLine in lines.Keys)
                {
                    sw.WriteLine("    <value>" + ((WordLine)lines[sentenceLine]).Value + "</value>");
                }
                sw.WriteLine("  </VALUE>");
                sw.WriteLine("</MAIN>");
            }
        }

        // New one with various jsons
        public void ExportFiles(string _filePath)
        {
            // for
            using (StreamWriter sw = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8))
            {

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
