using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public AssetReferenceGameObject SkillIndicator;
        private GameObject indicatorGO;

        public void DisplayIndicator()
        {
            if (!SkillIndicator.RuntimeKeyIsValid())
                return;

            if (indicatorGO == null)
            {
                var operation = this.SkillIndicator.InstantiateAsync();
                operation.Completed += (op) => {
                    this.indicatorGO = op.Result;
                    this.indicatorGO.SetActive(true);
                };
            }
            else
                this.indicatorGO.SetActive(true);
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
