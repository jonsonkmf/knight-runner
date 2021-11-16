using System;
using System.Collections.Generic;

namespace ScriptableSystem
{
    [Serializable]
    public class BaseEvent: IEventListener
    {
        private readonly List<Action> _actions = new List<Action>();

        public virtual void Invoke()
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
    }
}