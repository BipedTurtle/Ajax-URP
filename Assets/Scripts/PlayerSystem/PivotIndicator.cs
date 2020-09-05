using GameUI;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem
{
    public class PivotIndicator : Indicator
    {
        private GameObject image;
        private Renderer renderer;
        [SerializeField] private Material transparentMat;
        [SerializeField] private Material indicatorMat;
        private void Awake()
        {
            this.image = transform.GetChild(0).gameObject;
            this.renderer = this.image.GetComponent<Renderer>();
        }
        public override void TurnOn()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(PivotAround);

            this.dontTurnIndicatorOn = false;
        }

        public override void TurnOff()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(PivotAround);

            this.dontTurnIndicatorOn = true;
            this.renderer.sharedMaterial = this.transparentMat;
        }


        private bool dontTurnIndicatorOn;
        public void PivotAround()
        {
            Vector3 center = PlayerController.Instance.transform.localPosition;

            var toCursorVector = ControlUtility.GetPlayerToCursorVector();
            var lookRotation = Quaternion.LookRotation(toCursorVector);

            transform.SetPositionAndRotation(center, lookRotation);

            if (this.dontTurnIndicatorOn | this.renderer.sharedMaterial.Equals(this.indicatorMat))
                return;

            //this.image.SetActive(true);
            this.renderer.sharedMaterial = this.indicatorMat;
        }
    }
}
