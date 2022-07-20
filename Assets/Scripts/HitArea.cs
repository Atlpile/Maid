using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{

    //Animation Check
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var targetStats = other.GetComponent<CharacterStats>();
            var characterStats = GetComponentInParent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
