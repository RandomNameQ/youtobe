using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CriticalStat:STATBASE
{
    [Header("Critical Modifiers")]
    public float criticalChance;
    public float criticalMultiplier;
}
