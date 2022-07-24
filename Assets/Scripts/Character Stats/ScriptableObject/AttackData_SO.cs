using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Player专属攻击参数")]
    public int smallBulletDamage;
    public int middleBulletDamage;
    public int smallBulletMagicLoss;
    public int middleBulletMagicLoss;

    [Header("通用攻击参数")]
    public int damage;
    public float attackArea;

}
