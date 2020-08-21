using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class DamageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;
        private StringBuilder sb;
        private void Awake()
        {
            this.sb = new StringBuilder();
        }

        private void OnEnable()
        {
            this.DisplayDamage(200f);
        }


        private float VerticallyMoveTo => this.tmp.rectTransform.anchoredPosition.y + 30f;
        private readonly float fadeTime = 1f;
        public void DisplayDamage(float damage)
        {
            this.sb.Clear();

            int roundedDamage = (int)damage;
            this.sb.Append(roundedDamage);
            this.tmp.text = this.sb.ToString();

            LeanTween.value(gameObject, this.SetTextAlpha, from: 1f, to: 0, time: this.fadeTime);
            LeanTween.moveY(this.tmp.rectTransform, this.VerticallyMoveTo, this.fadeTime);
        }


        private void SetTextAlpha(float newAlpha)
        {
            var currentColor = this.tmp.color;
            currentColor.a = newAlpha;
            this.tmp.color = currentColor;
        }
    }
}