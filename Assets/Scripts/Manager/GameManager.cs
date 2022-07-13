using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public CharacterStats maidStats;
    [HideInInspector] public CharacterStats smallBulletStats;
    [HideInInspector] public PlayerController_Maid maidController;
    [HideInInspector] public HasKey keys;

    private bool isGameOver;
    private Coin coins;

    [HideInInspector] public float gameTime;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        //     UIManager.Instance.PauseGame();


        if (maidController != null)
            isGameOver = maidController.isDead;

        User_UI.Instance.GameOverUI(isGameOver);

        // userUI.GameOverUI(isGameOver);

        // if (isGameOver)
        //     return;


    }


    private void FixedUpdate()
    {
        //获取游戏时间
        gameTime += Time.fixedDeltaTime;

        maidStats.ResumeMagic(gameTime);
        // Debug.Log(gameTime);
    }


    #region 注册

    public void RegisterPlayer(CharacterStats maid)
    {
        maidStats = maid;
    }
    public void RegisterPlayer(PlayerController_Maid maid)
    {
        maidController = maid;
    }

    public void RegisterSmallBullet(CharacterStats smallBullet)
    {
        smallBulletStats = smallBullet;
    }

    public void RegisterCoin(Coin coin)
    {
        coins = coin;
    }

    public void RegisterKey(HasKey key)
    {
        keys = key;
    }


    #endregion

}
