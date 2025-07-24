using UnityEngine;

using Game.InputSystem;
using Game.InputSystem.Events;
using Game.EventSystem;

using DG.Tweening;

namespace Game.Bubble {
    public class Bubble : MonoBehaviour {

        [Header("Movement Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float destinationY = 10f;
        [SerializeField] private float maxWobbleAmount = 0.5f;
        [SerializeField] private float wobbleSpeed = 1f;

        private Camera mainCamera;
        private CircleCollider2D m_Collider;

        private Vector2 mousePos;
        private bool OnMouseClick;


        private void Awake() {
            mainCamera = Camera.main;
            m_Collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnLeftMouseDown>(OnLeftMouseDown);
            EventBus.Subscribe<EVT_OnLeftMouseUp>(OnLeftMouseUp);

            float wobbleAmount = Random.Range(-maxWobbleAmount, maxWobbleAmount);

            // Move the bubble up to the destinationY over the specified duration
            transform.DOMoveY(destinationY, duration).SetEase(Ease.Linear);

            // Add a side-to-side movement to simulate a bubble's wobble
            transform.DOMoveX(transform.position.x + wobbleAmount, wobbleSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnLeftMouseDown>(OnLeftMouseDown);
            EventBus.Unsubscribe<EVT_OnLeftMouseUp>(OnLeftMouseUp);

            transform.DOKill();
        }

        private void Update() {
            if (IsMouseOver() && OnMouseClick) {
                Destroy(gameObject);
            }
        }

        // ---------- Events Methods ----------
        private void OnLeftMouseDown(EVT_OnLeftMouseDown evt) { OnMouseClick = true; }
        private void OnLeftMouseUp(EVT_OnLeftMouseUp evt) { OnMouseClick = false; }


        // ---------- Getters and Setters ----------

        private bool IsMouseOver() {
            if (InputManager.Instance == null) return false;

            Vector3 screenPoint = InputManager.Instance.GetMousePosition();
            screenPoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);

            return m_Collider.OverlapPoint(worldPoint);
        }
    }
}