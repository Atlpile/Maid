using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController_DarkGirl : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    [Header("地面检测")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float checkRadius;

    [Header("头顶射线检测")]
    public float rayHeight;         //射线y轴位置
    public float rayLength;         //射线长度
    public float rayXPositionLeft;
    public float rayXPositionRight;


    // [Header("地面射线检测")]
    // public float downRayHeight;
    // public float downRayLength;
    // public float rayXPostionMiddleDown;
    // // public GameObject playerPoint;
    // // public GameObject playerPosition;
    // public Vector3 playerDownPoint;

    [Header("移动设置")]
    public float currentSpeed;
    public float moveSpeed;
    private float horizontalMove;

    [Header("跳跃设置")]
    public float jumpForce;
    private int jumpCount;

    [Header("下蹲设置")]
    public float crouchSpeed = 3f;
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    [Header("Player状态检测")]
    public bool isGround;
    public bool isPlatform;
    public bool isJump;
    public bool jumpPressed;
    public bool isFall;
    public bool isCrouch;
    public bool crouchPressed;
    public bool crouchHeld;
    public bool isHeadBlocked;
    public bool isHurt;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        currentSpeed = moveSpeed;
        ReadyToCrouch();

    }
    void Update()
    {
        InputCheck();
    }
    private void FixedUpdate()
    {
        GroundMovement();
        Jump();
        GroundCheck();
        RayCastCheck();
    }

    #region Player可用的操作
    /// <summary>
    /// 左右移动
    /// </summary>
    void GroundMovement()
    {
        CrouchState();
        //FIXME:下蹲会导致滑行
        // if(isCrouch) 
        // {
        //     currentSpeed = 0;
        //     return;
        // }


        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1
        rb.velocity = new Vector2(horizontalMove * currentSpeed, rb.velocity.y);

        // if (horizontalMove != 0)
        //     // transform.localScale = new Vector3(horizontalMove, 1, 1);
        //     transform.localRotation = Quaternion.Euler(0, 0, 0);
        // else
        //     transform.localRotation = Quaternion.Euler(0, 180, 0);

        bool plyerHasXAxisSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (plyerHasXAxisSpeed)
        {
            if (rb.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (rb.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

    }
    /// <summary>
    /// 跳跃
    /// </summary>
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;                                                                  //设置可跳跃的次数
            isJump = false;
            isFall = false;
        }
        if (!isGround)
        {
            isJump = true;                                                                  //添加该条件后，可实现空中跳跃  
            isFall = true;
        }

        if (jumpPressed && isGround && !isJump && !isHeadBlocked)                                      //实现跳跃的条件
        {
            // isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            AudioManager.Instance.PlayerJumpAudio();

            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)                                    //实现空中跳跃的条件
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            AudioManager.Instance.PlayerJumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
        else
        {
            jumpPressed = false;                                                            //修复人物在空中，按跳跃键时，jumpPress=true的情况
        }
    }
    /// <summary>
    /// 下蹲时Collider2D的大小设置
    /// </summary>
    public void ReadyToCrouch()
    {
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y - 1.2f);
    }
    /// <summary>
    /// 下蹲与站立的状态切换
    /// </summary>
    public void CrouchState()
    {
        //下蹲与站立之间的状态切换
        if (crouchHeld && isGround && !isCrouch)
            Crouch();
        else if (isHeadBlocked)
            Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
            StandUp();
        else if (isCrouch && !isGround)
            StandUp();

        if (isCrouch)
            currentSpeed = crouchSpeed;
        else
            currentSpeed = moveSpeed;
    }
    /// <summary>
    /// 下蹲
    /// </summary>
    public void Crouch()
    {
        isCrouch = true;                        //player的下蹲状态开启
        //使用下蹲时碰撞体的大小和位置
        coll.size = colliderCrouchSize;         //设置player下蹲时碰撞体的size，为colliderCrouchSize
        coll.offset = colliderCrouchOffset;     //设置player下蹲时碰撞体的offset，为colliderCrouchOffset
    }
    /// <summary>
    /// 站起
    /// </summary>
    public void StandUp()
    {
        isCrouch = false;                       //player的下蹲状态关闭
        //使用站立时碰撞体的大小和位置
        coll.size = colliderStandSize;          //设置player站立时碰撞体的size，为colliderCrouchSize
        coll.offset = colliderStandOffset;      //设置player站立时碰撞体的offset，为colliderCrouchOffset
    }

    #endregion

    #region 检测功能
    /// <summary>
    /// 按键输入检测
    /// </summary>
    void InputCheck()
    {
        //跳跃按键输入检测
        if (Input.GetButtonDown("Jump") && isGround)                                            //若跳跃键被按下，且在地面上    由于在InputManager中可以获取Jump预设，故使用GetButtonDown()
            jumpPressed = true;                                                                 //跳跃按键按下状态开启
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
            jumpPressed = true;

        //下蹲按键输入检测
        crouchHeld = Input.GetButton("Crouch");
        if (Input.GetButtonDown("Crouch"))                                                      //若按下下蹲按键
            crouchPressed = true;                                                               //触发瞬按下蹲状态
        else
            crouchPressed = false;
    }
    /// <summary>
    /// 地面检测
    /// </summary>
    void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);     //设置地面检测参数（地面检测的位置，检测范围，检测图层）
        isPlatform = Physics2D.OverlapCircle(groundCheck.position, checkRadius, platformLayer);

        // RaycastHit2D playerGroundPosition = Raycast(new Vector2(rayXPostionMiddleDown, coll.size.y - downRayHeight), Vector2.down, downRayLength, groundLayer);

        //地面生成PlayerPoint
        // RaycastHit2D downCheck = Raycast(new Vector2(0f, coll.size.y - downRayHeight), Vector2.down, downRayLength, groundLayer);
        // Debug.Log(downCheck.point);
        // playerDownPoint = downCheck.point;


        if (isGround)                                                                           //若在地面上
        {
            //rb.gravityScale = 1;                                                                //人物重力为1（Bug：当人物不处于触地状态，会发生重力为1的跳跃）
            isJump = false;                                                                     //跳跃状态关闭
        }
    }
    /// <summary>
    /// 射线检测（用于下蹲）
    /// </summary>
    void RayCastCheck()
    {
        //头顶检测（射线的起点，射线的方向，射线的长度，射线检测的目标图层）
        RaycastHit2D leftHeadCheck = Raycast(new Vector2(rayXPositionLeft, coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);
        RaycastHit2D rightHeadCheck = Raycast(new Vector2(rayXPositionRight, coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);
        //FIXME:头顶检测单向平台时，会触发下蹲

        if (leftHeadCheck || rightHeadCheck)                  //若触发头顶射线检测
            isHeadBlocked = true;       //player处于遮挡头部状态开启
        else isHeadBlocked = false;     //player处于遮挡头部状态关闭



    }

    //射线预设
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;                                                           //获取player的坐标（中心点），定义为pos
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);            //定义射线检测信息hit：（人物中心点位置+射线起点，射线方向，长度，图层）
        Color color = hit ? Color.red : Color.green;                                                //设置射线颜色：触发射线检测则为红色，未触发射线检测则为绿色
        Debug.DrawRay(pos + offset, rayDiraction * length, color);                                  //显示射线（人物中心点位置+射线起点，射线方向，射线长度，射线颜色）

        return hit;                                                                                 //将射线检测的信息hit返回给Raycast方法
    }
    //显示检测范围
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);                               //显示检测范围（地面检测的自身位置，半径范围）
    }
    #endregion


}
