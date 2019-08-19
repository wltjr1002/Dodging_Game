namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Book : MonoBehaviour
    {
        private BulletManager bulletManager;
        public Vector3 initialPosition;
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
        public void Update()
        {
            if(isIdle) ResetPosition();
        }
        public void Reset()
        {
            StopAllCoroutines();
            ResetPosition();
            isIdle = true;
        }
        public void ResetPosition()
        {
            transform.localPosition = initialPosition;
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
            Debug.Log("BOOK INIT");
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
            Camera camera = FindObjectOfType<Camera>();
            transform.position = new Vector3(Random.Range(-1*cameraRect.x, cameraRect.x),transform.position.y,0);
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
                            bulletManager.MakeCircleBullet(transform.position, 0.1f, 20, 1, bulletType);
                            yield return new WaitForSeconds(0.4f);
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
            yield return new WaitForSeconds(3);
            isIdle = true;
        }

        public IEnumerator Fall()
        {
            BulletType bulletType = BulletType.Linear;
            float theta = 0;
            int dtheta = Random.Range(0,2)*2-1;
            Camera camera = FindObjectOfType<Camera>();
            float minY = cameraRect.y;
            transform.position = new Vector3(Random.Range(-1*cameraRect.x, cameraRect.x),transform.position.y,0);
            while (transform.position.y > minY - 0.5)
            {
                yield return new WaitForSeconds(0.05f);
                Vector3 direction = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                bulletManager.ChangeBulletSprite(2);
                bulletManager.MakeBullet(transform.position, direction, 1, bulletType);
                transform.localPosition += Vector3.down * Time.deltaTime;
                theta += dtheta * 2;
            }
            yield return new WaitForSeconds(3);
            ResetPosition();
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
                bulletManager.MakeBullet(selfPosition, playerPosition - selfPosition, 3 + (i + 1) * 0.5f);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(3);
            ResetPosition();
            isIdle = true;
        }

    }
}
