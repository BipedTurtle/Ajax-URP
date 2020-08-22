using UnityEngine;

namespace Entities.Stats
{
    [CreateAssetMenu(fileName = "SkillInfo", menuName = "Stats/SkillInfoArchetype")]
    public class SkillInfoArchetype : ScriptableObject
    {
        [Range(1f, 3f)]
        [SerializeField] private float _baseDamageModifier;
        public float BaseDamageModifier
        {
            get => _baseDamageModifier;
            set
            {
                _baseDamageModifier = Mathf.Clamp(value, 1f, 3f);
            }
        }


        [Range(1f, 3f)]
        [SerializeField] private float _extraDamageModifier;
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


        public SkillInfo Copy()
            => new SkillInfo(
                damageModifier: this.BaseDamageModifier,
                criticalChance: this.CriticalChance);
    }
}
