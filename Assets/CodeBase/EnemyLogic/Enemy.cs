using System.Collections;
using CodeBase.EnemyLogic.PushLogic;
using CodeBase.EnemyLogic.RagdollLogic;
using CodeBase.ScriptableObjects.Events;
using CodeBase.ScriptableObjects.GameVariables;
using Dreamteck.Splines;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.EnemyLogic
{
    public abstract class Enemy: MonoBehaviour, ISpawnableItem
    {
        [SerializeField] private IntVariable _currentScore;
        [SerializeField] private GameEvent _onEnemyDied;
        [SerializeField] private GameEvent _onSpearDied;

        [SerializeField] private SplineVariable _spline;

        [SerializeField] [ChildGameObjectsOnly]
        protected Animator _animator;

        [SerializeField] private BoolGameEvent _onGameState;

        [SerializeField] [ChildGameObjectsOnly]
        private EnemyCollider _collider;
        
        [SerializeField] [ChildGameObjectsOnly]
        private EnemyCollider _colliderHorse;


        [SerializeField] [ChildGameObjectsOnly]
        protected TriggerZone _triggerZone;

        [SerializeField] [ChildGameObjectsOnly]
        protected SplineFollower _follower;

        [SerializeField] private bool _isBoss = false;
        
        protected IRagdollController _ragdollControllerHorse;

        protected Animator _horseAnimator;

        protected IRagdollController _ragdollController;
        private IForceApplier _forceApplier;
        protected bool _isDead = false;
        protected bool IsActive = false;
        private float _followSpeed;
        private float _waitTime;
        private Coroutine _currentCoroutine;
        protected SplineFollower _hero;

        public float XoffsetSpear;

        public void SetSpawnPosition(double position, Vector2 spawnOffset, SplineComputer spline, float playerTime, SplineFollower hero)
        {
            _follower.startPosition = spline.Travel(position, _followSpeed * playerTime, Spline.Direction.Forward);
            if(_followSpeed > 0.5f) _waitTime = playerTime - spline.CalculateLength(position, 1)/_followSpeed;
            _follower.motion.offset = spawnOffset;
            _hero = hero;
        }


        protected void Awake()
        {
            if (TryGetComponent(out Animator horseanim))
            {
                _horseAnimator = horseanim;
            }

            _followSpeed = _follower.followSpeed;
            _follower.followSpeed = 0;
            SetActive(false); //статус активности
            if (TryGetComponent<EnemyCollider>(out EnemyCollider colliderHorse))
            {
                _colliderHorse = colliderHorse;
            }
        }

        protected virtual void OnEnable()
        {
            _onGameState.AddAction(SetActive);
            _triggerZone.OnSpearEnter += SpearDie;
        }

        private void SetActive(bool state)
        {
            IsActive = state;
            if (state)
            {
                if (_horseAnimator != null)
                {
                    _horseAnimator.speed = 1;
                }

                _currentCoroutine = StartCoroutine(ResetSpeed());
                _animator.speed = 1;
            }
            else
            {
                if (_horseAnimator != null)
                {
                    _horseAnimator.speed = 0;
                }
                
                if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
                
                _follower.followSpeed = 0;
                _animator.speed = 0;
            }
        }

        private IEnumerator ResetSpeed()
        {
            yield return new WaitForSeconds(_waitTime);
            if(IsActive) _follower.followSpeed = _followSpeed;
        }

        protected virtual void OnDisable()
        {
            _onGameState.RemoveAction(SetActive);
            _triggerZone.OnSpearEnter -= SpearDie;
        }

        protected virtual void Start()
        {
            if (_colliderHorse!=null && _horseAnimator!=null)
            {
                _ragdollControllerHorse = new RagdollController(_colliderHorse.rigidbodies, _horseAnimator);
                _ragdollControllerHorse.IsRagdoll(state: false);
            }
            _ragdollController = new RagdollController(_collider.rigidbodies, _animator);
            _forceApplier = new ForceApplier(_collider.rigidbodies);
            _follower.spline = _spline.Value;
            _ragdollController.IsRagdoll(state: false);
        }

        protected virtual void Die()
        {
            if (_isDead) return;
            _currentScore.Value++;
            _isDead = true;
            _onEnemyDied.Invoke();
            _ragdollController.IsRagdoll(true);
        }

        private void SpearDie(Vector3 pushForce)
        {
            if (_isDead) return;
            Die();
            _onSpearDied.Invoke();
            if (_isBoss==true)
            {
                Handheld.Vibrate();
            }
                //Handheld.Vibrate();
            _forceApplier.Apply(pushForce);
        }
        protected  void FireDieHorse()
        {
            if (_colliderHorse!=null && _horseAnimator!=null)
            {
                _ragdollControllerHorse.IsRagdoll(state: true);
            }
        }
    }
}