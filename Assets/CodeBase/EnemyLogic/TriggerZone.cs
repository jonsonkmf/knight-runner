using System;
using CodeBase.CameraLogic;
using CodeBase.SpearLogic;
using ScriptableSystem.GameEvent;
using UnityEngine;

namespace CodeBase.EnemyLogic
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private bool _isBoss = false;
        [SerializeField] private GameEvent _onBossKilled;
        public event Action<Vector3> OnSpearEnter;
        public event Action OnDragonFireEnter;

        

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Spear spear))
            {
                if (_isBoss)
                {
                    _onBossKilled.Invoke();
                    OnSpearEnter?.Invoke(spear.GetSuperForce());
                }
                else
                {
                    OnSpearEnter?.Invoke(spear.GetForce());
                }
            }

            else if (other.TryGetComponent(out DragonFire dragonAttack))
                OnDragonFireEnter?.Invoke();
           
        }
    }
}