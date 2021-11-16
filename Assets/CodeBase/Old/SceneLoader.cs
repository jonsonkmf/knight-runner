using System.Collections;
using ScriptableSystem.GameEvent;
using ScriptableSystem.GameEventParameterized.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Old
{
    public class SceneLoader : SerializedMonoBehaviour
    {
        [SerializeField] private GameEvent _onSceneStartLoading;
        [SerializeField] private FloatGameEvent _onSceneLoadProgress;
        [SerializeField] private GameEvent _onSceneLoaded;
        [SerializeField] private StringGameEvent _onLoadScene;
        
        private void Awake() => DontDestroyOnLoad(this);

        private void OnEnable() => _onLoadScene.AddAction(Load);

        private void OnDisable() => _onLoadScene.RemoveAction(Load);

        private void Load(string sceneName) => StartCoroutine(LoadSceneAsync(sceneName));

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            _onSceneStartLoading.Invoke();
            var operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                _onSceneLoadProgress.Invoke(progress);
                yield return null;
            }


            _onSceneLoaded.Invoke();
        }
    }
}