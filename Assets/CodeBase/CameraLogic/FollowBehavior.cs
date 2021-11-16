using UnityEngine;

namespace CodeBase.CameraLogic
{
    public abstract class FollowBehavior
    {
        protected readonly Transform Follower;

        protected FollowBehavior(Transform follower)
        {
            Follower = follower;
        }
        
        public abstract void Follow();
    }
}