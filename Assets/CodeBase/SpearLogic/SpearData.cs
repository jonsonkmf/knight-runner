using ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.SpearLogic
{
    [InlineEditor()]
    [CreateAssetMenu(
        menuName = SOConstants.DataSubmenu + "Spear",
        fileName = "Spear",
        order = SOConstants.AssetMenuOrder)]
    public class SpearData : ScriptableObject
    {
        [Title("InGameForce")] [SerializeField] [MinValue(0)]
        private float _forceValue;

        [SerializeField] [MinValue(0)] private float _upForceValue = 100f;

        [Title("SuperForce")] [SerializeField] [MinValue(0)]
        private float _upSuperForceValue = 100f;

        [SerializeField] [MinMaxSlider("_forceValue", "MaxSuperForceValue", true)] private Vector2 _superForceValue;

        private float MaxSuperForceValue => _forceValue * 4f;
        
        public Vector3 GetForce(Vector3 forceDirection)
        {
            var upForce = Vector3.up * _upForceValue;
            var force = forceDirection * _forceValue + upForce;
            return force;
        }

        public Vector3 GetSuperForce(float factor, Vector3 forceDirection)
        {
            Debug.Log(factor);
            var upForce = Vector3.up * _upSuperForceValue;
            var force = Mathf.Lerp(_superForceValue.x, _superForceValue.y, factor) * forceDirection + upForce;
            return force;
        }
    }
}