using PlayerSystem.Skills;
using System;
using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "SkillsLibrary", menuName = "SkillsLibrary")]
    public class SkillsLibrary : ScriptableObject
    {
        #region Ranged Skills
        public BasicAttackRanged basicAttack;
        public BackShot backShot;
        #endregion
    }
}
