using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieStat:STATBASE
{
    [Header("Zombie Modifiers")]
    public float health;
    public float damage;
    public float movementSpeed;
    public float attackSpeed;
    public float armor;

    [Header("Special Abilities")]
    public bool canInfect;
    public float infectionChance;
    public float infectionDuration;
}
