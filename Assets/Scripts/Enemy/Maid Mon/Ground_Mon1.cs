using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Mon1 : SimpleEnemy
{
    #region Animation Event

    public void Mon1DeathAudio()
    {
        //播放Enemy死亡音效
        AudioManager.Instance.EnemyDeathAudio();
    }

    public void Mon1AttackAudio()
    {
        AudioManager.Instance.EnemyAttackAudio();
    }

    #endregion
}
