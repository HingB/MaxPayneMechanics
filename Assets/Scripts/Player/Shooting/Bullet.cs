using System;
using System.Collections;
using UnityEngine;
using World;

namespace Player.Shooting
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 3f;
        [SerializeField] private float _decelerationTime = 0.2f;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _initialVelocity;
        private Vector2 _targetVelocity;

        public void StartMovement(float speed, Transform firePoint)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _rigidbody2D.velocity = firePoint.right * (speed * 2);
            
            _initialVelocity = _rigidbody2D.velocity;
            _targetVelocity = _initialVelocity * 0.5f;
            

            StartCoroutine(SlowDownCoroutine());
            Destroy(gameObject, _lifeTime);
        }

        private IEnumerator SlowDownCoroutine()
        {
            float elapsed = 0f;

            while (elapsed < _decelerationTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _decelerationTime;

                _rigidbody2D.velocity = Vector2.Lerp(_initialVelocity, _targetVelocity, t);

                yield return null;
            }

            _rigidbody2D.velocity = _targetVelocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
                Destroy(obstacle.gameObject);
            
            Destroy(gameObject);
        }
    }
}