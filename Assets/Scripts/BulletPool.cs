namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject bullet;
        private List<GameObject> activeBullets;
        private List<GameObject> inactiveBullets;
        [HideInInspector]
        public bool isInitialized = false;

        public void Initialize(int numMaxObject)
        {
            activeBullets = new List<GameObject>();
            inactiveBullets = new List<GameObject>();
            StartCoroutine(InitializeCoroutine(numMaxObject));
        }
        public IEnumerator InitializeCoroutine(int numMaxObject)
        {
            isInitialized = false;
            
            for (int i = 0; i < numMaxObject; i++)
            {
                GameObject newBullet = Instantiate(bullet, Vector3.zero, Quaternion.identity, transform);
                inactiveBullets.Add(newBullet);
                newBullet.SetActive(false);
                yield return new WaitForFixedUpdate();
            }
            isInitialized = true;
        }
        
        public GameObject GetObject()
        {
            GameObject newBullet;
            if(inactiveBullets.Count>0)
            {
                newBullet = inactiveBullets[0];
                activeBullets.Add(newBullet);
                inactiveBullets.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(bullet, Vector3.zero, Quaternion.identity, transform);
                activeBullets.Add(newBullet);
            }
            newBullet.SetActive(true);
            return newBullet;
        }
        
        public void GetBackObject(GameObject bullet)
        {
            bullet.SetActive(false);
            if(activeBullets.Contains(bullet))
            {
                activeBullets.Remove(bullet);
                inactiveBullets.Add(bullet);
            }
        }

        public bool isBulletInPosition(Vector2 position)
        {
            return false;
        }
    }
}
