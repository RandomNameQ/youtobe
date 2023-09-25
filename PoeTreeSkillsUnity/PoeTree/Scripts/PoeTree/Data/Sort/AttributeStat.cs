using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeStat:STATBASE
{
    [Header("Attribute Modifiers")]
    public float strength;
    public float dexterity;
    public float intellect;
    public float agility;

    [Header("Secondary Attributes")]
    public float health;
    public float mana;
    public float armor;
    public float attackDamage;

    [Header("Other Modifiers")]
    public float moveSpeed;
    public float criticalChance;
    public float criticalDamage;
}
