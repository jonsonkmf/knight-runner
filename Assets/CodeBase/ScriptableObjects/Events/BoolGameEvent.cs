using ScriptableSystem;
using ScriptableSystem.GameEventParameterized;
using UnityEngine;

namespace CodeBase.ScriptableObjects.Events
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "Bool",
        fileName = "Bool",
        order = SOConstants.AssetMenuOrder)]
    public class BoolGameEvent: GameEventBase<bool>
    {
        
    }
}