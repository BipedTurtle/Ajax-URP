using Entities.Stats;
using QuestSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entities.EnemySystem
{
    public class Enemy : MonoBehaviour, IHittable
    {
        private void OnEnable()
        {
            this.LoadStats();
            InteractionChart.Instance.AddEnemy(this);
        }


        private void OnDisable()
        {
            InteractionChart.Instance.RemoveEnemy(this);
        }


        [SerializeField] private float health;
        public Vector3 Position => transform.localPosition;

        public virtual void OnHit(EntityStats inflicterStats, SkillInfo skillInfo)
        {
            this.enemyStats.ProcessAttack(inflicterStats, skillInfo, transform.localPosition);

            bool isDead = this.enemyStats.Health <= 0;
            if (isDead)
                this.Die();
        }


        [SerializeField] private AssetReference selfReference;
        private void Die()
        {
            QuestObjectBuilder.Clear();
            QuestObjectBuilder.SetSubject(this.selfReference);
            QuestObjectBuilder.SetEventType(QuestEventType.Death);
            var questObject = QuestObjectBuilder.Build();
            QuestLibrary.UpdateQuestProgress(questObject);
            
            Debug.Log("die");
        }


        [SerializeField] private AssetReference enemyStatsArchetype;
        public EntityStats enemyStats;
        private void LoadStats()
        {
            var statsOpHandle = this.enemyStatsArchetype.LoadAssetAsync<EntityStatsArchetype>();
            statsOpHandle.Completed += (op) =>
            {
                var archetype = op.Result;
                this.enemyStats = archetype.Copy();
            };
        }
    }
}