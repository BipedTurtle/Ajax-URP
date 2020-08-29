using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    public class QuestList : MonoBehaviour
    {
        private List<Quest> questList = new List<Quest>();
        public async void Init()
        {
            foreach (var quest in QuestLibrary.Quests) {
                if (this.questList.Contains(quest))
                    continue;

                var info = await quest.GetInfo();
                this.AddQuestButton(info.questName, info.npcName);
                this.questList.Add(quest);
            }
        }


        [SerializeField] private RectTransform questButtonsContainer;
        [SerializeField] private AssetReferenceGameObject questButtonReference;
        private void AddQuestButton(string questName, string npcName)
        {
            var buttonGO = this.questButtonReference.InstantiateAsync();
            buttonGO.Completed += (op) => {
                var go = op.Result;
                go.transform.SetParent(this.questButtonsContainer);

                var questButton = op.Result.GetComponent<QuestButton>();
                questButton.DisplayInfo(questName, npcName);
            };
        }

    }
}