using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class AttackIndicator : MonoBehaviour
    {
        private Canvas canvas;
        private Image indicator;
        public bool On { get; private set; }
        private void Start()
        {
            this.canvas = GetComponent<Canvas>();
            this.indicator = GetComponentInChildren<Image>();
        }


        public void TurnOn()
        {
            this.indicatorShouldBeOff = false;  
            this.On = true;
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.FollowCursor);
            Cursor.visible = false;
        }


        public void TurnOff()
        {
            this.canvas.enabled = false;
            this.indicatorShouldBeOff = true;
            this.On = false;
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.FollowCursor);
            Cursor.visible = true;
        }


        private Vector2 indicatorPos;
        private bool indicatorShouldBeOff;
        private void FollowCursor()
        {
            // no camera parameter is needed when using ScreenSpace - Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.canvas.transform as RectTransform,
                Input.mousePosition,
                null,
                out indicatorPos);
            this.indicator.rectTransform.anchoredPosition = this.indicatorPos;

            if (this.indicatorShouldBeOff | this.canvas.enabled)
                return;
            this.canvas.enabled = true;
        }
    }
}
