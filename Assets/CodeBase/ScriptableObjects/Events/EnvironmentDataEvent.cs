using ScriptableSystem;
using ScriptableSystem.GameEventParameterized;
using UnityEngine;

namespace CodeBase.LevelLogic
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "EnvironmentData",
        fileName = "EnvironmentDataEvent",
        order = SOConstants.AssetMenuOrder)]
    public class EnvironmentDataEvent: GameEventBase<EnvironmentData>
    {
    }
}