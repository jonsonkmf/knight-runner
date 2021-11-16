using System;
using CodeBase.Rider;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Unity.Collections;
using UnityEngine;

namespace CodeBase.SpearLogic
{
    public class SpearAnimationMovement : MonoBehaviour
    {
        [SerializeField] private RiderAnimator _animator;
        [SerializeField] private FloatGameEvent _onInputChanged;
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _leftState;
        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _rightState;
        [SerializeField] private GameEvent _onBossFightStarted;
        [SerializeField] private Vector2 _spearRotation = new Vector2(-80,80);
        [SerializeField] [ReadOnly] private Transform _currentHand;
        [SerializeField] private GameEvent _onLeftSwipe;
        [SerializeField] private GameEvent _onRightSwipe;
        [SerializeField] private GameObject _rightShield;
        [SerializeField] private GameObject _leftShield;
        
        private RiderSide _currentSide;
        private Transform _currentState;
        private bool _isFighting = false;
        
        private void OnEnable()
        {
            _onLeftSwipe.AddAction(SetRightHand);
            _onRightSwipe.AddAction(SetLeftHand);
            _onBossFightStarted.AddAction(StartFighting);
        }

        private void OnDisable()
        {
            _onLeftSwipe.RemoveAction(SetRightHand);
            _onRightSwipe.RemoveAction(SetLeftHand);
            _onBossFightStarted.RemoveAction(StartFighting);
        }

        private void StartFighting()
        {
            SetRightHand();
            _isFighting = true;
        }


        private void Start() => SetHandSide(_animator.GetSide());
        

        private void SetRightHand()
        {
            if(_isFighting) return;
            if(_currentSide== RiderSide.Right) return;
            _currentSide = RiderSide.Right;
            _animator.SetSide(_currentSide);
            this.InvokeDelegate(() => { SetHandSide(_currentSide); }, 0.2f);
           // SetHandSide(_currentSide);
        }

        private void SetLeftHand()
        {
            if(_isFighting) return;
            if(_currentSide== RiderSide.Left) return;
            _currentSide = RiderSide.Left;
            _animator.SetSide(_currentSide);
            this.InvokeDelegate(() => { SetHandSide(_currentSide); }, 0.2f);
            //SetHandSide(_currentSide);
        }

        private void SetHandSide(RiderSide side)
        {
            _currentSide = side;
            var xAngle = transform.localRotation.eulerAngles.x;
            if (side == RiderSide.Left)
            {
                _currentState = _leftState;
                _currentHand = _leftHand;
                transform.localRotation = Quaternion.Euler(new Vector3(xAngle, _spearRotation.x, 0));
                
                
                _leftShield.SetActive(false);
                _rightShield.SetActive(true);
            }
            else if (side == RiderSide.Right)
            {
                _currentState = _rightState;
                _currentHand = _rightHand;
                transform.localRotation = Quaternion.Euler(new Vector3(xAngle, _spearRotation.y, 0));
                
                _leftShield.SetActive(true);
                _rightShield.SetActive(false);
            }
            
            transform.position = _currentState.position;
           // Debug.Log(transform.localRotation.eulerAngles.x);
        }

        private void Update() => transform.position = _currentHand.transform.position;
    }
}