using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Bullet1;
    public Vector3 crouchShootPosition;
    private PlayerController_DarkGirl darkGirl;
    private Animator darkGirlAnim;



    void Start()
    {
        darkGirl = GetComponentInParent<PlayerController_DarkGirl>();
        darkGirlAnim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //射击输入检测
        if (Input.GetButtonDown("Fire") && !darkGirl.isCrouch)
            Player_StandFire();
        else if (Input.GetButtonDown("Fire") && darkGirl.isCrouch)
            Player_CrouchFire();
    }

    public void Player_StandFire()
    {
        Instantiate(Bullet1, transform.position, transform.rotation);
        darkGirlAnim.SetTrigger("Fire");
        AudioManager.Instance.PlayerFireAudio();
    }

    public void Player_CrouchFire()
    {
        Instantiate(Bullet1, (transform.position + crouchShootPosition), transform.rotation);
        darkGirlAnim.SetTrigger("Fire");
        AudioManager.Instance.PlayerFireAudio();
    }
}
