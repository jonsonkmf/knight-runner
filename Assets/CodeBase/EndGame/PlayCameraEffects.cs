using System;
using ScriptableSystem.GameEvent;
using UnityEngine;

namespace CodeBase.EndGame
{
    public class PlayCameraEffects: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private GameEvent _onLevelComplete;

        private void OnEnable() => _onLevelComplete.AddAction(_effect.Play);

        private void OnDisable() => _onLevelComplete.RemoveAction(_effect.Play);
    }
}