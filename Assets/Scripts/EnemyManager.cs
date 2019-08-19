namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyManager : MonoBehaviour
    {
        public GameObject[] enemyObjects;
        private BulletManager bulletManager;
        private float lastEnemySpawnedTime;
        public void Initialize()
        {
            bulletManager = FindObjectOfType<BulletManager>();
        }
        void Start()
        {
            lastEnemySpawnedTime = float.MinValue;
        }

        void Update()
        {
            
        }

        public void SpawnEnemy(GameObject enemy, float delay)
        {
            StartCoroutine(SpawnEnemyCorutine(enemy,delay));
        }
        public IEnumerator SpawnEnemyCorutine(GameObject enemy, float delay)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(enemy, enemy.transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().Initialize();
        }
    }
}

