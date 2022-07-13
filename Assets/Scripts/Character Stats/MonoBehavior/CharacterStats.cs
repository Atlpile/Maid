using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("角色数据")]
    public CharacterData_SO templateCharacterData;   //角色模板数据
    public CharacterData_SO characterData;
    public AttackData_SO templateAttackData;
    public AttackData_SO attackData;

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

    #region 可从Data_SO中读取的属性

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

    public float PlayerOriginSpeed
    {
        get { if (characterData != null) return characterData.playerOriginSpeed; else return 0; }
        set { characterData.playerOriginSpeed = value; }
    }
    public float PlayerCurrentSpeed
    {
        get { if (characterData != null) return characterData.playerCurrentSpeed; else return 0; }
        set { characterData.playerCurrentSpeed = value; }
    }
    public int playerJumpCount
    {
        get { if (characterData != null) return characterData.playerJumpCount; else return 0; }
        set { characterData.playerJumpCount = value; }
    }

    #endregion

    #region 角色战斗的数值计算

    //角色受伤数值计算 && 角色受伤效果
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        //———————————————————————————————受伤/血量数值计算———————————————————————————————————————————

        int damage = Mathf.Max(attacker.attackData.damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //———————————————————————————————Character受伤物理/动画反馈———————————————————————————————————————————

        //获取character的受伤方向
        var attackerRot = attacker.transform.rotation.y + 0.5f;
        var defenerRot = defener.transform.rotation.y + 0.5f;

        //direcion < 0返回-1，direction > 0返回1，direction = 0返回0
        var hitDirection = Mathf.Sign(attackerRot);
        var trapHitDirection = Mathf.Sign(defenerRot);


        if (defener.CompareTag("Enemy"))
        {
            //为受伤方向添加速度（力）     
            defener.GetComponent<Rigidbody2D>().velocity = new Vector2(hitDirection * hitterVelocityX, hitterVelocityY);

            //若defener为Enemy，则播放一次Hurt动画【目前Enemy动画中没有Hurt动画】
            // if (defener.CompareTag("Enemy"))
            //     defener.GetComponent<Animator>().Play("Dead");
        }

        if (defener.CompareTag("Player") && !attacker.CompareTag("Trap"))
        {
            defener.GetComponent<Rigidbody2D>().velocity = new Vector2(hitDirection * hitterVelocityX, hitterVelocityY + 5);
            defener.GetComponent<Animator>().SetTrigger("Hurt");
        }

        if (defener.CompareTag("Player") && attacker.CompareTag("Trap"))
        {
            defener.GetComponent<Rigidbody2D>().velocity = new Vector2(-trapHitDirection * hitterVelocityX, hitterVelocityY + 5);
            defener.GetComponent<Animator>().SetTrigger("Hurt");
        }

    }

    //Player攻击消耗蓝量的数值计算
    public void TakeMagic(CharacterStats playerMagic, CharacterStats bulletMagicDamage)
    {
        //蓝量数值计算
        CurrentMagic = Mathf.Max(CurrentMagic - bulletMagicDamage.attackData.magicDamage, 0);
    }

    //PlayerDash消耗蓝量的数值计算
    public void TakeMagic(CharacterStats playerMagic, int dashMagicDamage)
    {
        CurrentMagic = Mathf.Max(CurrentMagic - dashMagicDamage, 0);
    }

    //Player通过传入的时间参数回蓝
    public void ResumeMagic(float time)
    {
        //Player 0.5秒回1滴蓝
        if (time > 0.5f)
        {
            if (CurrentMagic < MaxMagic)
            {
                CurrentMagic++;
                GameManager.Instance.gameTime = 0;
            }
        }
    }

    //TODO:Player升级经验数值

    #endregion


}
