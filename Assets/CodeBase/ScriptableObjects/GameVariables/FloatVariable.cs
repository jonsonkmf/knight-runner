using ScriptableSystem;
using ScriptableSystem.GameVariable;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [CreateAssetMenu(
        menuName = SOConstants.VariableSubmenu + "Float",
        fileName = "Float",
        order = SOConstants.AssetMenuOrder)]
    public class FloatVariable: GameVariable<float>
    {
        
    }
}