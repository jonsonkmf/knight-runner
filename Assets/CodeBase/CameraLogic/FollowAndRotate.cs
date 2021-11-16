using DG.Tweening;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class FollowAndRotate: FollowBehavior
    {
        private readonly Transform _target;
        private readonly Transform _target2;
        private readonly float _offsetZ;
        private readonly float _followSpeed;
        private readonly float _positionY;

        public FollowAndRotate(Transform follower, Transform target,Transform target2, float offsetY, float offsetZ, float followSpeed) : base(follower)
        {
            _target = target;
         //   _offsetZ = offsetZ;
            _followSpeed = followSpeed;
            _positionY = Follower.position.y;// + offsetY;

            _target2 = target2;
        }

        public override void Follow()
        {
            Follower.position = Vector3.MoveTowards(Follower.position, _target.position, _followSpeed * Time.deltaTime/10); 
            //Follower.LookAt(_target2);
        }
    }
}