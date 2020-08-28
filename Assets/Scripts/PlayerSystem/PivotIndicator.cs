using Managers;
using UnityEngine;

namespace PlayerSystem
{
    public class PivotIndicator : MonoBehaviour
    {
        private GameObject image;
        private void Awake()
        {
            this.mainCamera = Camera.main;
            this.zDepth = this.mainCamera.transform.localPosition.y;

            this.image = transform.GetChild(0).gameObject;
        }


        private void OnEnable()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(PivotAround);

            this.dontTurnIndicatorOn = false;
        }


        private void OnDisable()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(PivotAround);

            this.dontTurnIndicatorOn = true;
            this.image.SetActive(false);
        }


        private Camera mainCamera;
        private float zDepth;
        private bool dontTurnIndicatorOn;
        public void PivotAround()
        {
            Vector3 center = PlayerController.Instance.transform.localPosition;

            var toCursorVector = ControlUtility.GetPlayerToCursorVector();
            var lookRotation = Quaternion.LookRotation(toCursorVector);

            transform.SetPositionAndRotation(center, lookRotation);

            if (this.dontTurnIndicatorOn | this.image.activeSelf)
                return;

            this.image.SetActive(true);
        }
    }
}
