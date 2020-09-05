using GameUI;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem.Skills
{
    public abstract class Skill : ScriptableObject
    {
        protected float nextActivation;
        [Tooltip("This is Unnecessary for basic attack skills")] [SerializeField] protected float _coolDown;
        public float CoolDown => this._coolDown;
        public bool canActivate => Time.time > this.nextActivation;

        public AssetReference SkillIndicatorReference;
        public IndicatorSpawnType indicatorSpawnPos;
        private Indicator indicator;
        public void DisplayIndicator()
        {
            if (SkillIndicatorReference.RuntimeKey.Equals(ReferenceCenter.Instance.emptyReference.RuntimeKey))
                return;

            var spawnPos = IndicatorUtility.GetSpawnPos(this.indicatorSpawnPos);
            if (this.indicator == null)
            {
                var operation = this.SkillIndicatorReference.InstantiateAsync();
                operation.Completed += (op) => {
                    this.indicator = op.Result.GetComponent<Indicator>();
                    this.indicator.transform.localPosition = spawnPos;
                    this.indicator.TurnOn();
                };
            }
            else {
                this.indicator.transform.localPosition = spawnPos;
                this.indicator.TurnOn();
            }
        }


        public void DisableIndicator()
        {
            if (this.indicator == null)
                return;

            this.indicator.TurnOff();
        }


        public abstract void Execute();
    }
}
