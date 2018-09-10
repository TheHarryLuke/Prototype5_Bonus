using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    static private bool m_gameIsPaused = false;

    // Use this for initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    static public GameManager GetInstance()
    {
        return instance;
    }

    void Update ()
    {
        //Time pause code
        if (m_gameIsPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    static public void SetGamePaused(bool paused)
    {
        m_gameIsPaused = paused;
    }

    static public bool GetIsGamePaused()
    {
        return m_gameIsPaused;
    }

    static public void GameOver(bool _bEscaped)
    {
        SceneManager.LoadScene("EndScreen");
    }
}
