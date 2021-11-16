using System;

namespace ScriptableSystem
{
    public interface IEventListener
    {
        void AddAction(Action action);
        void RemoveAction(Action action);
    }
}