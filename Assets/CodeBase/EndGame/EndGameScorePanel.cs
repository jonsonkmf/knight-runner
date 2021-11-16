using System;
using CodeBase.EnemyLogic;
using CodeBase.ScriptableObjects.Events;
using UnityEngine;

namespace CodeBase.EndGame
{
    public class EndGameScorePanel : MonoBehaviour
    {
        [SerializeField] private IntGameEvent _onBonusCollected;
        [SerializeField] private int _value;
        [SerializeField] private GameObject _effect;
        private float timer;
        private void OnCollisionEnter(Collision other)
        {
            var enemyCollider = other.gameObject.GetComponentInParent(typeof(EnemyCollider));


            if(enemyCollider == null) return;
            
            Debug.Log("TouchPanel!");

           //_onBonusCollected.Invoke(_value);
           // _effect.transform.position = transform.position;
        }

        private void OnCollisionStay(Collision other)
        {
            var enemyCollider = other.gameObject.GetComponentInParent(typeof(EnemyCollider));
            if(enemyCollider == null) return;
            timer += Time.deltaTime;
            if (timer>10)
            {
                _effect.transform.position = transform.position;
                _onBonusCollected.Invoke(_value);
            }
        }
    }
}