using QuestSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts
{
    public class Test : MonoBehaviour
    {

        public AssetReference questReference;
        private async void Start()
        {
            QuestLibrary.BeginQuest(questReference);
        }
    }
}
