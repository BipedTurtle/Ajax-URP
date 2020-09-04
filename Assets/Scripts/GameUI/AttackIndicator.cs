using Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class AttackIndicator : MonoBehaviour
    {
        private Canvas canvas;
        private Image indicator;
        public bool On { get; private set; }
        [SerializeField] private Sprite indicatorSprite;
        [SerializeField] private Sprite transparency;
        private void Start()
        {
            this.canvas = GetComponent<Canvas>();
            this.indicator = GetComponentInChildren<Image>();

            this.FollowCursor_Cache = this.FollowCursor;
        }


        public void TurnOn()
        {
            this.indicatorShouldBeOff = false;  
            this.On = true;
            //this.indicator.sprite = this.indicatorSprite;
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.FollowCursor_Cache);
            Cursor.visible = false;
        }


        public void TurnOff()
        {
            this.indicator.sprite = this.transparency;
            this.indicatorShouldBeOff = true;
            this.On = false;
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.FollowCursor_Cache);
            Cursor.visible = true;
        }


        private Vector2 indicatorPos;
        private bool indicatorShouldBeOff;
        private Action FollowCursor_Cache;
        private void FollowCursor()
        {
            // no camera parameter is needed when using ScreenSpace - Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.canvas.transform as RectTransform,
                Input.mousePosition,
                null,
                out indicatorPos);
            this.indicator.rectTransform.anchoredPosition = this.indicatorPos;

            if (this.indicatorShouldBeOff | this.indicator.sprite.Equals(this.indicatorSprite))
                return;
            this.indicator.sprite = this.indicatorSprite;
        }
    }
}
