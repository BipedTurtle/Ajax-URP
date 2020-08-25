using Entities.NPC_System;
using Managers;
using PlayerSystem;
using UnityEngine;

namespace Assets.Scripts.PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class AuxilaryKeysChecker : MonoBehaviour
    {
        private void Awake()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.CheckNPCInteraction);
        }


        private void CheckNPCInteraction()
        {
            if (!Input.GetKeyDown(KeyCode.T))
                return;

            Debug.Log("check npc interaction");
            foreach (var npc in NPC.NPCsInTheScene) {
                bool interactionSucceeded = npc.Interact();
                if (interactionSucceeded)
                    return;
            }
        }

    }
}