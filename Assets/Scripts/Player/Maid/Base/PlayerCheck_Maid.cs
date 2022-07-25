using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck_Maid : MonoBehaviour
{
    private PlayerController_Maid maid;
    private Maid_Dash maidDash;

    [Header("地面检测")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float checkRadius;

    [Header("头顶射线检测")]
    public float rayHeight;                 //射线y轴位置
    public float rayLength;                 //射线长度

    public float platformXRayPosition_Left;
    public float platformXRayPosition_Right;

    public float rayXPositionLeft;
    public float rayXPositionRight;

    private void Awake()
    {
        maid = GetComponent<PlayerController_Maid>();
        maidDash = GetComponent<Maid_Dash>();
    }


    //射线参数（射线的起点，射线的方向，射线的长度，射线检测的目标图层）
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;                                                           //获取player的坐标（中心点），定义为pos
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);            //定义射线检测信息hit：（人物中心点位置+射线起点，射线方向，长度，图层）
        Color color = hit ? Color.red : Color.green;                                                //设置射线颜色：触发射线检测则为红色，未触发射线检测则为绿色
        Debug.DrawRay(pos + offset, rayDiraction * length, color);                                  //显示射线（人物中心点位置+射线起点，射线方向，射线长度，射线颜色）

        return hit;                                                                                 //将射线检测的信息hit返回给Raycast方法
    }

    public void InputCheck()
    {
        //Jump输入检测
        if (Input.GetButtonDown("Jump") && maid.isGround)
            maid.jumpPressed = true;
        if (Input.GetButtonDown("Jump") && maid.jumpCount > 0)
            maid.jumpPressed = true;

        //Crouch输入检测
        maid.crouchHeld = Input.GetButton("Crouch");

        //Dash输入检测
        if (Input.GetKeyDown(KeyCode.LeftShift) && maid.maidStats.CurrentMagic > 10)
        {
            //若游戏当前时间 >= 上一次使用冲锋的时间点+冲刺CD时间（CD时间）
            if (Time.time >= (maidDash.lastDash + maidDash.dashCoolDown))
            {
                //执行冲刺功能
                maidDash.ReadyToDash();
                //冲刺消耗蓝量
                maid.maidStats.TakeDashMagicLoss(maid.maidStats, maidDash.dashLossMagic);
            }
        }
    }

    public void GroundCheck()
    {
        //设置地面检测Gizmos参数（地面检测的位置，检测范围，检测图层）
        maid.isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        maid.isPlatform = Physics2D.OverlapCircle(groundCheck.position, checkRadius, platformLayer);
    }

    public void HeadCheck()
    {
        //头顶检测Platform
        RaycastHit2D platformLeftCheck = Raycast(new Vector2(platformXRayPosition_Left, maid.coll.size.y - rayHeight), Vector2.up, rayLength, platformLayer);
        RaycastHit2D platformRightCheck = Raycast(new Vector2(platformXRayPosition_Right, maid.coll.size.y - rayHeight), Vector2.up, rayLength, platformLayer);

        //头顶检测Ground
        RaycastHit2D leftHeadCheck = Raycast(new Vector2(rayXPositionLeft, maid.coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);
        RaycastHit2D rightHeadCheck = Raycast(new Vector2(rayXPositionRight, maid.coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);

        //用于修复头顶撞Platform时触发下蹲
        if (platformLeftCheck || platformRightCheck)
            return;

        if (leftHeadCheck || rightHeadCheck)
            maid.isHeadBlocked = true;
        else maid.isHeadBlocked = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mon4_Bullet"))
        {
            maid.maidStats.TakePlayerDamage(GameManager.Instance.mon4Stats, maid.maidStats);
            Destroy(other.gameObject);
        }
    }

}
