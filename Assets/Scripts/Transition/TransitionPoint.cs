using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public E_TransitionType transitionType;
    public E_DestinationTag destinationTag;
    public bool canTrans;

    void Update()
    {
        //传送门按键检测
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            //将自身脚本信息传送
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canTrans = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;       //不可传送
    }
}
