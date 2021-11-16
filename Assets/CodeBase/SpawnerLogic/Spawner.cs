using System.Collections.Generic;
using CodeBase.EnemyLogic;
using CodeBase.SplineLogic;
using Dreamteck.Splines;
using UnityEngine;

namespace CodeBase.EnemySpawnerLogic
{
    public class Spawner<Item> where Item : ISpawnableItem
    {
        private readonly float _offset;
        private readonly List<SpawnItem<Item>> _spawnerData;
        private readonly IInstantiator _instatitor;
        private readonly float _splineWidth;
        private readonly SplineComputer _computer;
        private readonly float _playerSpeed;
        private readonly SplineFollower _hero;

        private Item[] _items;
        public Spawner(List<SpawnItem<Item>> spawnerData, IInstantiator instatitor, float splineWidth, SplineComputer computer, float playerSpeed,SplineFollower hero)
        {
            _spawnerData = spawnerData;
            _instatitor = instatitor;
            _splineWidth = splineWidth;
            _computer = computer;
            _playerSpeed = playerSpeed;
            _offset = 1f;
            _hero = hero;
        }
        

        public Item[] Spawn()
        {
            _items = new Item[_spawnerData.Count];
            for (var i = 0; i < _spawnerData.Count; i++)
            {
                var item = _spawnerData[i];
                _items[i] = SpawnItem(item._spawnPrefab, item._percentage, item._side);
            }

            return _items;
        }

        private Item SpawnItem(Item spawnableItem, double percentage, SplineSide side)
        {
            var item = _instatitor.Spawn(spawnableItem);
            float playerTime = _computer.CalculateLength(0, percentage) / _playerSpeed;
            item.SetSpawnPosition(percentage, GetSpawnOffset(side), _computer, playerTime, _hero);
            return (Item)item;
        }

        private Vector2 GetSpawnOffset(SplineSide side)
        {
            return side switch
            {
                SplineSide.Left => new Vector2(-(_splineWidth / 2 - _offset), 0.7f),
                SplineSide.Right => new Vector2((_splineWidth / 2 - _offset), 0.7f),
                _ => Vector2.zero
            };
        }
    }
}