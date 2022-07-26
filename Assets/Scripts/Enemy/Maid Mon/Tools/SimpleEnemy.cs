using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{

    #region Enemy参数

    [Header("Reference")]
    protected CharacterStats monStats;
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    [Header("Enemy Type")]
    public E_SimpleEnemyType enemyTypes;
    [SerializeField] private bool isGuard;
    [SerializeField] private bool isPatrol;

    [Header("移动和朝向监视")]
    [SerializeField] private float monCurrentSpeed;
    private float monPatrolSpeed;
    private float monChaseSpeed;
    private int enemyHorizontalMove;
    [SerializeField] private bool facingRight = true;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    [Header("Enemy状态监视")]
    // [SerializeField] private bool isPatroling;
    // [SerializeField] private bool isChasing;
    [SerializeField] private bool findPlayer;
    [SerializeField] protected bool canAttack;
    [SerializeField] private bool isGround;
    [SerializeField] private bool isFrontBlock;
    [SerializeField] private bool isDead;

    [Header("获取Player的位置")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerPoint;
    // [SerializeField] private float playerDistanceX;
    // [SerializeField] private float playerDistanceY;

    [Header("Enemy的各种检测")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform blockCheck;
    [SerializeField] protected Transform attackCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float blockCheckRadius;
    [SerializeField] protected float attackCheckRadius;

    [Header("飞行巡逻监视")]
    [SerializeField] protected Transform patrolArea;
    [SerializeField] protected float flyPatrolRange;
    [SerializeField] private Vector2 flyPatrolPoint;

    [Header("Enemy Guard")]
    [SerializeField] private Vector2 guardPoint;

    [Header("Enemy的远程攻击设置")]
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Transform firePoint;

    [Header("攻击CD监视")]
    [SerializeField] private float starttimeBetweenAttacks;		//开始攻击间隔时间
    [SerializeField] private float timeBetweenAttacks;          //攻击间隔时间

    #endregion

    #region 调用区域

    protected virtual void Awake()
    {
        monStats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerPoint = GameObject.FindGameObjectWithTag("PlayerPoint").GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        monCurrentSpeed = monStats.PatrolSpeed;
        monPatrolSpeed = monStats.PatrolSpeed;
        monChaseSpeed = monStats.ChaseSpeed;

        flyPatrolPoint = transform.position;
        guardPoint = transform.position;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        isDead = monStats.CurrentHealth == 0;
        if (isDead)
        {
            anim.Play("Dead");
            coll.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        EnemyCheck();

        SimpleEnemyState();
    }

    /// <summary>
    /// Enemy简单状态切换
    /// </summary>
    private void SimpleEnemyState()
    {
        switch (enemyTypes)
        {
            case E_SimpleEnemyType.Ground:

                if (findPlayer && isGround) { GroundChase(); return; }

                if (isPatrol && !isGuard) GroundPatrol();
                else if (isGuard && !isPatrol) EnemyGuard();

                break;

            case E_SimpleEnemyType.Fly:

                if (findPlayer && isPatrol && !isGuard) { FlyChase(); return; }
                else if (findPlayer && isGuard && !isPatrol) { FlyGuardEnemyAttack(); return; }

                if (isPatrol && !isGuard) FlyPatrol();
                else if (isGuard && !isPatrol) EnemyGuard();

                break;

            case E_SimpleEnemyType.Static:

                break;
        }
    }

    #endregion

    #region Enemy通用功能


    private void EnemyGuard()
    {
        monStats.CurrentSpeed = 0;
        anim.Play("Idle");

        //TODO:若不在守卫点，则回到守卫点
    }

    private bool IsRight(Transform playerTransfrom)
    {
        //若Player在自身的右侧，则向右移动为真
        if (transform.position.x < playerTransfrom.transform.position.x)
            return true;

        //否则向右移动为假，则向左移动
        else
            return false;
    }


    #endregion

    #region Ground Enemy功能

    private void SwitchDirection()
    {
        if (isFrontBlock)
        {
            rb.velocity = new Vector2(0, 0);
            facingRight = !facingRight;
        }
    }

    private void GroundPatrol()
    {
        SwitchDirection();

        if (!facingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            enemyHorizontalMove = -1;
        }
        else if (facingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            enemyHorizontalMove = 1;
        }

        anim.Play("Run");
        monCurrentSpeed = monPatrolSpeed;
        rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
    }

    private void GroundChase()
    {
        if (canAttack && timeBetweenAttacks <= 0f)
        {
            GroundChaseAttack();
        }
        else if (!canAttack)
        {
            GroundChaseMovement();
        }

        timeBetweenAttacks -= Time.deltaTime;
    }

    private void GroundChaseMovement()
    {
        monCurrentSpeed = monChaseSpeed;

        if (IsRight(player))
        {
            // Debug.Log("向右移动");
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            enemyHorizontalMove = 1;

            anim.Play("Run");
            rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
        }
        else
        {
            // Debug.Log("向左移动");
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            enemyHorizontalMove = -1;

            anim.Play("Run");
            rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
        }
    }

    protected virtual void GroundChaseAttack()
    {
        //若Player在自身的右侧，则朝向Player
        if (IsRight(player))
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (canAttack)
        {
            Debug.Log("攻击一次");
            anim.SetTrigger("Attack");

            timeBetweenAttacks = starttimeBetweenAttacks;
        }

    }


    // private void GroundChase1()
    // {
    //     playerDistanceX = Vector3.Distance(player.position, transform.position);

    //     //若Player在攻击范围内，且攻击冷却已过，则朝向Player执行攻击
    //     if (playerDistanceX <= 3 && timeBetweenAttacks <= 0f)
    //     {
    //         GroundChaseAttack();
    //     }
    //     //若Player不在攻击范围内，则向Player移动
    //     else if (playerDistanceX > 3)
    //     {
    //         monCurrentSpeed = monChaseSpeed;

    //         if (RightMove(player))
    //         {
    //             // Debug.Log("向右移动");
    //             transform.localRotation = Quaternion.Euler(0, 180, 0);
    //             enemyHorizontalMove = 1;

    //             anim.Play("Run");
    //             rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
    //         }
    //         else
    //         {
    //             // Debug.Log("向左移动");
    //             transform.localRotation = Quaternion.Euler(0, 0, 0);
    //             enemyHorizontalMove = -1;

    //             anim.Play("Run");
    //             rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
    //         }
    //     }

    //     timeBetweenAttacks -= Time.deltaTime;
    // }

    // private bool OutPatrolRange()
    // {
    //     //若Enemy在A点和B点范围内，则返回false，否则返回true
    //     if (transform.position.x > leftPoint.position.x && transform.position.x < rightPoint.position.x)
    //         return false;
    //     else
    //         return true;
    // }


    #endregion

    #region Fly Enemy功能

    private void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-flyPatrolRange, flyPatrolRange);
        float randomY = UnityEngine.Random.Range(-flyPatrolRange, flyPatrolRange);

        Vector3 randomPoint = new Vector2(guardPoint.x + randomX, guardPoint.y + randomY);
        flyPatrolPoint = randomPoint;

        if (transform.position.x < flyPatrolPoint.x)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);                  //向右翻转
        else
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);                    //向左翻转
    }

    private void FlyPatrol()
    {
        //到达目标点后随机获取新的巡逻点
        if (Vector2.Distance(transform.position, flyPatrolPoint) < 0.1f)
            GetNewWayPoint();

        monCurrentSpeed = monPatrolSpeed;
        transform.position = Vector2.MoveTowards(transform.position, flyPatrolPoint, monCurrentSpeed * Time.deltaTime);
    }

    private void FlyChase()
    {
        if (canAttack && timeBetweenAttacks <= 0f)
        {
            FlyChaseAttack();
        }
        else if (!canAttack)
        {
            FlyChaseMovement();
        }

        timeBetweenAttacks -= Time.deltaTime;
    }

    private void FlyChaseMovement()
    {
        monCurrentSpeed = monChaseSpeed;

        //若Player在自身的右侧，则朝向Player
        if (IsRight(player))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //向设定的PlayerPoint移动
        anim.Play("Idle");
        transform.position = Vector2.MoveTowards(transform.position, playerPoint.position, monCurrentSpeed * Time.deltaTime);

    }

    private void FlyChaseAttack()
    {
        if (IsRight(player))
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (canAttack)
        {
            Debug.Log("攻击一次");
            anim.Play("Attack");

            timeBetweenAttacks = starttimeBetweenAttacks;
        }
        else
        {
            anim.Play("Idle");
        }
    }

    protected virtual void FlyGuardEnemyAttack()
    {
        //若Player在自身的右侧，则朝向Player
        if (IsRight(player))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            firePoint.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            firePoint.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (timeBetweenAttacks <= 0f)
        {
            Debug.Log("攻击一次");
            anim.SetTrigger("Attack");
            Instantiate(enemyBullet, firePoint.position, firePoint.localRotation);
            timeBetweenAttacks = starttimeBetweenAttacks;
        }

        timeBetweenAttacks -= Time.deltaTime;
    }

    // private void FlyChase1()
    // {
    //     playerDistanceX = Vector3.Distance(playerPoint.position, transform.position);
    //     playerDistanceY = playerPoint.transform.position.y - transform.position.y;

    //     //若Player在自身的右侧，则朝向Player
    //     if (transform.position.x < player.transform.position.x)
    //         transform.localRotation = Quaternion.Euler(0, 180, 0);
    //     else
    //         transform.localRotation = Quaternion.Euler(0, 0, 0);

    //     if (playerDistanceX <= 2 && timeBetweenAttacks <= 0f)
    //     {
    //         if (canAttack)
    //         {
    //             Debug.Log("攻击一次");
    //             anim.Play("Attack");

    //             timeBetweenAttacks = starttimeBetweenAttacks;
    //         }
    //         else
    //         {
    //             anim.Play("Idle");
    //         }
    //     }
    //     else if (playerDistanceX >= 2)
    //     {
    //         anim.Play("Idle");

    //         monCurrentSpeed = monChaseSpeed;
    //         transform.position = Vector2.MoveTowards(transform.position, playerPoint.position, monCurrentSpeed * Time.deltaTime);
    //     }

    //     timeBetweenAttacks -= Time.deltaTime;
    // }


    #endregion

    #region Static Enemy功能

    protected virtual void StaticEnemyAttack()
    {
        //若计时器<=0，则播放Attack动画，生成子弹
        if (timeBetweenAttacks <= 0f && canAttack)
        {
            Debug.Log("攻击一次");
            anim.Play("Attack");
            timeBetweenAttacks = starttimeBetweenAttacks;
        }
        //若计时器>0，且Attack动画播放完成，则播放Idle动画
        else if (timeBetweenAttacks > 0f && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            timeBetweenAttacks -= Time.deltaTime;
            anim.Play("Idle");
        }
    }

    // protected virtual void EnemySkill()
    // {

    // }

    #endregion

    #region Enemy检测

    protected virtual void EnemyCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isFrontBlock = Physics2D.OverlapCircle(blockCheck.position, blockCheckRadius, groundLayer);
        canAttack = Physics2D.OverlapCircle(attackCheck.position, attackCheckRadius, playerLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(blockCheck.position, blockCheckRadius);
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SmallBullet"))
        {
            AudioManager.Instance.EnemyHurtAudio();

            var maidStats = GameManager.Instance.maidStats;
            monStats.TakeEnemySmallBulletDamage(maidStats, monStats);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("MiddleBullet"))
        {
            AudioManager.Instance.EnemyHurtAudio();

            var maidStats = GameManager.Instance.maidStats;
            monStats.TakeEnemyMiddleBulletDamage(maidStats, monStats);

            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            findPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            findPlayer = false;
        }
    }

    #endregion

}
