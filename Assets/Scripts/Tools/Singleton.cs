using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型单例模式（用于实现各种Manager的单例模式）
public class Singleton<T> : MonoBehaviour where T : Singleton<T>    //约束T为SignleTon类型，即各种Manager的类型设为T类型
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }
    public static bool IsInitialized
    {
        get { return instance != null; }
    }


    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }


    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
