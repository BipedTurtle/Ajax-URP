using UnityEngine;

namespace Entities.Stats
{
    [CreateAssetMenu(fileName = "AttackInfo", menuName = "Stats/AttackInfoArchetype")]
    public class AttackInfoArchetype : ScriptableObject
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


        public AttackInfo Copy()
            => new AttackInfo(
                damage: this.Damage,
                criticalChance: this.CriticalChance);
    }
}
