using Managers;
using UnityEngine;
using Utility;

namespace PlayerSystem
{
    public class PivotIndicator : MonoBehaviour
    {
        private void Awake()
        {
            this.mainCamera = Camera.main;
            this.zDepth = this.mainCamera.transform.localPosition.y;
        }


        private void OnEnable()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(PivotAround);
        }


        private void OnDisable()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(PivotAround);
        }


        private Camera mainCamera;
        private float zDepth;
        private void PivotAround()
        {
            Vector3 center = PlayerController.Instance.transform.localPosition;

            Vector3 mousePositionWorld = this.mainCamera.ScreenToWorldPoint(Input.mousePosition.Set(z: this.zDepth));
            var playerPos = PlayerController.Instance.transform.localPosition;
            var toCursorVector = (mousePositionWorld - playerPos).Set(y: 0);
            var lookRotation = Quaternion.LookRotation(toCursorVector);

            transform.SetPositionAndRotation(center, lookRotation);
        }
    }
}
