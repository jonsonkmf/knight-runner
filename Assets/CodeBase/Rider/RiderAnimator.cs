using System;
using CodeBase.ScriptableObjects.Events;
using ScriptableSystem;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Rider
{
    public class RiderAnimator : MonoBehaviour
    {
        [SerializeField] [ChildGameObjectsOnly]
        private Animator _animator;

        [SerializeField] private GameEvent _onSpearAttack;

        [SerializeField] private BoolGameEvent _onGameState;
        [SerializeField] private test _triggerForRotateSpear;
        [SerializeField] private Transform _transformSpear;

        private static readonly int AnimSpeed = Animator.StringToHash("animSpeed");
        private static readonly int IsLeft = Animator.StringToHash("IsLeft");
        private RiderSide _currentSide = RiderSide.Left;
        private static readonly int Attack = Animator.StringToHash("SpearAttack");
        private float _spearX;

        private void OnEnable()
        {
            _triggerForRotateSpear.SetSpearRotationX += SetSpearRotate;
            _onGameState.AddAction(SyncAnimators);
            _onSpearAttack.AddAction(SpearAttack);
        }

        private void OnDisable()
        {
            _triggerForRotateSpear.SetSpearRotationX -= SetSpearRotate;
            _onGameState.RemoveAction(SyncAnimators);
            _onSpearAttack.RemoveAction(SpearAttack);
        }

        private void SpearAttack()
        {
            _animator.SetTrigger(Attack);


                //  this.InvokeDelegate(
             //   () =>
              //  {
              
              
             // _transformSpear.localRotation =Quaternion.Lerp( _transformSpear.localRotation,Quaternion.Euler(new Vector3(_spearX, _transformSpear.localRotation.eulerAngles.y, 0)),0.1f);

              
//                    _transformSpear.localRotation = Quaternion.Euler(new Vector3(_spearX, _transformSpear.localRotation.eulerAngles.y, 0));
                    // }, 0.4f);
        /*    this.InvokeDelegate(
                () =>
                {
                    _spearX = 0;
                    _transformSpear.localRotation =
                        Quaternion.Euler(new Vector3(_spearX, _transformSpear.localRotation.eulerAngles.y, 0));
                }, 0.96f);/**/
            //добавить поворот копья вверх вниз в зависимости от типа врага
        }

        private void Update()
        {
            _transformSpear.localRotation =Quaternion.Lerp( _transformSpear.localRotation,Quaternion.Euler(new Vector3(_spearX, _transformSpear.localRotation.eulerAngles.y, 0)),0.1f);

        }

        private void SyncAnimators(bool state)
        {
            _animator.SetFloat(AnimSpeed, state ? 2f : 0.6f);
        }


        private void Awake() => _currentSide = _animator.GetBool(IsLeft) ? RiderSide.Left : RiderSide.Right;

        public void SetSide(RiderSide side)
        {
            _currentSide = side;
            _animator.SetBool(IsLeft, _currentSide == RiderSide.Left);
        }


        public RiderSide GetSide() => _currentSide;

        private void SetSpearRotate(float x)
        {
            Debug.Log($"SetSpearRotate {x}");
            _spearX = x;
        }
    }
}