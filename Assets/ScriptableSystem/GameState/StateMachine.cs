using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableSystem.GameState
{
    [InlineEditor]
    [CreateAssetMenu(
        menuName = SOConstants.StateSubmenu + "StateMachine",
        fileName = "State",
        order = SOConstants.AssetMenuOrder)]
    public class StateMachine : SerializedScriptableObject
    {
        [SerializeField] [ReadOnly] private State _currentState;

        public void LoadState(State state)
        {
            if(_currentState != null) _currentState.OnExit.Invoke();
            _currentState = state;
            _currentState.OnEnter.Invoke();
        }
        
        
    }
}