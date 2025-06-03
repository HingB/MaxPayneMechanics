using Player.Shooting;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Settings/ShooterSettings")]
    public class PlayerShooterSettings : ScriptableObject
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private float _shootDelay;
        [SerializeField] private float _bulletSpeed;

        public Bullet Bullet => _bullet;
        public float ShootDelay => _shootDelay;
        public float BulletSpeed => _bulletSpeed;
    }
}