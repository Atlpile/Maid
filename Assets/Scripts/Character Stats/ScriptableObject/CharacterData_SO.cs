using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("角色通用属性")]
    public int maxHealth;           //血量上限
    public int currentHealth;       //当前血量
    public int maxDefence;         //防御上限
    public int currentDefence;      //当前防御

    [Header("Player专有属性")]
    public float originSpeed;
    public float currentSpeed;
    public int playerJumpCount;
    public int maxMagic;
    public int currentMagic;

    [Header("Enemy专有属性")]
    public float patrolSpeed;
    public float chasePseed;
}
