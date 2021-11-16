using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class ProgressBar: SerializedMonoBehaviour
    {
        [SerializeField] private FloatGameEvent _onProgressing;
        [SerializeField] private GameEvent _onResetProgress;
        [SerializeField] private GameEvent _onProgressBarOff;
        
        [SerializeField] [ChildGameObjectsOnly]
        private Image _progressBar;

        private void Awake()
        {
            _onProgressing.AddAction(UpdateProgress);
            _onResetProgress.AddAction(ResetProgress);
            _onProgressBarOff.AddAction(Deactivate);
        }

        private void OnDestroy()
        {
            _onProgressing.RemoveAction(UpdateProgress);
            _onResetProgress.RemoveAction(ResetProgress);
            _onProgressBarOff.RemoveAction(Deactivate);

        }

        private void Deactivate() => gameObject.SetActive(false);

        private void ResetProgress()
        {
            gameObject.SetActive(true);
            _progressBar.fillAmount = 0;
        }

        private void UpdateProgress(float progress)
        {
            _progressBar.fillAmount = progress;
        }
    }
}