using UnityEngine;

using Game.InputSystem;
using Game.InputSystem.Events;
using Game.EventSystem;

using DG.Tweening;
using TMPro;

namespace Game.Bubble {
    public class Bubble : MonoBehaviour {

        [Header("Movement Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float destinationY = 10f;
        [SerializeField] private float maxWobbleAmount = 0.5f;
        [SerializeField] private float wobbleSpeed = 1f;

        [Header("Letter Settings")]
        [SerializeField] private TextMeshProUGUI letterText;

        private Camera mainCamera;
        private CircleCollider2D m_Collider;

        private Vector2 mousePos;
        private bool OnMouseClick;

        // ========== Lifecycle ==========

        private void Awake() {
            mainCamera = Camera.main;
            m_Collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnLeftMouseDown>(OnLeftMouseDown);
            EventBus.Subscribe<EVT_OnLeftMouseUp>(OnLeftMouseUp);

            BubbleMovement();
            BubbleLetterGen();
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




        // ========== Event Methods ==========
        private void OnLeftMouseDown(EVT_OnLeftMouseDown evt) { OnMouseClick = true; }
        private void OnLeftMouseUp(EVT_OnLeftMouseUp evt) { OnMouseClick = false; }




        // ========== Methods ==========


        // ---------- Movement ----------
        /// <summary>
        /// Move the bubble up to the destinationY over the specified duration
        /// </summary>
        private void BubbleMovement() {
            float wobbleAmount = Random.Range(-maxWobbleAmount, maxWobbleAmount);

            // Move the bubble up to the destinationY over the specified duration
            transform.DOMoveY(destinationY, duration).SetEase(Ease.Linear);

            // Add a side-to-side movement to simulate a bubble's wobble
            transform.DOMoveX(transform.position.x + wobbleAmount, wobbleSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }


        // ---------- Letter Generator ----------
        /// <summary>
        /// Generate a random letter for the bubble
        /// </summary>
        private void BubbleLetterGen() {
            char randomLetter = (char)Random.Range('A', 'Z' + 1);

            if (letterText != null) {
                letterText.text = randomLetter.ToString();
            } else {
                Debug.LogWarning("Bubble: Letter Text is null");
            }
        }




        // ========== Getters and Setters ==========
        /// <summary>
        /// Check if the mouse is over the bubble
        /// </summary>
        /// <returns>a bool of whether the mouse is over the bubble</returns>
        private bool IsMouseOver() {
            if (InputManager.Instance == null) return false;

            Vector3 screenPoint = InputManager.Instance.GetMousePosition();
            screenPoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);

            return m_Collider.OverlapPoint(worldPoint);
        }


        /// <summary>
        /// Get the letter of the bubble
        /// </summary>
        /// <returns>a string of the letter</returns>
        public char GetBubbleLetter() {
            return letterText.text[0];
        }
    }
}