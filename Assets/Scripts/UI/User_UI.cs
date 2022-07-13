using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User_UI : Singleton<User_UI>
{
    public GameObject pauseMenu;
    public GameObject gameOverUI;

    private Button pauseButton;
    private Button resumeButton;
    private Button exitButton;
    private Button yesButton;
    private Button noButton;


    protected override void Awake()
    {
        base.Awake();

        pauseButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        resumeButton = transform.GetChild(1).GetChild(3).GetComponent<Button>();
        exitButton = transform.GetChild(1).GetChild(4).GetComponent<Button>();
        yesButton = transform.GetChild(2).GetChild(2).GetComponent<Button>();
        noButton = transform.GetChild(2).GetChild(3).GetComponent<Button>();

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(LoadMainMenu);
        yesButton.onClick.AddListener(LoadSavePoint);
        noButton.onClick.AddListener(LoadMainMenu);

        DontDestroyOnLoad(this);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;

    }

    public void LoadMainMenu()
    {
        pauseMenu.SetActive(false);
        SceneController.Instance.TransitionToMain();

        Time.timeScale = 1;
    }

    public void LoadSavePoint()
    {
        //加载存档点
        SceneController.Instance.TransitionToLoadGame();
    }

    public void GameOverUI(bool playerDead)
    {
        // if (gameOverUI != null)
        gameOverUI.SetActive(playerDead);
    }
}
