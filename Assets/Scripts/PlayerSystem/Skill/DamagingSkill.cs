using Entities.Stats;
using UnityEngine;

namespace PlayerSystem.Skills
{
    public class DamagingSkill : Skill
    {
        [SerializeField] protected SkillInfoArchetype attackInfoArchetype;
        protected SkillInfo skillInfo;

        public override void Execute() { }

        protected virtual void OnEnable()
        {
            this.skillInfo = this.attackInfoArchetype.Copy();
            this.nextActivation = 0;
        }
    }
}