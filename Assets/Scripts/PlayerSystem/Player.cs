using Entities.Stats;
using GameUI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        #region SkillsLibrary
        [SerializeField] private SkillsLibrary skillsLibrary;
        public SkillsLibrary SkillsLibrary => skillsLibrary;
        #endregion
        public PoolingData weaponsAndEffectsPoolingData;
        #region AssetReferenceGameObject SelfReference
        [SerializeField] private AssetReferenceGameObject _selfReference;
        public AssetReferenceGameObject SelfReference => _selfReference;
        #endregion
        #region AssetReference PlayerStats
        [SerializeField] private AssetReference playerStatsArchetype;
        public EntityStats PlayerStats { get; private set; }
        #endregion
        #region PlayerClass
        [SerializeField] private PlayerClass _playerClass;
        public PlayerClass PlayerClass => _playerClass;
        #endregion

        public static Player Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;


            var statsOpHandle = this.playerStatsArchetype.LoadAssetAsync<EntityStatsArchetype>();
            statsOpHandle.Completed += (op) =>
            {
                var archetype = op.Result;
                this.PlayerStats = archetype.Copy();
                this.UpdateStatusProgress();

                Addressables.Release(op);
            };
        }


        [SerializeField] private ProgressBar healthBar;
        [SerializeField] private ProgressBar manaBar;
        //[SerializeField] private ProgressBar expBar;
        public void UpdateStatusProgress()
        {
            healthBar.UpdateProgress(this.PlayerStats.CurrentHealth, this.PlayerStats.MaxHealth);
            manaBar.UpdateProgress(this.PlayerStats.CurrentMana, this.PlayerStats.MaxMana);
        }
    }
}
