using UnityEngine;

namespace Entities.Stats
{
    [CreateAssetMenu(fileName = "EntityStatsArchetype", menuName = "Stats/EntityStatsArchetype")]
    public class EntityStatsArchetype : ScriptableObject
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

        [SerializeField] private float _criticalChance;
        public float CriticalChance
        {
            get => _criticalChance;
            set
            {
                _criticalChance = Mathf.Clamp(value, 0, 1f);
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


        public EntityStats Copy()
            => new EntityStats(
                health: this.Health,
                mana: this.Mana,
                criticalChance: this.CriticalChance,
                movementSpeed: this.MovementSpeed,
                armor: this.Armor);
    }
}
