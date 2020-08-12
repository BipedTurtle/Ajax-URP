using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "palyerInfo", menuName = "PlayerSystem/PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField] private float _range = 3f;
        public float Range => _range;
    }
}
