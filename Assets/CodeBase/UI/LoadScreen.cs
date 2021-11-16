using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.UI
{
    public class LoadScreen: SerializedMonoBehaviour
    {
        [SerializeField] private GameEvent _onSceneStartLoading;
        [SerializeField] private GameEvent _onSceneLoaded;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _onSceneStartLoading.AddAction(Activate);
            _onSceneLoaded.AddAction(Deactivate);
        }

        private void OnDestroy()
        {
            _onSceneStartLoading.RemoveAction(Activate);
            _onSceneLoaded.RemoveAction(Deactivate);
        }

        private void Activate() => gameObject.SetActive(true);
        private void Deactivate() => gameObject.SetActive(false);
    }
}