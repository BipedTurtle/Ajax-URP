using System;
using Entities.Stats;
using Managers;
using PlayerSystem;
using UnityEngine;
using Utility;
using Utility.MyTweenLibrary;

namespace Entities.EnemySystem
{
    public class BasicMeleeEnemy : Enemy
    {
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


        [SerializeField] private float detectionRange = 15f;
        protected virtual void MoveTowardsTarget()
        {
            if (Time.frameCount % 10 != 0)
                return;

            base.animator.SetBool("Walk", base.playerDetected);
            if (base.playerDetected)
                base.agent.SetDestination(this.playerTransform.localPosition);
            else
                base.agent.ResetPath();
        }


        private MyTweenState rotationTweeningState;
        protected virtual void TurnTowardsTarget()
        {
            if (!base.playerDetected)
                return;

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

            if (base.agent.enabled)
                base.agent.ResetPath();
            this.StopMoving();
            base.animator.SetTrigger("Attack");

            this.nextAttackTime = Time.timeSinceLevelLoad + base.EnemyStats.AttackSpeed;
        }

        private void StopMoving()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.MoveTowardsTarget_Cahce);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.TurnTowardsTarget_Cache);
        }



        protected override void Freeze()
        {
            base.agent.ResetPath();

            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.ActAttack_Cache);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.MoveTowardsTarget_Cahce);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.TurnTowardsTarget_Cache);
        }


        /// <summary>
        /// below methods get called by animation clips
        /// </summary>
        protected override void UnFreeze()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.ActAttack_Cache);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.MoveTowardsTarget_Cahce);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TurnTowardsTarget_Cache);
        }

        private void ResumeMoving()
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
