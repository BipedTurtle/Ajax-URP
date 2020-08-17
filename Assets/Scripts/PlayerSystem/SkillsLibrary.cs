using PlayerSystem.Skills;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "SkillsLibrary", menuName = "SkillsLibrary")]
    public class SkillsLibrary : ScriptableObject
    {
        public BasicAttack basicAttack;
        public BackShot backShot;
    }
}
