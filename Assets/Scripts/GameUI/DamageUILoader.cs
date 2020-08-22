using System.Collections.Generic;
using UnityEngine;

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

            this.mainCamera = Camera.main;
            DontDestroyOnLoad(gameObject);
        }


        [SerializeField] private DamageUI damageUIPrefab;
        [SerializeField] private float offset = 15f;
        private Camera mainCamera;
        public void LoadDamageUI(float damage, Vector3 at)
        {
            DamageUI ui = this.GetInacitveUI();

            Vector2 screenPoint = this.mainCamera.WorldToScreenPoint(at);
            RectTransform canvasSpace = transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasSpace, screenPoint, null, out Vector2 anchoredPosition);

            Vector2 displayPos = anchoredPosition + Vector2.left * this.offset;
            ui.DisplayDamage(spawnPos: displayPos, damage: damage);
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
