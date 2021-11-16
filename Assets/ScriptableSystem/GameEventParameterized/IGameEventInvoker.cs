namespace ScriptableSystem.GameEventParameterized
{
    public interface IGameEventInvoker<in TData>
    {
        void Invoke(TData value);
    }
}