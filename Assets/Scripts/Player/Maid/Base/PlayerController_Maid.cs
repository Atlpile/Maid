using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Maid : MonoBehaviour
{
    #region 角色参数

    [Header("Reference")]
    [HideInInspector] public CharacterStats maidStats;
    [HideInInspector] public PlayerCheck_Maid maidCheck;
    [HideInInspector] public Maid_Dash maidDash;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D coll;
    [HideInInspector] public Animator anim;

    [Header("移动监视")]
    public float currentSpeed;
    [HideInInspector] public float playerHorizontalMove;

    [Header("跳跃参数")]
    public float jumpForce;
    public int jumpCount;

    [Header("下蹲参数")]
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    [Header("Player按键检测及监视")]
    public bool jumpPressed;
    public bool crouchHeld;

    [Header("Player状态检测及监视")]
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
        maidCheck = GetComponent<PlayerCheck_Maid>();
        maidDash = GetComponent<Maid_Dash>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        //TODO:注册和添加事件，都应放在OnEnable中
        GameManager.Instance.RegisterPlayer(maidStats);
        GameManager.Instance.RegisterPlayer(this);

        //加载场景时获取Player保存的位置
        SaveManager.Instance.LoadPlayerPositionData("PlayerPoX", "PlayerPoY");

        currentSpeed = maidStats.CurrentSpeed;

        ReadyToCrouch();
    }

    void Update()
    {
        if (maidCheck.enabled)
            maidCheck.InputCheck();
    }

    private void FixedUpdate()
    {
        //判断是否进入死亡状态
        isDead = maidStats.CurrentHealth == 0;
        if (isDead)
        {
            //TODO:修复死亡时的重力
            //【Bug修复】防止Player移动时，在死亡状态下滑行 
            rb.velocity = new Vector2(0, rb.velocity.y);
            maidStats.CurrentHealth = 0;
            return;
        }

        if (maidCheck.enabled)
        {
            maidCheck.HeadCheck();
            maidCheck.GroundCheck();
        }

        //判断是否进入受伤状态
        isHurt = anim.GetCurrentAnimatorStateInfo(2).IsName("Hurt");
        if (isHurt) return;

        maidDash.Dash();
        if (isDashing) return;


        Jump();
        GroundMovement();
    }

    #endregion

    #region Player基本功能 

    void GroundMovement()
    {
        CrouchStateCheck();

        playerHorizontalMove = Input.GetAxisRaw("Horizontal");

        //Player翻转
        if (playerHorizontalMove > 0f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else if (playerHorizontalMove < 0f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        if (isCrouch)
            return;

        //Player移动
        rb.velocity = new Vector2(playerHorizontalMove * currentSpeed, rb.velocity.y);
    }

    public void JumpStateCheck()
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
    }

    public void Jump()
    {
        JumpStateCheck();

        //FIXME:修复跳跃时，播放下蹲动画

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

    public void CrouchStateCheck()
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
    }

    private void Crouch()
    {
        isCrouch = true;
        rb.velocity = new Vector2(0, rb.velocity.y);

        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    private void StandUp()
    {
        isCrouch = false;
        currentSpeed = maidStats.OriginSpeed;

        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    #endregion

}
