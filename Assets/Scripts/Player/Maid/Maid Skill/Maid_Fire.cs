using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maid_Fire : MonoBehaviour
{
    //TODO:射线攻击 
    [Header("攻击类型")]
    [SerializeField] private GameObject smallBullet;
    [SerializeField] private GameObject middleBullet;

    [Header("蓄力设置")]
    public float middleBulletTime = 1f;
    public float currentTime;

    [Header("攻击位置")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector3 crouchAttackPosition;

    [Header("Reference")]
    private Animator maidAnim;
    private PlayerController_Maid maid;

    private void Start()
    {
        maidAnim = GetComponentInParent<Animator>();
        maid = GetComponentInParent<PlayerController_Maid>();
    }

    void Update()
    {
        if (maid.isHurt) return;

        //若Player蓝量>5，则可以攻击
        if (maid.maidStats.CurrentMagic > 5)
            AttackType();

        if (Input.GetButton("Fire"))
            currentTime += Time.deltaTime;
        else
            currentTime = 0;

        // if (currentTime > middleBulletTime)
        // {
        //     //TODO:播放蓄力音效
        //     //TODO:播放蓄力动画
        // }
    }

    private void AttackType()
    {
        //若按钮按下，则发射SmallBullet
        if (Input.GetButtonDown("Fire") && !maid.isCrouch)
            StandFireSmallBullet();
        else if ((Input.GetButtonDown("Fire") && maid.isCrouch))
            CrouchFireSmallBullet();

        //若按钮释放，且长按时间超过1s，则发射MiddleBullet
        if (Input.GetButtonUp("Fire") && currentTime > middleBulletTime && !maid.isCrouch)
            StandFireMiddleBullet();
        else if (Input.GetButtonUp("Fire") && currentTime > middleBulletTime && maid.isCrouch)
            CrouchFireMiddleBullet();
    }

    //站立攻击
    private void StandFireSmallBullet()
    {
        //生成预制体且获取预制体上的脚本组件，并计算蓝量消耗
        Instantiate(smallBullet, firePoint.position, firePoint.rotation);
        maid.maidStats.TakeSmallBulletMagicLoss(maid.maidStats, maid.maidStats);

        //Player攻击效果
        maidAnim.SetTrigger("Attack");
        AudioManager.Instance.PlayerFireAudio();
    }

    //下蹲攻击
    private void CrouchFireSmallBullet()
    {
        Instantiate(smallBullet, (firePoint.position + crouchAttackPosition), firePoint.rotation);
        maid.maidStats.TakeSmallBulletMagicLoss(maid.maidStats, maid.maidStats);

        maidAnim.SetTrigger("Attack");
        AudioManager.Instance.PlayerFireAudio();
    }

    //蓄力站立攻击
    private void StandFireMiddleBullet()
    {
        Instantiate(middleBullet, firePoint.position, firePoint.rotation);
        maid.maidStats.TakeMiddleBulletMagicLoss(maid.maidStats, maid.maidStats);

        maidAnim.SetTrigger("Attack");
        //TODO:播放蓄力后的攻击音效
        AudioManager.Instance.PlayerFireAudio();
    }

    //蓄力下蹲攻击
    private void CrouchFireMiddleBullet()
    {
        Instantiate(middleBullet, (firePoint.position + crouchAttackPosition), firePoint.rotation);
        maid.maidStats.TakeMiddleBulletMagicLoss(maid.maidStats, maid.maidStats);

        maidAnim.SetTrigger("Attack");
        //TODO:播放蓄力后的攻击音效
        AudioManager.Instance.PlayerFireAudio();
    }
}
