using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class FollowXZWithOffset: FollowBehavior
    {
        private readonly Transform _target;
        private readonly float _offsetZ;
        private readonly float _followSpeed;
        private readonly float _positionY;

        public FollowXZWithOffset(Transform follower, Transform target, float offsetY, float offsetZ, float followSpeed) : base(follower)
        {
            _target = target;
            _offsetZ = offsetZ;
            _followSpeed = followSpeed;
            _positionY = Follower.position.y + offsetY;
        }

        public override void Follow()
        {
            var newPosition = new Vector3(_target.position.x, _positionY, _target.position.z + _offsetZ);
            Follower.position = Vector3.MoveTowards(Follower.position, newPosition, _followSpeed * Time.deltaTime); ;
        }
    }
}