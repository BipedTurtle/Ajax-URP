using Managers;
using PlayerSystem.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class Skill_Icon : MonoBehaviour
    {
        [SerializeField] private Image mask;

        private float skillCoolDown;
        public void Init(Skill skill)
        {
            if (skill == null)
                return;

            this.skillCoolDown = skill.CoolDown;
        }


        private bool countDowning;
        public void StartCoolDownCount()
        {
            if (this.countDowning)
                return;

            this.mask.fillAmount = 1f;
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.CountDownLogic);
            this.countDowning = true;
        }


        private float currentCool;
        private void CountDownLogic()
        {
            if (this.currentCool < this.skillCoolDown) {
                this.currentCool += Time.deltaTime;

                float progress = Mathf.Clamp(this.skillCoolDown - this.currentCool, 0, this.skillCoolDown);
                float fill = progress / skillCoolDown;

                this.mask.fillAmount = fill;
            }
            else {
                UpdateManager.Instance.UnSubscribeFromGlobalUpdate(this.CountDownLogic);
                this.currentCool = 0;
                this.countDowning = false;
            }
        }

    }
}
