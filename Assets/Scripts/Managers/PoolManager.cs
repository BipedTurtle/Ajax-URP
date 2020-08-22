using UnityEngine;
using Utility;

namespace Managers
{
    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        public static PoolManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        public PoolingData atBeginningPoolingData;
        private async void Start()
        {
            Pool.ClearPool();

            if (atBeginningPoolingData == null)
                return;

            var pooledList = this.atBeginningPoolingData.toBePooledAtStart;
            for (int i = 0; i < pooledList.Count; i++) {
                var pooled = pooledList[i];
                await Pool.CreatePool(pooled, atBeginningPoolingData);
            }
        }
    }
}
