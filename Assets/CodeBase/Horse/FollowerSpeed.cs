using Dreamteck.Splines;
using ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Horse
{
    [InlineEditor()]
    [CreateAssetMenu(
        menuName = SOConstants.DataSubmenu + "FollowerSpeed",
        fileName = "HorseSpeed",
        order = SOConstants.AssetMenuOrder)]
    public class FollowerSpeed : ScriptableObject
    {
        [SerializeField] private SplineFollower _follower;

        public float Speed => _follower.followSpeed;
        public SplineFollower HeroFollower => _follower;
    }
}