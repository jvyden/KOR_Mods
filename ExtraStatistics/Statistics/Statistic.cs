using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ExtraStatistics.Statistics;

public class Statistic : MonoBehaviour
{
    public string statisticDisplayName;
    private Component _textMeshComponent;

    private static PropertyInfo _textInfo;
    
    public void Start()
    {
        this._textMeshComponent = this.gameObject.GetComponentByName("TMPro.TextMeshProUGUI");
        _textInfo = AccessTools.Property(_textMeshComponent.GetType(), "text");
        
        this.UpdateStatistic("");
    }
    
    public void UpdateStatistic<TStatistic>(TStatistic value)
    {
        string text = $"{this.statisticDisplayName}: {value.ToString()}";
        _textInfo.SetValue(_textMeshComponent, text);
    }
}