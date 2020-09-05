using UnityEngine;

namespace GameUI
{
    public abstract class Indicator : MonoBehaviour 
    {
        public abstract void TurnOn();
        public abstract void TurnOff();
    }
}
