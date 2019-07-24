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
        void ShootBullet()
        {
            bulletManager.MakeCircleBullet(transform.localPosition, 10);
        }

        private IEnumerator MainCorutine()
        {
            yield return new WaitForSeconds(1f);
            ShootBullet();
            Destroy(gameObject);
        }
    }

}
