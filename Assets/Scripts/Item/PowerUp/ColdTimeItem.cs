using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ColdTimeItemType
{
    Speed, JumpCount
}

public class ColdTimeItem : MonoBehaviour
{
    public E_ColdTimeItemType type;

    [Header("增幅效果")]
    public float addSpeed = 5;
    public int addJumpCount = 5;

    [Header("各个道具的冷却时间")]
    public float speedCD;
    public float jumpCD;

    private void FixedUpdate()
    {
        //超过冷却时间还原
        if (speedCD < 0)
        {
            ResumeSpeed();

            //重置道具的CD冷却
        }
    }



    //TODO:在冷却时间内可以使用功能，若超过冷却时间则还原
    public void AddSpeed()
    {
        //Player在冷却时间内提升增幅效果
        GameManager.Instance.maidStats.PlayerCurrentSpeed += addSpeed;
    }
    public void ResumeSpeed()
    {
        var originSpeed = GameManager.Instance.maidStats.PlayerOriginSpeed;

        GameManager.Instance.maidStats.PlayerCurrentSpeed = originSpeed;
    }

    private void AddJumpCount()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case E_ColdTimeItemType.Speed:
                    AddSpeed();
                    break;
                case E_ColdTimeItemType.JumpCount:
                    AddJumpCount();
                    break;
            }
        }
    }
}
