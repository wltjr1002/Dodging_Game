namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        private Vector3 _direction;
        private float _speed;
        public void Initialize(Vector3 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }
        public void Initialize(Vector3 position, Vector3 playerPosition, Movement movement)
        {

        }
        void Update()
        {
            Vector3 position = transform.localPosition;
            Vector3 newPosition = position + _direction * (_speed * Time.deltaTime);
            if (Mathf.Abs(newPosition.x) > 5 || Mathf.Abs(newPosition.y) > 10)
            {
                FindObjectOfType<BulletPool>().GetBackObject(gameObject);
                return;
            }
            transform.localPosition = newPosition;

        }
    }
}