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
        C, S, Z, B, V, G, J, Count
    }

    Vector2 scrollPos;
    DialogueFile dialogueFile = new DialogueFile();
    string importFilePathLbl = string.Empty;

    int answersLeft = 0;
    int randomsLeft = 0;

    [MenuItem("Window/LibraryCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LibraryCreator));
    }

    void Init()
    {
        dialogueFile = new DialogueFile();
        importFilePathLbl = string.Empty;
        answersLeft = 0;
        randomsLeft = 0;
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(30));
        AddImportButton();
        AddStartNewDialogueButton();
        AddExportButton();
        EditorGUILayout.EndHorizontal();

        foreach (int sentenceLine in dialogueFile.Sentences.Keys)
        {
            AddDialogueLine(sentenceLine, (DialogueSentence)dialogueFile.Sentences[sentenceLine]);
        }

        answersLeft = 0;
        randomsLeft = 0;

        EditorGUILayout.EndScrollView();
    }
    
    private void AddDialogueLine(int _currentLine, DialogueSentence sentence)
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(20));

        //Dialogue line
        EditorGUILayout.LabelField(_currentLine.ToString(), GUILayout.MaxWidth(30));


        //Dialogue Text
        string typeOfDialogue = "Word: ";
        EditorGUILayout.LabelField(typeOfDialogue, GUILayout.MaxWidth(50));
        sentence.DialogueText = EditorGUILayout.TextField("", sentence.DialogueText, GUILayout.MaxWidth(300));

        //Next line
        EditorGUILayout.LabelField("Letter: ", GUILayout.MaxWidth(40));
        sentence.letter = (Letters)EditorGUILayout.EnumPopup(sentence.letter, options: GUILayout.MaxWidth(80));

        AddNewLineButton(_currentLine);

        AddRemoveLineButton(_currentLine);

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
        string importFilePath = string.Empty;

        if (GUILayout.Button("IMPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            importFilePath = EditorUtility.OpenFilePanel("filepanel", "C:\\", "");

        if (importFilePath.Length > 0)
        {
            ImportFile(ref importFilePath);
        }
    }

    private void ImportFile(ref string _importFilePath)
    {
        string tmpPath = _importFilePath;
        _importFilePath = string.Empty;
        dialogueFile = new DialogueFile();
        dialogueFile.ImportDialogueFile(tmpPath);
    }

    private void AddExportButton()
    {
        string exportFilePath = string.Empty;

        if (GUILayout.Button("EXPORT", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
            exportFilePath = EditorUtility.SaveFilePanel("filepanel", "C:\\Users", "", "");

        if (exportFilePath.Length > 0)
        {
            ExportFile(ref exportFilePath);
        }
    }

    private void ExportFile(ref string _exportFilePath)
    {
        string tmpPath = _exportFilePath;
        _exportFilePath = string.Empty;
        dialogueFile.ExportDialogueFile(tmpPath);
    }

    private void AddStartNewDialogueButton()
    {
        if (GUILayout.Button("NEW", GUILayout.MaxWidth(100), GUILayout.MaxHeight(30)))
        {
            dialogueFile = new DialogueFile();
            dialogueFile.AddDialogueSentence(1, new DialogueSentence());
            importFilePathLbl = string.Empty;
        }
    }

    private void AddNewLineButton(int _currentLine)
    {
        if (GUILayout.Button("ADD NEW Word", GUILayout.MaxWidth(100)))
            dialogueFile.AddDialogueSentence(_currentLine + 1, new DialogueSentence());
    }

    private void AddRemoveLineButton(int _currentLine)
    {
        if (GUILayout.Button("REMOVE LINE", GUILayout.MaxWidth(100)))
            dialogueFile.RemoveDialogueSentence(_currentLine);
    }
    #region Aux Classes

    private class DialogueFile
    {
        Hashtable sentences;
        public Hashtable Sentences { get { return sentences; } }

        public DialogueFile()
        {
            sentences = new Hashtable();
        }

        public void ImportDialogueFile(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                Debug.LogError(this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " ERROR: not exists file _filePath=" + _filePath);
                return;
            }

            StreamReader dialogueFile = new StreamReader(_filePath, System.Text.Encoding.UTF8);

            string[] wordsFromXml = GameFunctions.GetTextXML("ZCS", "WORDS", "word");
            string[] lettersFromXml = GameFunctions.GetTextXML("ZCS", "LETTERS", "letter");

            for (int i = 0; i < wordsFromXml.Length; i++)
            {
                sentences.Add(i, new DialogueSentence(wordsFromXml[i], lettersFromXml[i]));
            }
            
            dialogueFile.Close();
        }

        public void ExportDialogueFile(string _filePath)
        {
            using (StreamWriter sw = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("<MAIN>");
                sw.WriteLine("  <WORDS>");
                foreach (int sentenceLine in sentences.Keys)
                {
                    sw.WriteLine("    <word>" + ((DialogueSentence)sentences[sentenceLine]).DialogueText + "</word>");
                }
                sw.WriteLine("  </WORDS>");
                sw.WriteLine("  <LETTERS>");
                foreach (int sentenceLine in sentences.Keys)
                {
                    sw.WriteLine("    <letter>" + ((DialogueSentence)sentences[sentenceLine]).letter + "</letter>");
                }
                sw.WriteLine("  </LETTERS>");
                sw.WriteLine("</MAIN>");
            }
        }

        public void AddDialogueSentence(int _line, DialogueSentence _ds)
        {
            if (sentences.ContainsKey(_line))
            {
                IncreaseSentenceLineNumber(_line);
                sentences[_line] = _ds;
            }
            else
            {
                sentences.Add(_line, _ds);
            }
        }

        public void RemoveDialogueSentence(int _line)
        {
            if (sentences.ContainsKey(_line))
            {
                sentences.Remove(_line);
                DecreaseSentenceLineNumber(_line);
            }
        }

        public void ModifySentenceNextLinesByNewParameter(int _line, int _newParamenter, int _previousParameter, bool isForRandom = false)
        {
            int offset = isForRandom ? 1 : 0;
            int newParam = _newParamenter - offset;
            int prevParam = Math.Max(0, _previousParameter - offset);

            if (newParam > prevParam)
            {
                for (int i = prevParam; i < newParam; i++)
                {
                    AddDialogueSentence(_line + 1 + i, new DialogueSentence());
                }
            }
            else if (newParam < prevParam)
            {
                for (int i = prevParam; i > newParam && i > 0; i--)
                {
                    RemoveDialogueSentence(_line + i);
                }
            }
        }

        private void IncreaseSentenceLineNumber(int _line, int _toIncrease = 1)
        {
            for (int i = 1; i <= _toIncrease; i++)
            {
                if (sentences.ContainsKey(_line + i))
                {
                    IncreaseSentenceLineNumber(_line + i, _toIncrease);
                    sentences.Remove(_line + i);
                }
            }
            sentences.Add(_line + _toIncrease, sentences[_line]);
            //ChangeSentenceLineReferences(_line, _line + _toIncrease);
        }

        private void DecreaseSentenceLineNumber(int _line, int _toDecrease = 1)
        {
            for (int i = _line; i <= sentences.Count; i++)
            {
                sentences.Add(i, sentences[i + _toDecrease]);
                sentences.Remove(i + _toDecrease);
                //ChangeSentenceLineReferences(i + _toDecrease, i);
            }
        }
    }

    private class DialogueSentence
    {
        string dialogueText;
        public string DialogueText { get { return dialogueText; } set { dialogueText = value; } }

        public Letters letter;
        string nextDialogueLine;
        public string NextDialogueLine { get { return nextDialogueLine; } set { nextDialogueLine = value; } }

        public DialogueSentence() { }

        public DialogueSentence(string word, string letter)
        {
            dialogueText = word;
            nextDialogueLine = letter;
        }
    }

    #endregion
}
