using ScriptableSystem;
using ScriptableSystem.GameVariable;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [CreateAssetMenu(
        menuName = SOConstants.VariableSubmenu + "Transform",
        fileName = "Transform",
        order = SOConstants.AssetMenuOrder)]
    public class TransformGameVariable: GameVariable<Transform>
    {
        
    }
}