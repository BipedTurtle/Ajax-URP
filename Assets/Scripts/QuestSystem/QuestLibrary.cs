using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    public static class QuestLibrary
    {
        private static List<Quest> quests = new List<Quest>();

        public static void UpdateQuestProgress(QuestObject questObject)
        {
            foreach (var quest in quests)
                quest.CheckForProgress(questObject);
        }


        public static void BeginQuest(AssetReference questReference)
        {
            var questOperationHandle = questReference.LoadAssetAsync<Quest>();
            questOperationHandle.Completed += (op) =>
            {
                var quest = op.Result;
                quests.Add(quest);

                // add quest info to quest list
                // here...

                Addressables.Release(op);
            };
        }
    }
}
