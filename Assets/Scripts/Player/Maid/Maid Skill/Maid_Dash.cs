using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid_Dash : MonoBehaviour
{
    private PlayerController_Maid maid;

    [Header("Dash参数")]
    public int dashLossMagic = 10;
    public float dashSpeed;                 //Dash的速度
    public float dashTime;                  //Dash的时长
    public float dashCoolDown;              //Dash的CD时间
    public float dashTimeLeft;             //Dash的剩余时间
    public float lastDash = -10f;          //上一次使用冲刺的时间点（使用负值，保证能在游戏的一开始时就能执行冲刺功能）

    private void Awake()
    {
        maid = GetComponent<PlayerController_Maid>();
    }

    public void ReadyToDash()
    {
        maid.isDashing = true;              //开启冲刺状态

        dashTimeLeft = dashTime;            //冲刺剩余时间，为冲刺时间（即：技能的倒计时）

        lastDash = Time.time;               //上一次使用冲锋的时间点，为当前时间

        // cdImage.fillAmount = 1;          //cd冷却图标还原
    }

    public void Dash()
    {
        if (maid.isDashing)
        {
            //若冲刺的剩余时间 > 0
            if (dashTimeLeft > 0)
            {
                //若y轴速度>0 且人物不在地面（处于起跳状态）
                if (maid.rb.velocity.y > 0 && !maid.isGround)
                {
                    maid.rb.velocity = new Vector2(dashSpeed * maid.playerHorizontalMove, maid.rb.velocity.y);
                }

                //【Bug修复】使用transform.localScale.x可以达到沿自身朝向冲刺的效果   【拓展：可以用做后撤效果】
                if (transform.localEulerAngles.y > 0)
                    maid.rb.velocity = new Vector2(dashSpeed * -transform.localScale.x, maid.rb.velocity.y);
                else
                    maid.rb.velocity = new Vector2(dashSpeed * transform.localScale.x, maid.rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;                                         //冲刺技能倒计时开始

                ShadowPool.instance.GetFromPool();                                      //在对象池中获取残影
            }

            //若冲刺的剩余时间 <= 0
            if (dashTimeLeft <= 0)
            {
                maid.isDashing = false;

                //【Bug修复】修复Player下蹲滑行时的Bug
                maid.rb.velocity = new Vector2(0, maid.rb.velocity.y);

                //若人物不在地面（冲刺过程中仍在空中）
                if (!maid.isGround)
                {
                    maid.rb.velocity = new Vector2(dashSpeed * maid.playerHorizontalMove, maid.rb.velocity.y);
                }
            }
        }
    }

}
