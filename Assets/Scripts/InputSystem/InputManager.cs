using UnityEngine;

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
        }

        private void Start() {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Player.Enable();
            inputActions.UI.Enable();
        }

        private void OnDisable() {
            inputActions.Player.Disable();
            inputActions.UI.Disable();
        }


        // ---------- Getters ----------
        public Vector2 GetMousePosition() {
            return inputActions.Player.MousePosition.ReadValue<Vector2>();
        }
    }
}