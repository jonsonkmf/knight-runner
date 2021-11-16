using ScriptableSystem;
using ScriptableSystem.GameEventParameterized;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    [CreateAssetMenu(
        menuName = SOConstants.EventSubmenu + "Transform",
        fileName = "TransformEvent",
        order = SOConstants.AssetMenuOrder)]
    public class TransformGameEvent: GameEventBase<Transform>
    {
        
    }
}