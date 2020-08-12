using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "PlayerSystem/Skills/BasicAttack")]
    public class BasicAttack : Skill
    {
        [SerializeField] private PlayerInfo playerInfo;
        private float nextAttack;
        public float attackInterval = 2f;
        private void OnEnable()
        {
            this.nextAttack = 0;
        }


        public override void Execute()
        {
            var player = PlayerController.Instance;

            bool targetInRange = (player.Target.localPosition - player.transform.localPosition).sqrMagnitude < Mathf.Pow(this.playerInfo.Range, 2);
            if (targetInRange) {
                player.Agent.ResetPath();
                goto AttackIfPossible;
            }
            else {
                var destination = player.Target.localPosition;
                player.Agent.SetDestination(destination);
            }


            AttackIfPossible:
            bool attackCoolHasReturned = Time.time > this.nextAttack;
            if (attackCoolHasReturned) {
                // attack logic comes here
                Debug.Log("Attack");
                player.Agent.ResetPath();

                this.nextAttack = Time.time + attackInterval;
            }

        }
    }
}
