using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChaseState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.anim.Play("Run");
    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        #region 追击Player

        //若Player在视线范围内，且处于地面状态，则追击Player
        if (fsm.ep.targetPoint && fsm.ep.isGround)
        {
            //Enemy朝向Player
            fsm.FlipToPlayer(fsm.ep.targetPoint);
            fsm.ChasePlayer();
        }
        else if (fsm.ep.targetPoint && fsm.isFly)
        {
            fsm.FlipToPlayer(fsm.ep.targetPoint);
            fsm.FlyChasePlayer();
        }

        #endregion

        #region 切换状态

        if (fsm.ep.targetPoint == null && fsm.isFly)
        {
            fsm.TransitionToState(fsm.flyState);
        }
        //若Player不在视线范围内，且不为守卫模式时，则播放Idle动画后——切换为巡逻状态
        else if (fsm.ep.targetPoint == null)
        {
            fsm.TransitionToState(fsm.idleState);
        }

        //若Player在攻击范围内，则切换为攻击状态
        if (fsm.ep.isAttck)
            fsm.TransitionToState(fsm.attackState);

        #endregion
    }
}
