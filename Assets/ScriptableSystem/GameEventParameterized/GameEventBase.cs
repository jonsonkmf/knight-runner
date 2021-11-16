using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableSystem.GameEventParameterized
{
    public abstract class GameEventBase<TData> : ScriptableObject, IGameEventListener<TData>, IGameEventInvoker<TData>
    {
        [ReadOnly] private readonly List<Action<TData>> _actions = new List<Action<TData>>();
        
        public void AddAction(Action<TData> action)
        {
            if(!_actions.Contains(action)) _actions.Add(action);
        }

        public void RemoveAction(Action<TData> action)
        {
            if(_actions.Contains(action)) _actions.Remove(action);
        }

        public void Invoke(TData value)
        {
            foreach (var action in _actions) action?.Invoke(value);
        }
    }
}