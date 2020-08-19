using Entities.Stats;
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
        [SerializeField] private AssetReference attackInfoArchetype;
        private AttackInfo attackInfo;
        private void OnEnable()
        {
            this.nextAttack = 0;

            var attackInfoOperationHandle = this.attackInfoArchetype.LoadAssetAsync<AttackInfoArchetype>();
            attackInfoOperationHandle.Completed += (op) =>
            {
                var archetype = op.Result;
                this.attackInfo = archetype.Copy();

                Addressables.Release(op);
            };
        }

        
        private Pool arrowPool;
        public override void Execute()
        {
            var player = PlayerController.Instance;

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


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private void ShootArrow()
        {
            if (this.arrowPool == null)
                this.arrowPool = Pool.GetPool(this.arrowReference).Result;

            var player = PlayerController.Instance.transform;
            var arrow = this.arrowPool.GetPooledObjectAt(player.localPosition, player.localRotation).GetComponent<Arrow>();

            arrow.SetFlightDistance(this.playerInfo.Range);
            arrow.SetAttackInfo(this.attackInfo);
        }
    }
}
