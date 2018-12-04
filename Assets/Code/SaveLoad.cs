using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class SaveLoad : Singleton<SaveLoad>
{
    Dictionary<ChallengeType, FileStream> files = new Dictionary<ChallengeType, FileStream>();
    string path;

    void Awake()
    {
        base.Awake();

        //Set Manager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
	// Use this for initialization
	void Start () {
        path = Application.persistentDataPath + "pruebasJson";
        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            files.Add((ChallengeType)i, new FileStream(path+i+".json", FileMode.Append));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddData(ChallengeType ct, LevelDataToSave l)
    {
        string text = JsonUtility.ToJson(l);
        byte[] vs = new UTF8Encoding(true).GetBytes(text);
        files[ct].Write(vs, 0, text.Length);
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            ChallengeType ct = ((ChallengeType)i);
            files[ct].Close();
        }
    }
}
