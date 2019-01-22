using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {
    public TextMesh levelText;
    public Image stars;

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
        stars.fillAmount = 1 - (save.maxPoints - save.points >= 0 ? save.maxPoints - save.points + save.minPoints : 0) / save.maxPoints;
    }

    void OnMouseDown()
    {
        if (state)
        {
            gameManager.Challenge_Type = type;
            gameManager.ModeSelected.CurrentLevel = save.level;
            gameManager.Game_Mode = gameMode;
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "Mi máximo es : " + save.maxPoints + "\n Mi minimo es: " + save.minPoints + "\n Mis puntos son: " + save.points;
        }
    }
}
