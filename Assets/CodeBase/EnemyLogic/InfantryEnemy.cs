using Dreamteck.Splines;
using UnityEngine;

namespace CodeBase.EnemyLogic
{
    [RequireComponent(typeof(SplineFollower))]
    public class InfantryEnemy : Enemy
    {
        [SerializeField] private int _damagePower;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _isMovable;
        [SerializeField] private Material _fireMaterial;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private bool _canFire = true;
        [SerializeField] private GameObject[] _weapons;
        [SerializeField] private attackzone _attackzone;
        [SerializeField] private bool _isDestroyable = true;
        [SerializeField] private ParticleSystem _fireParticle;
        [SerializeField] private EnemyChangeShield _shield;

        private Horse.Horse _herohorse;
        private float _maxOffsetX;
        private float _minOffsetX;
        private bool _right = true;
        private bool _isAttacked = false;

        private float _timer = 0;
        private float _timer1 = 0;
        private float _lerpTimer = 0;


        protected override void Start()
        {
            base.Start();
            _maxOffsetX = _follower.motion.offset.x;
            _minOffsetX = -_maxOffsetX;
            if (_minOffsetX > _maxOffsetX)
            {
                var c = _maxOffsetX;
                _maxOffsetX = _minOffsetX;
                _minOffsetX = c;
            }

            _animator.Play("Idle", 0, Random.Range(0f, 1f));
            if (_horseAnimator != null)
            {
                _horseAnimator.Play("Gallop", 0, Random.Range(0f, 1f));
            }

            _herohorse = FindObjectOfType<Horse.Horse>();
        }

        private void Update()
        {
            if (_isMovable && !_isDead && IsActive)
            {
                /*  if (Mathf.Abs(_follower.motion.offset.x) > Mathf.Abs(_maxOffsetX) * 0.95)
                  {
                      _animator.speed = 1;
                  }
                  else
                  {
                      if (_animator.speed > 0.1)
                      {
                          _animator.speed -= Time.deltaTime / 5;
                      }
                      else
                      {
                          _animator.speed = 0;
                      }
                  } /**/

                Move();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_attackzone != null)
            {
                _attackzone._player += Attack;
            }

            if (_canFire)
            {
                _triggerZone.OnDragonFireEnter += FireDie;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _attackzone._player -= Attack;
            if (_canFire)
            {
                _triggerZone.OnDragonFireEnter -= FireDie;
            }
        }

        private void Move()
        {
            //Debug.Log(_herohorse.FolowProgress);
            if (!IsActive) return;
            _timer += Time.deltaTime;
            if (_timer < 2)
            {
                _animator.SetBool("Idle", true);
                if (_follower.startPosition - _herohorse.FolowProgress < 0.03)
                {
//                       Debug.Log("Тормозим перемещение");
                    _isMovable = false;
                }

                //  _animator.speed = 0;
                _lerpTimer = 0;
                return;
            }

            if (_follower.motion.offset.x > _minOffsetX * 0.95 && _right)
            {
                _lerpTimer += Time.deltaTime;
                _follower.motion.offset =
                    new Vector2(Mathf.Lerp(_follower.motion.offset.x, _minOffsetX, _moveSpeed / 3 * _lerpTimer),
                        _follower.motion.offset.y);
                _animator.SetBool("Idle", false);
                _animator.SetBool("toleft", true);
            }
            else
            {
                _timer1 += Time.deltaTime;
                if (_timer1 < 2)
                {
                    _animator.SetBool("Idle", true);
                    if (_follower.startPosition - _herohorse.FolowProgress < 0.03)
                    {
                        //      Debug.Log("Тормозим перемещение");
                        _isMovable = false;
                    }

                    //    _animator.speed = 0;
                    _lerpTimer = 0;
                    return;
                }

                if (_follower.motion.offset.x < _maxOffsetX * 0.95)
                {
                    _lerpTimer += Time.deltaTime;
                    _follower.motion.offset =
                        new Vector2(Mathf.Lerp(_follower.motion.offset.x, _maxOffsetX, _moveSpeed / 3 * _lerpTimer),
                            _follower.motion.offset.y);
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("toleft", false);

                    _right = false;
                }
                else
                {
                    _lerpTimer = 0;
                    _timer = 0;
                    _timer1 = 0;
                    _right = true;
                }
            }
        }

        private void Attack(IDamagable player)
        {
            if (_isDead) return;
            if (_isAttacked) return;
            _isAttacked = true;
            _animator.SetTrigger("attack");
            player.TakeDamage(_damagePower);
        }

        private void FireDie()
        {
            Die();
            FireDieHorse();
            if (_fireParticle != null)
            {
                _fireParticle.Play();
            }

            _skinnedMeshRenderer.sharedMaterial = _fireMaterial;
            foreach (var weapon in _weapons)
            {
                foreach (var material in weapon.GetComponent<MeshRenderer>().materials)
                {
                    material.color = Color.black;
                }
                //weapon.GetComponent<MeshRenderer>().sharedMaterial.color = Color.black;
            }

            _shield.CurrentShield.material.color = Color.black;
        }


        protected override void Die()
        {
            base.Die();
            foreach (var weapon in _weapons)
            {
                weapon.transform.parent = transform.parent;

                weapon.GetComponent<Rigidbody>().isKinematic = false;
            }

            if (!_isDestroyable) return;

            this.InvokeDelegate(() => { Destroy(this.gameObject); }, 5);
        }
    }
}