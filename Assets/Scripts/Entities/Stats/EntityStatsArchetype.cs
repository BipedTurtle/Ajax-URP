using System;
using UnityEngine;

namespace Entities.Stats
{
    [CreateAssetMenu(fileName = "EntityStatsArchetype", menuName = "Stats/EntityStatsArchetype")]
    public class EntityStatsArchetype : ScriptableObject
    {
        [SerializeField] float _maxHealth;
        public float Maxhealth
        {
            get => _maxHealth;
            private set
            {
                _maxHealth = Mathf.Clamp(value, 0, value);
            }
        }

        [SerializeField] float _maxMana;
        public float MaxMana
        {
            get => _maxMana;
            set
            {
                _maxMana = Mathf.Clamp(value, 0, value);
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


        [Range(1f, 10f)] [SerializeField] private float _range;
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


        public EntityStats Copy()
            => new EntityStats(
                health: this.Maxhealth,
                mana: this.MaxMana,
                baseDamage: this.BaseDamage,
                extradmage: this.ExtraDamage,
                criticalChance: this.CriticalChance,
                range: this.Range,
                movementSpeed: this.MovementSpeed,
                armor: this.Armor,
                attackSpeed: this.AttackSpeed);
    }
}
