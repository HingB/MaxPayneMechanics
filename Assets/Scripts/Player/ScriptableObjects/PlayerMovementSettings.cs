using System;
using Player.Movement;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Settings/MovementSettings", fileName = "MovementSettings", order = 1)]
    public class PlayerMovementSettings : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _maxMovementSpeed;
        [SerializeField] private float _movementAcceleration;
        [SerializeField] private float _movementDeceleration;
        [Header("Jump")]
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private AnimationCurve _timeCurve;
        [SerializeField] private float _jumpDuration = 0.6f;
        [SerializeField] private float _jumpDistance = 3f;
        [SerializeField] private float _jumpHeight = 2f;
        
        [Header("GroundDetection")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius = 0.1f;

        public float MaxMovementSpeed => _maxMovementSpeed;
        public float MovementAcceleration => _movementAcceleration;
        public float MovementDeceleration => _movementDeceleration;
        public float JumpDuration => _jumpDuration;
        public float JumpDistance => _jumpDistance;
        public float JumpHeight => _jumpHeight;
        public AnimationCurve JumpCurve => _jumpCurve;
        public AnimationCurve TimeCurve => _timeCurve;
        public LayerMask GroundLayer => _groundLayer;
        public float GroundCheckRadius => _groundCheckRadius;
    }
}
