using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Mon2 : SimpleEnemy
{
    protected override void EnemyCheck()
    {
        canAttack = Physics2D.OverlapCircle(attackCheck.position, attackCheckRadius, playerLayer);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        Gizmos.DrawWireSphere(patrolArea.position, flyPatrolRange);
    }
}
