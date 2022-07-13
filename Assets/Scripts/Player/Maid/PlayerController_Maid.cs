using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Maid : MonoBehaviour
{
    #region 角色参数

    private CharacterStats maidStats;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [HideInInspector] public Animator anim;

    [Header("移动参数")]
    public float currentSpeed;
    private float horizontalMove;

    [Header("跳跃参数")]
    public float jumpForce;
    public int jumpCount;

    [Header("下蹲参数")]
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    [Header("Dash参数")]
    public float dashSpeed;                 //Dash的速度
    public float dashTime;                  //Dash的时长
    public int dashMagic = 10;

    public float dashCoolDown;              //Dash的CD时间
    private float dashTimeLeft;             //Dash的剩余时间
    private float lastDash = -10f;          //上一次使用冲刺的时间点（使用负值，保证能在游戏的一开始时就能执行冲刺功能）

    [Header("头顶射线检测")]
    public float rayHeight;                 //射线y轴位置
    public float rayLength;                 //射线长度

    public float platformXRayPosition_Left;
    public float platformXRayPosition_Right;

    public float rayXPositionLeft;
    public float rayXPositionRight;

    [Header("地面检测")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float checkRadius;

    [Header("Player按键检测")]
    public bool jumpPressed;
    public bool crouchPressed;
    public bool crouchHeld;

    [Header("Player状态检测")]
    public bool isGround;
    public bool isPlatform;
    public bool isJump;
    public bool isFall;
    public bool isCrouch;
    public bool isHeadBlocked;
    public bool isHurt;
    public bool isDashing;
    public bool isDead;

    #endregion

    #region 调用区域

    private void Awake()
    {
        maidStats = GetComponent<CharacterStats>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();


    }

    void Start()
    {
        //TODO:注册和添加事件，都应放在OnEnable中
        GameManager.Instance.RegisterPlayer(maidStats);
        GameManager.Instance.RegisterPlayer(this);

        //读取Player数据
        SaveManager.Instance.LoadPlayerPositionData("PlayerPoX", "PlayerPoY");

        currentSpeed = maidStats.PlayerCurrentSpeed;

        ReadyToCrouch();
    }

    void Update()
    {
        InputCheck();
    }

    private void FixedUpdate()
    {
        //判断是否进入死亡状态
        isDead = maidStats.CurrentHealth == 0;
        if (isDead)
        {
            //【Bug修复】防止Player移动时，在死亡状态下滑行
            rb.velocity = new Vector2(0, 0);
            maidStats.CurrentHealth = 0;
            return;
        }

        HeadCheck();
        GroundCheck();

        //判断是否进入受伤状态
        isHurt = anim.GetCurrentAnimatorStateInfo(2).IsName("Hurt");
        if (isHurt) return;

        Dash();
        if (isDashing)
            return;

        Jump();
        GroundMovement();
    }

    #endregion

    #region Player基本功能 

    void GroundMovement()
    {
        CrouchState();

        horizontalMove = Input.GetAxisRaw("Horizontal");

        //Player翻转
        if (horizontalMove > 0f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else if (horizontalMove < 0f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        if (isCrouch)
            return;

        //Player移动
        rb.velocity = new Vector2(horizontalMove * currentSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        if (isCrouch)
            return;

        if (isGround || isPlatform)
        {
            jumpCount = maidStats.playerJumpCount;                                                                  //设置可跳跃的次数
            isJump = false;
            isFall = false;
        }

        if (!isGround && rb.velocity.y > 0)
        {
            isJump = true;                                                                  //添加该条件后，可实现空中跳跃 
            isFall = false;
        }
        if (!isGround && rb.velocity.y < 0)
        {
            isJump = false;
            isFall = true;
        }

        if (jumpPressed && !isJump && !isHeadBlocked && isGround)                           //实现跳跃的条件
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.Instance.PlayerJumpAudio();

            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isFall)                                    //实现空中跳跃的条件
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

    public void ReadyToCrouch()
    {
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y - 1.2f);
    }
    public void CrouchState()
    {
        //下蹲与站立之间的状态切换
        if (crouchHeld && !isCrouch && isGround)            //长按下蹲，不为下蹲状态，处于地面
            Crouch();
        // else if (!isGround && isHeadBlocked)                 //处于地面，头部被遮挡（修复了处于空中状态时执行下蹲操作）
        //     Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)  //未按下蹲，处于下蹲状态，头部未被遮挡
            StandUp();
        else if (isCrouch && !isGround)                     //处于下蹲状态，不处于地面状态
            StandUp();

        // if (isCrouch)
        //     currentSpeed = crouchSpeed;
        // else
        //     currentSpeed = moveSpeed;
    }
    public void Crouch()
    {
        isCrouch = true;
        rb.velocity = new Vector2(0, rb.velocity.y);

        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    public void StandUp()
    {
        isCrouch = false;
        currentSpeed = maidStats.PlayerOriginSpeed;
        // rb.bodyType = RigidbodyType2D.Dynamic;

        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    #endregion

    #region Player拓展功能
    void ReadyToDash()
    {
        isDashing = true;                                                               //开启冲刺状态

        dashTimeLeft = dashTime;                                                        //冲刺剩余时间，为冲刺时间（即：技能的倒计时）

        lastDash = Time.time;                                                           //上一次使用冲锋的时间点，为当前时间

        // cdImage.fillAmount = 1;                                                         //cd冷却图标还原
    }
    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)                                                       //若冲刺的剩余时间 > 0
            {
                //冲刺优化
                if (rb.velocity.y > 0 && !isGround)                                        //若y轴速度>0 且人物不在地面（处于起跳状态）
                {
                    //rb.velocity = new Vector2(dashSpeed * transform.localScale.x, jumpForce);
                    // rb.velocity = new Vector2(dashSpeed * horizontalInput, jumpForce);   //为冲刺效果添加一个向上的力（结束冲刺后会小跳一下）
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);
                }

                //【Bug修复】使用transform.localScale.x可以达到沿自身朝向冲刺的效果   【拓展：可以用做后撤效果】
                if (transform.localEulerAngles.y > 0)
                    rb.velocity = new Vector2(dashSpeed * -transform.localScale.x, rb.velocity.y);
                else
                    rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;                                         //冲刺技能倒计时开始

                ShadowPool.instance.GetFromPool();                                      //在对象池中获取残影
            }

            if (dashTimeLeft <= 0)                                                      //若冲刺的剩余时间 <= 0
            {
                isDashing = false;

                //【Bug修复】修复Player下蹲滑行时的Bug
                rb.velocity = new Vector2(0, rb.velocity.y);

                //冲刺优化
                if (!isGround)                                                          //若人物不在地面（冲刺过程中仍在空中）
                {
                    //rb.velocity = new Vector2(dashSpeed * transform.localScale.x, jumpForce);
                    // rb.velocity = new Vector2(dashSpeed * horizontalInput, jumpForce);   
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);
                }
            }
        }
    }

    #endregion

    #region Player检测

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;                                                           //获取player的坐标（中心点），定义为pos
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);            //定义射线检测信息hit：（人物中心点位置+射线起点，射线方向，长度，图层）
        Color color = hit ? Color.red : Color.green;                                                //设置射线颜色：触发射线检测则为红色，未触发射线检测则为绿色
        Debug.DrawRay(pos + offset, rayDiraction * length, color);                                  //显示射线（人物中心点位置+射线起点，射线方向，射线长度，射线颜色）

        return hit;                                                                                 //将射线检测的信息hit返回给Raycast方法
    }

    private void InputCheck()
    {
        //Jump输入检测
        if (Input.GetButtonDown("Jump") && isGround)
            jumpPressed = true;
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
            jumpPressed = true;

        //Crouch输入检测
        crouchHeld = Input.GetButton("Crouch");

        //Dash输入检测
        if (GameManager.Instance.maidStats.CurrentMagic > 10)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //TODO:Dash消耗蓝量
                if (Time.time >= (lastDash + dashCoolDown))     //若游戏当前时间 >= 上一次使用冲锋的时间点+冲刺CD时间（CD时间）
                {
                    ReadyToDash();                              //执行冲刺功能
                    GameManager.Instance.maidStats.TakeMagic(GameManager.Instance.maidStats, dashMagic);
                }
            }
    }

    private void GroundCheck()
    {
        //设置地面检测Gizmos参数（地面检测的位置，检测范围，检测图层）
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isPlatform = Physics2D.OverlapCircle(groundCheck.position, checkRadius, platformLayer);
    }

    private void HeadCheck()
    {
        //头顶检测（射线的起点，射线的方向，射线的长度，射线检测的目标图层）

        //头顶撞Platform检测
        RaycastHit2D platformLeftCheck = Raycast(new Vector2(platformXRayPosition_Left, coll.size.y - rayHeight), Vector2.up, rayLength, platformLayer);
        RaycastHit2D platformRightCheck = Raycast(new Vector2(platformXRayPosition_Right, coll.size.y - rayHeight), Vector2.up, rayLength, platformLayer);

        //头顶撞Ground检测
        RaycastHit2D leftHeadCheck = Raycast(new Vector2(rayXPositionLeft, coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);
        RaycastHit2D rightHeadCheck = Raycast(new Vector2(rayXPositionRight, coll.size.y - rayHeight), Vector2.up, rayLength, groundLayer);

        //用于修复头顶撞Platform时触发下蹲
        if (platformLeftCheck || platformRightCheck)
            return;

        if (leftHeadCheck || rightHeadCheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    #endregion
}
