using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Padrox
{
    public class InputManager : Singleton<InputManager>
    {
        private PlayerControls _controls;

        #region Events
        public delegate void InteractPressed();
        public event InteractPressed OnInteractPressed;
        #endregion

        protected override void Awake() {
            base.Awake();
            _controls = new PlayerControls();
        }

        private void OnEnable() {
            _controls.Enable();
            _controls.Player.Interact.performed += Interact;
        }

        private void OnDisable() {
            _controls.Disable();
            _controls.Player.Interact.performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext ctx) {
            OnInteractPressed?.Invoke();
        }

        public Vector2 GetMoveInput() {
            return _controls.Player.Movement.ReadValue<Vector2>();
        }
    }
}
