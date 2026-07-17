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

        [Header("Settings")]
        [SerializeField] private int totalEnemies = 10;
        [SerializeField] private float spawnInterval = 3f;

        private int spawnedEnemies;

        private void Start()
        {
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
                Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation);

                spawnedEnemies++;

                // Wait before next spawn
                if (spawnedEnemies < totalEnemies)
                    yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}