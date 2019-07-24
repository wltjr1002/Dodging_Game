namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Enemy : MonoBehaviour
    {
        public BulletManager bulletManager;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(MainCorutine());
        }
        void ShootBullet(BulletType bulletType, float angleOffset)
        {
            bulletManager.ChangeBulletSprite((int)bulletType);
            switch (bulletType)
            {
                case BulletType.Linear:
                    {
                        bulletManager.MakeCircleBullet(transform.localPosition, 0.1f, 36, angleOffset, 0, 360, 2, bulletType);
                        break;
                    }
                case BulletType.Homing:
                case BulletType.Random:
                    {
                        bulletManager.MakeCircleBullet(transform.localPosition, 0.1f, 30, 3, bulletType);
                        break;
                    }
                case BulletType.Spiral:
                default:
                    {
                        bulletManager.MakeCircleBullet(transform.localPosition, 0.1f, 30, -angleOffset, 0, 360, 1, bulletType);
                        break;
                    }
            }

        }

        private IEnumerator MainCorutine()
        {
            BulletType bulletType = (BulletType)Random.Range(0, 3);
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                ShootBullet(bulletType, 5 * i);
            }
            Destroy(gameObject);
        }
    }

}
