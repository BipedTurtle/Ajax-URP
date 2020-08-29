using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace QuestSystem.QuestUI
{
    public class QuestObjectUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI tmp;
        public async void Display(QuestObject questObject)
        {
            #region Addressable lodings
            var entityToSpriteDictionaryOpHandle = Addressables.LoadAssetAsync<EntityToSpriteDictionary>("EntityToSpriteDictionary.asset");
            var dictionary = await entityToSpriteDictionaryOpHandle.Task;

            var emptyReferenceOpHandle = Addressables.LoadAssetAsync<EmptyReference>("EmptyReference.asset");
            var emptyReference = await emptyReferenceOpHandle.Task;
            #endregion

            AssetReference subject = questObject.Subject;
            this.image.gameObject.SetActive(true);
            var sprite = await dictionary.GetSprite(subject);
            this.image.sprite = sprite;

            string textDescription = $"{dictionary.GetName(subject)}: {questObject.CurrentCount} / {questObject.questFulfillCount}";
            this.tmp.text = textDescription;

            Addressables.Release(entityToSpriteDictionaryOpHandle);
            Addressables.Release(emptyReferenceOpHandle);
        }


        private void OnDisable()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}