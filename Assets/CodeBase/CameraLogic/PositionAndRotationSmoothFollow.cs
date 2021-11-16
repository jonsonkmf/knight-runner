using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class PositionAndRotationSmoothFollow: FollowBehavior
    {
        private readonly Transform _target;
        private readonly float _followSpeed;

        public PositionAndRotationSmoothFollow(Transform follower, Transform target, float followSpeed) : base(follower)
        {
            _target = target;
            _followSpeed = followSpeed;
        }

        public override void Follow()
        { 
            if(_followSpeed <= 0) InstanceFollow();
            Follower.position = Vector3.MoveTowards(Follower.position, _target.position, _followSpeed * Time.deltaTime); 
            Follower.rotation = Quaternion.RotateTowards(Follower.rotation, _target.rotation, _followSpeed * Time.deltaTime);
        }


        private void InstanceFollow()
        {
            Follower.position = _target.position;
            Follower.rotation = _target.rotation;
        }
        
    }
}