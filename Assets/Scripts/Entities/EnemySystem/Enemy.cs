using System.Collections.Generic;
using UnityEngine;

namespace Entities.EnemySystem
{
    public class Enemy : MonoBehaviour, IHittable
    {
        private void OnEnable()
        {
            InteractionChart.Instance.AddEnemy(this);
        }


        private void OnDisable()
        {
            InteractionChart.Instance.RemoveEnemy(this);
        }


        [SerializeField] private float health;
        public Vector3 Position => transform.localPosition;

        public virtual void OnHit()
        {
            Debug.Log("i'm hit");
        }
    }
}