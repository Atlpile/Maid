using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    private Animator saveAnim;

    private void Start()
    {
        saveAnim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.E) && other.gameObject.CompareTag("Player"))
        {
            saveAnim.Play("save");
        }
    }
}
