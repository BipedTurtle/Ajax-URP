using Entities.EnemySystem;
using System.Collections.Generic;

namespace Entities
{
    public class InteractionChart
    {
        #region Singleton
        private static readonly InteractionChart instance = new InteractionChart();

        public List<Enemy> EnemiesAlive { get; } = new List<Enemy>(30);
        public List<IHittable> HittablesActive { get; } = new List<IHittable>(30);
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static InteractionChart()
        {

        }


        // keep it from being instantiated outside
        private InteractionChart()
        {

        }

        public static InteractionChart Instance => instance;
        #endregion

        public void AddEnemy(Enemy enemy)
        {
            this.EnemiesAlive.Add(enemy);
            this.HittablesActive.Add(enemy);
        }


        public void RemoveEnemy(Enemy enemy)
        {
            this.EnemiesAlive.Remove(enemy);
            this.HittablesActive.Remove(enemy);
        }


        public void AddHittable(IHittable hittable)
            => this.HittablesActive.Add(hittable);


        public void RemoveHittable(IHittable hittable)
            => this.HittablesActive.Remove(hittable);
    }
}
