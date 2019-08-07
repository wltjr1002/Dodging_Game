namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Enemy01 : Enemy
    {
        void Start()
        {
            StartCoroutine(MainCorutine());
        }
        private IEnumerator MainCorutine()
        {
            BulletType bulletType = BulletType.Linear;
            float theta = 0;

            Camera camera = FindObjectOfType<Camera>();
            float minY = camera.ScreenToWorldPoint(new Vector3(0,-1, 0)).y;
            Debug.Log(minY);
            while (transform.localPosition.y > minY)
            {
                yield return null;
                Vector3 direction = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                bulletManager.ChangeBulletSprite(2);
                bulletManager.MakeBullet(transform.localPosition,direction,1,bulletType);
                transform.localPosition += Vector3.down * Time.deltaTime * 3;
                theta += 1;
            }
            Destroy(gameObject);
        }
    }
}