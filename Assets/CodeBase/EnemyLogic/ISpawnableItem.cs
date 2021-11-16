using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using UnityEngine;

namespace CodeBase.EnemyLogic
{
    public interface ISpawnableItem
    {
        void SetSpawnPosition(double position, Vector2 spawnOffset, SplineComputer spline, float playerTime, SplineFollower hero);

        GameObject gameObject
        {
            get;
        }
    }
}