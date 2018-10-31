using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour {

    public GameObject prefabLevel;
    public GameObject prefabSpecialLevel;
    public float distanceBetweenLevels = 3;
    public int currentLevel = 0;

	// Use this for initialization
	void Start () {
        InitLevels();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitLevels()
    {
        CreateSection(ChallengeType.ZCS, 0);
        CreateSection(ChallengeType.GJ, 5);
        CreateSection(ChallengeType.BV, -5);
    }

    private void CreateSection(ChallengeType type, int posX)
    {
        GameObject aux;
        int i;
        for (i = currentLevel; i < 10; ++i)
        {
            aux = Instantiate(prefabLevel, new Vector3(posX, i * distanceBetweenLevels, 0), prefabLevel.transform.rotation);
            aux.GetComponent<LevelData>().Mode = GameMode.Type2;
            aux.GetComponent<LevelData>().type = type;
        }
        aux = Instantiate(prefabSpecialLevel, new Vector3(0, i * distanceBetweenLevels, 0), prefabLevel.transform.rotation);
        aux.GetComponent<LevelData>().Mode = GameMode.Type1;
    }
}
