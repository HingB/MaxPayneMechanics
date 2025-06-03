using Player.Enums;
using Player.ScriptableObjects;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerWorldStateHandler
    {
        private PlayerMovementSettings _settings;
        private PlayerWorldState _playerWorldState;
        private Transform _groundCheckTransform;

        public void Init(PlayerMovementSettings playerMovementSettings, Transform groundCheckTransform)
        {
            _settings = playerMovementSettings;
            _groundCheckTransform = groundCheckTransform;
        }

        public PlayerWorldState UpdateCurrentPlayerWorldState()
        {
            if (Physics2D.OverlapCircle(_groundCheckTransform.position, _settings.GroundCheckRadius, _settings.GroundLayer))
            {
                _playerWorldState = PlayerWorldState.Ground;
            }
            else
            {
                _playerWorldState = PlayerWorldState.Air;
            }

            return _playerWorldState;
        }
    }
}