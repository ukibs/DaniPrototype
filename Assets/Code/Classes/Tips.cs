using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : Singleton<Tips> {

    public Dictionary<ChallengeType, List<string>> dicTips = new Dictionary<ChallengeType, List<string>>();

    private void Start()
    {
        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            //
            string lettersFile = ((ChallengeType)i).ToString();
            string stringJsonTypes = System.IO.File.ReadAllText("Assets/Resources/tips/tips" + lettersFile + ".json");
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
