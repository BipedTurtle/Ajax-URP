using Entities;
using GameUI;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInfo playerInfo;
        public PoolingData playerPoolingData;

        #region Singleton
        public static PlayerController Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion


        private void Start()
        {
            this.raycastingCamera = Camera.main;
            this.Agent = GetComponent<NavMeshAgent>();

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.CheckKeyboard);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.RightClick);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.LeftClick);
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.UpdateOnTarget);
        }


        public Transform Target { get; private set; }
        private async void UpdateOnTarget()
        {
            if (Target == null |
                Time.frameCount % 2 != 0)
                return;

            this.TurnTowardsTarget();

            var basicAttack = await SkillsLibrary.GetBasicAttack();
            basicAttack.Execute();
        }


        private readonly float rotationAmount = .1f;
        private float rotationProgress;
        private void TurnTowardsTarget()
        {
            var toTargetVector = this.Target.localPosition - transform.localPosition;
            var targetRotation = Quaternion.LookRotation(toTargetVector);
            this.rotationProgress += this.rotationAmount;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, .18f);

            if (this.rotationProgress >= 1f)
                this.rotationProgress = 0;
        }


        [SerializeField] private AttackIndicator attackIndicator;
        private async void CheckKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.A) &
                !this.attackIndicator.On)
                    this.attackIndicator.TurnOn();

            if (Input.GetKeyDown(KeyCode.F)) {
                var backShot = await SkillsLibrary.GetBackShot();
                backShot.Execute();
            }
        }


        public NavMeshAgent Agent { get; private set; }
        private Camera raycastingCamera;
        private void RightClick()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            if (this.attackIndicator.On) {
                this.attackIndicator.TurnOff();
                this.Target = null;
                return;
            }


            bool hittableHit = this.RaycastAtMousePoint(this.hittableHitResults, 1 << 10);
            if (hittableHit) {
                Transform hittableTransform = this.hittableHitResults[0].transform;
                this.Target = hittableTransform;
                var hittable = hittableTransform.GetComponent(typeof(IHittable)) as IHittable;
                //hittable.OnHit();

                return;
            }

            bool groundHit = this.RaycastAtMousePoint(this.groundHitResults, (1 << 9));
            if (groundHit) {
                var destinationGround = this.groundHitResults[0].point;
                Agent.SetDestination(destinationGround);
                this.CancelActions();
            }
        }


        private void LeftClick()
        {
            if (!Input.GetMouseButtonDown(0))
                return;
            
            // if attack indicator is directly on an enemy, set it as target; otherwise find the closest enemy from the cursor and then set it as the target
            if (this.attackIndicator.On) {
                this.attackIndicator.TurnOff();

                bool cursorOnHittable = this.RaycastAtMousePoint(this.hittableHitResults, 1 << 10);
                if (cursorOnHittable)
                    this.Target = this.hittableHitResults[0].transform;
                else
                    this.Target = ControlUtility.GetClosestEnemyFromCursor();
            }
        }


        public void CancelActions()
        {
            this.Target = null;
            this.rotationProgress = 0;
        }


        private RaycastHit[] groundHitResults = new RaycastHit[1];
        private RaycastHit[] hittableHitResults = new RaycastHit[1];
        private bool RaycastAtMousePoint(RaycastHit[] raycastHits, int mask)
        {
            var ray = this.raycastingCamera.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, maxDistance: 30f, layerMask: mask);

            return hitCount != 0;
        }
    }

}
