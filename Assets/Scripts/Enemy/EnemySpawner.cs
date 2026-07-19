using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform spawnPoint;

        [Header("Spawn Effect")]
        [SerializeField] private GameObject spawnEffect;
        [SerializeField] private float effectDuration = 1f;

        [Header("Spawn Settings")]
        [SerializeField] private int totalEnemies = 10;
        [SerializeField] private float spawnInterval = 3f;

        [Header("Enable When All Enemies Die")]
        [Tooltip("All these objects will be enabled after the last enemy dies.")]
        [SerializeField] private GameObject[] objectsToEnable;

        private int spawnedEnemies;
        private int aliveEnemies;

        private void Start()
        {
            // Hide all objects at the beginning
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (spawnedEnemies < totalEnemies)
            {
                // Spawn Effect
                if (spawnEffect != null)
                {
                    GameObject effect = Instantiate(
                        spawnEffect,
                        spawnPoint.position,
                        spawnPoint.rotation);

                    Destroy(effect, effectDuration);
                }

                // Wait for effect
                yield return new WaitForSeconds(effectDuration);

                // Spawn Enemy
                GameObject enemy = Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation);

                // Give Health reference to this spawner
                Health health = enemy.GetComponent<Health>();

                if (health != null)
                    health.SetSpawner(this);

                spawnedEnemies++;
                aliveEnemies++;

                // Wait before next spawn
                if (spawnedEnemies < totalEnemies)
                    yield return new WaitForSeconds(spawnInterval);
            }
        }

        public void EnemyKilled()
        {
            aliveEnemies--;

            Debug.Log($"Enemy Killed! Remaining: {aliveEnemies}");

            if (spawnedEnemies >= totalEnemies && aliveEnemies <= 0)
            {
                Debug.Log("All Enemies Defeated!");

                foreach (GameObject obj in objectsToEnable)
                {
                    if (obj != null)
                        obj.SetActive(true);
                }
            }
        }
    }
}