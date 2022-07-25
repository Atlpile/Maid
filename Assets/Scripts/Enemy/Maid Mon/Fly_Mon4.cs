using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Mon4 : SimpleEnemy
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterMon4CharacterStats(monStats);
    }

    protected override void EnemyCheck()
    {

    }

    protected override void OnDrawGizmos()
    {

    }
}
