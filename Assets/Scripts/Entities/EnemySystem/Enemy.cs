using Entities.Stats;
using Managers;
using QuestSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using GameUI;
using Utility.MyTweenLibrary;
using PlayerSystem;
using System;
using Utility;

namespace Entities.EnemySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public abstract class Enemy : MonoBehaviour, IHittable
    {
        protected NavMeshAgent agent;
        protected Animator animator;
        protected Transform playerTransform;
        protected virtual void Awake()
        {
            this.agent = GetComponent<NavMeshAgent>();
            this.animator = GetComponent<Animator>();
            this.playerTransform = Player.Instance.transform;

            this.DetectPlayer_Cache = this.DetectPlayer;
        }


        private Action DetectPlayer_Cache;
        protected virtual void OnEnable()
        {
            this.LoadStats();
            this.agent.enabled = true;
            InteractionChart.Instance.AddEnemy(this);

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.DetectPlayer_Cache);
        }


        [SerializeField] private float health;
        public Vector3 Position => transform.localPosition;

        public virtual void OnHit(EntityStats inflicterStats, SkillInfo skillInfo)
        {
            this.EnemyStats.ProcessAttack(inflicterStats, skillInfo, transform.localPosition);

            bool isDead = this.EnemyStats.CurrentHealth <= 0;
            if (isDead)
                this.Die();

            EnemyHealthbarLoader.Instance.LoadHealthBar(this);
        }


        [SerializeField] private AssetReference selfReference;
        [SerializeField] private float sinkToGroundAfterThisMuchTime = 2.5f;
        private void Die()
        {
            #region Quest-related
            QuestObjectBuilder.SetSubject(this.selfReference);
            QuestObjectBuilder.SetEventType(QuestEventType.Death);
            QuestObjectBuilder.SetObject(ReferenceCenter.Instance.emptyReference);
            var questObject = QuestObjectBuilder.Build();
            QuestLibrary.UpdateQuestProgress(questObject);
            #endregion

            InteractionChart.Instance.RemoveEnemy(this);
            this.Freeze();
            this.animator.SetTrigger("Die");
            this.agent.enabled = false;
            MyTween.Instance.Move(transform, transform.localPosition + Vector3.down * 2f, time: 1f, wait: this.sinkToGroundAfterThisMuchTime);

            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.DetectPlayer_Cache);
        }

        protected abstract void Freeze();
        protected abstract void UnFreeze();

        protected float SqrDistanceToPlayer => (this.playerTransform.transform.localPosition - transform.localPosition).Set(y: 0).sqrMagnitude;

        [SerializeField] private AssetReference enemyStatsArchetype;
        public EntityStats EnemyStats { get; private set; }
        private void LoadStats()
        {
            var statsOpHandle = this.enemyStatsArchetype.LoadAssetAsync<EntityStatsArchetype>();
            statsOpHandle.Completed += (op) =>
            {
                var archetype = op.Result;
                this.EnemyStats = archetype.Copy();

                this.agent.stoppingDistance = this.EnemyStats.Range;
            };
        }


        [SerializeField] private float playerDetectionRange = 15f;
        protected bool playerDetected;
        private void DetectPlayer()
        {
            if (Time.frameCount % 5 != 0)
                return;

            this.playerDetected = this.SqrDistanceToPlayer < Mathf.Pow(this.playerDetectionRange, 2);
        }

    }
}