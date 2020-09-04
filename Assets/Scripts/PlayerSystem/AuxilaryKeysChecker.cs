using Entities.NPC_System;
using Managers;
using PlayerSystem;
using QuestSystem;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;

namespace Assets.Scripts.PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class AuxilaryKeysChecker : MonoBehaviour
    {
        private LevelTimer timer;
        private void Awake()
        {
            UpdateManager.Instance.SubscribeToGlobalUpdate(this.CheckKeys);
            var timerOpHandle = Addressables.LoadAssetAsync<LevelTimer>(this.levelTimerReference);
            timerOpHandle.Completed += (op) =>
            {
                this.timer = op.Result;
                Addressables.Release(op);
            };
        }


        private void CheckKeys()
        {
            this.CheckNPCInteraction();
            this.OpenQuestList();
        }


        private void CheckNPCInteraction()
        {
            if (!Input.GetKeyDown(KeyCode.T))
                return;

            foreach (var npc in NPC.NPCsInTheScene) {
                bool interactionSucceeded = npc.Interact();
                if (interactionSucceeded)
                    return;
            }
        }


        [SerializeField] private AssetReferenceGameObject QuestListReference;
        private QuestList questList;
        [SerializeField] private AssetReference levelTimerReference;
        private void OpenQuestList()
        {
            if (!Input.GetKeyDown(KeyCode.L))
                return;

            if (this.questList == null) {
                var opHandle = this.QuestListReference.InstantiateAsync();
                opHandle.Completed += (op) =>
                {
                    this.questList = op.Result.GetComponent<QuestList>();
                    this.questList.Init();
                    Time.timeScale = 0;

                    var levelTimerOpHandle = Addressables.LoadAssetAsync<LevelTimer>(this.levelTimerReference);
                    levelTimerOpHandle.Completed += (levelTimerOp) => {
                        var levelTimer = levelTimerOp.Result;
                        levelTimer.PauseTimer();
                        Addressables.Release(levelTimerOp);
                    };
                };
                return;
            }

            bool questListActive = this.questList.gameObject.activeSelf;
            this.questList.gameObject.SetActive(!questListActive);
            if (!questListActive)
                this.questList.Init();


            if (questListActive) {
                this.timer.ResumeTimer();
                Time.timeScale = 1; }
            else {
                this.timer.PauseTimer();
                Time.timeScale = 0; }
        } 

    }
}