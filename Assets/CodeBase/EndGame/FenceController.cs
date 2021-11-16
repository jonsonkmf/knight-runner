using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.EndGame
{
    public class FenceController: MonoBehaviour
    {
        [SerializeField][ChildGameObjectsOnly] private Fence[] _fences;
        [SerializeField] private FloatGameEvent _onSuccessValueUpdated;
        [SerializeField] private GameEvent _onBossDefeated;
        [SerializeField] private bool _isRandom;

        private int _fenceToActivate = 0;
        private void OnEnable()
        {
            _onSuccessValueUpdated.AddAction(CalculateFences);
            _onBossDefeated.AddAction(ActivateFences);
        }

        private void OnDisable()
        {
            _onSuccessValueUpdated.RemoveAction(CalculateFences);
            _onBossDefeated.RemoveAction(ActivateFences);
        }

        private void CalculateFences(float successValue)
        {
            _fenceToActivate = Mathf.RoundToInt(Mathf.Lerp(0, _fences.Length, successValue));
            Debug.Log($"Fence to Activate: {_fenceToActivate}");
        }

        private void ActivateFences()
        {
            if (_isRandom) _fenceToActivate = Random.Range(0, _fences.Length);
            for (int i = 0; i < _fenceToActivate; i++) _fences[i].ActivateFence(true);
            for(int j = _fenceToActivate; j < _fences.Length; j++) _fences[j].ActivateFence(false);
        }
    }
}