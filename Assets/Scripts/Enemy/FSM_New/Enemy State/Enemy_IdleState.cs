using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.idleTimer = 0;

        fsm.anim.Play("Idle");

        //若为守卫模式，则朝向Player（用于翻转时使Enemy反应一段时间）
        if (fsm.isGuard)
            fsm.FlipToPlayer(fsm.ep.targetPoint);

        //若不为守卫模式，且Player在范围内，则朝向Player
        if (!fsm.isGuard && fsm.ep.targetPoint)
            fsm.FlipToPlayer(fsm.ep.targetPoint);

    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        fsm.idleTimer += Time.deltaTime;
        // Debug.Log(fsm.timer);



        //——————————————————————————————————————————不处于守卫模式状态切换——————————————————————————————————————————

        //若超过Idle时间，且不为守卫模式，则切换为巡逻状态
        if (fsm.idleTimer >= fsm.ep.idleTime && !fsm.isGuard)
            fsm.TransitionToState(fsm.patrolState);

        //若超过Idle时间，Player在视线范围内，且不为守卫模式，且处于地面，则切换为追击状态
        if (fsm.idleTimer >= fsm.ep.idleTime && fsm.ep.targetPoint && !fsm.isGuard && fsm.ep.isGround)
            fsm.TransitionToState(fsm.chaseState);

        //——————————————————————————————————————————处于守卫模式状态切换——————————————————————————————————————————

        //若超过Idle时间，且为守卫模式，且目标为空，则切换为守卫状态
        if (fsm.idleTimer >= fsm.ep.idleTime && fsm.isGuard && !fsm.ep.targetPoint)
            fsm.TransitionToState(fsm.guardState);

        //若超过Idle时间，且为守卫模式，则切换为追击状态（用于守卫模式下的追击Player）
        if (fsm.idleTimer >= fsm.ep.idleTime && fsm.isGuard)
            fsm.TransitionToState(fsm.chaseState);

        //——————————————————————————————————————————攻击状态的切换——————————————————————————————————————————

        // if (fsm.attackTimer >= fsm.ep.attackTime && fsm.ep.isAttck)
        //     fsm.TransitionToState(fsm.attackState);

        // if (fsm.attackTimer >= fsm.ep.attackTime && fsm.ep.targetPoint)
        //     fsm.TransitionToState(fsm.chaseState);
    }

}
