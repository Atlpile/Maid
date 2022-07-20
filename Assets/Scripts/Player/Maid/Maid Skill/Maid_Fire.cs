using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid_Fire : MonoBehaviour
{
    //TODO:中蓄力攻击      
    //TODO:大蓄力攻击 

    public GameObject smallBullet;
    public Vector3 crouchAttackPosition;
    private Animator maidAnim;
    private PlayerController_Maid maid;

    private void Start()
    {
        maidAnim = GetComponentInParent<Animator>();
        maid = GetComponentInParent<PlayerController_Maid>();
    }

    void Update()
    {
        //若Player受伤，则不执行攻击
        if (maid.isHurt)
            return;

        //若Player蓝量>5，则可以攻击
        if (GameManager.Instance.maidStats.CurrentMagic > 5)
        {
            if (Input.GetButtonDown("Fire") && !maid.isCrouch)
                Player_StandFire();
            else if (Input.GetButtonDown("Fire") && maid.isCrouch)
                Player_CrouchFire();
        }
    }

    public void Player_StandFire()
    {
        //生成预制体且获取预制体上的脚本组件，并计算蓝量消耗
        var smallBullet_characterStats = GameManager.Instance.maidStats;
        Instantiate(smallBullet, transform.position, transform.rotation);
        GameManager.Instance.maidStats.TakeMagic(GameManager.Instance.maidStats, smallBullet_characterStats);

        //攻击效果
        maidAnim.SetTrigger("Attack");
        AudioManager.Instance.PlayerFireAudio();
    }

    public void Player_CrouchFire()
    {
        var smallBullet_characterStats = GameManager.Instance.maidStats;
        Instantiate(smallBullet, (transform.position + crouchAttackPosition), transform.rotation);
        GameManager.Instance.maidStats.TakeMagic(GameManager.Instance.maidStats, smallBullet_characterStats);

        //攻击效果
        maidAnim.SetTrigger("Attack");
        AudioManager.Instance.PlayerFireAudio();
    }
}
