using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    public int level = 0;
    public TextMesh levelText;
    public ChallengeType type;
    
   
    public float points;
    public float maxPoints;
    public float minPoints;

    private GameMode gameMode;
    private GameObject enterLevel;
    private GameManager gameManager;
    private bool state = false;

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

    public void UpdateData(GameMode mode, ChallengeType type, bool state = false, int level = 0)
    {
        Mode = mode;
        this.type = type;
        this.state = state;
        this.level = level;
        levelText.text = level + "";
        if(!state) levelText.color = Color.red;
    }

    void OnMouseDown()
    {
        if (state)
        {
            gameManager.Challenge_Type = type;
            gameManager.Game_Mode = gameMode;
            enterLevel.SetActive(true);
        }
    }
}
