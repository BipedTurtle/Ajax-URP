using Managers;
using PlayerSystem;
using UnityEngine;

namespace Utility
{
    public class TrackingCamera : MonoBehaviour
    {
        private Transform player;
        private void Start()
        {
            this.player = PlayerController.Instance.transform;
            mainCamera = GetComponent<Camera>();

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TrackPlayer);
        }

        private static Camera mainCamera;
        public static Vector2 GetAnchorPos(Vector3 at, RectTransform canvasTransform)
        {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(at);
            RectTransform canvasSpace = canvasTransform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasSpace, screenPoint, null, out Vector2 anchorPos);

            return anchorPos;
        }


        private void TrackPlayer()
        {
            float cameraY = transform.localPosition.y;
            transform.localPosition = this.player.localPosition.Set(y: cameraY);
        }
    }
}