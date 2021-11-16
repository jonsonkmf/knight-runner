using CodeBase.LevelLogic;
using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LevelManager : MonoBehaviour
    {
        [OnValueChanged("SetRandomSeed")] [Title("Data")] [SerializeField]
        private Level[] _levelPrefabs;

        [SerializeField] private Horse.Horse _playerPrefab;
        [SerializeField] private PlayerData _playerData;

        [Title("Events")] [SerializeField] private BoolGameEvent _onGameState;
        [SerializeField] private GameEvent _onLevelEndButtonPressed;
        [SerializeField] private GameEvent _onLevelStarted;
        [SerializeField] private GameEvent _onInputEnded;
        [SerializeField] private GameEvent _onLevelLoaded;
        [SerializeField] private GameEvent _onLevelComplete;
        [SerializeField] private GameEvent _onFinish;
        [SerializeField] private GameEvent _onPlayerKilled;
        [SerializeField] private GameEvent _onGameOver;
        
        [Title("Scores")]
        [SerializeField] private IntGameEvent _onBonusCollected;
        [SerializeField] private IntGameEvent _onTotalScoreCalculated;
        [SerializeField] private IntVariable _currentScore;

        private int _currentLevelNumber;
        private Level _currentLevel;
        private Horse.Horse _currentPlayer;
        private int _totalScores;


        private void SetRandomSeed()
        {
            for (var i = 0; i < _levelPrefabs.Length; i++)
            {
                Level level = _levelPrefabs[i];
                level.SetRandomSeed(i + 1);
            }
        }

        private void Awake()
        {
            LoadPlayerProgress();
            SpawnLevel(_currentLevelNumber);
            SpawnPlayer();
            _onInputEnded.AddAction(StartLevel);
        }

        private void Start()
        {
            _onGameState.Invoke(false);
            _onLevelLoaded.Invoke();
            _currentScore.Value = 0;
        }

        private void OnEnable()
        {
            _onFinish.AddAction(CompleteLevel);
            _onPlayerKilled.AddAction(GameOver);
            _onLevelEndButtonPressed.AddAction(LoadLevel);
            _onBonusCollected.AddAction(UpdateTotalScores);
        }

        private void UpdateTotalScores(int scores)
        {
            _totalScores = _currentScore.Value + scores;
            _onTotalScoreCalculated.Invoke(_totalScores);
        }

        private void OnDisable()
        {
            _onFinish.RemoveAction(CompleteLevel);
            _onPlayerKilled.RemoveAction(GameOver);
            _onLevelEndButtonPressed.RemoveAction(LoadLevel);
            _onBonusCollected.RemoveAction(UpdateTotalScores);
        }


        private void LoadLevel()
        {
            _currentScore.Value = 0;
            DestroyCurrentLevel();
            DestroyPlayer();
            SpawnLevel(_currentLevelNumber);
            SpawnPlayer();
            _onInputEnded.AddAction(StartLevel);
            _onLevelLoaded.Invoke();
        }


        private void StartLevel()
        {
            _onLevelStarted.Invoke();
            _onGameState.Invoke(true);
            _onInputEnded.RemoveAction(StartLevel);
        }

        private void GameOver()
        {
            _onGameState.Invoke(false);
            _onGameOver.Invoke();
        }

        private void CompleteLevel()
        {
            UpdateCurrentLevelNumber();
            _playerData.SaveScores(_totalScores);
            _playerData.SaveLevelProgress(_currentLevelNumber);
            _onGameState.Invoke(false);
           // _onLevelComplete.Invoke();
            this.InvokeDelegate(() => { _onLevelComplete.Invoke(); }, 3f);
        }

        private void LoadPlayerProgress() => _currentLevelNumber = _playerData.CurrentLeveNumber % _levelPrefabs.Length;

        private void SpawnLevel(int levelNumber) => _currentLevel = Instantiate(_levelPrefabs[levelNumber]);

        private void SpawnPlayer() => _currentPlayer = Instantiate(_playerPrefab);

        private void DestroyPlayer() => Destroy(_currentPlayer.gameObject);

        private void DestroyCurrentLevel() => Destroy(_currentLevel.gameObject);


        private void UpdateCurrentLevelNumber()
        {
            _currentLevelNumber++;
            _currentLevelNumber %= _levelPrefabs.Length;
        }
    }
}