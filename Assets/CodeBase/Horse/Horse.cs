using System;
using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Horse
{
    public class Horse : MonoBehaviour
    {
        [SerializeField] private SplineVariable _spline;
        
        [SerializeField] [ChildGameObjectsOnly]
        private SplineFollower _splineFollower;

        [SerializeField] private BoolGameEvent _onGameState;
        [SerializeField] private GameEvent _onBossFightTracking;

        [SerializeField] [ChildGameObjectsOnly]
        private HorseAnimator _animator;

        public float FolowProgress;
        private float _speed;
        private bool _isActive = false;
        private bool _changeOffset = false;

        private void Awake()
        {
            _speed = _splineFollower.followSpeed;
        }

        private void Start()
        {
            _splineFollower.spline = _spline.Value;
            _splineFollower.followSpeed = 0;
        }

        private void Update()
        {
            FolowProgress = (float) _splineFollower.result.percent;
            if (_changeOffset==true)
            {
                var xoffset = _splineFollower.motion.offset;
                if (xoffset.x>-0.8)
                {
                _splineFollower.motion.offset =
                    new Vector2(_splineFollower.motion.offset.x- Time.deltaTime, _splineFollower.motion.offset.y);
                    
                }
            }
        }/**/

        private void OnEnable()
        {
            _onGameState.AddAction(SetActive);
            _onBossFightTracking.AddAction(StartChangeOffset);
        }

        private void OnDisable()
        {
            _onGameState.RemoveAction(SetActive);
            _onBossFightTracking.RemoveAction(StartChangeOffset);
        }


        private void SetActive(bool isActive)
        {
            _isActive = isActive;
            if (isActive)
            {
                _splineFollower.followSpeed = _speed;
                _animator.IsRunning(true);
            }
            else
            {
                _splineFollower.followSpeed = 0;
                _animator.IsRunning(false);
            }
        }

        private void StartChangeOffset()
        {
            _changeOffset = true;
        }
    }
}