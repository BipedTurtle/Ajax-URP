using UnityEngine;

namespace Entities.Stats
{
    public class AttackInfo
    {
        [Range(1f, 1000f)]
        [SerializeField] private float _damage;
        public float Damage
        {
            get => _damage;
            set
            {
                _damage = Mathf.Clamp(value, 1f, value);
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


        public AttackInfo(float damage, float criticalChance)
        {
            this.Damage = damage;
            this.CriticalChance = CriticalChance;
        }
    }
}
