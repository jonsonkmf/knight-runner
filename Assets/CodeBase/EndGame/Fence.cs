using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.EndGame
{
    public class Fence: MonoBehaviour
    {
        [SerializeField] [ChildGameObjectsOnly] 
        private Rigidbody[] _fenceParts;

        [SerializeField][ChildGameObjectsOnly]
        private FenceTrigger _trigger;

        public void ActivateFence(bool state) => _trigger.SetActive(state);

        private void Awake() => IsKinematic(true);

        private void OnEnable() => _trigger.OnTriggered += DestroyFence;

        private void OnDisable() => _trigger.OnTriggered -= DestroyFence;

        private void DestroyFence() => IsKinematic(false);

        private void IsKinematic(bool state)
        {
            foreach (var part in _fenceParts) part.isKinematic = state;
        }
    }
}