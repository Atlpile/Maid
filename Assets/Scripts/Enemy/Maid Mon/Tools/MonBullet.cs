using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonBullet : MonoBehaviour
{
    public E_MonBulletType monBulletType;
    private Rigidbody2D m_rigidbody2D;
    private Animator bulletAnim;

    [Header("Straight Parameter")]
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float destroyDistance;
    [SerializeField] private float currentDistance;
    private Vector3 bulletStartPos;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        bulletAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (monBulletType == E_MonBulletType.Bullet4)
        {
            bulletAnim.Play("Run");
            m_rigidbody2D.velocity = transform.right * BulletSpeed;
            bulletStartPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        currentDistance = (transform.position - bulletStartPos).sqrMagnitude;

        if (currentDistance > destroyDistance && monBulletType == E_MonBulletType.Bullet4)
        {
            bulletAnim.Play("Destroy");
            m_rigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    //Animation Event
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}