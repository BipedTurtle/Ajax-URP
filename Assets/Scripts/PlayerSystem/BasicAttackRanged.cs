using Entities.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "PlayerSystem/Skills/BasicAttack")]
    public class BasicAttackRanged : Skill
    {
        [SerializeField] private PlayerInfo playerInfo;
        private float nextAttack;
        public float attackInterval = 2f;
        private void OnEnable()
        {
            this.nextAttack = 0;
        }


        private Pool arrowPool;
        public override void Execute()
        {
            var player = PlayerController.Instance;

            //bool aligned = this.TurnTowardsTarget();
            //Debug.Log($"forward: {player.transform.forward}\naligned: {aligned}");
            bool targetInRange = (player.Target.localPosition - player.transform.localPosition).sqrMagnitude < Mathf.Pow(this.playerInfo.Range, 2);
            if (targetInRange) {
                player.Agent.ResetPath();
                this.AttackIfPossible();
            }
            else {
                var destination = player.Target.localPosition;
                player.Agent.SetDestination(destination);
            }
        }


        private void AttackIfPossible()
        {
            var player = PlayerController.Instance;

            bool attackCoolHasReturned = Time.time > this.nextAttack;

            var toTargetVector = (player.Target.transform.localPosition - player.transform.localPosition).normalized;
            var dotProduct = Vector3.Dot(player.transform.forward.Set(y: 0), toTargetVector.Set(y: 0));
            bool aligned = dotProduct > .999f;

            if (attackCoolHasReturned & aligned) {
                this.ShootArrow();
                this.nextAttack = Time.time + attackInterval;
            }
        }


        private bool TurnTowardsTarget()
        {
            var player = PlayerController.Instance;

            var toTargetVector = (player.Target.transform.localPosition - player.transform.localPosition).normalized;
            var dotProduct = Vector3.Dot(player.transform.forward.Set(y: 0), toTargetVector.Set(y: 0));

            if (dotProduct < .99f) {
                var currentRotation = player.transform.localRotation;
                var targetRotation = Quaternion.LookRotation(toTargetVector);
                float progress = .2f;
                player.transform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, progress);

                return false;
            }

            return true;
        }


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private void ShootArrow()
        {
            if (this.arrowPool == null)
                this.arrowPool = Pool.GetPool(this.arrowReference).Result;

            var player = PlayerController.Instance.transform;
            var arrow = this.arrowPool.GetPooledObjectAt(player.localPosition, player.localRotation).GetComponent<Arrow>();
            arrow.SetFlightDistance(this.playerInfo.Range);

        }
    }
}
