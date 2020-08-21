using Entities.Stats;
using UnityEngine;

namespace PlayerSystem.Skills
{
    public class DamagingSkill : Skill
    {
        [SerializeField] protected AttackInfoArchetype attackInfoArchetype;
        protected AttackInfo attackInfo;
        protected virtual void OnEnable()
        {
            this.attackInfo = this.attackInfoArchetype.Copy();
        }
    }
}