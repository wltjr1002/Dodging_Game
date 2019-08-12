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

        // Update is called once per frame
        void Update()
        {
            /*
            if(Time.time < lastEnemySpawnedTime + 2f) return;
            else
            {
                lastEnemySpawnedTime = Time.time;
                GameObject enemy;
                if(Random.Range(1,4)<2) enemy = Instantiate(enemyObjects[1], new Vector3(0, 2, 0), Quaternion.identity, transform);
                else enemy = Instantiate(enemyObjects[0], new Vector3(Random.Range(-2f,2f), 2, 0), Quaternion.identity, transform);
                enemy.GetComponent<Enemy>().bulletManager = bulletManager;
            }
            */
        }
    }
}

