using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlatform : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.W) && other.gameObject.CompareTag("Player"))
        {
            var platformCollider = GetComponent<BoxCollider2D>();
            platformCollider.isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //两种方法判断Trigger

        //若触发器的类型是BoxCollider，且脱离Trigger
        // if (other.GetType().ToString() == "UnityEngine.BoxCollider2D")

        if (other.GetComponentInParent<BoxCollider2D>())
        {
            var platformCollider = GetComponent<BoxCollider2D>();
            platformCollider.isTrigger = false;
        }
    }
}
