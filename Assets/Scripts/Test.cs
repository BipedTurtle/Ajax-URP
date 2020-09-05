using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using Utility;
using TMPro;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        public UnityEvent anEvent;
        public AssetReference timer;
        public TextMeshProUGUI tmp;
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
