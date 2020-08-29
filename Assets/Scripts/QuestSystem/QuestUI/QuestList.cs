using QuestSystem.QuestUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    public class QuestList : MonoBehaviour
    {
        public static int Focus { get; set; } = -1;

        private List<Quest> questList = new List<Quest>();
        public async void Init()
        {
            foreach (var quest in QuestLibrary.Quests) {
                if (this.questList.Contains(quest))
                    continue;

                var info = await quest.GetInfo();
                this.AddQuestButton(quest);
                this.questList.Add(quest);
            }
        }


        [SerializeField] private RectTransform questButtonsContainer;
        [SerializeField] private AssetReferenceGameObject questButtonReference;
        private void AddQuestButton(Quest quest)
        {
            var buttonGO = this.questButtonReference.InstantiateAsync();
            buttonGO.Completed += (op) => {
                var go = op.Result;
                go.transform.SetParent(this.questButtonsContainer);

                var questButton = op.Result.GetComponent<QuestButton>();
                questButton.Init(quest);
                questButton.AddListener(this.DisplayDescription);
            };
        }


        [SerializeField] private TextMeshProUGUI questHeader;
        [SerializeField] private TextMeshProUGUI questDescription;
        [SerializeField] private TextMeshProUGUI progressHeader;
        [SerializeField] private AssetReferenceGameObject questObjectUIReference;
        [SerializeField] private RectTransform questObjectsUI_Container;
        /// <summary>
        /// this method is called when the corresponding QuestButton is pressed
        /// </summary>
        public async void DisplayDescription(Quest quest)
        {
            int questID = quest.GetInstanceID();
            if (QuestList.Focus == questID)
                return;

            var info = await quest.GetInfo();

            this.questHeader.text = info.questName;
            this.questDescription.text = info.questDescription;
            progressHeader.gameObject.SetActive(true);

            var questObjects = quest.questObjects;
            foreach (var questObject in questObjects) {
                var questObjectUI_GO = await this.questObjectUIReference.InstantiateAsync().Task;
                questObjectUI_GO.transform.SetParent(this.questObjectsUI_Container);

                var questObjectUI = questObjectUI_GO.GetComponent<QuestObjectUI>();
                questObjectUI.Display(questObject);
            }

            QuestList.Focus = quest.GetInstanceID();
        }


        private void OnDisable()
        {
            this.questHeader.text = "";
            this.questDescription.text = "";
            this.progressHeader.gameObject.SetActive(false);
            QuestList.Focus = -1;
        }
    }
}