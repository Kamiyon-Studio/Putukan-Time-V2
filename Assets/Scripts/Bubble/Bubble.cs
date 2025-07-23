using UnityEngine;

using Game.InputSystem;

namespace Game.Bubble {
    public class Bubble : MonoBehaviour {

        private Camera mainCamera;
        private CircleCollider2D m_Collider;

        private Vector2 mousePos;


        private void Awake() {
            mainCamera = Camera.main;
            m_Collider = GetComponent<CircleCollider2D>();
        }

        private void Update() {
            if (IsMouseOver()) {
                Debug.Log("Mouse over");
            }
        }

        private bool IsMouseOver() {
            Vector3 screenPoint = InputManager.Instance.GetMousePosition();
            screenPoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);

            return m_Collider.OverlapPoint(worldPoint);
        }
    }
}