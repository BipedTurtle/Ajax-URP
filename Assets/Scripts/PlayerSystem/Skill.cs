using UnityEngine;

namespace PlayerSystem.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public abstract void Execute();
    }
}
