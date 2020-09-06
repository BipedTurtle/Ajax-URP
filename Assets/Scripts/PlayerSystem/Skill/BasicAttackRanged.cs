using Entities.Stats;
using Entities.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "PlayerSystem/Skills/BasicAttack")]
    public class BasicAttackRanged : DamagingSkill
    {
        [SerializeField] private PlayerInfo playerInfo;
        protected override void OnEnable()
        {
            base.OnEnable();
            this.nextActivation = 0;
        }


        private Pool arrowPool;
        public override void Execute()
        {
            var player = PlayerController.Instance;

            float range = Player.Instance.PlayerStats.Range;
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
            var player = Player.Instance;

            bool attackCoolHasReturned = Time.time > this.nextActivation;

            var toTargetVector = (PlayerController.Instance.Target.transform.localPosition - player.transform.localPosition);
            var dotProduct = Vector3.Dot(player.transform.forward.Set(y: 0).normalized, toTargetVector.Set(y: 0).normalized);
            bool aligned = dotProduct > .995f;

            if (attackCoolHasReturned & aligned) {
                this.ShootArrow();
                this.nextActivation = Time.time + player.PlayerStats.AttackSpeed;
            }
        }


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private void ShootArrow()
        {
            if (this.arrowPool == null)
                this.arrowPool = Pool.GetPool(this.arrowReference).Result;

            var player = Player.Instance;
            var arrow = this.arrowPool.GetPooledObjectAt(player.transform.localPosition, player.transform.localRotation).GetComponent<Arrow>();
            arrow.SetFlightDistance(player.PlayerStats.Range);
            arrow.SetAttackInfo(player.PlayerStats, this.skillInfo);
        }
    }
}
