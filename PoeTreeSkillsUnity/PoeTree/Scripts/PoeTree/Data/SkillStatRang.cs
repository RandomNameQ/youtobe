using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillStatRang
{
    public int SpawnChance;
    public StatRang SkillRang;
}

[System.Serializable]
public enum StatRang
{
    SSSR,
    SS,
    S,
    A,
    B,
    C,
    D,
    E,
    F
}
