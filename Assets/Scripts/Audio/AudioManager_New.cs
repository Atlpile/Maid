using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager_New : Singleton<AudioManager_New>
{
    [Header("音乐数据库")]
    public SoundDetailsList_SO soundDetailsData;    //声音信息数据
    public SceneSoundList_SO sceneSoundData;        //场景声音数据

    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSource;

    private Coroutine soundRoutine;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("SnapShots")]
    public AudioMixerSnapshot normalSnapShot;
    public AudioMixerSnapshot ambientSnapShot;
    public AudioMixerSnapshot muteSnapShot;
    private float musicTransitionSecond = 8f;

    public float MusciStartSecond => Random.Range(5f, 15f);

    private void OnAfterSceneLoadedEvent()
    {
        //获取当前场景名称
        string currentScene = SceneManager.GetActiveScene().name;

        //通过获取当前场景，返回当前场景声音类型
        SceneSoundItem sceneSound = sceneSoundData.GetSceneSoundItem(currentScene);

        //若场景音乐不为空，则不执行一下内容
        if (sceneSound == null) return;

        //获取场景音效和场景音乐
        SoundDetails ambient = soundDetailsData.GetSoundDetails(sceneSound.ambient);
        SoundDetails music = soundDetailsData.GetSoundDetails(sceneSound.music);

        //若声音协程不为空，则终止声音协程
        if (soundRoutine != null) StopCoroutine(soundRoutine);
        //执行新协程
        soundRoutine = StartCoroutine(PlaySoundRoutine(music, ambient));
    }

    //播放背景音乐
    private void PlayMusicClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));

        //游戏BGM片段，为传入的BGM片段
        gameSource.clip = soundDetails.soundClip;

        //若游戏声音物体启用，则播放该AudioSource中的声音
        if (gameSource.isActiveAndEnabled) gameSource.Play();

        normalSnapShot.TransitionTo(transitionTime);
    }

    //播放环境音乐
    private void PlayAmbientClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConvertSoundVolume(soundDetails.soundVolume));

        //游戏环境音片段，为传入的环境音片段
        ambientSource.clip = soundDetails.soundClip;

        //若环境声音物体启用，则播放该AudioSource中的声音
        if (ambientSource.isActiveAndEnabled) ambientSource.Play();

        ambientSnapShot.TransitionTo(transitionTime);
    }

    //等待随机时间后，播放背景音乐
    private IEnumerator PlaySoundRoutine(SoundDetails music, SoundDetails ambient)
    {
        if (music != null && ambient != null)
        {
            PlayAmbientClip(ambient, 1f);
            yield return new WaitForSeconds(MusciStartSecond);
            PlayMusicClip(music, musicTransitionSecond);
        }
    }

    private float ConvertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
}
