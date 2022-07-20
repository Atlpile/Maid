using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsFader : MonoBehaviour
{
    private Animator tipsAnim;

    public bool isPlayer = false;

    private void Awake()
    {
        tipsAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        tipsAnim.SetBool("IsPlayer", isPlayer);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Player在范围内，则显示提示
        if (other.CompareTag("Player"))
            isPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Player退出范围内，则淡化提示
        if (other.CompareTag("Player"))
            isPlayer = false;
    }
}
