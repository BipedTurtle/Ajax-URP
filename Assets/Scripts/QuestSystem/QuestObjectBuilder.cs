using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    public static class QuestObjectBuilder
    {
        private static QuestObject questObject;

        public static QuestObject SetSubject(AssetReference subject)
        {
            CreateQuestObjectIfNull();
            questObject.Subject = subject;
            return questObject;
        }

        public static QuestObject SetEventType(QuestEventType eventType)
        {
            CreateQuestObjectIfNull();
            questObject.QuestEvent = eventType;
            return questObject;
        }

        public static QuestObject SetObject(AssetReference @object)
        {
            CreateQuestObjectIfNull();
            questObject.Object = @object;
            return questObject;
        }


        public static QuestObject Build() => questObject;


        public static void Clear()
        {
            CreateQuestObjectIfNull();
            questObject.Clear();
        }


        // you can't do questObject = new QuestObject() above because the execution order of scripts get massed up
        private static void CreateQuestObjectIfNull()
        {
            if (questObject == null)
                questObject = new QuestObject();
        }
    }

}
