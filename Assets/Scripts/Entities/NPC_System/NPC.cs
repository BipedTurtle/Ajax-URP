using PlayerSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entities.NPC_System
{
    public class NPC : MonoBehaviour
    {
        public static List<NPC> NPCsInTheScene { get; } = new List<NPC>(10);

        [SerializeField] private AssetReference dialogueReference;
        private Dialogue dialogue;
        [SerializeField] private AssetReferenceGameObject _selfReference;
        public AssetReferenceGameObject SelfReference => _selfReference;
        [SerializeField] private string _npcName = "TestNPC";
        public string NPC_Name => _npcName;
        private void Awake()
        {
            NPCsInTheScene.Add(this);
        }


        private async void Start()
        {
            var dialogueOpHandle = this.dialogueReference.LoadAssetAsync<Dialogue>();
            this.dialogue = await dialogueOpHandle.Task;
        }


        private readonly float interactionDistanceThreshold = 4f;
        public bool Interact()
        {
            float sqrDistance = (PlayerController.Instance.transform.localPosition - transform.localPosition).sqrMagnitude;
            bool withinInteractionRange = sqrDistance < Mathf.Pow(this.interactionDistanceThreshold, 2);

            if (withinInteractionRange){
                PlayerSpecificBehaviors.Instance.GoToDialoguePosition(this);
                PlayerSpecificBehaviors.positioningFinished += dialogue.StartDialogue;
            }

            return withinInteractionRange;
        }
    }
}
