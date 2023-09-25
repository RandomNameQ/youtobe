using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnslaughtStat:STATBASE
{
    [Header("Onslaught Modifiers")]
    public float onslaughtDurationIncreased;
    public float onslaughtEffectIncreased;
    public float chanceToGainOnslaughtOnKill;
}
