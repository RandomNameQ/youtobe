using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireStat:STATBASE
{
    [Header("Fire Damage Modifiers")]
    public float fireIncreased;
    public float fireMultiplier;
    public float convertFireToCold;
    public float convertFireToLighting;
    public float convertFireToPhysical;

    [Header("Damage over Time Modifiers")]
    public float fireDamageOverTimeIncreased;
    public float fireDamageOverTimeMore;
    public float fireDamageSpeedIncreased;

    [Header("Resistance Penetration")]
    public float fireResistancePenetration;

    [Header("Utility Mods")]
    public float igniteEffectIncreased;
    public float burningEffectIncreased;
    public float igniteChance;

    public float chanceToExplosion;
    public float fireDamageAreaOfEffectIncreased;
    public float fireAilmentDurationIncreased;
}
