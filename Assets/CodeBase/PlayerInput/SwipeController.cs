using System;
using System.ComponentModel;
using CodeBase.ScriptableObjects.Events;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerInput
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private GameEvent _onLeftSwipe;
        [SerializeField] private GameEvent _onRightSwipe;
        [SerializeField] private BoolGameEvent _onGameState;
        [SerializeField] private float _speed;
        [SerializeField] private FloatGameEvent _onChangeValue;


        [SerializeField] private float _targetPosition = 1;
        [SerializeField] [Sirenix.OdinInspector.ReadOnly] private float _currentPosition = 1;
        private const float Tolerance = 0.01f;
        private bool _isActive;

        private void OnEnable()
        {
            _onLeftSwipe.AddAction(MoveRight);
            _onRightSwipe.AddAction(MoveLeft);
            _onGameState.AddAction(SetActive);
        }

        private void MoveLeft() => _targetPosition = 0;

        private void MoveRight() => _targetPosition = 1;

        private void OnDisable()
        {
            _onLeftSwipe.RemoveAction(MoveRight);
            _onRightSwipe.RemoveAction(MoveLeft);
            _onGameState.RemoveAction(SetActive);
        }

        private void SetActive(bool state)
        {
            if (state) _targetPosition = 1;
            _isActive = state;
        }

        private void Update()
        {
            if(!_isActive) return;
            if (!IsSwiping()) return;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _currentPosition = Mathf.MoveTowards(_currentPosition, _targetPosition, Time.deltaTime * _speed);
            _onChangeValue.Invoke(_currentPosition);
        }

        private bool IsSwiping() => Math.Abs(_currentPosition - _targetPosition) > Tolerance;
    }
}