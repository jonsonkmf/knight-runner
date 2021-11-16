using System;
using CodeBase.EnemyLogic;
using UnityEngine;

namespace CodeBase.EndGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class FenceTrigger : MonoBehaviour
    {
        public event Action OnTriggered;
        private BoxCollider _collider;
        private void Awake() => _collider = GetComponent<BoxCollider>();

        public void SetActive(bool state) => _collider.isTrigger = state;

        private void OnTriggerEnter(Collider other)
        {
            var enemyCollider = other.gameObject.GetComponentInParent(typeof(EnemyCollider));
            if (enemyCollider == null) return;

            OnTriggered?.Invoke();
            Debug.Log("FenceDestroyed!");
        }
    }
}