using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Enemy_Parameter
{
    [Header("移动设置")]
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("巡逻/追击设置")]
    public Transform[] patrolPoints;
    public int patrolPosition;
    public Transform guardPoint;
    public Transform[] chasePoints;
    public Transform targetPoint;
    public Transform playerPoint;
    public Transform flyPlayerPoint;
    public float idleTime;

    public float flyPatrolRange;
    [HideInInspector] public Vector2 guardPosition;
    [HideInInspector] public Vector2 flyPoint;

    [Header("攻击设置")]
    public bool isAttck;
    public Transform attackCheck;
    public LayerMask playerLayer;
    public float attackAreaTemp;

    [Header("地面检测")]
    public bool isGround;

    [Header("Enemy状态")]
    public bool isDead;

}

public class Enemy_FSM : MonoBehaviour
{
    protected CharacterStats characterStats;
    private Enemy_BaseState currentState;
    public Enemy_Parameter ep;                 //用于获取参数

    [HideInInspector] public int animState;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D m_rigidbody2D;
    [HideInInspector] public float idleTimer;
    [HideInInspector] public AnimatorStateInfo info;

    public bool isGuard;

    //TODO:判断是否为空中敌人
    public bool isFly;

    //TODO:判断是否为BOSS


    //获取状态
    public Enemy_PatrolState patrolState = new Enemy_PatrolState();
    public Enemy_AttackState attackState = new Enemy_AttackState();
    public Enemy_ChaseState chaseState = new Enemy_ChaseState();
    public Enemy_GuardState guardState = new Enemy_GuardState();
    public Enemy_IdleState idleState = new Enemy_IdleState();
    public Enemy_FlyState flyState = new Enemy_FlyState();

    private PlayerController_Maid maid;

    #region 状态切换

    public virtual void Init()                              //组件赋值
    {
        anim = GetComponent<Animator>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        //获取另一个物体下的脚本
        maid = FindObjectOfType<PlayerController_Maid>();

    }
    private void Awake()
    {
        Init();
    }
    private void Start()                                    //参数赋值
    {
        //获取另一个物体脚本下的子物体脚本中的transform组件【即：自动获取PlayerPoint】
        ep.playerPoint = maid.GetComponentInChildren<PlayerPoint>().transform;
        ep.flyPlayerPoint = maid.GetComponentInChildren<FlyPlayerPoint>().transform;
        ep.guardPosition = transform.position;

        ep.attackAreaTemp = characterStats.AttackArea;

        //若为守卫，则切换为守卫状态
        if (isGuard)
        {
            TransitionToState(guardState);
        }
        else if (isFly)
        {
            TransitionToState(flyState);
        }
        else
        {
            TransitionToState(patrolState);
        }

    }
    public virtual void Update()                            //状态执行
    {

    }
    private void FixedUpdate()
    {
        ep.isDead = characterStats.CurrentHealth == 0;
        if (ep.isDead)
        {
            anim.Play("Dead");
            //【Bug修复】修复Enemy播放死亡动画后仍可以继续攻击
            gameObject.tag = "Untagged";
            //【优化】将销毁物体已由Animation Event事件触发
            return;
        }

        currentState.UpdateState(this);
        // anim.SetInteger("State", animState);
        AttckCheck();
    }
    public void TransitionToState(Enemy_BaseState state)    //状态切换
    {
        currentState = state;
        currentState.EnterState(this);
    }

    #endregion

    #region 巡逻状态功能： 向巡逻目标移动

    public void MoveToTarget()
    {
        //向数组中的巡逻点移动
        transform.position = Vector2.MoveTowards(
            transform.position,
            ep.patrolPoints[ep.patrolPosition].position,
            ep.patrolSpeed * Time.deltaTime
        );

        FlipDirection();
    }
    public void FlipDirection()
    {
        //若Enemy在目标点的左侧
        if (transform.position.x < ep.patrolPoints[ep.patrolPosition].position.x)
            // if (Vector2.Distance(transform.position, ep.patrolPoints[ep.patrolPosition].position) < 0.01f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);                      //向右翻转
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);                    //向左翻转
    }
    public void SwitchPoint()
    {
        //切换巡逻点
        ep.patrolPosition++;

        //若巡逻点超出数组的长度，则重置巡逻点
        if (ep.patrolPosition >= ep.patrolPoints.Length)
        {
            ep.patrolPosition = 0;
        }
    }

    #endregion

    #region 追击状态功能： 向Player移动

    public void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            ep.targetPoint.position,
            ep.chaseSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Enemy朝向Player
    /// </summary>
    /// <param name="player">Player的Transform</param>
    public void FlipToPlayer(Transform player)
    {
        if (player != null)
        {
            if (transform.position.x < player.position.x)                   //当前位置在目标的左侧
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);          //向左翻转
            else                                                            //当前位置在目标的右侧
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);        //向右翻转
        }
    }

    //判断Player是否在追击点范围内
    public bool PlayerInChasePoint()
    {
        //若Player在追击点范围内
        if (ep.targetPoint != null && ep.targetPoint.position.x >= ep.chasePoints[0].position.x || ep.targetPoint.position.x <= ep.chasePoints[1].position.x)
            return true;
        else
            return false;
    }

    //判断Enemy是否在追击点范围内
    public bool EnemyInChasePoint()
    {
        //若Enemy在追击点范围内
        if (ep.targetPoint != null && transform.position.x < ep.chasePoints[0].position.x || transform.position.x > ep.chasePoints[1].position.x)
            return true;
        else
            return false;
    }

    #endregion

    #region 守卫状态功能：向守卫点移动

    public void MoveToGuardPoint()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            ep.guardPoint.position,
            ep.patrolSpeed * Time.deltaTime
        );

        FlipToGuardPoint();
    }
    public void FlipToGuardPoint()
    {
        if (transform.position.x <= ep.guardPoint.position.x)                  //当前位置在守卫点左侧
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);                              //向左翻转
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);                            //向右翻转

    }

    #endregion

    #region 飞行状态功能：

    public void FlyToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, ep.flyPoint, ep.patrolSpeed * Time.deltaTime);

        FlipFlyDirection();
    }

    public void FlipFlyDirection()
    {
        if (transform.position.x < ep.flyPoint.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);                      //向右翻转
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);                    //向左翻转
    }

    public void FlyChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            ep.targetPoint.position,
            ep.chaseSpeed * Time.deltaTime
        );
    }

    //随机生成巡逻点
    public void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-ep.flyPatrolRange, ep.flyPatrolRange);
        float randomY = UnityEngine.Random.Range(-ep.flyPatrolRange, ep.flyPatrolRange);

        Vector3 randomPoint = new Vector2(ep.guardPosition.x + randomX, ep.guardPosition.y + randomY);

        ep.flyPoint = randomPoint;
    }

    #endregion

    #region 检测攻击距离

    public void AttckCheck()
    {
        ep.isAttck = Physics2D.OverlapCircle(ep.attackCheck.position, characterStats.AttackArea, ep.playerLayer);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ep.attackCheck.position, ep.attackAreaTemp);
    }

    #endregion

    #region 检测Player

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isFly)
        {
            ep.targetPoint = ep.flyPlayerPoint;
        }
        else if (other.CompareTag("Player"))
        {
            ep.targetPoint = ep.playerPoint;
        }

        if (other.CompareTag("Ground"))
        {
            ep.isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ep.targetPoint = null;
        }
    }

    #endregion
}
