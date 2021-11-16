using System;
using CodeBase.ScriptableObjects.GameVariables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TotalScoreText: SerializedMonoBehaviour
    {
        [ChildGameObjectsOnly][SerializeField] private TMP_Text _resultText;
        [SerializeField] private IntVariable _scores;
        [SerializeField] private string _text;
        private void Awake() => _scores.OnChanged.AddAction(SetResultText);

        private void OnEnable() => SetResultText();


        private void OnDestroy() => _scores.OnChanged.RemoveAction(SetResultText);

        private void SetResultText()
        {
            _resultText.SetText($"{_text}{_scores.Value}");
        }
    }
}