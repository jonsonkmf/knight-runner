using System.Collections;
using CodeBase.EnemyLogic;
using DG.Tweening;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.SpearLogic
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SplineFollower))]
    public class SpearElement : MonoBehaviour, ISpawnableItem
    {
        [Header("Event")] [SerializeField] private GameEvent _onElementCollected;

        [Title("MoveToSpearAnimation")] [SerializeField]
        private float _time;

        [SerializeField] private Ease _ease = Ease.Linear;


        private SplineFollower _follower;
        
        private void Awake() => _follower = GetComponent<SplineFollower>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Spear spear))
                StartCoroutine(MoveToSpear(spear.transform));
        }

        private IEnumerator MoveToSpear(Transform spear)
        {
            transform.parent = spear;
            transform.DOLocalMove(Vector3.zero, _time).SetEase(_ease);
            yield return new WaitForSeconds(_time);
            _onElementCollected.Invoke();
            gameObject.SetActive(false);
        }

        public void SetSpawnPosition(double position, Vector2 spawnOffset, SplineComputer spline, float playerTime,
            SplineFollower hero)
        {
            _follower.spline = spline;
            _follower.startPosition = position;
            _follower.motion.offset = spawnOffset;
        }
    }
}