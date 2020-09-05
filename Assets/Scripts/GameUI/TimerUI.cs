using System.Text;
using TMPro;
using UnityEngine;
using Utility;

namespace GameUI
{
    public class TimerUI : MonoBehaviour
    {
        private TextMeshProUGUI tmp;

        private void Start()
        {
            this.tmp = GetComponentInChildren<TextMeshProUGUI>();
        }


        private StringBuilder sb;
        public void UpdateUI(int sec)
        {
            this.tmp.text = NumberToStringUtility.GetTimeBySecond(sec);
        }
    }
}
