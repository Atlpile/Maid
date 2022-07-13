using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PatrolState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.anim.Play("Run");

        //切换巡逻点
        fsm.SwitchPoint();
    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        //向巡逻点移动
        fsm.MoveToTarget();

        //TODO:若出现多个巡逻点，当巡逻点为中间的巡逻点，则不切换Enemy朝向
        //若自身离巡逻点的距离接近于0，则播放完Idle动画后——再次切换为巡逻状态
        if (Vector2.Distance(fsm.transform.position, fsm.ep.patrolPoints[fsm.ep.patrolPosition].position) < 0.1f)
            fsm.TransitionToState(fsm.idleState);

        //若Player在视线范围内，则切换为Idle状态
        if (fsm.ep.targetPoint)
            fsm.TransitionToState(fsm.idleState);


    }
}
