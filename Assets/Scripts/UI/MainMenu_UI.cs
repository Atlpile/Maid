using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_UI : MonoBehaviour
{
    private Button newGameBtn;
    private Button continueBtn;
    private Button quitBtn;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(NewGame);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("level"))
            SceneController.Instance.TransitionToLoadGame();
        else
            NewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
