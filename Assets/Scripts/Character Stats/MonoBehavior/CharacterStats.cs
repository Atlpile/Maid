using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("角色数据")]
    public CharacterData_SO templateCharacterData;      //角色模板数据
    public CharacterData_SO characterData;              //角色数据
    public AttackData_SO templateAttackData;            //攻击模板数据
    public AttackData_SO attackData;                    //攻击数据

    [Header("受伤效果")]
    public int hitterVelocityX = 5;
    public int hitterVelocityY = 5;

    private void Awake()
    {
        //场景结束后，还原角色数据
        if (templateCharacterData != null)
            characterData = Instantiate(templateCharacterData);
        if (templateAttackData != null)
            attackData = Instantiate(templateAttackData);
    }

    #region 可从Data_SO中读取的角色属性

    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int MaxDefence
    {
        get { if (characterData != null) return characterData.maxDefence; else return 0; }
        set { characterData.maxDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }


    public float OriginSpeed
    {
        get { if (characterData != null) return characterData.originSpeed; else return 0; }
        set { characterData.originSpeed = value; }
    }
    public float CurrentSpeed
    {
        get { if (characterData != null) return characterData.currentSpeed; else return 0; }
        set { characterData.currentSpeed = value; }
    }
    public int PlayerJumpCount
    {
        get { if (characterData != null) return characterData.playerJumpCount; else return 0; }
        set { characterData.playerJumpCount = value; }
    }
    public int MaxMagic
    {
        get { if (characterData != null) return characterData.maxMagic; else return 0; }
        set { characterData.maxMagic = value; }
    }
    public int CurrentMagic
    {
        get { if (characterData != null) return characterData.currentMagic; else return 0; }
        set { characterData.currentMagic = value; }
    }


    public float PatrolSpeed
    {
        get { if (characterData != null) return characterData.patrolSpeed; else return 0; }
        set { characterData.patrolSpeed = value; }
    }
    public float ChaseSpeed
    {
        get { if (characterData != null) return characterData.chasePseed; else return 0; }
        set { characterData.chasePseed = value; }
    }

    #endregion

    #region 可从SO中读取的攻击属性

    public int Damage
    {
        get { if (attackData != null) return attackData.damage; else return 0; }
        set { attackData.damage = value; }
    }
    public float AttackArea
    {
        get { if (attackData != null) return attackData.attackArea; else return 0; }
        set { attackData.attackArea = value; }
    }
    public int SmallBulletDamage
    {
        get { if (attackData != null) return attackData.smallBulletDamage; else return 0; }
        set { attackData.smallBulletDamage = value; }
    }
    public int MiddleBulletDamage
    {
        get { if (attackData != null) return attackData.middleBulletDamage; else return 0; }
        set { attackData.middleBulletDamage = value; }
    }

    #endregion

    #region 角色战斗的数值计算

    //角色受伤数值计算 && 角色受伤效果
    public void TakePlayerDamage(CharacterStats attacker, CharacterStats defener)
    {
        //———————————————————————————————受伤/血量数值计算———————————————————————————————————————————

        int damage = Mathf.Max(attacker.attackData.damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //———————————————————————————————Player受伤物理效果———————————————————————————————————————————

        //获取character的受伤方向
        var attackerRot = attacker.transform.rotation.y + 0.5f;
        var defenerRot = defener.transform.rotation.y + 0.5f;

        //direcion < 0返回-1，direction > 0返回1，direction = 0返回0
        var hitDirection = Mathf.Sign(attackerRot);
        var trapHitDirection = Mathf.Sign(defenerRot);

        //———————————————————————————————受击目标判断———————————————————————————————————————————

        // if (defener.CompareTag("Enemy"))
        // {
        //     //为受伤方向添加速度（力）     
        //     defener.GetComponent<Rigidbody2D>().velocity = new Vector2(-hitDirection * hitterVelocityX, hitterVelocityY);

        //     //若defener为Enemy，则播放一次Hurt动画【目前Enemy动画中没有Hurt动画】
        //     // if (defener.CompareTag("Enemy"))
        //     //     defener.GetComponent<Animator>().Play("Dead");
        // }

        if (attacker.CompareTag("Enemy") && defener.CompareTag("Player"))
        {
            defener.GetComponent<Rigidbody2D>().velocity = new Vector2(-hitDirection * hitterVelocityX, hitterVelocityY + 5);
            defener.GetComponent<Animator>().SetTrigger("Hurt");
        }

        if (attacker.CompareTag("Trap") && defener.CompareTag("Player"))
        {
            defener.GetComponent<Rigidbody2D>().velocity = new Vector2(-trapHitDirection * hitterVelocityX, hitterVelocityY + 5);
            defener.GetComponent<Animator>().SetTrigger("Hurt");
        }

    }

    /// <summary>
    /// SmallBullet造成的伤害数值计算
    /// </summary>
    /// <param name="attacker">Player</param>
    /// <param name="defener">Enemy</param>
    public void TakeEnemySmallBulletDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.attackData.smallBulletDamage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
    }

    /// <summary>
    /// MiddleBullet造成的伤害数值计算
    /// </summary>
    /// <param name="attacker">Player</param>
    /// <param name="defener">Enemy</param>
    public void TakeEnemyMiddleBulletDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.attackData.middleBulletDamage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
    }

    /// <summary>
    /// SmallBullet消耗蓝量的数值计算
    /// </summary>
    /// <param name="playerMagic">Player蓝量</param>
    /// <param name="bulletMagicLoss">攻击消耗的蓝量</param>
    public void TakeSmallBulletMagicLoss(CharacterStats playerMagic, CharacterStats bulletMagicLoss)
    {
        CurrentMagic = Mathf.Max(CurrentMagic - bulletMagicLoss.attackData.smallBulletMagicLoss, 0);
    }

    /// <summary>
    /// MiddleBullet消耗蓝量的数值计算
    /// </summary>
    /// <param name="playerMagic"></param>
    /// <param name="bulletMagicLoss"></param>
    public void TakeMiddleBulletMagicLoss(CharacterStats playerMagic, CharacterStats bulletMagicLoss)
    {
        CurrentMagic = Mathf.Max(CurrentMagic - bulletMagicLoss.attackData.middleBulletMagicLoss, 0);
    }

    /// <summary>
    /// Dash消耗蓝量的数值计算
    /// </summary>
    /// <param name="playerMagic">Player蓝量</param>
    /// <param name="dashMagicLoss">Dash消耗的蓝量</param>
    public void TakeDashMagicLoss(CharacterStats playerMagic, int dashMagicLoss)
    {
        CurrentMagic = Mathf.Max(CurrentMagic - dashMagicLoss, 0);
    }

    /// <summary>
    /// Player通过传入的时间参数回蓝
    /// </summary>
    /// <param name="time"></param>
    public void ResumeMagic(float time)
    {
        // Player 0.5秒回1滴蓝

        if (time > 0.5f)
        {
            if (CurrentMagic < MaxMagic)
            {
                CurrentMagic++;
                GameManager.Instance.gameTime = 0;
            }
        }

    }

    #endregion

}
