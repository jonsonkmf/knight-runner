using Dreamteck.Splines;
using ScriptableSystem;
using ScriptableSystem.GameVariable;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [CreateAssetMenu(
        menuName = SOConstants.VariableSubmenu + "Spline",
        fileName = "Spline",
        order = SOConstants.AssetMenuOrder)]
    public class SplineVariable: GameVariable<SplineComputer>
    {
        
    }
}