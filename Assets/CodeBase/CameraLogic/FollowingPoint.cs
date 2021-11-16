using CodeBase.ScriptableObjects.GameVariables;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class FollowingPoint : MonoBehaviour
    {
        [SerializeField] private TransformGameVariable _transformPoint;
        private void Awake() => _transformPoint.Value = transform;
    }
}