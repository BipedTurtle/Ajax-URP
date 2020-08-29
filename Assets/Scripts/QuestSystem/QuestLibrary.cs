using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    public static class QuestLibrary
    {
        public static List<Quest> Quests { get; } = new List<Quest>();

        public static void UpdateQuestProgress(QuestObject questObject)
        {
            foreach (var quest in Quests)
                quest.CheckForProgress(questObject);
        }


        public static void BeginQuest(AssetReference questReference)
        {
            var questOperationHandle = questReference.LoadAssetAsync<Quest>();
            questOperationHandle.Completed += (op) =>
            {
                var quest = op.Result;
                Quests.Add(quest);

                // add quest info to quest list
                // here...

                Addressables.Release(op);
            };
        }
    }
}
