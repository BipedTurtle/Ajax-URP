using Entities.Stats;
using Managers;
using QuestSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

namespace Entities.EnemySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour, IHittable
    {
        protected NavMeshAgent agent;
        protected Animator animator;
        protected virtual void Awake()
        {
            this.agent = GetComponent<NavMeshAgent>();
            this.animator = GetComponent<Animator>();
        }


        protected virtual void OnEnable()
        {
            this.LoadStats();
            InteractionChart.Instance.AddEnemy(this);
        }


        protected virtual void OnDisable()
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
            QuestObjectBuilder.SetSubject(this.selfReference);
            QuestObjectBuilder.SetEventType(QuestEventType.Death);
            QuestObjectBuilder.SetObject(ReferenceCenter.Instance.emptyReference);
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

                this.agent.stoppingDistance = this.enemyStats.Range;
            };
        }
    }
}