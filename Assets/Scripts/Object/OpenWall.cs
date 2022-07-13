using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWall : MonoBehaviour
{
    public Animator wallAnim;     //用于获取特定的门
    private Animator buttonAnim;

    private void Start()
    {
        buttonAnim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.E) && other.gameObject.CompareTag("Player"))
        {
            //通过按钮实现开门功能，播放按钮和开门动画
            buttonAnim.Play("open");
            wallAnim.Play("open");
        }
    }




}
