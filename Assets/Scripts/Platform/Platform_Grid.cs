using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Grid : MonoBehaviour
{
    PlatformEffector2D platformEffector;

    private void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            StartCoroutine("StayOn");
    }

    IEnumerator StayOn()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                platformEffector.rotationalOffset = 180;
                StartCoroutine("Exit");
            }

            yield return null;
        }

    }

    IEnumerator Exit()
    {
        StopCoroutine("StayOn");

        yield return new WaitForSeconds(0.7f);
        platformEffector.rotationalOffset = 0;
    }
}
