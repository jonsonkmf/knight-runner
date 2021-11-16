using UnityEngine;

namespace CodeBase.EnemyLogic.PushLogic
{
    public interface IForceApplier
    {
        void Apply(Vector3 force);
    }
}