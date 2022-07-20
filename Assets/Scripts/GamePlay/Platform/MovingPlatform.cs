using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
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
        //向目标移动（当前组件的位置，目标点的位置，移动速度）
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        //若自身离目标点的距离接近于0）
        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.01f)
            Invoke("SwitchPoint", waitTime);

    }
    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
            targetPoint = pointA;
        else
            targetPoint = pointB;
    }


    //使Player在平台上移动
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = playerTransform;
        }
    }

}
