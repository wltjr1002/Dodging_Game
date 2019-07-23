namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletManager : MonoBehaviour
    {
        private int maxBullets = 200;

        [SerializeField]
        private GameObject bulletPoolObject;
        private BulletPool bulletPool;
        public void Initialize()
        {
            bulletPool = Instantiate(bulletPoolObject,Vector3.zero,Quaternion.identity,transform).GetComponent<BulletPool>();
            Debug.Log(maxBullets);
            bulletPool.Initialize(maxBullets);
        }

        public void MakeBullet()
        {
            Bullet newBullet = bulletPool.GetObject().GetComponent<Bullet>();
            newBullet.transform.localPosition = new Vector3(Random.Range(-2f,2f),Random.Range(-5f,5f),0);
            float angle = Random.Range(0,2*Mathf.PI);
            Vector3 direction = new Vector3(Mathf.Cos(angle),Mathf.Sin(angle),0);
            newBullet.Initialize(direction, 2f);
        }

        public void MakeBullet(Vector3 position, Vector3 direction)
        {
            Bullet newBullet = bulletPool.GetObject().GetComponent<Bullet>();
            newBullet.transform.localPosition = position;
            newBullet.Initialize(direction, 2f);
        }

        public void MakeCircleBullet(int n)
        {
            Vector3 circleCenter = new Vector3(Random.Range(-2f,2f),Random.Range(-5f,5f),0);
            for(int i = 0; i<n; i++)
            {
                float angle = Mathf.PI * 2 * i / n;
                Vector3 direction = new Vector3(Mathf.Cos(angle),Mathf.Sin(angle),0);
                MakeBullet(circleCenter,direction);
            }
        }

    }

}
