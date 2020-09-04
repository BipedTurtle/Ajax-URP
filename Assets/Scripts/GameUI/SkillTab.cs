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


        private void Start()
        {
            this.InitializeIcons();
        }


        [SerializeField] private Skill_Icon Q_Mask;
        [SerializeField] private Skill_Icon W_Mask;
        [SerializeField] private Skill_Icon E_Mask;
        [SerializeField] private Skill_Icon R_Mask;
        [SerializeField] private Skill_Icon F_Mask;
        public void InitializeIcons()
        {
            this.Q_Mask.Init(this.Q);
            this.W_Mask.Init(this.W);
            this.E_Mask.Init(this.E);
            this.R_Mask.Init(this.R);
            this.F_Mask.Init(this.F);
        }


        public Skill Q;
        public Skill W;
        public Skill E;
        public Skill R;
        public Skill F;
        public void CheckSkills()
        {
            if (this.CheckSkillActivation(this.Q, KeyCode.Q)) {
                this.Q_Mask.StartCoolDownCount();
                return; }
            if (this.CheckSkillActivation(this.W, KeyCode.W)) {
                this.W_Mask.StartCoolDownCount();
                return; }
            if (this.CheckSkillActivation(this.E, KeyCode.E)) {
                this.E_Mask.StartCoolDownCount();
                return; }
            if (this.CheckSkillActivation(this.R, KeyCode.R)) {
                this.R_Mask.StartCoolDownCount();
                return; }
            if (this.CheckSkillActivation(this.F, KeyCode.F)) {
                this.F_Mask.StartCoolDownCount();
                return; }
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
                    skill.Execute();
                    return true;
                }
            }

            return false;
        }

    }
}