using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/Stats")]
public class SkillNodeStat : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    
    public AllSkillStat Stats;

    public SkillStatType StatType;
    public SkillStatRang StatRang; 

   
}
