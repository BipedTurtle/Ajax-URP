using Entities.Stats;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entities.EnemySystem
{
    public class Enemy : MonoBehaviour, IHittable
    {
        public Vector3 Position => transform.localPosition;

        [SerializeField] private AssetReference enemyStatsArchetype;
        private EntityStats enemyStats;
        private void Awake()
        {
            var statsOperationHandle = this.enemyStatsArchetype.LoadAssetAsync<EntityStatsArchetype>();
            statsOperationHandle.Completed += (op) =>
            {
                var archetype = op.Result;
                this.enemyStats = archetype.Copy();
                Addressables.Release(op);
            };
        }


        private void OnEnable()
        {
            InteractionChart.Instance.AddEnemy(this);
        }


        private void OnDisable()
        {
            InteractionChart.Instance.RemoveEnemy(this);
        }


        private void TakeDamage(AttackInfo attackInfo)
        {
            this.enemyStats.ProcessAttack(attackInfo);

            bool isDead = this.enemyStats.Health <= 0;
            if (isDead)
                this.Die();
        }


        protected void Die()
        {
            Debug.Log($"{name} is dead");
        }


        public virtual void OnHit(AttackInfo attackInfo)
        {
            this.TakeDamage(attackInfo ?? throw new NullReferenceException("The attack/skill/projectile has no AttackInformation attached, Make sure you attach the AttackInfoArchetype"));
        }
    }
}