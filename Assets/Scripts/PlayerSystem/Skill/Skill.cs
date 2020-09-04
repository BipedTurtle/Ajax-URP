using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem.Skills
{
    public abstract class Skill : ScriptableObject
    {
        protected float nextActivation;
        [Tooltip("This is Unnecessary for basic attack skills")] [SerializeField] protected float coolDown;
        public bool canActivate => Time.time > this.nextActivation;

        public AssetReferenceGameObject SkillIndicator;
        public IndicatorSpawnType indicatorSpawnPos;
        private GameObject indicatorGO;
        public void DisplayIndicator()
        {
            if (!SkillIndicator.RuntimeKeyIsValid())
                return;

            var spawnPos = IndicatorUtility.GetSpawnPos(this.indicatorSpawnPos);
            if (indicatorGO == null)
            {
                var operation = this.SkillIndicator.InstantiateAsync();
                operation.Completed += (op) => {
                    this.indicatorGO = op.Result;
                    this.indicatorGO.transform.localPosition = spawnPos;
                    this.indicatorGO.SetActive(true);
                };
            }
            else {
                this.indicatorGO.transform.localPosition = spawnPos;
                this.indicatorGO.SetActive(true);
            }
        }


        public void DisableIndicator()
        {
            if (indicatorGO == null)
                return;

            this.indicatorGO.SetActive(false);
        }


        public abstract void Execute();
    }
}
