using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "palyerInfo", menuName = "PlayerSystem/PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public PlayerClass playerClass;
    }
}