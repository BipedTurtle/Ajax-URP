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

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.TrackPlayer);
        }


        private void TrackPlayer()
        {
            float cameraY = transform.localPosition.y;
            transform.localPosition = this.player.localPosition.Set(y: cameraY);
        }
    }
}