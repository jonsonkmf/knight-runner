using ScriptableSystem;
using ScriptableSystem.GameVariable;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [CreateAssetMenu(
        menuName = SOConstants.VariableSubmenu + "Int",
        fileName = "IntVariable",
        order = SOConstants.AssetMenuOrder)]
    public class IntVariable: GameVariable<int>
    {
        
    }
}