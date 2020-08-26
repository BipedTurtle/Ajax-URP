using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Empty Reference", menuName = "Quests/Empty Reference")]
    public class EmptyReference : ScriptableObject
    {
        public AssetReference emptyReference;
        public static EmptyReference Instance { get; private set; }
        private void OnEnable()
        {
            if (Instance != null)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;
        }
    }
}
