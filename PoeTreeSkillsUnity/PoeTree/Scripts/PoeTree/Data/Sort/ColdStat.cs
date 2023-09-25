using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColdStat:STATBASE
{
    [Header("Cold Damage Modifiers")]
    public float coldIncreased;
    public float coldMultiplier;
    public float convertColdToFire;
    public float convertColdToLighting;
    public float convertColdToPhysically;

    [Header("Damage over Time Modifiers")]
    public float coldDamageOverTimeIncreased;
    public float coldDamageOverTimeMore;
    public float coldDamageSpeedIncreased;

    [Header("Resistance Penetration")]
    public float coldResistancePenetration;


    [Header("Utility Mods")]
    public float chillEffectIncreased;
    public float freezeEffectIncreased;
    public float freezeChance;

    public float chanceToExplosion;
    public float coldDamageAreaOfEffectIncreased;
    public float coldAilmentDurationIncreased;
}