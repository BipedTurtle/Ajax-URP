using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Managers;

namespace QuestSystem
{
    [Serializable]
    public class QuestObject
    {
        public AssetReference Subject;
        public QuestEventType QuestEvent;
        public AssetReference Object;

        public int questFulfillCount;
        public int CurrentCount { get; set; }

        public bool questObjectComplete => (this.CurrentCount >= questFulfillCount);


        public QuestObject()
        {
            this.Clear();
        }


        public QuestObject(int questFulfillCount, AssetReference subject, QuestEventType questEvent, AssetReference @object)
        {
            this.questFulfillCount = questFulfillCount;

            this.Subject = subject;
            this.QuestEvent = questEvent; 
            this.Object = @object;
        }


        public bool CheckQuestFulfillment(QuestObject questObject)
        {
            if (questObject.Subject.AssetGUID == this.Subject.AssetGUID &
                questObject.QuestEvent == this.QuestEvent &
                questObject.Object.AssetGUID == this.Object.AssetGUID)
            {
                this.CurrentCount++;
                return this.questObjectComplete;
            }


            return false;
        }


        public void Clear()
        {
            if (EmptyReference.Instance == null)
                return;

            var emptyReference = ReferenceCenter.Instance.emptyReference;

            this.Subject = emptyReference;
            this.QuestEvent = QuestEventType.None;
            this.Object = emptyReference;
        }
    }
}