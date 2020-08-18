using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "palyerInfo", menuName = "PlayerSystem/PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField] private float _range = 3f;
        public float Range => _range;


        public PlayerClass playerClass;
    }
}