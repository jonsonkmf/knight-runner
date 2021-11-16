using System;
using CodeBase.EnemyLogic;
using CodeBase.ScriptableObjects.Events;
using UnityEngine;

    public class Chest : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _particleSystem;
        private static readonly int OpenChest = Animator.StringToHash("OpenChest");

        [SerializeField] private IntGameEvent _gameEvent;

      /*  private void OnCollisionEnter(Collision other)
        {
            var enemyCollider = other.gameObject.GetComponentInParent(typeof(EnemyCollider));
            if (enemyCollider == null) return;

            _animator.SetTrigger(OpenChest);
            _particleSystem.Play();
        }/**/
      private void OnEnable()
      {
          _gameEvent.AddAction(OpenChestfunc);
      }

      private void OnDisable()
      {
          _gameEvent.RemoveAction(OpenChestfunc);
      }

      private void OpenChestfunc(int value)
      {
          if (value==50)
          {
              _animator.SetTrigger(OpenChest);
              _particleSystem.Play();
          }
      }

      
    }
