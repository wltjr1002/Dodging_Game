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
        private Sprite bulletSprite;
        public Sprite defaultBulletSprite;
        [SerializeField]
        private GameObject bulletPoolObject;
        private BulletPool bulletPool;
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

        public void MakeBullet(Vector3 position, Vector3 direction, float speed, BulletType bulletType = BulletType.Linear)
        {
            Bullet newBullet = bulletPool.GetObject().GetComponent<Bullet>();
            newBullet.transform.localPosition = position;
            newBullet.Initialize(bulletType, position, direction, speed);
            newBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        }

        public void MakeBullet(Vector3 position, Vector3 initPosition, Vector3 direction, float speed, BulletType bulletType = BulletType.Linear)
        {
            Bullet newBullet = bulletPool.GetObject().GetComponent<Bullet>();
            newBullet.transform.localPosition = position;
            newBullet.Initialize(bulletType, initPosition, direction, speed);
            newBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        }

        public void MakeCircleBullet(Vector3 position, float radius, int n, float speed = 2f, BulletType bulletType = BulletType.Linear)
        {
            for (int i = 0; i < n; i++)
            {
                float angle = Mathf.PI * 2 * i / n;
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                MakeBullet(position, position + direction * radius, direction, speed, bulletType);
            }
        }

        public void MakeCircleBullet(Vector3 position, float radius, int n, float angleOffset, float angleStart = 0, float angleEnd = 360, float speed = 2f, BulletType bulletType = BulletType.Linear)
        {
            for (int i = 0; i < n; i++)
            {
                float angle = Mathf.Deg2Rad*(angleOffset + angleStart + (angleEnd - angleStart) * i / (n-1));
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                MakeBullet(position, position + direction * radius, direction, speed, bulletType);
            }
        }

        public void ChangeBulletSprite(int spriteIdx)
        {
            if (bulletSprites.Length == 0) bulletSprite = defaultBulletSprite;
            bulletSprite = bulletSprites[spriteIdx%bulletSprites.Length];
        }

    }

}
