using Managers;
using UnityEngine;
using Utility;

namespace Entities.Weapons
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float flySpeed = 3f;
        [SerializeField] private Transform collisionPoint;
        private readonly float collisionRadius = .5f;

        private void OnEnable()
        {
            UpdateManager.Instance.SubscribeToGlobalFixedUpdate(this.Fly);
        }


        private void OnDisable()
        {
            UpdateManager.Instance.UnsubscribeFromGlobalFixedUpdate(this.Fly);
        }


        private void Fly()
        {
            var movementThisFrame = transform.forward * flySpeed * Time.fixedDeltaTime;
            transform.localPosition += movementThisFrame;

            var collisionResult = this.CheckCollision();
            bool somethingIsHit = collisionResult != null;
            if (somethingIsHit) {
                collisionResult.OnHit();
                Pool.ReturnToPool(gameObject);
            }
        }


        private IHittable CheckCollision()
        {
            var hittablesActive = InteractionChart.Instance.HittablesActive;
            for (int i = 0; i < hittablesActive.Count; i++) {
                var hittable = hittablesActive[i];
                var sqrDistance = (hittable.Position - this.collisionPoint.position).Set(y: 0).sqrMagnitude;
                //Debug.Log($"arrow pos: {collisionPoint.position}, targetPos: {hittable.Position}\nsqrDistance: {sqrDistance}");
                bool withinRange = sqrDistance < Mathf.Pow(this.collisionRadius, 2);

                if (withinRange)
                    return hittable;
            }

            return null;
        }
    }
}
