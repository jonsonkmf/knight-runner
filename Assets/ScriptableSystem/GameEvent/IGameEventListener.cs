using System;

namespace ScriptableSystem.GameEvent
{
    public interface IGameEventListener
    {
        void AddAction(Action action);
        void RemoveAction(Action action);
    }
}