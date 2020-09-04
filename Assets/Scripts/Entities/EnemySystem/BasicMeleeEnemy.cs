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


        private int rotationTweenID = -1;
        protected virtual void TurnTowardsTarget()
        {
            //if (Time.frameCount % 15 != 0)
            //    return;

            var toPlayerVector = (playerTransform.localPosition - transform.localPosition).Set(y: 0);
            var lookRotation = Quaternion.LookRotation(toPlayerVector);

            float angleBetween = Vector3.Angle(toPlayerVector, transform.forward);
            if (angleBetween < 10f) {
                LeanTween.cancel(gameObject);
                transform.rotation = lookRotation;
                return;
            }

            bool isAlreadyRotating = this.rotationTweenID == -1 ? false : LeanTween.isTweening(this.rotationTweenID);
            if (isAlreadyRotating)
                return;
            else
                this.rotationTweenID = LeanTween.rotate(gameObject, lookRotation.eulerAngles, .15f).uniqueId;
        }


        private float SqrDistanceToPlayer => (this.playerTransform.transform.localPosition - transform.localPosition).Set(y: 0).sqrMagnitude;
        private float nextAttackTime;
        protected virtual void ActAttack()
        {
            if (Time.frameCount % 15 != 0)
                return;

            float range = (base.EnemyStats == null) ? 1f : base.EnemyStats.Range;
            bool isOutOfRange = this.SqrDistanceToPlayer > Mathf.Pow(range, 2);
            bool coolHasNotReturned = Time.timeSinceLevelLoad < this.nextAttackTime;
            if (isOutOfRange | coolHasNotReturned)
                return;

            base.agent.ResetPath();
            base.animator.SetTrigger("Attack");
            this.Freeze();

            this.nextAttackTime = Time.timeSinceLevelLoad + base.EnemyStats.AttackSpeed;
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
            Vector3 center = transform.localPosition.Set(y:playerHeight) + transform.forward * base.EnemyStats.Range;
            bool playerHit = EnemyPhysicsCheck.CheckSpherePlayer(center, this.attackRadius);

            var player = Player.Instance;
            if (playerHit) {
                player.PlayerStats.ProcessAttack(base.EnemyStats, this.basicAttackSkillInfo);
                player.UpdateStatusProgress();
            }
        }
    }
}
