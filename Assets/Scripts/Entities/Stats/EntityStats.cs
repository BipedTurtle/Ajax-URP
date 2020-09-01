using GameUI;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Stats
{
    public class EntityStats
    {
        [SerializeField] float _health;
        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Clamp(value, 0, value);
            }
        }

        [SerializeField] float _mana;
        public float Mana
        {
            get => _mana;
            set
            {
                _mana = Mathf.Clamp(value, 0, value);
            }
        }

        [SerializeField] private float _baseDamage;
        public float BaseDamage
        {
            get => _baseDamage;
            set
            {
                _baseDamage = Mathf.Clamp(value, 0, value);
            }
        }

        public float _extraDaamge;
        public float ExtraDamage
        {
            get => _extraDaamge;
            set
            {
                _extraDaamge = Mathf.Clamp(value, 0, value);
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

        [Range(1f, 10f)][SerializeField] private float _range;
        public float Range
        {
            get => _range;
            set
            {
                _range = Mathf.Clamp(value, 1f, 10f);
            }
        }

        [SerializeField] private float _movementSpeed;
        public float MovementSpeed
        {
            get => _movementSpeed;
            set
            {
                _movementSpeed = Mathf.Clamp(value, 0, value);
            }
        }

        [SerializeField] private float _armor;
        public float Armor
        {
            get => _armor;
            set
            {
                _armor = Mathf.Clamp(value, 0, 100f);
            }
        }


        [SerializeField] private float _attackSpeed;
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = Mathf.Clamp(value, .1f, 5f);
            }
        }


        public EntityStats(float health, float mana, float baseDamage, float extradmage, float criticalChance, float range, float movementSpeed, float armor, float attackSpeed)
        {
            this.Health = health;
            this.Mana = mana;
            this.BaseDamage = baseDamage;
            this.ExtraDamage = extradmage;
            this.CriticalChance = criticalChance;
            this.Range = range;
            this.MovementSpeed = movementSpeed;
            this.Armor = armor;
            this.AttackSpeed = attackSpeed;
        }


        private static Func<float, float> ArmorFormula = (armor) => (0.1f) * Mathf.Sqrt(armor);
        public void ProcessAttack(EntityStats inflicter, SkillInfo skill, Vector3 hitPosition)
        {
            float dmgBlockedPercentage = ArmorFormula(this.Armor);
            float totalDamage = inflicter.BaseDamage * skill.BaseDamageModifier + inflicter.ExtraDamage * skill.ExtraDaamgeModifier;
            float damageTaken = totalDamage * (1f - dmgBlockedPercentage);

            float randomFloat = Random.Range(0, 1f);
            //Debug.Log($"random: {randomFloat}, critical Chance: {info.CriticalChance}");
            float criticalThreshold = (skill.CriticalChance == 0) ? -1f : inflicter.CriticalChance + skill.CriticalChance;
            bool isCriticalDamage = randomFloat <= criticalThreshold;
            damageTaken *= (isCriticalDamage) ? 2f : 1f;

            DamageUILoader.Instance.LoadDamageUI(damageTaken, hitPosition);
            
            this.Health -= damageTaken;
            //Debug.Log($"DamageTaken: {damageTaken}\nHealth Remaining: {this.Health}");
        }
    }
}
