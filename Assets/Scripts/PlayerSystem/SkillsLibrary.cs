using PlayerSystem.Skills;
using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace PlayerSystem
{
    public static class SkillsLibrary
    {
        private static BasicAttack basicAttack;
        public async static Task<BasicAttack> GetBasicAttack()
        {
            if (basicAttack == null)
                basicAttack = await Addressables.LoadAssetAsync<BasicAttack>("Skills/BasicAttackRanged.asset").Task;
            return basicAttack;
        }


        private static BackShot backShot;
        public async static Task<BackShot> GetBackShot()
        {
            if (backShot == null)
                backShot = await Addressables.LoadAssetAsync<BackShot>("Skills/BackSho.asset").Task;
            return backShot;
        }
    }
}
