namespace ExtraStatistics.Statistics.Trackers;

public class MissTracker : Tracker
{
    public static int Misses = 0; 
    
    protected override int GetStatistic()
    {
        return Misses;
    }

    protected override string StatisticDisplayName => "Miss";
}