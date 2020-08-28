using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using Utility;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        public UnityEvent executeAfter;
        public AssetReference timer;
        private void Start()
        {
            var timerOpHandle = this.timer.LoadAssetAsync<LevelTimer>();
            timerOpHandle.Completed += (op) =>
            {
                var timer = op.Result;

                timer.StartTimer();
            };
        }
    }
}
