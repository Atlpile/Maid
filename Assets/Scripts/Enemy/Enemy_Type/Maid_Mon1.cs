using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid_Mon1 : Enemy_FSM
{

    public override void Init()
    {
        base.Init();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    #region Animation Event

    public void EnemyDeathAudio()
    {
        //播放Enemy死亡音效
        AudioManager.Instance.EnemyDeathAudio();
    }
    public void EnemyAttackAudio()
    {
        AudioManager.Instance.EnemyAttackAudio();
    }
    public void DestroyEnemy()
    {
        Destroy(transform.parent.gameObject);
    }

    #endregion

}
