using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public class ReferenceCenter : MonoBehaviour
    {
        #region Singleton
        public static ReferenceCenter Instance { get; private set; }
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

        public AssetReference emptyReference;
    }
}
