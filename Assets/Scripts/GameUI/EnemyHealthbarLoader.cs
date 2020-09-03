using Entities.EnemySystem;
using Entities.Stats;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GameUI
{
    public class EnemyHealthbarLoader : MonoBehaviour
    {
        public static EnemyHealthbarLoader Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        Queue<EnemyHealthbar> healthbarQueue = new Queue<EnemyHealthbar>(20);
        private void Start()
        {
            foreach (Transform healthbar in transform)
                this.healthbarQueue.Enqueue(healthbar.GetComponent<EnemyHealthbar>());
        }


        private Dictionary<Enemy, EnemyHealthbar> enemyToHealthbarDictionary = new Dictionary<Enemy, EnemyHealthbar>();
        public void LoadHealthBar(Enemy enemyHit)
        {
            bool enemyAlreadyHasHealthbar = this.enemyToHealthbarDictionary.ContainsKey(enemyHit);
            if (enemyAlreadyHasHealthbar) {
                this.enemyToHealthbarDictionary[enemyHit].ExtendLife();
                return;
            }

            var healthbar = this.healthbarQueue.Dequeue();
            healthbar.Init(enemyHit);
            this.enemyToHealthbarDictionary[enemyHit] = healthbar;
        }


        public void ReturnHealthbar(Enemy enemytracked)
        {
            var healthbar = this.enemyToHealthbarDictionary[enemytracked];
            this.healthbarQueue.Enqueue(healthbar);
            this.enemyToHealthbarDictionary.Remove(enemytracked);

            healthbar.Canvas.enabled = false;
        }
    }
}
