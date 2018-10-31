using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevelManager : MonoBehaviour {
    public GameObject[] buttonGroups;
    protected GameManager gameManager;
    protected string challengeTypeString = "ZCS";

    // Use this for initialization
    protected void Start () {
        gameManager = FindObjectOfType<GameManager>();
        SetChallengeType();
    }
	
	// Update is called once per frame
	protected void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    protected void SetChallengeType()
    {
        for (int i = 0; i < buttonGroups.Length; i++)
            buttonGroups[i].SetActive(false);

        switch (gameManager.Challenge_Type)
        {
            case ChallengeType.ZCS:
                challengeTypeString = "ZCS";
                buttonGroups[0].SetActive(true);
                break;
            case ChallengeType.BV:
                challengeTypeString = "BV";
                buttonGroups[1].SetActive(true);
                break;
            case ChallengeType.GJ:
                challengeTypeString = "GJ";
                buttonGroups[2].SetActive(true);
                break;
        }
    }

    public abstract void ReceiveLetter(string letter);
}
