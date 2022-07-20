using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation_Maid : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController_Maid maid;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maid = GetComponent<PlayerController_Maid>();
    }

    private void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("VelocityY", rb.velocity.y);

        anim.SetBool("Ground", maid.isGround);

        anim.SetBool("Jump", maid.isJump);

        anim.SetBool("Crouch", maid.isCrouch);
        anim.SetBool("Fall", maid.isFall);

        anim.SetBool("Dashing", maid.isDashing);

        anim.SetBool("Dead", maid.isDead);

    }


    //Animation Event
    public void PlayWalk()
    {
        AudioManager.Instance.PlayerWalkAudio();
    }
    public void PlayDash()
    {
        AudioManager.Instance.PlayerDashAudio();
    }
}
