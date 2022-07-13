using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    //sceneName为任意名称，用于存储当前人物的场景
    string sceneName = "level";

    //从PlayerPrefs获取sceneName键值
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #region  数据的保存与加载

    //存储Player位置
    public void SavePlayerPositionData(float playerPositionX, float playerPositionY, string keyX, string keyY)
    {
        PlayerPrefs.SetFloat(keyX, playerPositionX);
        PlayerPrefs.SetFloat(keyY, playerPositionY);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void LoadPlayerPositionData(string keyX, string keyY)
    {
        //若有键值，则读取数据
        if (PlayerPrefs.HasKey(keyX) && PlayerPrefs.HasKey(keyY))
        {
            var savePositionX = PlayerPrefs.GetFloat(keyX);
            var savePositionY = PlayerPrefs.GetFloat(keyY);

            GameManager.Instance.maidStats.transform.position = new Vector2(savePositionX, savePositionY);
        }
    }

    //存储Player数据
    public void Save(Object data, string key)
    {
        //将传入的所有数据都变为Json类型，true可以使Json数据在文本中更美观
        var jsonData = JsonUtility.ToJson(data, true);

        //保存键值数据
        PlayerPrefs.SetString(key, jsonData);
        //保存场景名称


        //保存数据至磁盘
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        //若有键值，则读取数据
        if (PlayerPrefs.HasKey(key))
        {
            //将Json数据写入对应目标
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    #endregion
}
