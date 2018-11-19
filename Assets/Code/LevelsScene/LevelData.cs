using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : LevelDataToSave {

    public TextMesh levelText;
    public ChallengeType type;

    private GameMode gameMode;
    private CanvasLevels canvas;
    private GameManager gameManager;
    public bool state = false;

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
        canvas = CanvasLevels.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateData(GameMode mode, ChallengeType type, bool state, int level, LevelDataToSave save = null)
    {
        Mode = mode;
        this.type = type;
        this.state = state;
        this.level = level;
        levelText.text = level + "";
        if(!state) levelText.color = Color.red;

        if(save != null)
        {
            points = save.points;
            maxPoints = save.maxPoints;
            minPoints = save.minPoints;
        }
    }

    void OnMouseDown()
    {
        if (state)
        {
            gameManager.Challenge_Type = type;
            gameManager.Game_Mode = gameMode;
            gameManager.SetPoints(this);
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "Mi máximo es : " + maxPoints + "\n Mi minimo es: " + minPoints + "\nMi dificultad es " + gameManager.infoType[gameManager.Challenge_Type].Difficulty;
        }
    }
}
