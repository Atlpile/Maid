using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    /// <summary>
    /// 音效事件
    /// </summary>
    public static event Action<SoundDetails> InitSoundEffect;
    public static void CallInitSountEffect(SoundDetails soundDetails)
    {
        InitSoundEffect?.Invoke(soundDetails);
    }
}
