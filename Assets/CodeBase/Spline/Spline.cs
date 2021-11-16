using CodeBase.ScriptableObjects.GameVariables;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.SplineLogic
{
    public class Spline: MonoBehaviour
    {
        [SerializeField] private SplineVariable _variable;

        [SerializeField] [ChildGameObjectsOnly]
        private SplineComputer _spline;
        private void Awake() => _variable.Value = _spline;
    }
}