using Managers;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Entities.EnemySystem
{
    public class BasicMeleeEnemy : Enemy
    {
        private Transform player;
        protected override void Awake()
        {
            base.Awake();

            this.player = PlayerController.Instance.transform;
        }


        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.Attack);
            this.UnFreeze();
        }


        protected virtual void MoveTowardsTarget()
        {
            if (Time.frameCount % 15 != 0)
                return;

            base.agent.SetDestination(this.player.localPosition);

        }


        protected virtual void TurnTowardsTarget()
        {
            if (Time.frameCount % 15 != 0)
                return;

            var toPlayerVector = (player.localPosition - transform.localPosition).Set(y: 0);
            var lookRotation = Quaternion.LookRotation(toPlayerVector).eulerAngles;

            LeanTween.cancel(gameObject);
            LeanTween.rotate(gameObject, lookRotation, .15f);
        }


        private float SqrDistanceToPlayer => (this.player.transform.localPosition - transform.localPosition).Set(y: 0).sqrMagnitude;
        private float nextAttackTime;
        protected virtual void Attack()
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
        /// this method gets called by animation clips
        /// </summary>
        private void UnFreeze()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.MoveTowardsTarget);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TurnTowardsTarget);
        }
    }
}
