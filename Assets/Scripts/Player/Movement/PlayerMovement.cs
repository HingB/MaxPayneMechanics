using System;
using System.Collections;
using Player.Input;
using Player.ScriptableObjects;
using UnityEngine;
using NaughtyAttributes;
using Player.Enums;
using Player.Structs;
using UnityEngine.Events;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Expandable]
        [SerializeField] private PlayerMovementSettings _settings;
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private TimeRetarder _timeRetarder;
        
        private PlayerInputHandler _playerInputHandler;
        private PlayerWorldStateHandler _playerWorldStateHandler;
        private Rigidbody2D _rigidbody2D;
        
        private PlayerMovementState _playerMovementState;
        private Vector2 _currentVelocity;
        private Vector2 _lastNonZeroDirection;
        private JumpSettings _jumpSettings;
        
        private Coroutine _jumpCoroutine;

        public PlayerMovementState PlayerMovementState => _playerMovementState;
        
        public void OnEnable()
        {
            GetAllComponents();

            _playerMovementState = PlayerMovementState.Run;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void GetAllComponents()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerInputHandler = new PlayerInputHandler();
            _playerWorldStateHandler = new PlayerWorldStateHandler();
            
            _playerInputHandler.Init();
            _playerWorldStateHandler.Init(_settings, _groundCheckTransform);

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _playerInputHandler.Jump += OnJumpInput;
        }

        private void UnSubscribe()
        {
            _playerInputHandler.Jump -= OnJumpInput;
            
            _playerInputHandler.Disable();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        { 
            var moveInput = _playerInputHandler.CurrentMovementInput;

            if (Mathf.Abs(moveInput.x) > 0.01f)
            {
                CalculateVelocity(moveInput.x * _settings.MaxMovementSpeed, _settings.MovementAcceleration);
                _lastNonZeroDirection = moveInput;
            }
            else
            {
                CalculateVelocity(0, _settings.MovementDeceleration);
            }

            _rigidbody2D.velocity = new Vector2(_currentVelocity.x, _rigidbody2D.velocity.y);
        }

        private void CalculateVelocity(float target, float increaseSpeed)
        {
            _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, target, increaseSpeed * Time.fixedDeltaTime);
        }
        
        private void OnJumpInput()
        {
            if (!_jumpSettings.IsJumping &&
                _playerWorldStateHandler.UpdateCurrentPlayerWorldState() == PlayerWorldState.Ground)
                StartJump();
        }

        private void StartJump()
        {
            _jumpSettings.IsJumping = true;
            _jumpSettings.JumpTimer = 0;

            int direction = Mathf.Abs(_playerInputHandler.CurrentMovementInput.x) < 0.01f
                ? _lastNonZeroDirection.x > 0 ? 1 : -1
                : (int)Mathf.Sign(_playerInputHandler.CurrentMovementInput.x);

            _jumpSettings.JumpStartPosition = _rigidbody2D.position;
            _jumpSettings.JumpEndPosition = _jumpSettings.JumpStartPosition +
                                            new Vector2(direction * _settings.JumpDistance, 0);

            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector2.zero;
            
            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);

            _playerMovementState = PlayerMovementState.Jump;
            _timeRetarder.SlowTime(_settings.JumpDuration, _settings.TimeCurve);
            _jumpCoroutine = StartCoroutine(JumpRoutine());
        }
        
        private IEnumerator JumpRoutine()
        {
            while (_jumpSettings.JumpTimer < _settings.JumpDuration)
            {
                _jumpSettings.JumpTimer += Time.fixedDeltaTime;
                float time = Mathf.Clamp01(_jumpSettings.JumpTimer / _settings.JumpDuration);

                float yOffset = _settings.JumpCurve.Evaluate(time) * _settings.JumpHeight;
                Vector2 currentPos = Vector2.Lerp(_jumpSettings.JumpStartPosition, _jumpSettings.JumpEndPosition, time);
                currentPos.y += yOffset;

                _rigidbody2D.MovePosition(currentPos);

                yield return new WaitForFixedUpdate();
            }

            _jumpSettings.IsJumping = false;
            _rigidbody2D.MovePosition(_jumpSettings.JumpEndPosition);
            _rigidbody2D.gravityScale = 1f;
            _playerMovementState = PlayerMovementState.Run;
        }
    }
}