using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Mon1 : SimpleEnemy
{
    #region Animation Event

    public void Mon1AttackAudio()
    {
        AudioManager.Instance.EnemyAttackAudio();
    }

    public void Mon1DeathAudio()
    {
        AudioManager.Instance.EnemyDeathAudio();
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    #endregion
}
