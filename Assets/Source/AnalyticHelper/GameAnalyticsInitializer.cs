using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameAnalyticsInitializer : MonoBehaviour
{
    private void Start()
    {
        GameAnalytics.Initialize();
    }
}
