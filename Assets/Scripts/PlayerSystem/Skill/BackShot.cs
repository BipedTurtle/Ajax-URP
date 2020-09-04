﻿using Entities.Weapons;
using GameUI;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace PlayerSystem.Skills
{
    [CreateAssetMenu(fileName = "BackShot", menuName = "PlayerSystem/Skills/BackShot")]
    public class BackShot : DamagingSkill
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }


        public override void Execute()
        {
            if (!base.canActivate)
                return;

            this.TurnTowardsCursorPosition();
            this.InstantiateArrow();
            UpdateManager.Instance.SubscribeToGlobalFixedUpdate(this.SlideBackwards);

            base.nextActivation = Time.time + base._coolDown;
        }


        [SerializeField] private float movementAmount = 2f;
        [SerializeField] private float slideSpeed = 2f;
        private Vector3 movementDirection;
        private float distanceMoved;
        private void SlideBackwards()
        {
            var player = PlayerController.Instance;
            player.CancelActions();
            player.Agent.ResetPath();
            player.Agent.updateRotation = false;

            var movementThisFrame = this.movementAmount * Time.deltaTime * this.slideSpeed;
            if (distanceMoved < this.movementAmount) {
                this.distanceMoved += movementThisFrame;
                player.transform.localPosition += this.movementDirection * movementThisFrame;
            }
            else {
                UpdateManager.Instance.UnsubscribeFromGlobalFixedUpdate(this.SlideBackwards);
                this.distanceMoved = 0;
                player.Agent.updateRotation = true;
            }
        }


        [SerializeField] private AssetReferenceGameObject arrowReference;
        private Pool arrowPool;
        private async void InstantiateArrow()
        {
            var player = Player.Instance;
            var playerPoolingData = player.weaponsAndEffectsPoolingData;

            if (arrowPool == null)
                arrowPool = await Pool.GetPool(this.arrowReference, playerPoolingData);
            var offset = player.transform.forward * .7f;
            var spawnPos = player.transform.localPosition + offset;
            var arrow = arrowPool.GetPooledObjectAt(spawnPos, player.transform.localRotation).GetComponent<Arrow>();

            arrow.SetFlightDistance(player.PlayerStats.Range);
            arrow.SetAttackInfo(player.PlayerStats, base.skillInfo);

            arrow.transform.SetPositionAndRotation(spawnPos, player.transform.localRotation);
        }


        private void TurnTowardsCursorPosition()
        {
            var toCursorVector = ControlUtility.GetPlayerToCursorVector().normalized;
            var player = PlayerController.Instance.transform;
            player.localRotation = Quaternion.LookRotation(toCursorVector);

            this.movementDirection = -toCursorVector;
        }
    }
}
