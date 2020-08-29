using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

namespace QuestSystem.QuestUI
{
    [CreateAssetMenu(fileName = "Entity to Sprite Dictionary", menuName = "Quests/EntityToSpriteDictionary")]
    public class EntityToSpriteDictionary : ScriptableObject
    {
        public List<AssetReference> entitiesReferences;
        public List<AssetReferenceSprite> sprites;
        public List<string> entityNames = new List<string>();

        public async Task<Sprite> GetSprite(AssetReference entityRef)
        {
            var index = this.entitiesReferences.FindIndex(r => r.RuntimeKey.Equals(entityRef.RuntimeKey));
            if (index == -1)
                return await Addressables.LoadAssetAsync<Sprite>("defaultSprite.sprite").Task;

            var corresPondingSpriteRef = this.sprites[index];
            return await corresPondingSpriteRef.LoadAssetAsync().Task;
        }


        public string GetName(AssetReference entityRef)
        {
            var index = this.entitiesReferences.FindIndex(r => r.RuntimeKey.Equals(entityRef.RuntimeKey));
            if (index == -1)
                return "";

            return this.entityNames[index];
        }
    }
}