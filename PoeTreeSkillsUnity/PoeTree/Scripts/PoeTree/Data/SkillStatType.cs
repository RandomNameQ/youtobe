using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags, Serializable]
public enum SkillStatType
{
    None = 0, // No skill stat selected (useful for default or no selection)
    
    // Damage Types
    Physic = 1 << 0,
    Fire = 1 << 1,
    Cold = 1 << 2,
    Lighting = 1 << 3,
    Poison = 1 << 4,
    Chaos = 1 << 5,
    Bleeding = 1 << 6,
    ElementalDamage = Fire | Cold | Lighting | Poison | Chaos, // Combine various elemental damage types
    
    // Offensive Stats
    AttackSpeed = 1 << 7,
    Critical = 1 << 8,
    Range = 1 << 9,
    ProjectileSpeed = 1 << 10,
    ProjectileDamage = 1 << 11,

    // Attributes
    Dexterity = 1 << 12,
    Intellect = 1 << 13,
    Strength = 1 << 14,

    // Defensive Stats
    Life = 1 << 15,
    Mana = 1 << 16,
    Charges = 1 << 17,

    // Utility Stats
    Rage = 1 << 18,
    Curses = 1 << 19,
    Summon = 1 << 20
}

