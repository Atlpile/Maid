using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Y : MonoBehaviour
{
    public float speed;
    public float waitTime;
    public Transform pointA;
    public Transform pointB;
    public Transform targetPoint;
    private Transform playerTransform;

    private void Start()
    {
        targetPoint = pointB;
        SwitchPoint();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }
    private void Update()
    {
        MoveToTarget();
    }

    public void MoveToTarget()
    {
        //向目标移动（当前组件的位置，目标点的位置，敌人速度的初始值）
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        //若自身离目标点的距离接近于0）
        if (Mathf.Abs(transform.position.y - targetPoint.position.y) < 0.01f)
        {
            Invoke("SwitchPoint", waitTime);
        }
    }
    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.y - transform.position.y) > Mathf.Abs(pointB.position.y - transform.position.y))
            targetPoint = pointA;
        else
            targetPoint = pointB;
    }


    //使Player在平台上移动
    void OnTriggerEnter2D(Collider2D other)
    {
        //若BoxCollider2D检测到Player
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = playerTransform;
        }
    }
}
