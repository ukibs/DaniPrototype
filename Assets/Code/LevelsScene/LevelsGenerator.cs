﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour {

    public GameObject prefabLevel;
    public GameObject prefabSpecialLevel;
    public float distanceBetweenLevels = 3;

    private GameManager gameManager;
    private int currentY = 0;
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
            //Para cambiar posición en el menú, solo tocar la X que son los números mágicos que estan puestos ahora mismo (-5,0,5)
            NewLevel(ChallengeType.ZCS, 0, section * 10 + i);
            NewLevel(ChallengeType.GJ, -5, section * 10 + i);
            NewLevel(ChallengeType.BV, 5, section * 10 + i);

            currentY += (int)distanceBetweenLevels;
            
        }
        aux = Instantiate(prefabSpecialLevel, new Vector3(0, currentY, 0), prefabLevel.transform.rotation);
        aux.GetComponent<LevelData>().Mode = GameMode.Type1;
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
