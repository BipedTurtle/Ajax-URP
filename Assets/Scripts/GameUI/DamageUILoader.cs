﻿using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace GameUI
{
    public class DamageUILoader : MonoBehaviour
    {
        #region Singleton
        public static DamageUILoader Instance { get; private set; }
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
            foreach (Transform ui in transform)
                this.damageUIs.Enqueue(ui.GetComponent<DamageUI>());

            DontDestroyOnLoad(gameObject);
        }


        [SerializeField] private DamageUI damageUIPrefab;
        [SerializeField] private float offset = 15f;
        public void LoadDamageUI(float damage, Vector3 at, bool isCritical = false)
        {
            DamageUI ui = this.GetInacitveUI();

            var anchoredPosition = TrackingCamera.GetAnchorPos(at, transform as RectTransform);
            Vector2 displayPos = anchoredPosition + Vector2.left * this.offset;
            ui.DisplayDamage(spawnPos: displayPos, damage: damage, isCritical: isCritical);
        }


        private Queue<DamageUI> damageUIs = new Queue<DamageUI>();
        private DamageUI GetInacitveUI()
        {
            bool thereIsNoUIAvailable = this.damageUIs.Count == 0;
            if (thereIsNoUIAvailable) {
                var extraUI = Instantiate(this.damageUIPrefab);
                extraUI.transform.SetParent(transform);
                return extraUI;
            }

            return this.damageUIs.Dequeue();
        }


        public void ReturnTMP(DamageUI damageUI)
            => this.damageUIs.Enqueue(damageUI);
    }
}
