using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableSystem.GameState
{
    [InlineEditor]
    [CreateAssetMenu(
        menuName = SOConstants.StateSubmenu + "State",
        fileName = "State",
        order = SOConstants.AssetMenuOrder)]
    public class State : SerializedScriptableObject
    {
        [SerializeField] [ReadOnly] private readonly BaseEvent _onEnter = new BaseEvent();                                                                                                                                                                              
        [SerializeField] [ReadOnly] private readonly BaseEvent _onExit = new BaseEvent();
        
        public BaseEvent OnEnter => _onEnter;
        public BaseEvent OnExit => _onExit;
    }
    
}