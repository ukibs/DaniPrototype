using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
    public TextMesh levelText;

    private LevelDataToSave save;
    private bool state = false;
    private ChallengeType type;
    private GameMode gameMode;
    private CanvasLevels canvas;
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
        canvas = CanvasLevels.Instance;
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
    }

    void OnMouseDown()
    {
        if (state)
        {
            gameManager.Challenge_Type = type;
            gameManager.Game_Mode = gameMode;
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "Mi máximo es : " + save.maxPoints + "\n Mi minimo es: " + save.minPoints + "\n Mis puntos son: " + save.points;
        }
    }
}
