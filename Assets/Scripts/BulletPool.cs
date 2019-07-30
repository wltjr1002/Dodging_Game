namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject bullet;
        private List<Bullet> activeBullets;
        private List<Bullet> inactiveBullets;
        [HideInInspector]
        public bool isInitialized = false;

        public void Initialize(int numMaxObject)
        {
            activeBullets = new List<Bullet>();
            inactiveBullets = new List<Bullet>();
            StartCoroutine(InitializeCoroutine(numMaxObject));
        }
        public IEnumerator InitializeCoroutine(int numMaxObject)
        {
            isInitialized = false;

            for (int i = 0; i < numMaxObject; i++)
            {
                Bullet newBullet = Instantiate(bullet, Vector3.zero, Quaternion.identity, transform).GetComponent<Bullet>();
                newBullet.bulletPool = this;
                inactiveBullets.Add(newBullet);
                newBullet.gameObject.SetActive(false);
                yield return new WaitForFixedUpdate();
            }
            isInitialized = true;
        }

        public Bullet GetObject()
        {
            Bullet newBullet;
            if (inactiveBullets.Count > 0)
            {
                newBullet = inactiveBullets[0];
                activeBullets.Add(newBullet);
                inactiveBullets.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(bullet, Vector3.zero, Quaternion.identity, transform).GetComponent<Bullet>();
                newBullet.bulletPool = this;
                activeBullets.Add(newBullet);
            }
            newBullet.gameObject.SetActive(true);
            return newBullet;
        }

        public void GetBackObject(Bullet bullet)
        {

            if (activeBullets.Contains(bullet))
            {
                activeBullets.Remove(bullet);
                inactiveBullets.Add(bullet);
                bullet.gameObject.SetActive(false);
            }
        }

        public void DestroyAllBullet()
        {
            foreach (Bullet bullet in activeBullets)
            {
                bullet.gameObject.SetActive(false);
                inactiveBullets.Add(bullet);
            }
            activeBullets.Clear();
        }

        public bool isBulletInPosition(Vector2 position)
        {
            int bulletInPosition = activeBullets.FindIndex(
                x =>
                {
                    Vector3 bulletPos = x.transform.localPosition;
                    float dx = bulletPos.x - position.x;
                    float dy = bulletPos.y - position.y;
                    float dsSqare = dx*dx + dy*dy;
                    return dsSqare<0.01f;
                });
            return bulletInPosition != -1;
        }
    }
}
