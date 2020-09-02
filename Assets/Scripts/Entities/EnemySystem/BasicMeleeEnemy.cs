using Entities.Stats;
using Managers;
using PlayerSystem;
using UnityEngine;
using Utility;

namespace Entities.EnemySystem
{
    public class BasicMeleeEnemy : Enemy
    {
        private Transform playerTransform;
        [SerializeField] private SkillInfoArchetype basicAttackArchetype;
        private SkillInfo basicAttackSkillInfo;
        protected override void Awake()
        {
            base.Awake();

            this.playerTransform = PlayerController.Instance.transform;
            this.basicAttackSkillInfo = this.basicAttackArchetype.Copy();
        }


        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.ActAttack);
            this.UnFreeze();
        }


        protected virtual void MoveTowardsTarget()
        {
            if (Time.frameCount % 15 != 0)
                return;

            base.agent.SetDestination(this.playerTransform.localPosition);

        }


        protected virtual void TurnTowardsTarget()
        {
            if (Time.frameCount % 15 != 0)
                return;

            var toPlayerVector = (playerTransform.localPosition - transform.localPosition).Set(y: 0);
            var lookRotation = Quaternion.LookRotation(toPlayerVector).eulerAngles;

            LeanTween.cancel(gameObject);
            LeanTween.rotate(gameObject, lookRotation, .15f);
        }


        private float SqrDistanceToPlayer => (this.playerTransform.transform.localPosition - transform.localPosition).Set(y: 0).sqrMagnitude;
        private float nextAttackTime;
        protected virtual void ActAttack()
        {
            if (Time.frameCount % 15 != 0)
                return;

            float range = (base.enemyStats == null) ? 1f : base.enemyStats.Range;
            bool isOutOfRange = this.SqrDistanceToPlayer > Mathf.Pow(range, 2);
            bool coolHasNotReturned = Time.timeSinceLevelLoad < this.nextAttackTime;
            if (isOutOfRange | coolHasNotReturned)
                return;

            base.agent.ResetPath();
            base.animator.SetTrigger("Attack");
            this.Freeze();

            this.nextAttackTime = Time.timeSinceLevelLoad + base.enemyStats.AttackSpeed;
        }


        private void Freeze()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.MoveTowardsTarget);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.TurnTowardsTarget);
        }


        /// <summary>
        /// below methods get called by animation clips
        /// </summary>
        private void UnFreeze()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.MoveTowardsTarget);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TurnTowardsTarget);
        }


        [SerializeField] private float attackRadius = .5f;
        private void DealDamage()
        {
            float playerHeight = this.playerTransform.localPosition.y;
            Vector3 center = transform.localPosition.Set(y:playerHeight) + transform.forward * base.enemyStats.Range;
            bool playerHit = EnemyPhysicsCheck.CheckSpherePlayer(center, this.attackRadius);
            Debug.Log($"is player Hit? {playerHit}");

            var player = Player.Instance;
            if (playerHit)
            {
                player.PlayerStats.ProcessAttack(base.enemyStats, this.basicAttackSkillInfo);
                Debug.Log($"remainign player health: {player.PlayerStats.CurrentHealth}");
            }
        }
    }
}
