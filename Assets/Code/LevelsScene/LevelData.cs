using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
    public LevelDataToSave save;

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

    public void UpdateData(GameMode mode, ChallengeType type, bool state, int level, ref LevelDataToSave saveData)
    {
        save = saveData;
        Mode = mode;
        this.type = type;
        this.state = state;
        save.level = level;
        if (!state) levelText.color = Color.red;
        levelText.text = save.level + "";

        /*if (saveData != null)
        {
            save.points = saveData.points;
            save.maxPoints = saveData.maxPoints;
            save.minPoints = saveData.minPoints;
        }*/
    }

    void OnMouseDown()
    {
        if (state)
        {
            gameManager.Challenge_Type = type;
            gameManager.Game_Mode = gameMode;
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "Mi máximo es : " + save.maxPoints + "\n Mi minimo es: " + save.minPoints;
        }
    }
}
