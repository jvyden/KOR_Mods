using UnityEngine;

namespace ExtraStatistics.Statistics.Trackers;

public class CoinsTracker : Tracker
{
    protected override int GetStatistic()
    {
        int earnedCoins = GameManager.Instance.startMoney;
        earnedCoins += GameManager.Instance.Score / 10; // Coins earned from score
        earnedCoins -= PlayerPrefs.GetInt("money", 0); // Tracks coins earned by any other means (e.g. RGB battery coins) 
        
        return earnedCoins;
    }

    protected override string StatisticDisplayName => "Coins";
}