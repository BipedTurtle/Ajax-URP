using UnityEngine;

namespace Entities.Stats
{
    public class SkillInfo
    {
        [Range(1f, 3f)]
        [SerializeField] private float _baseDamageModifier;
        public float BaseDamageModifier
        {
            get => _baseDamageModifier;
            set
            {
                _baseDamageModifier = Mathf.Clamp(value, 0, 3f);
            }
        }


        private float _extraDamageModifier;
        public float ExtraDaamgeModifier
        {
            get => _extraDamageModifier;
            set
            {
                _extraDamageModifier = Mathf.Clamp(value, 0, 3f);
            }
        }


        [SerializeField] private float _criticalChance;
        public float CriticalChance
        {
            get => _criticalChance;
            set 
            {
                _criticalChance = Mathf.Clamp(value, 0, 1f);
            }
        }


        public SkillInfo(float damageModifier, float criticalChance)
        {
            this.BaseDamageModifier = damageModifier;
            this.CriticalChance = criticalChance;
        }
    }
}