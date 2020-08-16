using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "PoolingData", menuName = "ScriptableObjects/PoolData")]
public class PoolingData : ScriptableObject, IEnumerable
{
    public List<AssetReferenceGameObject> toBePooledAtStart = new List<AssetReferenceGameObject>();
    public List<int> counts = new List<int>();

    public IEnumerator GetEnumerator()
    {
        return toBePooledAtStart.GetEnumerator();
    }

    public int GetSpawnCountFor(AssetReferenceGameObject reference)
    {
        int index = 0;
        for (int i = 0; i < toBePooledAtStart.Count; i++) {
            if (toBePooledAtStart[i].RuntimeKey.Equals(reference.RuntimeKey)) {
                index = i;
                break;
            }
        }

        return counts[index];
    }
}
