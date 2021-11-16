using UnityEngine;

namespace ScriptableSystem.GameEventParameterized.Events
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "Float",
        fileName = "FloatEvent", 
        order = SOConstants.AssetMenuOrder)]
    public class FloatGameEvent: GameEventBase<float>
    {
        
    }
}