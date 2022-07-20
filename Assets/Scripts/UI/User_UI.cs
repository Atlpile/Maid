using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User_UI : Singleton<User_UI>
{
    [Header("UI面板")]
    public GameObject pauseMenu;
    public GameObject gameOverUI;

    [Header("按钮")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;


    protected override void Awake()
    {
        base.Awake();

        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(LoadMainMenu);
        yesButton.onClick.AddListener(LoadSavePoint);
        noButton.onClick.AddListener(LoadMainMenu);
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
