using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDetailsList_SO", menuName = "Sound/SoundDetailsList")]
public class SoundDetailsList_SO : ScriptableObject
{
    //声音数据列表
    public List<SoundDetails> soundDetailsList;

    //通过枚举变量，查找声音片段的名称
    public SoundDetails GetSoundDetails(E_SoundName name)
    {
        //当传入的name参数 = 找到的soundName参数时，返回SoundDetails数据
        return soundDetailsList.Find(s => s.soundName == name);
    }
}

//声音数据
[System.Serializable]
public class SoundDetails
{
    public E_SoundName soundName;       //声音名称
    public AudioClip soundClip;         //声音片段

    //随机声音
    [Range(0.1f, 1.5f)] public float soundPitchMin;     //音调的最低值
    [Range(0.1f, 1.5f)] public float soundPitchMax;     //音调的最高值
    [Range(0.1f, 1f)] public float soundVolume;         //声音的响度
}

