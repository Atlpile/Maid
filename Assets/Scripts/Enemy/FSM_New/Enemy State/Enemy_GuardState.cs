using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GuardState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.idleTimer = 0;

        //动画状态为守卫状态（初始播放Idle动画）
        fsm.anim.Play("Idle");
    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        //若自身不在守卫位置，则回到守卫点
        if (fsm.transform.position.x < fsm.ep.guardPoint.position.x || fsm.transform.position.x > fsm.ep.guardPoint.position.x)
        {
            fsm.anim.Play("Run");
            fsm.MoveToGuardPoint();
        }

        //若自身接近守卫点位置，则播放Idle动画（再次进入守卫状态）
        if (Mathf.Abs(fsm.transform.position.x - fsm.ep.guardPoint.position.x) < 0.001f)
        {
            // fsm.m_rigidbody2D.velocity = new Vector2(0, 0);
            fsm.TransitionToState(fsm.guardState);
        }

        //若Player在视线范围内，且处于地面，则播放Idle动画后，切换为追击状态
        if (fsm.ep.targetPoint)
            fsm.TransitionToState(fsm.idleState);

    }

}
