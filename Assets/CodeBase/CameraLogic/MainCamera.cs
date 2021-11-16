using System;
using System.Collections;
using CodeBase.ScriptableObjects.GameVariables;
using ScriptableSystem.GameEvent;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        [Title("ListenEvents")]
        [SerializeField] private GameEvent _onHeroStartTracking;
        [SerializeField] private GameEvent _onBossFightTracking;
        [SerializeField] private GameEvent _onBossStartTracking;
        [Title("CameraPoints")]
        [SerializeField] private TransformGameVariable _heroTrackPoint;
        [SerializeField] private TransformGameVariable _bossFightTrackPoint;
        [SerializeField] private TransformGameVariable _bossTrackPoint;
        
        [SerializeField][Range(0, 50f)] private float _bossOffsetY = 25f;
        [SerializeField][Range(-50f, 0)] private float _bossOffsetZ = -25f;
        [SerializeField] [Range(0, 100f)] private float _followSpeed;
        [SerializeField] [Range(0, 100f)] private float _followBossSpeed;


        [SerializeField] private Transform _target1;
        [SerializeField] private Transform _target2;

        private FollowBehavior _currentBehaviour;

        private void OnEnable()
        {
            _onHeroStartTracking.AddAction(FollowHero);
            _onBossStartTracking.AddAction(FollowBoss);
            _onBossFightTracking.AddAction(FollowBossFight);
        }

        private void OnDisable()
        {
            _onHeroStartTracking.RemoveAction(FollowHero);
            _onBossStartTracking.RemoveAction(FollowBoss);
            _onBossFightTracking.RemoveAction(FollowBossFight);
        }

        private void FollowBossFight()
        {
            Debug.Log("FollowBossFight");
            _currentBehaviour = new PositionAndRotationSmoothFollow(transform, _bossFightTrackPoint.Value, _followSpeed);
        }

        private void FollowBoss()
        {
            _currentBehaviour = new FollowXZWithOffset(transform, _bossTrackPoint.Value, _bossOffsetY, _bossOffsetZ, _followBossSpeed);
                this.InvokeDelegate(() => {  _currentBehaviour = new FollowAndRotate(transform, _target1, _target2,_bossOffsetY, _bossOffsetZ, _followBossSpeed); }, 1);
        }

        private void FollowHero()
        {
            _currentBehaviour = new PositionAndRotationSmoothFollow(transform, _heroTrackPoint.Value, 0);
        }



        private void LateUpdate()
        {
            _currentBehaviour.Follow();
        }
    }
}