using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    private CharacterStats mon1Stats;
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    [Header("Enemy类型设置")]
    public E_SimpleEnemyType enemyTypes;
    public bool isGuard;

    [Header("移动朝向")]
    private float monCurrentSpeed;
    private int enemyHorizontalMove;
    public bool facingRight = true;

    [Header("Enemy状态")]
    public bool isPatrol;
    public bool isChasing;
    public bool findPlayer;
    public bool canAttack;
    public bool isGround;
    public bool isFrontBlock;

    [Header("Player Check")]
    public Transform player;
    public float playerDistanceX;
    public float playerDistanceY;

    [Header("Check")]
    public Transform groundCheck;
    public Transform blockCheck;
    public Transform attackCheck;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float groundCheckRadius;
    public float blockCheckRadius;
    public float attackCheckRadius;


    [Header("攻击设置")]
    [SerializeField] private float starttimeBetweenAttacks;		//开始攻击间隔时间
    [SerializeField] private float timeBetweenAttacks;			//攻击间隔时间

    private void Awake()
    {
        mon1Stats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        monCurrentSpeed = mon1Stats.CurrentSpeed;
        // timeBetweenAttacks = starttimeBetweenAttacks;

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        EnemyCheck();

        SimpleEnemyState();

    }

    private void SimpleEnemyState()
    {
        switch (enemyTypes)
        {
            case E_SimpleEnemyType.Ground:
                if (findPlayer == true && isGround) { EnemyChase(); return; }

                if (isGuard == false) EnemyPatrol();
                else if (isGuard == true) EnemyGuard();

                // else if (canAttack == true) EnemyAttack();

                break;
            case E_SimpleEnemyType.Fly:
                break;
        }
    }

    #region Enemy基本功能

    private void EnemyPatrol()
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
        rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);
    }

    private void EnemyGuard()
    {
        mon1Stats.CurrentSpeed = 0;
        // Debug.Log("Enemy处于Guard模式");

        //TODO:若不在守卫点，则回到守卫点
    }

    private void EnemyChase()
    {
        playerDistanceX = Vector3.Distance(player.position, transform.position);
        // playerDistanceY = player.transform.position.y - transform.position.y;


        //若Player在攻击范围内，且攻击冷却已过，则朝向Player执行攻击
        if (playerDistanceX <= 3 && timeBetweenAttacks <= 0f)
        {
            //若Player在自身的右侧，则朝向Player
            if (transform.position.x < player.transform.position.x)
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
        //若Player不在攻击范围内，则向Player移动
        else if (playerDistanceX > 3)
        {
            if (RightMove(player))
            {
                // Debug.Log("向右移动");
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                enemyHorizontalMove = 1;

                //TODO:在SO中添加Chase的速度
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

        timeBetweenAttacks -= Time.deltaTime;

    }

    public bool RightMove(Transform playerTransfrom)
    {
        //若Player在自身的右侧，则向右移动为真
        if (transform.position.x < playerTransfrom.transform.position.x)
            return true;

        //否则向右移动为假，则向左移动
        else
            return false;
    }

    private void SwitchDirection()
    {
        if (!isGround || isFrontBlock)
        {
            rb.velocity = new Vector2(0, 0);
            facingRight = !facingRight;
        }

    }

    #endregion

    #region Enemy攻击功能

    protected virtual void EnemyAttack()
    {
        //若计时器<=0，则播放Attack动画
        if (timeBetweenAttacks <= 0f)
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

    protected virtual void EnemySkill()
    {

    }

    #endregion

    #region Enemy检测

    private void EnemyCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isFrontBlock = Physics2D.OverlapCircle(blockCheck.position, blockCheckRadius, groundLayer);
        canAttack = Physics2D.OverlapCircle(attackCheck.position, attackCheckRadius, playerLayer);
    }

    private void OnDrawGizmos()
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
            mon1Stats.TakeEnemySmallBulletDamage(maidStats, mon1Stats);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("MiddleBullet"))
        {
            AudioManager.Instance.EnemyHurtAudio();

            var maidStats = GameManager.Instance.maidStats;
            mon1Stats.TakeEnemyMiddleBulletDamage(maidStats, mon1Stats);

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
