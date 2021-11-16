namespace ScriptableSystem.GameVariable
{
    public interface IVariableSetter<in TValue>
    {
        TValue Value { set; }
    }
}