using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UIManager不需要切换场景

public class UIManager : Singleton<UIManager>
{


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //TODO:显示玩家技能冷却
    //TODO:显示玩家道具数量
    //TODO:显示开门道具数量



}
