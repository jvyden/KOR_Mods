using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace GeneralTweaks.Components;

public class LegacyLeaderboardInjector : MonoBehaviour
{
    private static readonly FieldInfo _highscoreUserTextInfo = AccessTools.Field(typeof(DisplayHighscores), "highscoreUserText");
    private static readonly FieldInfo _highscoreTextInfo = AccessTools.Field(typeof(DisplayHighscores), "highscoreText");
    
    public void Awake()
    {
        const int scoreRows = 5;
        Array scoreUserTextMeshes = Array.CreateInstance(_highscoreUserTextInfo.FieldType.GetElementType()!, scoreRows);
        Array scoreTextMeshes = Array.CreateInstance(_highscoreTextInfo.FieldType.GetElementType()!, scoreRows);

        for (int i = 0; i < scoreRows; i++)
        {
            Component userTextObj = this.transform
                .GetChild(i)
                .gameObject
                .GetComponentByName("TMPro.TextMeshProUGUI");
            
            Component textObj = this.transform
                .GetChild(i)
                .GetChild(0)
                .gameObject
                .GetComponentByName("TMPro.TextMeshProUGUI");

            scoreUserTextMeshes.SetValue(userTextObj, i);
            scoreTextMeshes.SetValue(textObj, i);
        }

        this.gameObject.SetActive(false); // So OnEnable doesn't get called in the components we add
        this.gameObject.AddComponent<Highscores>();

        DisplayHighscores display = this.gameObject.AddComponent<DisplayHighscores>();
        _highscoreUserTextInfo.SetValue(display, scoreUserTextMeshes);
        _highscoreTextInfo.SetValue(display, scoreTextMeshes);
        this.gameObject.SetActive(true);
        
        // disable More button
        this.transform.GetChild(5).gameObject.SetActive(false);
    }
}