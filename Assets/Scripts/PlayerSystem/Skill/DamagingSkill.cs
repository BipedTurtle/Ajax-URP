using Entities.Stats;
using UnityEngine;

namespace PlayerSystem.Skills
{
    public class DamagingSkill : Skill
    {
        [SerializeField] protected SkillInfoArchetype attackInfoArchetype;
        protected SkillInfo skillInfo;
        protected virtual void OnEnable()
        {
            this.skillInfo = this.attackInfoArchetype.Copy();
        }
    }
}