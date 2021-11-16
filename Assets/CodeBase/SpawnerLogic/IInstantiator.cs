using CodeBase.EnemyLogic;

namespace CodeBase.EnemySpawnerLogic
{
    public interface IInstantiator
    {
        ISpawnableItem Spawn(ISpawnableItem enemy);
    }
}