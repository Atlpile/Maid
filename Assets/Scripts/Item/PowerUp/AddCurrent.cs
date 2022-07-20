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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetBottleType(other);
        }
    }

    private void GetBottleType(Collision2D other)
    {
        switch (bottleType)
        {
            case E_BottleType.HP:
                AddHP(other.gameObject.GetComponent<CharacterStats>());
                break;
            case E_BottleType.MP:
                AddMP(other.gameObject.GetComponent<CharacterStats>());
                break;
            case E_BottleType.HMP:
                AddHMP(other.gameObject.GetComponent<CharacterStats>());
                break;
        }

        AudioManager.Instance.DrinkAudio();
        Destroy(gameObject);
    }

    private void AddHP(CharacterStats maidStats)
    {
        //若当前血量未超过血量上限，则增加血量
        if (maidStats.CurrentHealth < maidStats.MaxHealth)
        {
            //若增加的血量超过血量上限，则当前血量为血量上限
            if ((maidStats.CurrentHealth += addNumber) >= maidStats.MaxHealth)
                maidStats.CurrentHealth = maidStats.MaxHealth;
            else
                maidStats.CurrentHealth += addNumber;
        }
    }

    private void AddMP(CharacterStats maidStats)
    {
        if (maidStats.CurrentMagic < maidStats.MaxMagic)
        {
            if ((maidStats.CurrentMagic += addNumber) >= maidStats.MaxMagic)
                maidStats.CurrentMagic = maidStats.MaxMagic;
            else
                maidStats.CurrentMagic += addNumber;
        }
    }

    //TODO:HMP改为回满血满蓝
    private void AddHMP(CharacterStats maidStats)
    {
        //若当前血量未达到上限
        if (maidStats.CurrentHealth < maidStats.MaxHealth)
        {
            if ((maidStats.CurrentHealth += addNumber) >= maidStats.MaxHealth)
                maidStats.CurrentHealth = maidStats.MaxHealth;
            else
                maidStats.CurrentHealth += addNumber;
        }

        //若当前蓝量未达到上限
        if (maidStats.CurrentMagic < maidStats.MaxMagic)
        {
            if ((maidStats.CurrentMagic += addNumber) >= maidStats.MaxMagic)
                maidStats.CurrentMagic = maidStats.MaxMagic;
            else
                maidStats.CurrentMagic += addNumber;
        }
    }

}
