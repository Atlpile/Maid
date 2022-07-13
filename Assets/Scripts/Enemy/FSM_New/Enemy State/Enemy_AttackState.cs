using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : Enemy_BaseState
{
    public override void EnterState(Enemy_FSM fsm)
    {
        fsm.anim.Play("Attack");
    }

    public override void UpdateState(Enemy_FSM fsm)
    {
        fsm.info = fsm.anim.GetCurrentAnimatorStateInfo(0);
        if (fsm.info.normalizedTime >= .95f)
        {
            fsm.TransitionToState(fsm.chaseState);
        }
    }

}
