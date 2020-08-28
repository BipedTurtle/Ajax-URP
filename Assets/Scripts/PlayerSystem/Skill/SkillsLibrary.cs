using PlayerSystem.Skills;
using System;
using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "SkillsLibrary", menuName = "SkillsLibrary")]
    public class SkillsLibrary : ScriptableObject
    {
        public BasicAttackRanged basicAttack;
        public BackShot backShot;


        public Action GetSkillsKeyCheck(PlayerClass playerClass)
        {
            switch (playerClass)
            {
                case PlayerClass.Melee:
                    return SkillsKeyCheck_Melee;
                case PlayerClass.Ranged:
                    return SkillsKeyCheck_Ranged;
                case PlayerClass.Magician:
                    return SkillsKeyCheck_Magician;
                default:
                    return SkillsKeyCheck_Ranged;
            }
        }


        private void SkillsKeyCheck_Melee()
        {

        }


        private void SkillsKeyCheck_Ranged()
        {
            if (Input.GetKeyDown(KeyCode.F))
                this.backShot.DisplayIndicator();
            if (Input.GetKeyUp(KeyCode.F)) {
                this.backShot.DisableIndicator();
                this.backShot.Execute();
            }
        }


        private void SkillsKeyCheck_Magician()
        {

        }
    }
}
