using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Horse
{
    public class HorseAnimator : MonoBehaviour
    {
        [SerializeField] [ChildGameObjectsOnly]
        private Animator _animator;

        [SerializeField] private Animator _heroAnimator;


        private void Start()
        {
            if (_heroAnimator!=null)
            {
                _heroAnimator.speed = 0;
            }
        }

        private static readonly int Running = Animator.StringToHash("IsRunning");

        public void IsRunning(bool state)
        {
            _animator.SetBool(Running, state);
            if (_heroAnimator!=null)
            {
                _heroAnimator.speed = 0.8f;
            }
        }

    }
}