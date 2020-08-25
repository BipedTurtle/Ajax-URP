using System.Linq;
using Entities.NPC_System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PlayerSystem;

namespace GameUI
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;


        private Dictionary<string, int>.Enumerator dialogueEnumerator;
        private List<Transform> talkerTransforms = new List<Transform>();
        public void Init(Dictionary<string, int> textToSpeakerDictionary, IEnumerable<AssetReferenceGameObject> talkersReferences)
        {
            this.mainCamera = Camera.main;
            this.canvas = GetComponent<Canvas>();

            this.dialogueEnumerator = textToSpeakerDictionary.GetEnumerator();

            var NPC_References = NPC.NPCsInTheScene.Select(npc => npc.SelfReference.RuntimeKey).ToList();
            var player = PlayerController.Instance;
            foreach (var talkerRef in talkersReferences) {
                if (talkerRef.RuntimeKey.Equals(player.SelfReference.RuntimeKey)) {
                    this.talkerTransforms.Add(player.transform);
                    continue;
                }

                var npc = NPC.NPCsInTheScene.FirstOrDefault(n => n.SelfReference.RuntimeKey.Equals(talkerRef.RuntimeKey));
                if (npc != null)
                    this.talkerTransforms.Add(npc.transform);
            }

            Debug.Log($"talkers Count: {this.talkerTransforms.Count}");
            this.talkerTransforms.ForEach(t => Debug.Log(t.name));
        }


        private Canvas canvas;
        public void DisplayDialogue()
        {
            if (!this.dialogueEnumerator.MoveNext())
                return;

            var keyValuePair = this.dialogueEnumerator.Current;
            this.tmp.text = keyValuePair.Key;

            var talkerIndex = keyValuePair.Value;
            var talker = this.talkerTransforms[talkerIndex];
            this.SetDialogueLocation(talker);

            if (!this.canvas.enabled)
                this.canvas.enabled = true;
        }


        private Camera mainCamera;
        private void SetDialogueLocation(Transform targetPosition)
        {
            var thisCanvas = GetComponent<RectTransform>();
            var screenPoint = this.mainCamera.WorldToScreenPoint(targetPosition.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisCanvas, screenPoint, null, out Vector2 anchorPosition);

            this.tmp.rectTransform.anchoredPosition = anchorPosition;
        }
    }
}
