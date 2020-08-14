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
            this.canvas.enabled = true;
            this.On = true;
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.FollowCursor);
            Cursor.visible = false;
        }


        public void TurnOff()
        {
            this.canvas.enabled = false;
            this.On = false;
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.FollowCursor);
            Cursor.visible = true;
        }


        private Vector2 indicatorPos;
        private void FollowCursor()
        {
            // no camera parameter is needed when using ScreenSpace - Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.canvas.transform as RectTransform,
                Input.mousePosition,
                null,
                out indicatorPos);

            this.indicator.rectTransform.anchoredPosition = this.indicatorPos;
        }
    }
}
