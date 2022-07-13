using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使恢复瓶具有物理效果
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent((typeof(BoxCollider2D)))]
public class AddCurrent : MonoBehaviour
{
    public E_BottleType bottleType;

    public int addNumber;

    private void AddHP()
    {
        //若当前血量未超过血量上限，则增加血量
        if (GameManager.Instance.maidStats.CurrentHealth < GameManager.Instance.maidStats.MaxHealth)
        {
            //若增加的血量超过血量上限，则当前血量为血量上限
            if ((GameManager.Instance.maidStats.CurrentHealth += addNumber) >= GameManager.Instance.maidStats.MaxHealth)
                GameManager.Instance.maidStats.CurrentHealth = GameManager.Instance.maidStats.MaxHealth;
            else
                GameManager.Instance.maidStats.CurrentHealth += addNumber;
        }

        AudioManager.Instance.DrinkAudio();
        Destroy(gameObject);
    }

    private void AddMP()
    {
        if (GameManager.Instance.maidStats.CurrentMagic < GameManager.Instance.maidStats.MaxMagic)
        {
            if ((GameManager.Instance.maidStats.CurrentMagic += addNumber) >= GameManager.Instance.maidStats.MaxMagic)
                GameManager.Instance.maidStats.CurrentMagic = GameManager.Instance.maidStats.MaxMagic;
            else
                GameManager.Instance.maidStats.CurrentMagic += addNumber;
        }

        AudioManager.Instance.DrinkAudio();
        Destroy(gameObject);
    }

    private void AddHMP()
    {
        //若当前血量未达到上限
        if (GameManager.Instance.maidStats.CurrentHealth < GameManager.Instance.maidStats.MaxHealth)
        {
            if ((GameManager.Instance.maidStats.CurrentHealth += addNumber) >= GameManager.Instance.maidStats.MaxHealth)
                GameManager.Instance.maidStats.CurrentHealth = GameManager.Instance.maidStats.MaxHealth;
            else
                GameManager.Instance.maidStats.CurrentHealth += addNumber;
        }

        //若当前蓝量未达到上限
        if (GameManager.Instance.maidStats.CurrentMagic < GameManager.Instance.maidStats.MaxMagic)
        {
            if ((GameManager.Instance.maidStats.CurrentMagic += addNumber) >= GameManager.Instance.maidStats.MaxMagic)
                GameManager.Instance.maidStats.CurrentMagic = GameManager.Instance.maidStats.MaxMagic;
            else
                GameManager.Instance.maidStats.CurrentMagic += addNumber;
        }

        AudioManager.Instance.DrinkAudio();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (bottleType)
            {
                case E_BottleType.HP:
                    AddHP();
                    break;
                case E_BottleType.MP:
                    AddMP();
                    break;
                case E_BottleType.HMP:
                    AddHMP();
                    break;
            }
        }
    }
}
