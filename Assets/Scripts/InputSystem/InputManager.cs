using UnityEngine;
using UnityEngine.InputSystem;

using Game.EventSystem;
using Game.InputSystem.Events;

namespace Game.InputSystem {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

        private InputSystem_Actions inputActions;

        // ---------- Lifecycle ----------

        private void Awake() {
            if (Instance != null) {
                Debug.LogWarning("InputManager: Instance already exists");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Player.Enable();
            inputActions.UI.Enable();

            inputActions.Player.Attack.performed += OnAttackPerformed;
            inputActions.Player.Attack.canceled += OnAttackCancel;
        }

        private void OnDisable() {
            inputActions.Player.Disable();
            inputActions.UI.Disable();

            inputActions.Player.Attack.performed -= OnAttackPerformed;
            inputActions.Player.Attack.canceled -= OnAttackCancel;
        }

        // ---------- Event Methods ----------
        private void OnAttackPerformed(InputAction.CallbackContext callbackContext) { EventBus.Publish(new EVT_OnLeftMouseDown()); }
        private void OnAttackCancel(InputAction.CallbackContext callbackContext) { EventBus.Publish(new EVT_OnLeftMouseUp()); }


        // ---------- Getters ----------
        public Vector2 GetMousePosition() {
            return inputActions.Player.MousePosition.ReadValue<Vector2>();
        }
    }
}