using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScreenScript : MonoBehaviour {

    public GameObject titleScreen;
    public GameObject menuScreen;
    public GameObject instructionsScreen;
    public GameObject creditsScreen;

    // Use this for initialization
    void Start ()
    {
        GameManager.SetGamePaused(false);
        SetTitleScreen();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SetTitleScreen()
    {
        menuScreen.SetActive(false);
        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        titleScreen.SetActive(true);
    }

    public void SetMenuScreen()
    {
        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        titleScreen.SetActive(false);

        menuScreen.SetActive(true);
    }

    public void SetInstructionsScreen()
    {
        creditsScreen.SetActive(false);
        titleScreen.SetActive(false);
        menuScreen.SetActive(false);

        instructionsScreen.SetActive(true);
    }
    
    public void SetCreditsScreen()
    {
        titleScreen.SetActive(false);
        menuScreen.SetActive(false);
        instructionsScreen.SetActive(false);

        creditsScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
