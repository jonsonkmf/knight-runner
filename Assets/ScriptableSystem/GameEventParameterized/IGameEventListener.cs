using System;

namespace ScriptableSystem.GameEventParameterized
{
    public interface IGameEventListener<out TData>
    {
        void AddAction(Action<TData> action);
        void RemoveAction(Action<TData> action);
    }
}