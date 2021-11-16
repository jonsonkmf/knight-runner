using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerInput
{
    public class TouchController : SerializedMonoBehaviour
    {
        [SerializeField] private float _returnSpeed = 0.5f;
        [SerializeField] private float _increaseSpeed = 2f;
        [SerializeField] private IGameEventInvoker<float> _onChangeValue;
        [SerializeField] private IGameEventListener _onInputStarted;
        [SerializeField] private IGameEventListener _onInputEnded;
        [SerializeField] private IGameEventListener<bool> _onLevelState;
        private float _currentValue = 0;
        private float _finishValue = 0;
        private float _speed;
        private bool _isActive = true;

        private void OnEnable()
        {
            _onInputStarted.AddAction(SetFinishValue);
            _onInputEnded.AddAction(ResetFinishValue);
            _onLevelState.AddAction(SetActive);
        }

        private void OnDisable()
        {
            _onInputStarted.RemoveAction(SetFinishValue);
            _onInputEnded.RemoveAction(ResetFinishValue);
            _onLevelState.RemoveAction(SetActive);
        }

        private void SetActive(bool state) => _isActive = state;


        private void FixedUpdate()
        {
            if (_isActive == false) return;
            _currentValue = Mathf.MoveTowards(_currentValue, _finishValue, Time.deltaTime * _speed);
            _onChangeValue.Invoke(_currentValue);
        }

        private void ResetFinishValue()
        {
            if (_isActive == false) return;
            _finishValue = 0;
            _speed = _returnSpeed;
        }

        private void SetFinishValue()
        {
            if (_isActive == false) return;
            _finishValue = 1;
            _speed = _increaseSpeed;
        }
    }
}