using UnityEngine;

namespace ExtraStatistics.Statistics.Trackers;

public abstract class Tracker : MonoBehaviour
{
    protected abstract string GetStatistic();
    protected abstract string StatisticDisplayName { get; }
    
    private Statistic _statistic;
    
    private void Awake()
    {
        this._statistic = GetComponent<Statistic>();
        this._statistic.statisticDisplayName = StatisticDisplayName;
    }

    private void FixedUpdate()
    {
        this._statistic.UpdateStatistic(this.GetStatistic());
    }
}