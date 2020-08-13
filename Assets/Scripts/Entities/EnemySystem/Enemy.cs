using System.Collections.Generic;
using UnityEngine;

namespace Entities.EnemySystem
{
    public class Enemy : MonoBehaviour, IHittable
    {
        public static List<Enemy> EnemiesAlive { get; } = new List<Enemy>(20);
        private void OnEnable()
        {
            EnemiesAlive.Add(this);
        }


        private void OnDisable()
        {
            EnemiesAlive.Remove(this);
        }


        [SerializeField] private float health;
        public void OnHit()
        {
            Debug.Log("i'm hit");
        }
    }
}