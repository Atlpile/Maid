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

    private void AddHPLimit()
    {
        GameManager.Instance.maidStats.MaxHealth += 20;
        GameManager.Instance.maidStats.CurrentHealth += 20;

        //TODO:关闭BoxCollider，播放完动画后销毁物体
        AudioManager.Instance.GetAddLimitAudio();
        Destroy(gameObject);
    }

    private void AddMPLimit()
    {
        GameManager.Instance.maidStats.MaxMagic += 20;
        GameManager.Instance.maidStats.CurrentMagic += 20;

        AudioManager.Instance.GetAddLimitAudio();
        Destroy(gameObject);
    }

    private void AddATKLimit()
    {
        GameManager.Instance.maidStats.Damage += 20;

        AudioManager.Instance.GetAddLimitAudio();
        Destroy(gameObject);
    }

    private void AddDEFLimit()
    {
        GameManager.Instance.maidStats.MaxDefence += 20;
        GameManager.Instance.maidStats.CurrentDefence += 20;

        AudioManager.Instance.GetAddLimitAudio();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (limitType)
            {
                case E_LimitType.ATK:
                    AddATKLimit();
                    break;
                case E_LimitType.DEF:
                    AddDEFLimit();
                    break;
                case E_LimitType.HP:
                    AddHPLimit();
                    break;
                case E_LimitType.MP:
                    AddMPLimit();
                    break;
            }
        }
    }
}
