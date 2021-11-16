using ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    [InlineEditor()]
    [CreateAssetMenu(
        menuName = SOConstants.DataSubmenu + "Player",
        fileName = "PlayerData",
        order = SOConstants.AssetMenuOrder)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int _currentLeveNumber = 0;
        [SerializeField] private int _totalScores = 0;

        private readonly BaseEvent _onCoinChanged = new BaseEvent();

        public IEventListener OnCoinChanged => _onCoinChanged;
        public void SaveScores(int scores)
        {
            _totalScores = TotalScores + scores;
            _onCoinChanged.Invoke();
        }

        public void SaveLevelProgress(int levelNumber)
        {
            _currentLeveNumber = levelNumber;
        }

        public int TotalScores => _totalScores;

        public int CurrentLeveNumber => _currentLeveNumber;
    }
}