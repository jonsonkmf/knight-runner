using UnityEngine;

namespace CodeBase.EnemyLogic.PushLogic
{
    public class ForceApplier : IForceApplier
    {
        private readonly Rigidbody[] _rigidbodies;

        public ForceApplier(Rigidbody[] rigidbodies) => this._rigidbodies = rigidbodies;

        public ForceApplier(Rigidbody rigidbody) => _rigidbodies = new[] {rigidbody};

        public void Apply(Vector3 force)
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.AddForce(force * 100f);
            }
        }
    }
}