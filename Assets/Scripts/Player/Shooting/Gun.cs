using System;
using NaughtyAttributes;
using Player.Input;
using Player.ScriptableObjects;
using UnityEngine;

namespace Player.Shooting
{
    public class Gun : MonoBehaviour
    {
        [Expandable]
        [SerializeField] private PlayerShooterSettings _settings;
        [SerializeField] private Transform _firePoint;
        
        private PlayerInputHandler _input;
        private float _lastShootTime;

        private void OnEnable()
        {
            _input = new PlayerInputHandler();
            
            _input.Shoot += OnShoot;
        }

        private void OnDisable()
        {
            _input.Shoot -= OnShoot;
            
            _input.Disable();
        }

        private void OnShoot()
        {
            if (Time.time - _lastShootTime >= _settings.ShootDelay)
            {
                Shoot();
                _lastShootTime = Time.time;
            }
        }
        
        private void Shoot()
        {
            if (_settings.Bullet == null || _firePoint == null) return;

            Bullet bullet = Instantiate(_settings.Bullet, _firePoint.position, _firePoint.rotation);
            bullet.StartMovement(_settings.BulletSpeed, _firePoint);
        }

        private void Update()
        {
            LookAtTarget();
        }

        private void LookAtTarget()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(_input.CurrentMousePosition);
            mouseWorldPos.z = transform.position.z;

            Vector2 direction = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}