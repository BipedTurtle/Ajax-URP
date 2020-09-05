using System;
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
        #region Delegate Caches
        private Action ActAttack_Cache;
        private Action MoveTowardsTarget_Cahce;
        private Action TurnTowardsTarget_Cache;
        #endregion
        protected override void Awake()
        {
            base.Awake();

            this.playerTransform = PlayerController.Instance.transform;
            this.basicAttackSkillInfo = this.basicAttackArchetype.Copy();

            this.ActAttack_Cache = this.ActAttack;
            this.MoveTowardsTarget_Cahce = this.MoveTowardsTarget;
            this.TurnTowardsTarget_Cache = this.TurnTowardsTarget;
        }


        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.ActAttack_Cache);
            this.UnFreeze();
        }


        protected virtual void MoveTowardsTarget()
        {
            if (Time.frameCount % 10 != 0)
                return;

            base.agent.SetDestination(this.playerTransform.localPosition);
        }


        private MyTweenState rotationTweeningState;
        protected virtual void TurnTowardsTarget()
        {
            if (this.rotationTweeningState != null && this.rotationTweeningState.IsTweening)
                return;

            var toPlayerVector = (playerTransform.localPosition - transform.localPosition).Set(y: 0);
            float angleBetween = Vector3.Angle(toPlayerVector, transform.forward);
            if (angleBetween < 15f) {
                var lookRotation = Quaternion.LookRotation(toPlayerVector);
                transform.rotation = lookRotation;
                return;
            }

            bool isAlreadyRotating = (this.rotationTweeningState == null) ? false : this.rotationTweeningState.IsTweening;
            if (isAlreadyRotating)
                return;
            else
                this.rotationTweeningState = MyTween.Instance.Rotate(transform, toPlayerVector, .15f);
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
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.MoveTowardsTarget_Cahce);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.TurnTowardsTarget_Cache);
        }


        /// <summary>
        /// below methods get called by animation clips
        /// </summary>
        private void UnFreeze()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.MoveTowardsTarget_Cahce);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TurnTowardsTarget_Cache);
        }


        [SerializeField] private float attackRadius = .5f;
        private void DealDamage()
        {
            float playerHeight = this.playerTransform.localPosition.y;
            Vector3 center = transform.localPosition.Set(y: playerHeight) + transform.forward * base.EnemyStats.Range;
            bool playerHit = EnemyPhysicsCheck.CheckSpherePlayer(center, this.attackRadius);

            var player = Player.Instance;
            if (playerHit)
            {
                player.PlayerStats.ProcessAttack(base.EnemyStats, this.basicAttackSkillInfo);
                player.UpdateStatusProgress();
            }
        }
    }
}
