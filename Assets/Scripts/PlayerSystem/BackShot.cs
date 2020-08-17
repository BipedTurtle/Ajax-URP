using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utility;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BackShot", menuName = "PlayerSystem/Skills/BackShot")]
    public class BackShot : Skill
    {
        public async override void Execute()
        {
            this.InstantiateArrow();
            await this.SlideBackwards();
        }


        [SerializeField] private float movementAmount = 2f;
        [SerializeField] private float slideSpeed = 2f;
        private float distanceMoved;
        private async ValueTask SlideBackwards()
        {
            var player = PlayerController.Instance;
            player.CancelActions();
            player.Agent.ResetPath();

            var moveDirection = -player.transform.forward;

            var movementThisFrame = this.movementAmount * Time.deltaTime * this.slideSpeed;
            while (distanceMoved < this.movementAmount) {
                this.distanceMoved += movementThisFrame;
                player.transform.localPosition += moveDirection * movementThisFrame;
                await Task.Yield();
            }

            this.distanceMoved = 0;
        }


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private async void InstantiateArrow()
        {
            var player = PlayerController.Instance;
            var playerPoolingData = PlayerController.Instance.playerPoolingData;

            var arrowPool = await Pool.GetPool(this.arrowReference, playerPoolingData);
            var offset = player.transform.forward * .7f;
            var spawnPos = player.transform.localPosition + offset;
            var arrow = arrowPool.GetPooledObjectAt(spawnPos, player.transform.localRotation);

            arrow.transform.SetPositionAndRotation(spawnPos, player.transform.localRotation);
        }

    }
}
