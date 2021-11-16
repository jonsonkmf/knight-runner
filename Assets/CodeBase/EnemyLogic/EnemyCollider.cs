using UnityEngine;

namespace CodeBase.EnemyLogic
{
    public class EnemyCollider : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _rigidbodies;

        public Rigidbody[] rigidbodies => _rigidbodies;
    }
}