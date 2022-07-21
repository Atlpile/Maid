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
    public LayerMask groundLayer;
    public float groundCheckRadius;
    public float blockCheckRadius;


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
        // Debug.Log("追击Player");
        playerDistanceX = Vector3.Distance(player.position, transform.position);
        // playerDistanceY = player.transform.position.y - transform.position.y;

        //TODO:当Enemy离Player一定距离时，播放攻击动画
        if (playerDistanceX < 3)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            //若Player在自身的右侧，则朝向Player
            if (transform.position.x < player.transform.position.x)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            return;
        }

        if (RightMove(player))
        {
            // Debug.Log("向右移动");
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            enemyHorizontalMove = 1;
        }
        else
        {
            // Debug.Log("向左移动");
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            enemyHorizontalMove = -1;
        }

        //TODO:在SO中添加Chase的速度
        anim.Play("Run");
        rb.velocity = new Vector2(enemyHorizontalMove * monCurrentSpeed, rb.velocity.y);

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

    private void EnemyAttack()
    {

    }

    private void EnemySkill()
    {

    }

    #endregion

    #region Enemy检测

    private void EnemyCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isFrontBlock = Physics2D.OverlapCircle(blockCheck.position, blockCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(blockCheck.position, blockCheckRadius);
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
