using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tips : Singleton<Tips> {

    public Dictionary<ChallengeType, List<string>> dicTips = new Dictionary<ChallengeType, List<string>>();

    private void Start()
    {
        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            //
            string lettersFileName = ((ChallengeType)i).ToString();
            //TextAsset lettersFile = Resources.Load<TextAsset>("tips / tips" + lettersFileName + ".json");
            StreamReader lettersFileReader = new StreamReader("Assets/Resources/tips/tips" + lettersFileName + ".json");

            string stringJsonTypes = lettersFileReader.ReadToEnd();
                //System.IO.File.ReadAllText("Assets/Resources/tips/tips" + lettersFileName + ".json");
            TipsContainer tips = JsonUtility.FromJson<TipsContainer>(stringJsonTypes);

            List<string> tipsList = new List<string>(tips.tips);
            dicTips.Add((ChallengeType)i, tipsList);
            
        }
        
    }

    public string GetTip(ChallengeType ct)
    {
        int rand = Random.Range(0, dicTips[ct].Count);
        return dicTips[ct][rand];
    }
}

public class TipsContainer
{
    public string[] tips;
}
