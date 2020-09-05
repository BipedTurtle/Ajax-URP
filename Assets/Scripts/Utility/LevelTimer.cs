using GameUI;
using Managers;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility
{
    [CreateAssetMenu(fileName = "LevelTimer", menuName = "LevelTimer")]
    public class LevelTimer : ScriptableObject
    {
        private Stopwatch timer;

        public void StartTimer()
        {
            this.timeLimit = new TimeSpan(hours: 0, minutes: 15, seconds: 0);

            var timerUI_OpHandle = Addressables.InstantiateAsync("UI/TimerUI.prefab");
            timerUI_OpHandle.Completed += (op) =>
            {
                this.timer = new Stopwatch();
                this.timer.Start();

                this.timerUI = op.Result.GetComponent<TimerUI>();
                UpdateManager.Instance.SubscribeToGlobalUpdate(this.UpdateTimerUI);
            };
        }


        public void PauseTimer()
        {
            this.timer?.Stop();
        }


        public void ResumeTimer()
        {
            this.timer?.Start();
        }


        private TimerUI timerUI;
        private TimeSpan timeLimit;
        private void UpdateTimerUI()
        {
            if (Time.frameCount % 30 != 0)
                return;

            var remainingTime = this.timeLimit.Subtract(this.timer.Elapsed);
            var totalSeconds = (int)remainingTime.TotalSeconds;

            this.timerUI.UpdateUI(totalSeconds);

            bool timeOver = totalSeconds == 0;
            if (timeOver)
                UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.UpdateTimerUI);
        }
    }
}
