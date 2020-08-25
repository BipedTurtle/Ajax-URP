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
        
        
        //private async void OnEnable()
        //{
        //    foreach (var talkerRef in this.talkersReferences) {
        //        var op = talkerRef.LoadAssetAsync();
        //        GameObject talkerGo = await op.Task;
        //        this.talkers.Add(talkerGo.transform);

        //        //Addressables.Release(op);
        //    }

        //    this.BreakDialogueIntoPieces();
        //}

                
        public void StartDialogue()
        {
            var dialogueCanvasOpHandle = Addressables.InstantiateAsync("UI/DialogueCanvas.prefab");
            dialogueCanvasOpHandle.Completed += (op) =>
            {
                var go = op.Result;
                var dialogueCanvas = go.GetComponent<DialogueCanvas>();
                dialogueCanvas.Init(this.textToSpeakerDictionary, this.talkersReferences);
                //dialogueCanvas.DisplayDialogue();
            };
        }


        private void BreakDialogueIntoPieces()
        {
            var text = this.dialogueText.text;
            var paragraphs = text.Split(new[] { '#' }, StringSplitOptions.None);

            foreach (var paragraph in paragraphs) {
                if (string.IsNullOrEmpty(paragraph))
                    continue;

                int speaker = int.Parse(paragraph[0].ToString());
                this.textToSpeakerDictionary[text] = speaker;
            }
        }
    }
}
