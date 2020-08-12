using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerSystem
{
    public static class SkillsLibrary
    {
        private static BasicAttack basicAttack;
        public async static Task<BasicAttack> GetBasicAttack()
        {
            if (basicAttack == null)
                basicAttack = await Addressables.LoadAssetAsync<BasicAttack>("Skills/BasicAttack.asset").Task;
            return basicAttack;
        }
    }
}
