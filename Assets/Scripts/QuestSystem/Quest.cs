using Entities.NPC_System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "quest", menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public List<QuestObject> questObjects = new List<QuestObject>();
        private int objectivesCompletedCount;
        public bool QuestCompleted => this.objectivesCompletedCount == questObjects.Count;

        public event Action OnQuestCompleted;
        public void CheckForProgress(QuestObject questObjectGenerated)
        {
            foreach (QuestObject myQuestObject in questObjects) {
                if (myQuestObject.questObjectComplete)
                    continue;
                else {
                    bool objectiveCompleted = myQuestObject.CheckQuestFulfillment(questObjectGenerated);
                    this.objectivesCompletedCount += (objectiveCompleted) ? 1 : 0;
                    if (this.QuestCompleted)
                        this.OnQuestCompleted.Invoke(); }
            }
        }


        private void OnEnable()
        {
            this.OnQuestCompleted += this.DisplayCompleteMessage;
        }


        private void OnDisable()
        {
            this.questObjects.ForEach(q => q.CurrentCount = 0);
            this.objectivesCompletedCount = 0;
            this.OnQuestCompleted -= this.DisplayCompleteMessage;
        }


        public void DisplayCompleteMessage()
        {
            Debug.Log("quest complete!");
        }


        [SerializeField] private string questName;
        [SerializeField] private AssetReference relevantNPC_Reference;
        [SerializeField] private TextAsset questDescription;
        public async Task<(string questName, string npcName, string questDescription)> GetInfo()
        {
            var opHandle = this.relevantNPC_Reference.LoadAssetAsync<GameObject>();
            var npcGo = await opHandle.Task;

            var npc = npcGo.GetComponent<NPC>();
            return (this.questName, npc.NPC_Name, this.questDescription.text);
        }
        
    }
}
