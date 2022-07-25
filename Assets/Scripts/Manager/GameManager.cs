using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [Header("Reference")]
    [HideInInspector] public CharacterStats maidStats;
    [HideInInspector] public CharacterStats mon4Stats;
    [HideInInspector] public PlayerController_Maid maidController;

    [Header("监视是否持有相应的Key")]
    public bool hasRedKey = false;
    public bool hasGreenKey = false;
    public bool hasBlueKey = false;
    public bool hasYellowKey = false;

    private bool isGameOver;

    [HideInInspector] public float gameTime;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (maidController != null)
        {
            isGameOver = maidController.isDead;
            User_UI.Instance.GameOverUI(isGameOver);
        }

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


    //按设置的时间来更新游戏时间
    // private IEnumerator UpdateTime(float time)
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(time);
    //         gameTime = gameTime + time;
    //     }
    // }


    #region 注册

    public void RegisterPlayerCharacterStats(CharacterStats maid)
    {
        maidStats = maid;
    }
    public void RegisterPlayerController(PlayerController_Maid maid)
    {
        maidController = maid;
    }

    public void RegisterMon4CharacterStats(CharacterStats mon4)
    {
        mon4Stats = mon4;
    }

    #endregion

}
