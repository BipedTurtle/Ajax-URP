using System.Threading.Tasks;
using UnityEngine;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BackShot", menuName = "PlayerSystem/Skills/BackShot")]
    public class BackShot : Skill
    {
        public async override void Execute()
        {
            await this.SlideBackwards();
        }


        [SerializeField] private float movementAmount = 2f;
        [SerializeField] private float slideSpeed = 2f;
        private float distanceMoved;
        private async Task SlideBackwards()
        {
            var player = PlayerController.Instance;
            player.CancelActions();
            player.Agent.ResetPath();

            var moveDirection = -player.transform.forward;
            Vector3 destination = player.transform.localPosition + moveDirection * this.movementAmount;

            var movementThisFrame = this.movementAmount * Time.deltaTime * this.slideSpeed;
            while (distanceMoved < this.movementAmount) {
                this.distanceMoved += movementThisFrame;
                player.transform.localPosition += moveDirection * movementThisFrame;
                await Task.Yield();
            }

            this.distanceMoved = 0;
        }

    }
}
