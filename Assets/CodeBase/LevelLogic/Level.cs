using System.Collections.Generic;
using CodeBase.EnemyLogic;
using CodeBase.EnemySpawnerLogic;
using CodeBase.Horse;
using CodeBase.ScriptableObjects.GameVariables;
using CodeBase.SpearLogic;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.LevelLogic
{
    [InlineEditor()]
    public class Level : SerializedMonoBehaviour, IInstantiator
    {
        [SerializeField] [ChildGameObjectsOnly]
        private SplineComputer _spline;

        [SerializeField] [ChildGameObjectsOnly]
        private SplineMesh _splineMesh;

        [OnValueChanged("UpdateTrigger")] [SerializeField] [Range(0, 100f)]
        private float _triggerValue;

        [Title("Environment")] [SerializeField]
        private EnvironmentData _environmentData;

        [SerializeField] private EnvironmentDataEvent _onEnvironmentUpdate;
        [SerializeField] private GameObject[] _environmentObjects;
        [SerializeField] private Vector2 _offset = new Vector2(-15, 0);
        [SerializeField] [ReadOnly] private int _randomSeed;
        [SerializeField] private float _densityOn15Meters = 1;


        [Title("Events")] [SerializeField] private GameEvent _onSpearElementCollected;
        [SerializeField] private FloatGameEvent _onSuccessValueUpdated;

        [Title("Variables")] [SerializeField] private FollowerSpeed _playerSpeed;


        [OnValueChanged("UpdateEnemyList")] [ShowIf("CheckForSpline")] [TableList(ShowIndexLabels = true)]
        public List<SpawnItem<Enemy>> _enemies = new List<SpawnItem<Enemy>>();

        [OnValueChanged("UpdateSpearElementList")] [ShowIf("CheckForSpline")] [TableList(ShowIndexLabels = true)]
        public List<SpawnItem<SpearElement>> _spearElements = new List<SpawnItem<SpearElement>>();

        private Spawner<Enemy> _enemySpawner;
        private Spawner<SpearElement> _spearElementSpawner;

        private Enemy[] _currentEnemies;
        private SpearElement[] _currentSpearElements;
        private int _collectedSpearElements;
        private List<ObjectController> _environments = new List<ObjectController>();

        public void SetRandomSeed(int number)
        {
            _randomSeed = number;
            for (int i = 0; i < _environments.Count; i++)
            {
                ObjectController environment = _environments[i];
                environment.randomSeed = number * 100 + i;
            }
        }

        private bool CheckForSpline() => _spline != null && _splineMesh != null;

        private void UpdateTrigger() => _spline.triggerGroups[0].triggers[0].position =
            _spline.Travel(1.0f, _triggerValue, Spline.Direction.Backward);

        [Button]
        private void Spawn()
        {
            Destroy();
            SpawnEnemies();
            SpawnSpearElements();
        }

        [Button]
        private void Destroy()
        {
            if (_currentEnemies != null)
                foreach (var currentEnemy in _currentEnemies)
                {
                    if (currentEnemy == null) continue;
                    DestroyImmediate(currentEnemy.gameObject);
                }

            if (_currentSpearElements == null) return;
            foreach (var currentSpearElement in _currentSpearElements)
            {
                if (currentSpearElement == null) continue;
                DestroyImmediate(currentSpearElement.gameObject);
            }
        }

        [Button]
        private void SpawnEnvironments()
        {
            int spawnCount = Mathf.RoundToInt(_spline.CalculateLength() / 15f * _densityOn15Meters);
            SpawnEnvironment(new Vector3(-75, 0, 0), spawnCount);
            SpawnEnvironment(new Vector3(-30, 0, 0), spawnCount);
        }

        private void SpawnEnvironment(Vector3 spawnPosition, int spawnCount)
        {
            SplineComputer environment = Instantiate(_spline, spawnPosition, Quaternion.identity, transform);
            DestroyImmediate(environment.GetComponent<MeshGenerator>());
            var objectController = environment.gameObject.AddComponent<ObjectController>();
            objectController.objects = _environmentObjects;
            objectController.buildOnAwake = true;
            objectController.spawnCount = spawnCount;
            objectController.minOffset = _offset;
            _environments.Add(objectController);
        }


        [Button]
        private void ClearEnvironment()
        {
            foreach (ObjectController environment in _environments)
                DestroyImmediate(environment);
            _environments.Clear();
        }

        private void OnValidate()
        {
            UpdateEnemyList();
            UpdateSpearElementList();
        }

        private void Start()
        {
            SpawnEnemies();
            SpawnSpearElements();
            _onEnvironmentUpdate.Invoke(_environmentData);
            _collectedSpearElements = 0;
            _onSuccessValueUpdated.Invoke(0);
        }

        private void SpawnEnemies()
        {
            float splineWidth = _splineMesh.GetChannel(0).minScale.x;
            _enemySpawner = new Spawner<Enemy>(_enemies, this, splineWidth, _spline, _playerSpeed.Speed,
                _playerSpeed.HeroFollower);
            _currentEnemies = _enemySpawner.Spawn();
        }

        private void SpawnSpearElements()
        {
            float splineWidth = _splineMesh.GetChannel(0).minScale.x;
            _spearElementSpawner = new Spawner<SpearElement>(_spearElements, this, splineWidth, _spline,
                _playerSpeed.Speed, _playerSpeed.HeroFollower);
            _currentSpearElements = _spearElementSpawner.Spawn();
        }

        private void OnEnable() => _onSpearElementCollected.AddAction(UpdateSuccessValue);

        private void OnDisable() => _onSpearElementCollected.RemoveAction(UpdateSuccessValue);


        private void UpdateSuccessValue()
        {
            _collectedSpearElements++;
            _onSuccessValueUpdated.Invoke(Mathf.InverseLerp(0, _spearElements.Count, _collectedSpearElements));
        }

        private void UpdateEnemyList()
        {
            if (_spline == null) return;

            float currentOffset = 0;
            foreach (var enemyData in _enemies)
            {
                enemyData.UpdateData(_spline, currentOffset);
                currentOffset += enemyData._offset;
            }
        }

        private void UpdateSpearElementList()
        {
            if (_spline == null) return;

            float currentOffset = 0;
            foreach (var spearElement in _spearElements)
            {
                spearElement.UpdateData(_spline, currentOffset);
                currentOffset += spearElement._offset;
            }
        }


        public ISpawnableItem Spawn(ISpawnableItem enemy) =>
            Instantiate(enemy.gameObject, transform).GetComponent<ISpawnableItem>();
    }
}