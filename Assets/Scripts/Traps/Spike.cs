using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private CharacterStats spikeStats;

    private void Awake()
    {
        spikeStats = GetComponent<CharacterStats>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //FIXME：Player在Trap的2/5和4/5处受伤两次
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.maidStats.TakeDamage(spikeStats, other.GetComponent<CharacterStats>());
        }

        //TODO:若Enemy在范围内，则直接死亡
        if (other.CompareTag("Enemy"))
        {

        }
    }
}
