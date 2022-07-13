using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();    //切换巡逻点
    }

    public override void OnUpdate(Enemy enemy)
    {
        //移动巡逻
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animState = 1;    //到达目标点后停顿
            enemy.MoveToTarget();
        }

        //TODO:飞行敌人在圆半径的随机范围内巡逻

        //到达目标点后切换巡逻方向
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)   //若自身离巡逻点的距离接近于0
        {
            enemy.TransitionToState(enemy.patrolState);                                     //再次进入巡逻状态（用于切换巡逻点）
        }

        //若攻击范围内存在敌人，则切换为追击状态
        if (enemy.attackList.Count > 0)
            enemy.TransitionToState(enemy.attackState);
    }
}
