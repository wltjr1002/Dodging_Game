namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Book : MonoBehaviour
    {
        private BulletManager bulletManager;
        private Vector3 initialPosition;
        private Vector3 cameraRect;
        public bool isIdle;
        public void Initialize()
        {
            initialPosition = transform.localPosition;
            bulletManager = FindObjectOfType<BulletManager>();
            Camera camera = FindObjectOfType<Camera>();
            cameraRect = camera.ScreenToWorldPoint(new Vector3(1, 1, 0));

            isIdle = true;
        }
        public void Reset()
        {
            StopAllCoroutines();
            ResetPosition();
            isIdle = true;
        }
        public void Reset(Vector3 position)
        {
            StopAllCoroutines();
            ResetPosition(position);
            isIdle = true;
        }
        public void ResetPosition()
        {
            transform.localPosition = initialPosition;
        }
        public void ResetPosition(Vector3 position)
        {
            transform.localPosition = position;
            initialPosition = position;
        }
        public void Pattern(string name)
        {
            if (isIdle)
            {
                isIdle = false;
                StartCoroutine(name);
            }
        }
        public void Pattern(string name, object option)
        {
            if (isIdle)
            {
                isIdle = false;
                StartCoroutine(name, option);
            }
        }
        public void RandomPattern()
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    {
                        Pattern("Spread", BulletType.Random);
                        break;
                    }
                case 1:
                    {
                        Pattern("Fall");
                        break;
                    }
                case 2:
                    {
                        Pattern("Rush");
                        break;
                    }
                default: break;
            }
        }
        public IEnumerator Spread(BulletType bulletType)
        {
            bulletManager.ChangeBulletSprite((int)bulletType);
            switch (bulletType)
            {
                case BulletType.Linear:
                    {
                        //bulletManager.MakeCircleBullet(transform.localPosition, 0.1f, 12, angleOffset, 0, 360, 2, bulletType);
                        break;
                    }
                case BulletType.Homing:
                case BulletType.Random:
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            bulletManager.ChangeBulletSprite(1);
                            bulletManager.MakeCircleBullet(transform.position, 0.1f, 10, 3, bulletType);
                            yield return new WaitForSeconds(0.2f);
                        }
                        break;
                    }
                case BulletType.Spiral:
                default:
                    {
                        //bulletManager.MakeCircleBullet(transform.localPosition, 0.1f, 10, -angleOffset, 0, 360, 2, bulletType);
                        break;
                    }
            }
            yield return new WaitForSeconds(1);
            isIdle = true;
        }

        public IEnumerator Fall()
        {
            BulletType bulletType = BulletType.Linear;
            float theta = 0;
            int dtheta = Random.Range(0,2)*2-1;
            Camera camera = FindObjectOfType<Camera>();
            float minY = cameraRect.y;
            while (transform.position.y > minY)
            {
                yield return null;
                Vector3 direction = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                bulletManager.ChangeBulletSprite(2);
                bulletManager.MakeBullet(transform.position, direction, 1, bulletType);
                transform.localPosition += Vector3.down * Time.deltaTime * 3;
                theta += dtheta;
            }
            yield return new WaitForSeconds(1);
            ResetPosition();
            yield return new WaitForSeconds(1);
            isIdle = true;
        }

        public IEnumerator Rush()
        {
            int mark = Random.Range(0,2)*2-1;
            Vector3 playerPosition = FindObjectOfType<Player>().transform.position;
            Vector3 selfPosition = Vector3.Scale(cameraRect, new Vector3(0.9f*mark, 0.5f, 0));
            transform.position = selfPosition;
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 10; i++)
            {
                bulletManager.ChangeBulletSprite(0);
                bulletManager.MakeBullet(selfPosition, playerPosition - selfPosition, 2 + (i + 1) * 0.5f);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1);
            ResetPosition();
            yield return new WaitForSeconds(1);
            isIdle = true;
        }

    }
}
