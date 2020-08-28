using System.Text;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class TimerUI : MonoBehaviour
    {
        private TextMeshProUGUI tmp;
        private readonly string timerLabel = "Time Until Doom";

        private void Start()
        {
            this.tmp = GetComponentInChildren<TextMeshProUGUI>();
            this.sb = new StringBuilder();
        }


        private StringBuilder sb;
        public void UpdateUI(int min, int sec)
        {
            this.sb.Clear();
            this.sb.AppendFormat("{0}\r\n{1}:{2}", this.timerLabel, min < 10 ? $"0{min}" : min.ToString(), sec < 10 ? $"0{sec}" : sec.ToString());
            this.tmp.text = this.sb.ToString();
        }
    }
}
