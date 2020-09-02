using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class ProgressBar : MonoBehaviour
    {
        private StringBuilder sb;
        private void Start()
        {
            this.sb = new StringBuilder();
        }


        [SerializeField] private Image bar;
        [SerializeField] private TextMeshProUGUI tmp;
        public void UpdateProgress(float current, float max)
        {
            this.bar.fillAmount = current / max;

            this.sb.Clear();
            this.sb.AppendFormat("{0} / {1}", (int)current, (int)max);
            this.tmp.text = this.sb.ToString();
        }
    }
}
