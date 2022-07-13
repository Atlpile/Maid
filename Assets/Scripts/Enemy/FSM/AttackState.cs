using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        //切换到巡逻状态的条件
        if (enemy.attackList.Count == 0)
            enemy.TransitionToState(enemy.patrolState);


        if (enemy.attackList.Count > 1)
        {
            //获取攻击列表中所有攻击目标
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                //判断离自身最近的攻击目标
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) <
                    Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.attackList[i];    //目标点为攻击列表中最近的目标点
                }
            }
        }

        //若攻击范围中只有一个攻击目标，则向第一个攻击目标移动
        if (enemy.attackList.Count == 1)
            enemy.targetPoint = enemy.attackList[0];


        // //若目标标签为Player
        // if (enemy.targetPoint.CompareTag("Player"))
        //     enemy.AttackAction();                               //执行普通攻击


        //向目标移动
        if (enemy.targetPoint.CompareTag("Player"))
            enemy.MoveToPlayer();

    }
}
