using System;

namespace ExtraStatistics.Statistics.Trackers;

public class SpeedTracker : Tracker
{
    protected override string GetStatistic()
    {
        const float defaultSpeed = 1.8f;
        const float maxSpeed = 10.0f;
        float speed = GameManager.Instance.gameSpeed;
        
        float percent = (speed - defaultSpeed) / (maxSpeed - defaultSpeed) * 100.0f;

        return $"{Math.Round(percent, 0)}%";
    }

    protected override string StatisticDisplayName => "Speed";
}