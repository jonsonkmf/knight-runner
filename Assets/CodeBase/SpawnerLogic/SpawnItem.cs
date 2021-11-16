using System;
using CodeBase.EnemyLogic;
using CodeBase.SplineLogic;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.EnemySpawnerLogic
{
    [Serializable]
    public class SpawnItem<Item> where Item : ISpawnableItem
    {
        public Item _spawnPrefab;
        
        [VerticalGroup("Position")] [ReadOnly]
        [Range(0f, 1f)]
        public double _percentage = 0;

        [VerticalGroup("Position")]
        [Range(0f, 100f)]
        public float _offset = 0;
        
        public SplineSide _side;
        
        public void UpdateData(SplineComputer spline, float previousValue)
        {
            _percentage = spline.Travel(0, _offset + previousValue);

            if (_percentage == 1f) _offset = spline.CalculateLength() - previousValue;
        }
    }
}