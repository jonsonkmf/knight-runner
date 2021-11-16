using CodeBase.EnemyLogic;
using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using ScriptableSystem.GameEvent;
using UnityEngine;

namespace CodeBase.Health
{
    public class HealthManager : MonoBehaviour, IDamagable
    {
        [SerializeField] private IntVariable _maxHealth;
        [SerializeField] private IntVariable _currentHealth;
        [SerializeField] private GameEvent _playerDie;
        [SerializeField] private Material[] _materials;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderersHorse;
        [SerializeField] private Material _horseDamageMaterial;
        [SerializeField] private ParticleSystem[] _hitParticles;
        private Material[] _horseMaterials = new Material[3];

        private void Awake()
        {
            _currentHealth.Value = _maxHealth.Value;
            foreach (var material in _materials)
            {
                material.color = Color.white;
            }

            for (int i = 0; i < _skinnedMeshRenderersHorse.Length; i++)
            {
                _horseMaterials[i] = _skinnedMeshRenderersHorse[i].material;
            }
            
        }


        public void TakeDamage(int damage)
        {
            _currentHealth.Value -= damage;

            foreach (var hitParticle in _hitParticles)
            {
                hitParticle.Play();
            }
            
            foreach (var material in _materials)
            {
                material.color = Color.red;
            }
            
            for (int i = 0; i < _skinnedMeshRenderersHorse.Length; i++)
            {
                _skinnedMeshRenderersHorse[i].material = _horseDamageMaterial;
            }

            this.InvokeDelegate(() =>
            {
                foreach (var material in _materials)
                {
                    material.color = Color.white;
                }

                ;
                for (int i = 0; i < _skinnedMeshRenderersHorse.Length; i++)
                {
                    _skinnedMeshRenderersHorse[i].material = _horseMaterials[i];
                }

                ;
            }, 0.5f);
            if (_currentHealth.Value <= 0) _playerDie.Invoke();
        }
    }
}