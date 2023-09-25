using System.Collections.Generic;

[System.Serializable]
public class AllSkillStat
{
    public List<STATBASE> StatsType = new List<STATBASE>();

    // Create instances of your stat classes and add them to the list
    public FireStat fireStat = new FireStat();
    public ColdStat coldStat = new ColdStat();
    public LightStat lightningStat = new LightStat();
    public PhysicalStat physicalStat = new PhysicalStat();
    public ImpaleStat impaleStat = new ImpaleStat();
    public MinionStat minionStat = new MinionStat();
    public OnslaughtStat onslaughtStat = new OnslaughtStat();
    public SpeedStat speedStat = new SpeedStat();
    public GeneralIncreasedStat generalIncreasedStat = new GeneralIncreasedStat();
    public ZombieStat zombieStat = new ZombieStat();
    public AttributeStat attributeStat = new AttributeStat();
    public ChargeStat chargeStat = new ChargeStat();
    public CriticalStat criticalStat = new CriticalStat();

    // Constructor to add stat instances to the list
    public AllSkillStat()
    {
        // Add instances of your stat classes to the list
        StatsType.Add(fireStat);
        StatsType.Add(coldStat);
        StatsType.Add(lightningStat);
        StatsType.Add(physicalStat);
        StatsType.Add(impaleStat);
        StatsType.Add(minionStat);
        StatsType.Add(onslaughtStat);
        StatsType.Add(speedStat);
        StatsType.Add(generalIncreasedStat);
        StatsType.Add(zombieStat);
        StatsType.Add(attributeStat);
        StatsType.Add(chargeStat);
        StatsType.Add(criticalStat);
    }
}
