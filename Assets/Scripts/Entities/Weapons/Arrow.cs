using Entities.Stats;
using Managers;
using System.Collections;
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
            this.distanceFlew = 0;
            UpdateManager.Instance.SubscribeToGlobalFixedUpdate(this.Fly);
        }


        private void OnDisable()
        {
            UpdateManager.Instance.UnsubscribeFromGlobalFixedUpdate(this.Fly);
        }


        public void SetFlightDistance(float flightDistance)
            => this.flightDistance = flightDistance;


        private AttackInfo attackInfo;
        public void SetAttackInfo(AttackInfo attackInfo)
            => this.attackInfo = attackInfo;


        private float flightDistance = 3f;
        private float distanceFlew;
        private void Fly()
        {
            if (!gameObject.activeInHierarchy)
                return;

            var movementAmount = flySpeed * Time.fixedDeltaTime;
            this.distanceFlew += movementAmount;
            var movementThisFrame = transform.forward * movementAmount;
            transform.localPosition += movementThisFrame;

            var collisionResult = this.CheckCollision();
            bool somethingIsHit = collisionResult != null;
            if (somethingIsHit) {
                collisionResult.OnHit(this.attackInfo);
                Pool.ReturnToPool(gameObject);
            }

            if (this.distanceFlew >= this.flightDistance)
                Pool.ReturnToPool(gameObject);
        }



        private IHittable CheckCollision()
        {
            var hittablesActive = InteractionChart.Instance.HittablesActive;
            for (int i = 0; i < hittablesActive.Count; i++) {
                var hittable = hittablesActive[i];
                var sqrDistance = (hittable.Position - this.collisionPoint.position).Set(y: 0).sqrMagnitude;
                bool withinRange = sqrDistance < Mathf.Pow(this.collisionRadius, 2);

                if (withinRange)
                    return hittable;
            }

            return null;
        }
    }
}
