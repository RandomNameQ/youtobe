using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedStat:STATBASE
{
    [Header("Speed Modifiers")]
    public float movementSpeedIncreased;
    public float attackSpeedIncreased;
    public float castSpeedIncreased;
    public float projectileSpeedIncreased;
}
