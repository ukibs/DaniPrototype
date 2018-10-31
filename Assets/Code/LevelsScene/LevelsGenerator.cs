using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour {

    public GameObject prefabLevel;
    public GameObject prefabSpecialLevel;
    public float distanceBetweenLevels = 3;
    public int currentLevel = 0;

    private int currentY = 0;

	// Use this for initialization
	void Start () {
        InitLevels();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitLevels()
    {
        for (int i = 0; i <= (currentLevel / 10); ++i)
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
            aux = Instantiate(prefabLevel, new Vector3(0, currentY), prefabLevel.transform.rotation);
            aux.GetComponent<LevelData>().Mode = GameMode.Type2;
            aux.GetComponent<LevelData>().type = ChallengeType.ZCS;

            aux = Instantiate(prefabLevel, new Vector3(5, currentY), prefabLevel.transform.rotation);
            aux.GetComponent<LevelData>().Mode = GameMode.Type2;
            aux.GetComponent<LevelData>().type = ChallengeType.BV;

            aux = Instantiate(prefabLevel, new Vector3(-5, currentY), prefabLevel.transform.rotation);
            aux.GetComponent<LevelData>().Mode = GameMode.Type2;
            aux.GetComponent<LevelData>().type = ChallengeType.GJ;

            currentY += (int)distanceBetweenLevels;
            
        }
        aux = Instantiate(prefabSpecialLevel, new Vector3(0, currentY, 0), prefabLevel.transform.rotation);
        aux.GetComponent<LevelData>().Mode = GameMode.Type1;
        currentY += (int)distanceBetweenLevels;
    }
}
