using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    public int level = 0;
    public ChallengeType type;
    public float points;
    public GameObject enterLevel;

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        gameManager.Challenge_Type = type;
        enterLevel.SetActive(true);
    }
}
