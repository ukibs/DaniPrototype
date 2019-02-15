using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour {

    public GameObject prefabLevel;
    public GameObject prefabSpecialLevel;
    public float distanceBetweenLevels = 3;

    private GameManager gameManager;
    private int currentY = 0;
    private int currentX = -5;
    private int section = 0;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;
        InitLevels();
    }

    private void InitLevels()
    {
        for (int i = 0; i <= (gameManager.MaxLevel / 10); ++i)
        {
            CreateSection();
        }
    }

    private void CreateSection()
    {
        GameObject aux;
        int i;
        for (i = 0; i < 10; ++i)
        {
            for(int j = 0; j < (int)ChallengeType.Count; j++)
            {
                NewLevel((ChallengeType) j, currentX, section * 10 + i);
                currentX += 5;
            }
            currentX = -5;

            currentY += (int)distanceBetweenLevels;
            
        }

        aux = Instantiate(prefabSpecialLevel, new Vector3(0, currentY, 0), prefabLevel.transform.rotation);
        aux.GetComponent<LevelData>().Mode = GameMode.Type1;
        // Nivel especial
        if (gameManager.infoType[ChallengeType.BV].maxLevel >= 9 &&
            gameManager.infoType[ChallengeType.ZCS].maxLevel >= 9 &&
            gameManager.infoType[ChallengeType.GJ].maxLevel >= 9 )
        {
            aux.GetComponent<LevelData>().state = true;
        }
        //
        currentY += (int)distanceBetweenLevels;
        section++;
    }

    public void NewLevel(ChallengeType ct, float xPos, int level)
    {
        GameObject aux;
        bool auxState;
        //Create the level
        aux = Instantiate(prefabLevel, new Vector3(xPos, currentY), prefabLevel.transform.rotation);
        //Check if it has to be available
        auxState = (gameManager.infoType[ct].maxLevel >= level);

        if (gameManager.infoType[ct].levels.Count <= level)
        {
            gameManager.infoType[ct].levels.Add(new LevelDataToSave());
            LevelDataToSave toSave = gameManager.infoType[ct].levels[level];
            gameManager.SetPoints(ref toSave, ct);
        }

        LevelDataToSave dataToSave = gameManager.infoType[ct].levels[level];
        aux.GetComponent<LevelData>().UpdateData(GameMode.Type2, ct, auxState, level, ref dataToSave);
    }
}
