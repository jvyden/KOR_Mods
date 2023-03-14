namespace ExtraStatistics.Statistics.Trackers;

public class MissTracker : Tracker
{
    public static int Misses = 0; 
    
    protected override string GetStatistic()
    {
        return Misses.ToString();
    }

    protected override string StatisticDisplayName => "Miss";
}