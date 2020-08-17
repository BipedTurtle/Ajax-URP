using Managers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BackShot", menuName = "PlayerSystem/Skills/BackShot")]
    public class BackShot : Skill
    {
        public override void Execute()
        {
            this.InstantiateArrow();
            UpdateManager.Instance.SubscribeToGlobalFixedUpdate(this.SlideBackwards);
        }


        [SerializeField] private float movementAmount = 2f;
        [SerializeField] private float slideSpeed = 2f;
        private float distanceMoved;
        private void SlideBackwards()
        {
            var player = PlayerController.Instance;
            player.CancelActions();
            player.Agent.ResetPath();

            var moveDirection = -player.transform.forward;

            var movementThisFrame = this.movementAmount * Time.deltaTime * this.slideSpeed;
            if (distanceMoved < this.movementAmount) {
                this.distanceMoved += movementThisFrame;
                player.transform.localPosition += moveDirection * movementThisFrame;
            }
            else {
                UpdateManager.Instance.UnsubscribeFromGlobalFixedUpdate(this.SlideBackwards);
                this.distanceMoved = 0;
            }
        }


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private async void InstantiateArrow()
        {
            var player = PlayerController.Instance;
            var playerPoolingData = PlayerController.Instance.weaponsAndEffectsPoolingData;

            var arrowPool = await Pool.GetPool(this.arrowReference, playerPoolingData);
            var offset = player.transform.forward * .7f;
            var spawnPos = player.transform.localPosition + offset;
            var arrow = arrowPool.GetPooledObjectAt(spawnPos, player.transform.localRotation);

            arrow.transform.SetPositionAndRotation(spawnPos, player.transform.localRotation);
        }

    }
}
