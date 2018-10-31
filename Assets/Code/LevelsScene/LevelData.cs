using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    public int level = 0;
    public ChallengeType type;
    private GameMode gameMode;
    public float points;
    private GameObject enterLevel;

    private GameManager gameManager;

    public GameMode Mode
    {
        set
        {
            gameMode = value;
        }
    }

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;
        enterLevel = CanvasLevels.Instance.enterLevelPanel;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        gameManager.Challenge_Type = type;
        gameManager.Game_Mode = gameMode;
        enterLevel.SetActive(true);
    }
}
