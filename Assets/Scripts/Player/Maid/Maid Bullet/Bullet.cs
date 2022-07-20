using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float destroyDistance;
    public bool isDestroyed;

    private Rigidbody2D m_rigidbody2D;
    private Animator anim_bullet;
    private Vector3 startPos;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        anim_bullet = GetComponent<Animator>();

        m_rigidbody2D.velocity = transform.right * speed;
        startPos = transform.position;
    }

    private void Update()
    {
        float distance = (transform.position - startPos).sqrMagnitude;
        if (distance > destroyDistance)
        {
            //播放销毁动画，并销毁Bullet
            PlayDestoryBulletAnim();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //播放Enemy受击音效
            AudioManager.Instance.EnemyHurtAudio();

            var targetStats = other.GetComponent<CharacterStats>();
            var bulletStats = GameManager.Instance.maidStats;
            targetStats.TakeDamage(bulletStats, targetStats);
            // Debug.Log("Yes");

            // PlayDestoryBulletAnim();
            DestroyBullet();
        }
    }

    public void PlayDestoryBulletAnim()
    {
        isDestroyed = true;
        anim_bullet.SetBool("isDestroyed", isDestroyed);
        m_rigidbody2D.velocity = new Vector2(0, 0);
    }

    //Animation Event：销毁该Bullet
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
