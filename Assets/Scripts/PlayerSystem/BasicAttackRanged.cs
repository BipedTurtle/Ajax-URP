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
        [SerializeField] private AttackInfoArchetype attackInfoArchetype;
        private AttackInfo attackInfo;
        private void OnEnable()
        {
            this.nextAttack = 0;

            this.attackInfo = this.attackInfoArchetype.Copy();
            //var attackInfoOpHandle = this.attackInfoArchetype.LoadAssetAsync<AttackInfoArchetype>();
            //attackInfoOpHandle.Completed += (op) =>
            //{
            //    var archetype = op.Result;
            //    this.attackInfo = archetype.Copy();
            //    //Addressables.Release(op);
            //};
        }


        private Pool arrowPool;
        public override void Execute()
        {
            var player = PlayerController.Instance;

            float range = player.PlayerStats.Range;
            bool targetInRange = (player.Target.localPosition - player.transform.localPosition).sqrMagnitude < Mathf.Pow(range, 2);
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

            var player = PlayerController.Instance;
            var arrow = this.arrowPool.GetPooledObjectAt(player.transform.localPosition, player.transform.localRotation).GetComponent<Arrow>();
            arrow.SetFlightDistance(player.PlayerStats.Range);
            arrow.SetAttackInfo(this.attackInfo);
        }
    }
}
