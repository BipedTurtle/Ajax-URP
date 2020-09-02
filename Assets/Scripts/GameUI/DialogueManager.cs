using System.Linq;
using Entities.NPC_System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PlayerSystem;
using UnityEngine.UI;
using Managers;
using QuestSystem;
using System.Text;

namespace GameUI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private Image backgroundImage;

        private Dictionary<string, int>.Enumerator dialogueEnumerator;
        private List<Transform> talkerTransforms = new List<Transform>();
        private Dialogue dialogue;
        public void Init(Dialogue dialogue)
        {
            this.mainCamera = Camera.main;
            this.canvas = GetComponent<Canvas>();
            this.canvas.enabled = true;
            this.dialogue = dialogue;

            this.waitForNextLetter = new WaitForSeconds(this.displaySpeed);
            this.sb = new StringBuilder();

            var textToSpeakerDictionary = dialogue.TextToSpeakerDictionary;
            this.dialogueEnumerator = textToSpeakerDictionary.GetEnumerator();

            #region Get Talkers' transforms
            var talkersReferences = dialogue.TalkersReferences;
            var NPC_References = NPC.NPCsInTheScene.Select(npc => npc.SelfReference.RuntimeKey).ToList();
            var player = Player.Instance;
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
                this.FinishDialogue();
                return;
            }

            var keyValuePair = this.dialogueEnumerator.Current;
            var paragraph = keyValuePair.Key;
            this.sb.Clear();
            this.sb.Append(paragraph);

            this.tmp.text = paragraph;
            var rectTransform = this.tmp.rectTransform;
            var rectSize = new Vector2(rectTransform.rect.width, this.tmp.preferredHeight);
            rectTransform.sizeDelta = rectSize;
            this.backgroundImage.rectTransform.sizeDelta = rectSize;

            var talkerIndex = keyValuePair.Value - 1;
            var talker = this.talkerTransforms[talkerIndex];
            this.SetDialogueLocation(talker);

            StartCoroutine(this.DisplayLetterByLetter());

            if (!this.canvas.enabled)
                this.canvas.enabled = true;
        }


        [SerializeField] private float displaySpeed = .01f;
        private WaitForSeconds waitForNextLetter;
        private StringBuilder sb;
        private IEnumerator DisplayLetterByLetter()
        {
            string originalText = this.sb.ToString();

            int lettersCount = this.sb.Length;
            this.sb.Clear();
            this.sb.Append(' ', lettersCount);
            this.tmp.text = this.sb.ToString();

            int lettersDisplayIndex = 0;
            while (lettersDisplayIndex < lettersCount) {
                var letterToDisplay = originalText[lettersDisplayIndex];
                sb.Replace(' ', letterToDisplay, lettersDisplayIndex, 1);
                this.tmp.text = sb.ToString();

                lettersDisplayIndex++;
                yield return this.waitForNextLetter;
            }
        }


        private Camera mainCamera;
        private readonly Vector2 defaultOffset = new Vector2(0, 35f);
        private void SetDialogueLocation(Transform targetPosition)
        {
            var thisCanvas = GetComponent<RectTransform>();
            var screenPoint = this.mainCamera.WorldToScreenPoint(targetPosition.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisCanvas, screenPoint, null, out Vector2 anchorPosition);

            float boxHeight = this.tmp.rectTransform.rect.height / 2;
            Vector2 totalOffset = this.defaultOffset + Vector2.up * boxHeight;
            Vector2 displayPosition = anchorPosition + totalOffset;
            this.tmp.rectTransform.anchoredPosition = displayPosition;
            this.backgroundImage.rectTransform.anchoredPosition = displayPosition;
        }


        private void CheckPageFlip()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                StopAllCoroutines();
                this.DisplayDialogue();
            } 
        }


        private void FinishDialogue()
        {
            UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.CheckPageFlip);
            QuestLibrary.BeginQuest(this.dialogue.questReference);
            PlayerController.Instance.EnableInputs();
            
            Addressables.ReleaseInstance(gameObject);
        }
    }
}