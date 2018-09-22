using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    private GameManager gameManager;
    private MenuCanvasManagement menuCanvasManagement;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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

	public void StartGame()
    {
        // TODO: Buscar algo aqui que vaya entre escenas
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TryLetter(string letter)
    {
        //Debug.Log(letter);
        
        gameManager.ReceiveLetter(letter);
    }
}
