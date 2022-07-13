using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation_DarkGirl : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController_DarkGirl darkGirl;

    private void Start()
    {
        //获取组件
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        darkGirl = GetComponent<PlayerController_DarkGirl>();
    }

    private void Update()
    {
        //动画参数与人物状态同步
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("VelocityY", rb.velocity.y);

        anim.SetBool("Ground", darkGirl.isGround);           //动画的ground参数与代码中isGround状态同步
        // anim.SetBool("Platform", darkGirl.isPlatform);

        anim.SetBool("Jump", darkGirl.isJump);
        anim.SetBool("Crouch", darkGirl.isCrouch);
        anim.SetBool("Fall", darkGirl.isFall);

    }

    //Animation Event
    public void PlayWalk()
    {
        AudioManager.Instance.PlayerWalkAudio();
    }
}
