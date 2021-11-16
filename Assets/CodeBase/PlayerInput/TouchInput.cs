using CodeBase.ScriptableObjects.Events;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerInput
{
    public class TouchInput : SerializedMonoBehaviour
    {
        [SerializeField] private GameEvent _onInputStarted;
        [SerializeField] private GameEvent _onInputEnded;
        [SerializeField] private FloatGameEvent _onHorizontalOffsetChanged; 
        [SerializeField] private FloatGameEvent _onVerticalOffsetChanged; 
        [SerializeField] private float _sensityVertical = 1f;


        private const float ToleranceX = 0.1f;
        private const float ToleranceY = 0.00001f;
        private Vector2 _touchStartPosition;
        private Vector2 _touchEndPosition;
        private Vector2 _touchPreviousPosition;


        private void Update()
        {

#if UNITY_EDITOR
            UpdateClickPosition();
#else
            UpdateTouchPosition();
#endif
        }

        private void UpdateClickPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _onInputStarted.Invoke();
                _touchStartPosition = Input.mousePosition;
                _touchEndPosition = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButton(0))
            {
                OnTouchMove(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnTouchMove(Input.mousePosition);
                _onInputEnded.Invoke();
            }
        }

        private void UpdateTouchPosition()
        {
            if (Input.touchCount == 0) return;

            var touch = Input.GetTouch(0);
            var touchPhase = touch.phase;
            var touchPosition = touch.position;

            if (touchPhase == TouchPhase.Began)
            {
                _touchStartPosition = touchPosition;
                _touchEndPosition = touchPosition;
                _onInputStarted.Invoke();
                return;
            }

            if (touchPhase == TouchPhase.Moved)
            {
                OnTouchMove(touchPosition);
                return;
            }

            if (touchPhase == TouchPhase.Ended)
            {
                OnTouchMove(touchPosition);
                _onInputEnded.Invoke();
            }
        }

        private void OnTouchMove(Vector2 touchPosition)
        {
            _touchPreviousPosition = _touchEndPosition;
            _touchEndPosition = touchPosition;
            UpdateVerticalOffset();
            UpdateHorizontalOffset();
        }

        private void UpdateVerticalOffset()
        {
            var yDiff = (_touchEndPosition.y - _touchPreviousPosition.y) / (_sensityVertical * Screen.width);
            _onVerticalOffsetChanged.Invoke(yDiff);
        }
        private void UpdateHorizontalOffset()
        {
            var xDiff = (_touchEndPosition.x - _touchPreviousPosition.x) / (_sensityVertical * Screen.width);
            _onHorizontalOffsetChanged.Invoke(xDiff);
        }

    
    }
}