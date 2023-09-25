using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicalStat:STATBASE
{
    [Header("Physical Damage Modifiers")]
    public float physicalDamageIncreased;
    public float physicalDamageMultiplier;

    [Header("Damage over Time Modifiers")]
    public float physicalDamageOverTimeIncreased;
    public float physicalDamageOverTimeMore;
    public float physicalDamageSpeedIncreased;

    [Header("Utility Mods")]
    public float stunDurationIncreased;
    public float knockbackDistanceIncreased;

    [Header("Melee Mods")]
    public float meleeAttackDamageIncreased;
    public float meleeAttackSpeedIncreased;

    [Header("Ranged Mods")]
    public float rangedAttackDamageIncreased;
    public float rangedAttackSpeedIncreased;
}
