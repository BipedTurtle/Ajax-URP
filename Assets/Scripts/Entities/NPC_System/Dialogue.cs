using GameUI;
using Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entities.NPC_System
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "NPC/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private TextAsset _dialogueText;
        public TextAsset DialogueText => _dialogueText;
        [SerializeField] private AssetReferenceGameObject[] _talkersReferences;
        public AssetReferenceGameObject[] TalkersReferences => _talkersReferences;
        public Dictionary<string, int> TextToSpeakerDictionary { get; private set; } = new Dictionary<string, int>();
        public AssetReference questReference;


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
                var dialogueCanvas = go.GetComponent<DialogueManager>();
                dialogueCanvas.Init(this);
                dialogueCanvas.DisplayDialogue();
            };
        }


        private void BreakDialogueIntoPieces()
        {
            var text = this._dialogueText.text;
            var paragraphs = text.Split(new[] { "#" }, StringSplitOptions.None);

            foreach (var paragraph in paragraphs) {
                if (string.IsNullOrEmpty(paragraph))
                    continue;

                int speaker = int.Parse(paragraph[0].ToString());
                this.TextToSpeakerDictionary[paragraph.Trim()] = speaker;
            }
        }
    }
}