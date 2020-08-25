
using Entities.NPC_System;
using QuestSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        public AssetReference questReference;
        private void Start()
        {
            QuestLibrary.BeginQuest(questReference);
        }
    }
}
