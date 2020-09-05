﻿using System.Text;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class DamageUI : MonoBehaviour
    {
        //private Color normalHitColor;
        //private Color CriticalHitColor;
        //private void Awake()
        //{
        //    normalHitColor = new Color(255, 255, 179, 255);
        //    CriticalHitColor = new Color(255, 179, 179, 255);
        //}


        [SerializeField] private TextMeshProUGUI tmp;
        private StringBuilder sb;

        private float VerticallyMoveTo => this.tmp.rectTransform.anchoredPosition.y + 30f;
        private readonly float fadeTime = .5f;
        public void DisplayDamage(Vector2 spawnPos, float damage, bool isCritical)
        {
            this.tmp.rectTransform.anchoredPosition = spawnPos;

            if (this.sb == null)
                this.sb = new StringBuilder();
            this.sb.Clear();

            int roundedDamage = (int)damage;
            this.sb.Append(roundedDamage);
            this.tmp.text = this.sb.ToString();
            this.tmp.color = isCritical ? Color.red : Color.yellow;

            gameObject.SetActive(true);
            LeanTween.value(gameObject, this.SetTextAlpha, from: 1f, to: 0, time: this.fadeTime);
            LeanTween.moveY(this.tmp.rectTransform, this.VerticallyMoveTo, this.fadeTime).setOnComplete(this.ReturnToInactiveQueue);
        }


        private void SetTextAlpha(float newAlpha)
        {
            var currentColor = this.tmp.color;
            currentColor.a = newAlpha;
            this.tmp.color = currentColor;
        }


        private void ReturnToInactiveQueue()
        {
            DamageUILoader.Instance.ReturnTMP(this);
        }
    }
}