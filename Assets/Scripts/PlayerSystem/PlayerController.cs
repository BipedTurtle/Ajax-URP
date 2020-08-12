using Entities;
using Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInfo playerInfo;

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

            UpdateManager.Instance.SubscribeToGlobalUpdate(this.RightClick);
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


        public NavMeshAgent Agent { get; private set; }
        private Camera raycastingCamera;
        private void RightClick()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            bool hittableHit = this.RaycastAtMousePoint(this.hittableHitResults, 1 << 10);
            if (hittableHit)
            {
                Transform hittableTransform = this.hittableHitResults[0].transform;
                this.Target = hittableTransform;
                var hittable = hittableTransform.GetComponent(typeof(IHittable)) as IHittable;
                //hittable.OnHit();

                return;
            }

            bool groundHit = this.RaycastAtMousePoint(this.groundHitResults, (1 << 9));
            if (groundHit)
            {
                var destinationGround = this.groundHitResults[0].point;
                Agent.SetDestination(destinationGround);
                this.CancelActions();
            }
        }


        private void CancelActions()
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
