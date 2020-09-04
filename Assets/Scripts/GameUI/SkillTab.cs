using PlayerSystem.Skills;
using UnityEngine;

namespace GameUI
{
    public class SkillTab : MonoBehaviour 
    {
        public static SkillTab Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }


        public Skill Q;
        public Skill W;
        public Skill E;
        public Skill R;
        public Skill F;
        public void CheckSkills()
        {

            if (this.CheckSkillActivation(this.Q, KeyCode.Q)) return;
            if (this.CheckSkillActivation(this.W, KeyCode.W)) return;
            if (this.CheckSkillActivation(this.E, KeyCode.E)) return;
            if (this.CheckSkillActivation(this.R, KeyCode.R)) return;
            if (this.CheckSkillActivation(this.F, KeyCode.F)) return;
        }


        public bool CheckSkillActivation(Skill skill, KeyCode skillKey)
        {
            if (skill == null)
                return false;

            if (skill.canActivate) {
                if (Input.GetKeyDown(skillKey))
                    skill.DisplayIndicator();
                if (Input.GetKeyUp(skillKey)) {
                    skill.DisableIndicator();
                    skill.Execute(); }

                return true;
            }

            return false;
        }

    }
}