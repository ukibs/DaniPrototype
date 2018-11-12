using System.Collections;
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
	
	// Update is called once per frame
	void Update () {
		
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
        bool auxState;
        for (i = 0; i < 10; ++i)
        {
            aux = Instantiate(prefabLevel, new Vector3(0, currentY), prefabLevel.transform.rotation);
            auxState = (gameManager.ZCSCurrentLevel > section * 10 + i);
            aux.GetComponent<LevelData>().UpdateData(GameMode.Type2, ChallengeType.ZCS, auxState, section * 10 + i);

            aux = Instantiate(prefabLevel, new Vector3(5, currentY), prefabLevel.transform.rotation);
            auxState = (gameManager.BVCurrentLevel > section * 10 + i);
            aux.GetComponent<LevelData>().UpdateData(GameMode.Type2, ChallengeType.BV, auxState, section * 10 + i);

            aux = Instantiate(prefabLevel, new Vector3(-5, currentY), prefabLevel.transform.rotation);
            auxState = (gameManager.GJCurrentLevel > section * 10 + i);
            aux.GetComponent<LevelData>().UpdateData(GameMode.Type2, ChallengeType.GJ, auxState, section * 10 + i);

            currentY += (int)distanceBetweenLevels;
            
        }
        aux = Instantiate(prefabSpecialLevel, new Vector3(0, currentY, 0), prefabLevel.transform.rotation);
        aux.GetComponent<LevelData>().Mode = GameMode.Type1;
        currentY += (int)distanceBetweenLevels;
        section++;
    }
}
