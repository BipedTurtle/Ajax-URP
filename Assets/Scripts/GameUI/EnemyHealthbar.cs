using Entities.EnemySystem;
using Managers;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace GameUI
{
    public class EnemyHealthbar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image background;
        private Action SetExpiration_Cache;
        private Action UpdateStatus_Cache;
        private void Start()
        {
            this.Canvas = GetComponent<Canvas>();

            this.SetExpiration_Cache = this.SetExpiration;
            this.UpdateStatus_Cache = this.UpdateStatus;
        }


        private Enemy enemyTracked;
        public void Init(Enemy enemy)
        {
            this.enemyTracked = enemy;

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.UpdateStatus_Cache);

            this.expirationTime = Time.timeSinceLevelLoad + this.expireAfter;
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.SetExpiration_Cache);
        }


        public Canvas Canvas { get; private set; }
        private readonly Vector2 offset = new Vector2(0, 20);
        public void UpdateStatus()
        {
            var enemyStats = this.enemyTracked.EnemyStats;
            bool enemyIsDead = enemyStats.CurrentHealth <= 0;
            if (enemyIsDead) {
                EnemyHealthbarLoader.Instance.ReturnHealthbar(this.enemyTracked);
                this.Cease();
                return;
            }

            float fill = Mathf.Clamp01(enemyStats.CurrentHealth / enemyStats.MaxHealth);
            this.fillImage.fillAmount = fill;

            Vector2 anchoredPos = TrackingCamera.GetAnchorPos(this.enemyTracked.transform.localPosition, this.Canvas.transform as RectTransform);
            var displayPos = anchoredPos + this.offset;

            this.fillImage.rectTransform.anchoredPosition = displayPos;
            this.background.rectTransform.anchoredPosition = displayPos;

            if (!this.Canvas.enabled)
                this.Canvas.enabled = true;
        }


        private readonly float lifeExtension = 1.5f;
        public void ExtendLife()
        {
            this.expirationTime += this.lifeExtension;
        }


        private readonly float expireAfter = 3f;
        private float expirationTime;
        private void SetExpiration()
        {
            if (Time.timeSinceLevelLoad > this.expirationTime) {
                EnemyHealthbarLoader.Instance.ReturnHealthbar(this.enemyTracked);
                this.Cease();
            }
        }


        private void Cease()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.UpdateStatus_Cache);
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.SetExpiration_Cache);
        }

    }
}