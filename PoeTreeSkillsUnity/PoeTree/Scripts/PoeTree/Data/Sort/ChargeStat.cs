using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChargeStat:STATBASE
{
    [Header("Charge Modifiers")]
    public float powerCharges;
    public float frenzyCharges;
    public float enduranceCharges;
    public float maxCharges;

    [Header("Charge Effects")]
    public float powerChargeEffect;
    public float frenzyChargeEffect;
    public float enduranceChargeEffect;
}
