using System;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using UnityEngine;

namespace CodeBase.SpearLogic
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Spear : MonoBehaviour
    {
        [SerializeField] private SpearData _data;
        [SerializeField] private Transform _horse;
        [SerializeField] private FloatGameEvent _onLevelSuccessUpdate;
        [SerializeField] private GameEvent _onBossStartFight;
        private CapsuleCollider _collider;
        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            _collider.isTrigger = false;
        }

        private float _factor;

        private void OnEnable()
        {
            _onLevelSuccessUpdate.AddAction(UpdateFactor);
            _onBossStartFight.AddAction(MakeTrigger);
        }

        private void OnDisable()
        {
            _onLevelSuccessUpdate.RemoveAction(UpdateFactor);
            _onBossStartFight.RemoveAction(MakeTrigger);
        }

        private void MakeTrigger() => _collider.isTrigger = true;

        private void UpdateFactor(float factor) => _factor = factor;

        public Vector3 GetForce() => _data.GetForce(forceDirection: _horse.forward);
        public Vector3 GetSuperForce() => _data.GetSuperForce(_factor, _horse.forward);
    }
}