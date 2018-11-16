using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    public int level = 0;
    public TextMesh levelText;
    public ChallengeType type;
    
   
    public float points;
    public float maxPoints = 0;
    public float minPoints = 0;

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
            gameManager.SetPoints(this);
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "Mi máximo es : " + maxPoints + "\n Mi minimo es: " + minPoints;
        }
    }
}
