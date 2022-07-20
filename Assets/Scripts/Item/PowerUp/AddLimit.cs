using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLimit : MonoBehaviour
{
    public E_LimitType limitType;

    private Collider2D m_LimitCollider;

    private void Start()
    {
        m_LimitCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetLimitType(other);
        }
    }


    private void GetLimitType(Collider2D other)
    {
        switch (limitType)
        {
            case E_LimitType.ATK:
                AddATKLimit(other.GetComponent<CharacterStats>());
                break;
            case E_LimitType.DEF:
                AddDEFLimit(other.GetComponent<CharacterStats>());
                break;
            case E_LimitType.HP:
                AddHPLimit(other.GetComponent<CharacterStats>());
                break;
            case E_LimitType.MP:
                AddMPLimit(other.GetComponent<CharacterStats>());
                break;
        }

        AudioManager.Instance.GetAddLimitAudio();
        Destroy(gameObject);
    }

    private void AddHPLimit(CharacterStats maidStats)
    {
        maidStats.MaxHealth += 20;
        maidStats.CurrentHealth += 20;
    }

    private void AddMPLimit(CharacterStats maidStats)
    {
        maidStats.MaxMagic += 20;
        maidStats.CurrentMagic += 20;
    }

    private void AddATKLimit(CharacterStats maidStats)
    {
        maidStats.Damage += 20;
    }

    private void AddDEFLimit(CharacterStats maidStats)
    {
        maidStats.MaxDefence += 20;
        maidStats.CurrentDefence += 20;
    }


    //TODO:使物品具有上下浮动效果

    //TODO:拾取物品后，先透明，再销毁物体

}
