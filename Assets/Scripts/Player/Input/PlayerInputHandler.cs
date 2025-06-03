using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public class PlayerInputHandler
    {
        private PlayerInput _playerInput;
        public Vector2 CurrentMovementInput { get; private set; }
        public Vector2 CurrentMousePosition { get; private set; }
        public event UnityAction Jump;
        public event UnityAction Shoot;
        
        public PlayerInputHandler(bool init = true)
        {
            if (init)
                Init();
        }
        
        public void Init()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            
            _playerInput.Movement.Movement.performed += OnMovementPerformed;
            _playerInput.Movement.Movement.canceled += OnMovementCanceled;
            _playerInput.Movement.Jump.performed += OnJumpPerformed;

            _playerInput.Shooting.MousePosition.performed += OnMousePositionChanged;
            _playerInput.Shooting.Shoot.performed += OnShootPerformed;
        }

        public void Disable()
        {
            _playerInput.Movement.Movement.performed -= OnMovementPerformed;
            _playerInput.Movement.Movement.canceled -= OnMovementCanceled;
            _playerInput.Movement.Jump.performed -= OnJumpPerformed;

            _playerInput.Shooting.MousePosition.performed -= OnMousePositionChanged;
            _playerInput.Shooting.Shoot.performed -= OnShootPerformed;
            
            _playerInput.Dispose();
        }

        private void OnShootPerformed(InputAction.CallbackContext context)
        {
            Shoot?.Invoke();
        }

        private void OnMousePositionChanged(InputAction.CallbackContext context)
        {
            CurrentMousePosition = context.ReadValue<Vector2>();
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            Jump?.Invoke();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            CurrentMovementInput = context.ReadValue<Vector2>();
        }
        
        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            CurrentMovementInput = Vector2.zero;
        }
    }
}