using System;
using CodeBase.ScriptableObjects.Events;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using UnityEngine;

namespace PlayerInput
{
    public class HoldSwipeInput : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private FloatGameEvent _onOffsetChanged;
        [SerializeField] private GameEvent _onLeftSwipe;
        [SerializeField] private GameEvent _onRightSwipe;
        [SerializeField] private BoolGameEvent _onGameState;

        private float _currentOffset = 0f;
        private float _targetOffset = 0f;
        private bool _isActive = true;
        private const float Tolerance = 0.01f;
        private bool _isLeft = false;
        private void OnEnable()
        {
            _onOffsetChanged.AddAction(ChangeTargetOffset);
            _onGameState.AddAction(ResetSwipe);
        }

        private void OnDisable()
        {
            _onOffsetChanged.RemoveAction(ChangeTargetOffset);
            _onGameState.RemoveAction(ResetSwipe);
        }

        private void Update()
        {
            _currentOffset = Mathf.MoveTowards(_currentOffset, _targetOffset, Time.deltaTime * _speed);
            CheckForLeftSwipe();
        }

        private void ResetSwipe(bool state) => _isActive = state;


        private void ChangeTargetOffset(float inputOffset)
        {
            if (!_isActive) return;
            _targetOffset = Mathf.Clamp01(_targetOffset + inputOffset);
            CheckForLeftSwipe();
            CheckForRightSwipe();

        }

        private void CheckForRightSwipe()
        {
            if (!(Math.Abs(_currentOffset) < Tolerance)) return;
            _isLeft = false;
            _onRightSwipe.Invoke();
        }

        private void CheckForLeftSwipe()
        {
            if (!(Math.Abs(_currentOffset - 1) < Tolerance)) return;
            _isLeft = true;
            _onLeftSwipe.Invoke();
        }

        private void SetActive(bool state) => _isActive = state;
    }
}