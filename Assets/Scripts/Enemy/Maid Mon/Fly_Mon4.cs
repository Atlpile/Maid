using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Mon4 : SimpleEnemy
{
    [Header("Mon4 Move")]
    [SerializeField] private float moveTimer;
    [SerializeField] private float moveSpeed = 0.05f;
    [SerializeField] private float moveRange = 2f;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterMon4CharacterStats(monStats);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        UpAndDown();
    }

    //Enemy上下移动
    private void UpAndDown()
    {
        moveTimer += Time.fixedDeltaTime * moveRange;
        float y = Mathf.Sin(moveTimer) * moveSpeed;         //Sin函数从-1，1循环移动
        transform.Translate(new Vector3(0f, y, 0f));
    }

    protected override void EnemyCheck()
    {

    }

    protected override void OnDrawGizmos()
    {

    }

    //Animation Event
    public void Mon1DeathAudio()
    {
        AudioManager.Instance.EnemyDeathAudio();
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
