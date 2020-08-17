using UnityEngine;

namespace Entities
{
    public interface IHittable
    {
        Vector3 Position { get; }
        void OnHit();
    }
}
