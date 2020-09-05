using System;
using System.Collections;
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

        private void Update()
        {
            var randInt = UnityEngine.Random.Range(0, 60 * 35);
            this.tmp.text = NumberToStringUtility.GetTimeBySecond(randInt);
            
        }
    }
}
