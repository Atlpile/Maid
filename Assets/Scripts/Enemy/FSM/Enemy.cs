using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerController_DarkGirl darkGirl;
    Rigidbody2D e_rb;

    EnemyBaseState currentState;
    public PatrolState patrolState = new PatrolState();         //使用PatrolState脚本中的功能
    public AttackState attackState = new AttackState();

    [Header("Animation State")]
    public Animator anim;           //定义用于获取Animator组件的变量
    public int animState;

    [Header("Enemy的移动目标设置")]
    public float patrolSpeed;
    public float attackSpeed;
    public Transform pointA;
    public Transform pointB;
    public Transform targetPoint;
    public Transform playerPoint;

    public List<Transform> attackList = new List<Transform>();  //设置Enemy的攻击目标


    public virtual void Init()
    {
        anim = GetComponent<Animator>();
    }
    public void Awake()
    {
        Init();
    }
    private void Start()
    {
        TransitionToState(patrolState);                     //设置默认状态为巡逻状态
    }

    private void Update()
    {
        currentState.OnUpdate(this);                        //使状态中的功能持续执行
        anim.SetInteger("State", animState);                //AI状态机同步动画状态机
    }
    /// <summary>
    /// 状态的切换
    /// </summary>
    /// <param name="state">EnemyBaseState中的状态</param>
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;                               //获取EnemyBaseState的所有状态
        currentState.EnterState(this);                      //将挂载脚本的Enemy
    }



    public void MoveToTarget()
    {
        //向目标移动（当前组件的位置，目标点的位置，敌人速度的初始值）
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
        FlipDirection();    //到达目标点后翻转
    }
    /// <summary>
    /// Enemy的翻转
    /// </summary>
    public void FlipDirection()
    {
        //若Enemy在目标点的左侧
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);          //向右翻转
        else if (transform.position.x > targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);        //向左翻转
    }
    public void SwitchPoint()
    {
        //若Enemy离A点近
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
            targetPoint = pointA;
        else
            targetPoint = pointB;
    }
    public void MoveToPlayer()
    {
        //FIXME:Player与Enemy不在同一水平线时，Enemy会跳动
        // Vector2 pos = darkGirl.playerDownPoint;

        transform.position = Vector2.MoveTowards(transform.position, playerPoint.position, attackSpeed * Time.deltaTime);
        FlipDirection();

    }


    //检测攻击范围内的物体
    public void OnTriggerStay2D(Collider2D other)
    {
        if (!attackList.Contains(other.transform))
            attackList.Add(other.transform);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        attackList.Remove(other.transform);
    }
}
