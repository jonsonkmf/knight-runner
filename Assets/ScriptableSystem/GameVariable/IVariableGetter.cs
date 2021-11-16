namespace ScriptableSystem.GameVariable
{
    public interface IVariableGetter<out TValue>
    {
        TValue Value { get; }
        IEventListener OnChanged { get; }
    }
}