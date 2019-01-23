using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {
    public TextMesh levelText;
    public Image stars;

    private LevelDataToSave save;
    public bool state = false;
    private ChallengeType type;
    public GameMode gameMode;
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
        save = new LevelDataToSave();
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
        int rand = Random.Range(0, 2);
        if (rand % 2 == 0)
        {
            canvas.tip.SetActive(true);
            canvas.tipText.text = Tips.Instance.GetTip(type);
        }
        if (state)
        {
            if(gameMode == GameMode.Type2)
            {
                gameManager.ModeSelected.CurrentLevel = save.level;
            }
            gameManager.Challenge_Type = type;
            gameManager.Game_Mode = gameMode;
            canvas.enterLevelPanel.SetActive(true);
            canvas.startText.text = "" + save.points;
            canvas.starsLevel.fillAmount = 1 - (save.maxPoints - save.points >= 0 ? save.maxPoints - save.points + save.minPoints : 0) / save.maxPoints;
        }
    }
}
