using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManagement : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject levelSelector;

	// Use this for initialization
	void Start () {
        levelSelector.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToLevelSelector()
    {
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        levelSelector.SetActive(false);
    }
}
