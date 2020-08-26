using Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility
{
    public class Pool
    {
        private static Dictionary<object, Pool> pools = new Dictionary<object, Pool>();

        public List<GameObject> ObjectList { get; } = new List<GameObject>();

        private AssetReferenceGameObject assetReference;
        private GameObject loadedAsset;
        public async static Task CreatePool(AssetReferenceGameObject reference, PoolingData poolingData)
        {
            if (ContainsReference(reference))
                return;

            var pool = new Pool();
            pool.assetReference = reference;
            var operationHandle = reference.LoadAssetAsync();
            operationHandle.Completed += (op) => pool.loadedAsset = op.Result;
            int spawnCount = poolingData.GetSpawnCountFor(reference);

#if UNITY_EDITOR
            var parent = new GameObject($"{reference.editorAsset.name} - Pool").transform;
#endif
            for (int i = 0; i < spawnCount; i++) {
                var opHandle = reference.InstantiateAsync(Vector3.zero, Quaternion.identity);
                var instantiatedObject = await opHandle.Task;
#if UNITY_EDITOR
                instantiatedObject.name = $"{reference.editorAsset.name} - {Guid.NewGuid()}";
#endif
                pool.ObjectList.Add(instantiatedObject);
#if UNITY_EDITOR
                instantiatedObject.transform.SetParent(parent);
#endif
                instantiatedObject.SetActive(false);
            }

            pools[reference.RuntimeKey] = pool;
        }


        public async static Task<Pool> GetPool(AssetReferenceGameObject reference, PoolingData data = null)
        {
            if (!ContainsReference(reference))
                await CreatePool(reference, data ?? PoolManager.Instance.atBeginningPoolingData);

            return pools[reference.RuntimeKey];
        }


        private static bool ContainsReference(AssetReferenceGameObject reference)
        {
            foreach (var key in pools.Keys)
                if (key.Equals(reference.RuntimeKey))
                    return true;

            return false;
        }


        public GameObject GetPooledObjectAt(Vector3 spawnPos, Quaternion spawnRotation)
        {
            for (int i = 0; i < ObjectList.Count; i++)
                if (ObjectList[i].gameObject.activeInHierarchy)
                    continue;
                else {
                    var go = ObjectList[i];
                    go.SetActive(true);
                    go.transform.SetPositionAndRotation(spawnPos, spawnRotation);
                    return go;
                }

            var extraGo = GameObject.Instantiate(this.loadedAsset, spawnPos, spawnRotation);
            //extraGo.name = $"{this.assetReference.editorAsset.name} - {Guid.NewGuid()}";
            ObjectList.Add(extraGo);

            return extraGo;
        }


        public static void ReturnToPool(GameObject go)
        {
            go.SetActive(false);
        }


        public static void ClearPool(AssetReferenceGameObject reference)
        {
            if (!pools.ContainsKey(reference.RuntimeKey))
                return;

            var pool = pools[reference];
            foreach (var pooled in pool.ObjectList)
                if (!Addressables.ReleaseInstance(pooled))
                    GameObject.Destroy(pooled);
        }


        public static void ClearPool()
        {
            Pool.pools.Clear();
        }
    }
}