using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        //TODO:站桩功能
    }

    public override void OnUpdate(Enemy enemy)
    {
        //TODO:检测前方是否有敌人

        //TODO:若发现敌人，则追踪敌人（切换为攻击状态）

        //TODO:若超出范围，回到原点站桩（切换为站桩状态）
    }
}
