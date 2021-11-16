using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ResultScores : MonoBehaviour
{
    [Title("Text")]
    [SerializeField][ChildGameObjectsOnly] private TMP_Text _currentScores;
    [SerializeField][ChildGameObjectsOnly] private TMP_Text _bonusScores;
    [SerializeField][ChildGameObjectsOnly] private TMP_Text _resultScores;

    [Title("Events")] [SerializeField] private IntVariable _collectedScores;
    [SerializeField] private IntGameEvent _onBonusCollected;
    [SerializeField] private IntGameEvent _onTotalScoresCalculated;

    private void Awake()
    {
        _onBonusCollected.AddAction(UpdateBonusScores);
        _onTotalScoresCalculated.AddAction(UpdateTotalScores);
        _collectedScores.OnChanged.AddAction(UpdateCollectedCoins);
    }

    private void OnDestroy()
    {
        _onBonusCollected.RemoveAction(UpdateBonusScores);
        _onTotalScoresCalculated.RemoveAction(UpdateTotalScores);
        _collectedScores.OnChanged.RemoveAction(UpdateCollectedCoins);

    }

    private void UpdateCollectedCoins() => _currentScores.SetText(_collectedScores.Value.ToString());
    private void UpdateBonusScores(int scores) => _bonusScores.SetText($"+{scores}");

    private void UpdateTotalScores(int scores) => _resultScores.SetText(scores.ToString());

}
