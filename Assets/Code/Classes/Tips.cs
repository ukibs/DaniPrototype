using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : Singleton<Tips> {

    public Dictionary<ChallengeType, List<string>> dicTips = new Dictionary<ChallengeType, List<string>>();

    private void Start()
    {
        for (int i = 0; i < (int)ChallengeType.Count; i++)
        {
            dicTips.Add((ChallengeType)i, new List<string>());
        }

        dicTips[ChallengeType.ZCS].Add("eeeeo");
        dicTips[ChallengeType.ZCS].Add("uuuuo");
    }

    public string GetTip(ChallengeType ct)
    {
        int rand = Random.Range(0, dicTips[ct].Count);
        return dicTips[ct][rand];
    }
}
