using System.Linq;
using Entities.NPC_System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PlayerSystem;
using UnityEngine.UI;
using Managers;

namespace GameUI
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private Image backgroundImage;

        private Dictionary<string, int>.Enumerator dialogueEnumerator;
        private List<Transform> talkerTransforms = new List<Transform>();
        public void Init(Dictionary<string, int> textToSpeakerDictionary, IEnumerable<AssetReferenceGameObject> talkersReferences)
        {
            this.mainCamera = Camera.main;
            this.canvas = GetComponent<Canvas>();

            this.dialogueEnumerator = textToSpeakerDictionary.GetEnumerator();

            #region Get Talkers' transforms
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
            #endregion

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.CheckPageFlip);
        }


        private Canvas canvas;
        public void DisplayDialogue()
        {
            if (!this.dialogueEnumerator.MoveNext()) {
                Addressables.ReleaseInstance(gameObject);
                UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.CheckPageFlip);
                return;
            }

            var keyValuePair = this.dialogueEnumerator.Current;
            this.tmp.text = keyValuePair.Key;

            var talkerIndex = keyValuePair.Value - 1;
            var talker = this.talkerTransforms[talkerIndex];
            this.SetDialogueLocation(talker);

            if (!this.canvas.enabled)
                this.canvas.enabled = true;
        }


        private Camera mainCamera;
        private readonly Vector2 offset = new Vector2(0, 45f);
        private void SetDialogueLocation(Transform targetPosition)
        {
            var thisCanvas = GetComponent<RectTransform>();
            var screenPoint = this.mainCamera.WorldToScreenPoint(targetPosition.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisCanvas, screenPoint, null, out Vector2 anchorPosition);

            var displayPosition = anchorPosition + this.offset;
            this.tmp.rectTransform.anchoredPosition = displayPosition;
            this.backgroundImage.rectTransform.anchoredPosition = displayPosition;
        }


        private void CheckPageFlip()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                this.DisplayDialogue();
        }
    }
}
