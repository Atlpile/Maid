using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlyState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.anim.Play("Run");

        fsm.GetNewWayPoint();
    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        //TODO:随机生成巡逻点，并向目标移动
        fsm.FlyToTarget();

        if (Vector2.Distance(fsm.transform.position, fsm.ep.flyPoint) < 0.1f)
            fsm.TransitionToState(fsm.flyState);

        if (fsm.ep.targetPoint)
            fsm.TransitionToState(fsm.chaseState);
    }
}
