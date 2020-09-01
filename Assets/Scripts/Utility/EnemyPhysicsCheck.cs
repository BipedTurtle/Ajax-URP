using UnityEngine;
using PlayerSystem;

namespace Utility
{
    public static class EnemyPhysicsCheck
    {
        public static bool CheckSpherePlayer(Vector3 center, float radius)
        {
            var player = PlayerController.Instance.transform;

            float toPlayerSqrMagnitude = (center - player.localPosition).Set(y: 0).sqrMagnitude;
            return toPlayerSqrMagnitude < Mathf.Pow(radius, 2);
        }
    }
}
