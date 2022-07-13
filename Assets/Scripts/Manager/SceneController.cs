using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private GameObject player;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #region  传送设置

    /// <summary>
    /// 传送类型
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            //若为同场景传送
            case E_TransitionType.SameScene:
                //获取当前场景名称，获取目标点的目标标签
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
        }
    }

    /// <summary>
    /// 传送
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, E_DestinationTag destinationTag)
    {
        //获取Player
        player = GameManager.Instance.maidStats.gameObject;

        player.transform.SetPositionAndRotation(
              GetDestination(destinationTag).transform.position,
              GetDestination(destinationTag).transform.rotation
          );

        yield return null;
    }

    /// <summary>
    /// 获取目标点
    /// </summary>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
    private TransitionDestination GetDestination(E_DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }

    #endregion

    #region UI设置

    //加载场景
    IEnumerator LoadLevel(string scene)
    {
        //当场景名称不为空时
        if (scene != "")
        {
            //加载场景
            yield return SceneManager.LoadSceneAsync(scene);
            //TODO:生成Player的Prefab
            //TODO:保存数据

            //结束协程
            yield break;
        }
    }

    //加载主场景
    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("Main Menu");
        yield break;
    }

    //切换至保存后的场景
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    //切换至第一场景
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Level_01"));
    }

    //切换至主场景
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }



    #endregion
}
