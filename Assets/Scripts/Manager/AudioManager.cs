using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    public AudioClip BGMClip;
    //TODO:环境音效
    [Header("物品音效")]
    public AudioClip addLimitClip;
    public AudioClip drinkClip;


    [Header("Player音效")]
    public AudioClip jumpClip;
    public AudioClip groundClip;
    public AudioClip walkClip;
    public AudioClip hurtClip;
    public AudioClip fireClip;
    public AudioClip dashClip;
    public AudioClip getCoinClip;

    [Header("Enemy音效")]
    public AudioClip enemyAttackClip;
    public AudioClip enemyHurtClip;
    public AudioClip enemyDeathClip_1;

    //TODO:UI音效


    //物品音源
    private AudioSource addLimitSource;
    private AudioSource drinkSource;

    //Player音源
    private AudioSource playerSource;
    private AudioSource dashSource;
    private AudioSource fireSource;
    private AudioSource coinSource;
    //背景音乐音源
    private AudioSource BGMSource;
    //Enemy音源
    private AudioSource enemySource;
    private AudioSource enemyAttackSource;


    [Header("音效控制")]
    public AudioMixerGroup BGMGroup;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        playerSource = gameObject.AddComponent<AudioSource>();
        fireSource = gameObject.AddComponent<AudioSource>();
        dashSource = gameObject.AddComponent<AudioSource>();
        BGMSource = gameObject.AddComponent<AudioSource>();
        enemySource = gameObject.AddComponent<AudioSource>();
        enemyAttackSource = gameObject.AddComponent<AudioSource>();

        addLimitSource = gameObject.AddComponent<AudioSource>();
        drinkSource = gameObject.AddComponent<AudioSource>();
        coinSource = gameObject.AddComponent<AudioSource>();

        BGMSource.outputAudioMixerGroup = BGMGroup;
    }

    private void Start()
    {
        playerSource.playOnAwake = false;
        fireSource.playOnAwake = false;
        dashSource.playOnAwake = false;
        BGMSource.playOnAwake = false;
        enemySource.playOnAwake = false;
        enemyAttackSource.playOnAwake = false;

        addLimitSource.playOnAwake = false;
        drinkSource.playOnAwake = false;
        coinSource.playOnAwake = false;

        StartLevelAudio();
    }

    public void StartLevelAudio()
    {
        BGMSource.clip = BGMClip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    #region Player音效
    public void PlayerWalkAudio()
    {
        playerSource.clip = walkClip;
        playerSource.Play();
    }
    public void PlayerJumpAudio()
    {
        playerSource.clip = jumpClip;
        playerSource.Play();
    }
    public void PlayerFireAudio()
    {
        fireSource.clip = fireClip;
        fireSource.Play();
    }
    public void PlayerDashAudio()
    {
        dashSource.clip = dashClip;
        dashSource.Play();
    }
    #endregion

    public void EnemyAttackAudio()
    {
        enemyAttackSource.clip = enemyAttackClip;
        enemyAttackSource.Play();
    }

    public void EnemyHurtAudio()
    {
        enemySource.clip = enemyHurtClip;
        enemySource.Play();
    }
    public void EnemyDeathAudio()
    {
        enemySource.clip = enemyDeathClip_1;
        enemySource.Play();
    }

    //Item音效
    public void GetCoinAudio()
    {
        coinSource.clip = getCoinClip;
        coinSource.Play();
    }

    public void GetAddLimitAudio()
    {
        addLimitSource.clip = addLimitClip;
        addLimitSource.Play();
    }

    public void DrinkAudio()
    {
        drinkSource.clip = drinkClip;
        drinkSource.Play();
    }
}
