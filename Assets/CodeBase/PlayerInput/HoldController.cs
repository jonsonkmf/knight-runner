using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;


namespace PlayerInput
{
    public class HoldController : SerializedMonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private FloatGameEvent _onChangeValue;
        [SerializeField] private FloatGameEvent _onOffsetChanged;

        private float _currentOffset;
        private float _targetOffset;
        private bool _isActive = true;

        private void OnEnable()
        {
            _onOffsetChanged.AddAction(ChangeTargetOffset);
        }

        private void OnDisable()
        {
            _onOffsetChanged.RemoveAction(ChangeTargetOffset);
        }

        private void Update()
        {
            if (!_isActive) return;
            _currentOffset = Mathf.MoveTowards(_currentOffset, _targetOffset, Time.deltaTime * _speed);

            _onChangeValue.Invoke(_currentOffset);
        }


        private void ChangeTargetOffset(float inputOffset)
        {
            if (!_isActive) return;
            _targetOffset = Mathf.Clamp01(_targetOffset + inputOffset);
        }

        private void SetActive(bool state) => _isActive = state;
    }
}