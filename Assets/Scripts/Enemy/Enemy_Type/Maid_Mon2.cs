using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid_Mon2 : Enemy_FSM
{
    public override void Init()
    {
        base.Init();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    public void EnemyDeathAudio()
    {
        //播放Enemy死亡音效
        AudioManager.Instance.EnemyDeathAudio();
    }

    public void DestroyEnemy()
    {
        Destroy(transform.parent.gameObject);
    }
}
