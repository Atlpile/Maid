using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private Animator bulletAnim;
    private Vector3 bulletStartPos;

    [Header("Bullet参数")]
    public float smallBulletSpeed;
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
        m_rigidbody2D.velocity = transform.right * smallBulletSpeed;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Enemy"))
        // {
        //     //播放Enemy受击音效
        //     AudioManager.Instance.EnemyHurtAudio();

        //     var targetStats = other.GetComponent<CharacterStats>();
        //     var bulletStats = GameManager.Instance.maidStats;
        //     targetStats.TakeDamage(bulletStats, targetStats);

        //     // PlayDestoryBulletAnim();
        //     DestroyBullet();
        // }
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
