using UnityEngine;

namespace CodeBase.EnemyLogic.RagdollLogic
{
    public class RagdollController : IRagdollController
    {
        private readonly Rigidbody[] _rigidbodies;
        private readonly Animator _animator;

        public RagdollController(Rigidbody[] _rigidbodies, Animator _animator)
        {
            this._rigidbodies = _rigidbodies;
            this._animator = _animator;
        }

        private void Awake() => IsRagdoll(false);

        public void IsRagdoll(bool state)
        {
            _animator.enabled = !state;
            foreach (var rigidbody in _rigidbodies) rigidbody.isKinematic = !state;
        }
    }
}