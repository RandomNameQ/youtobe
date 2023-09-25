using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinionStat:STATBASE
{
    [Header("Minion Modifiers")]
    public float minionMaximumLifeIncreased;
    public float minionElementalResistancesIncreased;
    public float minionPhysicalDamageReductionIncreased;

    [Header("Utility Mods")]
    public float minionMovementSpeedIncreased;
    public float minionAttackSpeedIncreased;
    public float minionDamageIncreased;
    public float minionLifeRegenerationIncreased;

    [Header("Summoning Mods")]
    public int maximumMinions;
    public float minionBuffEffectIncreased;
    public float minionMaximumLifeIncreased2;
}
