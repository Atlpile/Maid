using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private Animator bulletAnim;
    private Vector3 bulletStartPos;

    [Header("Bullet参数")]
    public float BulletSpeed;
    public float destroyDistance;

    [Header("子弹状态判断")]
    public bool isDestroyed;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        bulletAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        m_rigidbody2D.velocity = transform.right * BulletSpeed;
        bulletStartPos = transform.position;
    }

    private void Update()
    {
        float distance = (transform.position - bulletStartPos).sqrMagnitude;
        if (distance > destroyDistance)
        {
            //播放销毁动画，并销毁Bullet
            PlayDestoryBulletAnim();
        }

        //TODO:当子弹碰到屏幕边界时，销毁Bullet
    }

    private void PlayDestoryBulletAnim()
    {
        isDestroyed = true;
        bulletAnim.SetBool("isDestroyed", isDestroyed);
        m_rigidbody2D.velocity = new Vector2(0, 0);
    }

    //Animation Event：销毁该Bullet
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
