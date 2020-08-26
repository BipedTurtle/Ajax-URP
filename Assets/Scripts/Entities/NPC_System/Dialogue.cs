using GameUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entities.NPC_System
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "NPC/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private TextAsset dialogueText;
        [SerializeField] private AssetReferenceGameObject[] talkersReferences;
        private Dictionary<string, int> textToSpeakerDictionary = new Dictionary<string, int>();


        private void OnEnable()
        {
            this.BreakDialogueIntoPieces();
        }


        public void StartDialogue()
        {
            var dialogueCanvasOpHandle = Addressables.InstantiateAsync("UI/DialogueCanvas.prefab");
            dialogueCanvasOpHandle.Completed += (op) =>
            {
                var go = op.Result;
                var dialogueCanvas = go.GetComponent<DialogueCanvas>();
                dialogueCanvas.Init(this.textToSpeakerDictionary, this.talkersReferences);
                dialogueCanvas.DisplayDialogue();
            };
        }


        private void BreakDialogueIntoPieces()
        {
            var text = this.dialogueText.text;
            var paragraphs = text.Split(new[] { "#" }, StringSplitOptions.None);

            foreach (var paragraph in paragraphs) {
                if (string.IsNullOrEmpty(paragraph))
                    continue;

                int speaker = int.Parse(paragraph[0].ToString());
                this.textToSpeakerDictionary[paragraph.Trim()] = speaker;
            }
        }
    }
}