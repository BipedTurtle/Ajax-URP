using Entities.NPC_System;
using Managers;
using System;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerSpecificBehaviors : MonoBehaviour
    {
        public static PlayerSpecificBehaviors Instance { get; private set; }
        private NavMeshAgent agent;
        private void Start()
        {
            Instance = this;

            this.agent = GetComponent<NavMeshAgent>();
        }


        public static event Action positioningFinished;
        private readonly float talkingDistance = 5f;
        public void GoToDialoguePosition(NPC npc)
        {
            this.agent.ResetPath();
            PlayerController.Instance.DisableInputs();

            var npcTransform = npc.transform;
            Vector3 destination = npcTransform.localPosition + npcTransform.forward * this.talkingDistance;
            this.agent.SetDestination(destination);
            UpdateManager.Instance.SubscribeToGlobalUpdate(CheckReachedPosition);


            void CheckReachedPosition()
            {
                float threshold = .01f;
                bool destinationReached = this.agent.remainingDistance < threshold;

                if (destinationReached) {
                    var lookVector = (npcTransform.localPosition - transform.localPosition).Set(y: 0);
                    var lookRotation = Quaternion.LookRotation(lookVector).eulerAngles;
                    LeanTween.rotate(gameObject, lookRotation, .25f);

                    UpdateManager.Instance.UnSubscribeFromGlobalUpdate(CheckReachedPosition);
                    positioningFinished?.Invoke();
                    positioningFinished = null;
                }
            }

        }



        
    }
}
