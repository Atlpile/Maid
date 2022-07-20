using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//场景背景音乐及环境音效
[CreateAssetMenu(fileName = "SceneSoundList_SO", menuName = "Sound/SceneSoundList")]
public class SceneSoundList_SO : ScriptableObject
{
    //场景声音数据列表
    public List<SceneSoundItem> sceneSoundList;

    public SceneSoundItem GetSceneSoundItem(string name)
    {
        return sceneSoundList.Find(s => s.sceneName == name);
    }
}

//场景声音类型
[System.Serializable]
public class SceneSoundItem
{
    /*[SceneName]*/
    public string sceneName;        //场景名称
    public E_SoundName ambient;     //环境音效
    public E_SoundName music;       //场景音乐
}
