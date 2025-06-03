using UnityEngine;

namespace Player.Structs
{
    [System.Serializable]
    public struct JumpSettings
    {
        public bool IsJumping { get; set; }
        public Vector2 JumpStartPosition { get; set; }
        public Vector2 JumpEndPosition { get; set; }
        
        public float JumpTimer { get; set; }
       
    }
}