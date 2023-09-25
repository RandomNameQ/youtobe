using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpaleStat:STATBASE
{
    [Header("Impale Modifiers")]
    public float impaleChance;
    public float impaleEffectIncreased;
    public int strengthIncreased;
    public float impaleOverwhelmsPhysicalDamageReduction;

    [Header("Utility Mods")]
    public float impaleDurationIncreased;
    public float impaleChanceOnCrit;

    public float impaleEffectOnCritIncreased;
    public float impaleChanceOnHit;
    public float impaleEffectOnHitIncreased;
}
