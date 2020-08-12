using UnityEngine;

namespace Entities.EnemySystem
{
    public class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] private float health;

        public void OnHit()
        {
            Debug.Log("i'm hit");
        }
    }
}