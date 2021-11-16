using ScriptableSystem;
using ScriptableSystem.GameEventParameterized;
using UnityEngine;

namespace CodeBase.ScriptableObjects.Events
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "Int",
        fileName = "Int",
        order = SOConstants.AssetMenuOrder)]
    public class IntGameEvent: GameEventBase<int>
    {
        
    }
}