using DG.Tweening;
using ScriptableSystem.GameEvent;
using UnityEngine;

namespace CodeBase.EndGame
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private GameEvent _onBossStartMoving;
        [SerializeField] [Range(0, 10f)] private float _openHeight = 5f;
        [SerializeField] [Range(0, 1f)] private float _openTime = 0.5f;
        [SerializeField] private Ease _easing = Ease.Linear;

        private void OnEnable() => _onBossStartMoving.AddAction(Open);

        private void OnDisable() => _onBossStartMoving.RemoveAction(Open);

        private void Open() => transform.DOMoveY(transform.position.y + _openHeight, _openTime).SetEase(_easing);
    }
}