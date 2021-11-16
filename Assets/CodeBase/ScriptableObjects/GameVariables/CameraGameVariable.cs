using ScriptableSystem;
using ScriptableSystem.GameVariable;
using UnityEngine;

namespace CodeBase.ScriptableObjects.GameVariables
{
    [CreateAssetMenu(
        menuName = SOConstants.VariableSubmenu + "Camera",
        fileName = "Camera",
        order = SOConstants.AssetMenuOrder)]
    public class CameraGameVariable: GameVariable<Camera>
    {
    }
}