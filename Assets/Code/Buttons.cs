using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    private GameManager gameManager;
    private BaseLevelManager levelManager;         //TODO: Revisar
    private MenuCanvasManagement menuCanvasManagement;

    private void Start()
    {
        gameManager = GameManager.instance;
        levelManager = FindObjectOfType<BaseLevelManager>();       //TODO: Revisar
        menuCanvasManagement = FindObjectOfType<MenuCanvasManagement>();
    }

    public void OpenLevelSelector()
    {
        menuCanvasManagement.GoToLevelSelector();
    }

    public void ReturToMain()
    {
        menuCanvasManagement.ReturnToMainMenu();
    }

    public void SetGameMode(string gameMode)
    {
        //
        switch (gameMode)
        {
            case "Type1":
                gameManager.Game_Mode = GameMode.Type1;
                break;
        }
    }

    public void SetChallengeType(string challengeType)
    {
        switch (challengeType)
        {
            case "ZCS":
                gameManager.Challenge_Type = ChallengeType.ZCS;
                break;
            case "BV":
                gameManager.Challenge_Type = ChallengeType.BV;
                break;
            case "GJ":
                gameManager.Challenge_Type = ChallengeType.GJ;
                break;
        }
    }

	public void StartGame()
    {
        switch(gameManager.Game_Mode)
        {
            case GameMode.Type1:
                SceneManager.LoadScene(1);
                break;
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TryLetter(string letter)
    {
        //Debug.Log(letter);
        
        levelManager.ReceiveLetter(letter);
    }
}
