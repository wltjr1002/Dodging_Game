namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletManager : MonoBehaviour
    {
        private int maxBullets = 500;
        [SerializeField]
        private Sprite[] bulletSprites;
        public Sprite defaultBulletSprite;
        [SerializeField]
        private GameObject bulletPoolObject;
        private BulletPool bulletPool;

        #region Options
        private Sprite bulletSprite;
        private Vector3 _parentPosition;
        private Vector3 _startPosition;
        private Vector3 _direction;
        private float _speed;
        private BulletType _bulletType;
        #endregion


        public void Initialize()
        {
            bulletPool = Instantiate(bulletPoolObject, Vector3.zero, Quaternion.identity, transform).GetComponent<BulletPool>();
            bulletPool.Initialize(maxBullets);
        }

        public bool isBulletInPosition(Vector2 position)
        {
            return bulletPool.isBulletInPosition(position);
        }
        public void DestroyAllBullet()
        {
            bulletPool.DestroyAllBullet();
        }

        public void MakeBullet(float option = 0)
        {
            Bullet newBullet = bulletPool.GetObject().GetComponent<Bullet>();
            newBullet.transform.localPosition = _startPosition;
            newBullet.Initialize(_bulletType, _startPosition, _direction, _speed, option);
            newBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        }

        public void MakeCircleBullet(float radius, int n, float option = 0)
        {
            MakeCircleBullet(radius, n, 0, 0, 360, option);
        }

        public void MakeCircleBullet(float radius, int n, float angleOffset, float angleStart, float angleEnd, float option = 0)
        {
            float firstAngle = angleOffset + angleStart;
            float angleInterval = (angleEnd - angleStart) * 1 / (n - 1);
            for (int i = 0; i < n; i++)
            {
                float angle = Mathf.Deg2Rad * (firstAngle + angleInterval * i);
                _direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                _startPosition = _parentPosition + _direction * radius;
                MakeBullet(option);
            }
        }

        void SetBulletSprite(int spriteIdx)
        {
            if (bulletSprites.Length == 0) bulletSprite = defaultBulletSprite;
            bulletSprite = bulletSprites[spriteIdx % bulletSprites.Length];
        }
        void SetBulletType(BulletType bulletType)
        {
            _bulletType = bulletType;
        }
        void SetBulletPosition(Vector3 position)
        {
            _parentPosition = position;
            _startPosition = position;
        }
        void SetBulletDirection(Vector3 direction)
        {
            _direction = direction;
        }
        void SetBulletSpeed(float speed)
        {
            _speed = speed;
        }
        public void SetBulletProperty(BulletType bulletType, int spriteIndex, Vector3 position, Vector3 direction, float speed)
        {
            if (bulletSprites.Length == 0) bulletSprite = defaultBulletSprite;
            bulletSprite = bulletSprites[spriteIndex % bulletSprites.Length];
            _bulletType = bulletType;
            _parentPosition = position;
            _startPosition = position;
            _direction = direction;
            _speed = speed;
        }
    }

}
