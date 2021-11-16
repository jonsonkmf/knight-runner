using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using ScriptableSystem.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image[] _heartImages;
        [SerializeField] private GameEvent _onLevelLoaded;
        [SerializeField] private IntVariable _health;

        private void Awake()
        {
            _onLevelLoaded.AddAction(ResetHealth);
            _health.OnChanged.AddAction(UpdateHealth);
        }

        private void OnDestroy()
        {
            _onLevelLoaded.RemoveAction(ResetHealth);
            _health.OnChanged.RemoveAction(UpdateHealth);
            
        }

        private void ResetHealth()
        {
            foreach (var hearts in _heartImages)
            {
                hearts.gameObject.SetActive(true);
            }
        }

        private void UpdateHealth()
        {
            for (int i = _heartImages.Length - 1; i >= _health.Value; i--)
            {
                _heartImages[i].gameObject.SetActive(false);
            }
        }
    }
}