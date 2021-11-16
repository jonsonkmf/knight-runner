using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableSystem.GameVariable
{
    public abstract class GameVariable<TValue> : ScriptableObject, IVariableGetter<TValue>, IVariableSetter<TValue>, IEventListener
    {
        private const string IsConstant = "_isConstant";

        [SerializeField]
        private TValue _value;

        [SerializeField] [ReadOnly]
        private TValue _runTimeValue;

        [SerializeField] private bool _isConstant = true;

        [HideIf(IsConstant)] [SerializeField]
        private bool _isSaving;

        private readonly List<Action> _actions = new List<Action>();
        private void Invoke()
        {
            foreach (var action in _actions) action?.Invoke();
        }

        public void AddAction(Action action)
        {
            if(!_actions.Contains(action)) _actions.Add(action);
        }

        public void RemoveAction(Action action)
        {
            if(_actions.Contains(action)) _actions.Remove(action);
        }
        
        public IEventListener OnChanged => this;

        public TValue Value
        {
            get => _isConstant ? _value : _runTimeValue;
            set
            {
                if (_isConstant) return;
                if (_isSaving) _value = value;
                _runTimeValue = value;
                Invoke();
            }
        }

        
      
    }
}