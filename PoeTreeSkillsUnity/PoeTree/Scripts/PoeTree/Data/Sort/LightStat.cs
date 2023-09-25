using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightStat:STATBASE
{
    [Header("Lightning Damage Modifiers")]
    public float lightningIncreased;
    public float lightningMultiplier;
    public float convertLightningToFire;
    public float convertLightningToCold;
    public float convertLightningToPhysical;

    [Header("Damage over Time Modifiers")]
    public float lightningDamageOverTimeIncreased;
    public float lightningDamageOverTimeMore;
    public float lightningDamageSpeedIncreased;

    [Header("Resistance Penetration")]
    public float lightningResistancePenetration;

    [Header("Utility Mods")]
    public float shockEffectIncreased;
    public float electrifyEffectIncreased;
    public float shockChance;

    public float chanceToChain;
    public float lightningDamageAreaOfEffectIncreased;
    public float lightningAilmentDurationIncreased;
}
