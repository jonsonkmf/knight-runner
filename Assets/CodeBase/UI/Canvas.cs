using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public abstract class Canvas : SerializedMonoBehaviour
    {
        [SerializeField] private GameEvent _onCanvasChanged;
        [SerializeField] private GameEvent _onGameState;
        
        protected virtual void Awake()
        {
            Deactivate();
            _onGameState.AddAction(Activate);
        }

        protected virtual void OnDestroy() => _onGameState.RemoveAction(Activate);

        protected virtual void OnEnable()
        {
            _onCanvasChanged.Invoke();
            _onCanvasChanged.AddAction(Deactivate);
        }

        protected virtual void OnDisable() => _onCanvasChanged.RemoveAction(Deactivate);

        private void Deactivate() => gameObject.SetActive(false);
        private void Activate() => gameObject.SetActive(true);
    }
}