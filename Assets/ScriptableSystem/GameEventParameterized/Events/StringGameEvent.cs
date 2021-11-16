using UnityEngine;

namespace ScriptableSystem.GameEventParameterized.Events
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "String",
        fileName = "StringEvent", 
        order = SOConstants.AssetMenuOrder)]
    public class StringGameEvent: GameEventBase<string>
    {
        
    }
}