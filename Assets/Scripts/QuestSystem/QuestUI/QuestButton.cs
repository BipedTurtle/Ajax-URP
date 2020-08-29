using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestButton : MonoBehaviour
    {

        private Quest relevantQuest;
        [SerializeField] private TextMeshProUGUI tmp;


        public async void Init(Quest quest)
        {
            this.relevantQuest = quest;

            var info = await quest.GetInfo();
            this.tmp.text = $"{info.questName}  from ({info.npcName})";
        }


        private UnityAction onClickedShowDescription;
        public void AddListener(Action<Quest> onButtonClicked)
        {
            this.onClickedShowDescription = delegate { onButtonClicked(this.relevantQuest); };

            var button = GetComponent<Button>();
            button.onClick.AddListener(onClickedShowDescription);
        }

    }
}