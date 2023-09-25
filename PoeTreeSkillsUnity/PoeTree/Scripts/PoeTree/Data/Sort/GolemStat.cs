using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GolemStat:STATBASE
{
    [Header("Golem Modifiers")]
    public int numberOfGolems;
    public float buffEffectIncreased;
    public float maximumLifeIncreased;
    public float damageWithSummonedGolemIncreased;
    public float golemElementalResistance;

    [Header("Utility Mods")]
    public float golemChaosDamageResists;
    public float golemPhysicalDamageResists;
    public float golemElementalDamageResists;
    public float golemStatusAilmentImmunity;

    public float golemAggroRadius;
    public float golemMovementSpeed;
}
