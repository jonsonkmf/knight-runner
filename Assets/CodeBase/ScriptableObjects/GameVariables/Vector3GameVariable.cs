using ScriptableSystem;
using ScriptableSystem.GameVariable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [InlineEditor()]
    [CreateAssetMenu(menuName = SOConstants.VariableSubmenu + "Vector3",
        fileName = "Vector3GameVariable",
        order = SOConstants.AssetMenuOrder)]
    public class Vector3GameVariable: GameVariable<Vector3>
    {
        
    }
}