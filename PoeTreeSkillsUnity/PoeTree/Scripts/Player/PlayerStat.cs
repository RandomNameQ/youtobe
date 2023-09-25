using System;
using System.Reflection;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public AllSkillStat Stats;

    public void UpdateSelfStat(SkillNodeStat skillStats, bool plusOrMinus)
    {
        foreach (var uiNode in skillStats.Stats.StatsType)
        {
            Type statType = uiNode.GetType();

            // Iterate through the fields of the stat object
            foreach (FieldInfo field in statType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(float))
                {
                    float value = (float)field.GetValue(uiNode);

                    if (Math.Abs(value) > float.Epsilon)
                    {
                        // Find the corresponding stat object in Stats by its type
                        STATBASE playerStat = Stats.StatsType.Find(stat => stat.GetType() == statType);

                        if (playerStat != null)
                        {
                            // Update the field in the playerStat object
                            FieldInfo playerField = statType.GetField(field.Name);
                            if (playerField != null)
                            {
                                float playerValue = (float)playerField.GetValue(playerStat);
                                playerValue += plusOrMinus ? value : -value;
                                playerField.SetValue(playerStat, playerValue);
                            }
                        }
                    }
                }
            }
        }
    }
}
