using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject enemyPrefab;
        public float spawnInterval;
        public int enemiesPerWave;
        public float enemiesMultiplier = 1;
        [Space]
        public int spawnedEnemyCount;
        public float spawnTimer;
    }

    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;
    
    [SerializeField] private GameObject spawnPointPrefab;

    [SerializeField] private List<Wave> waves;

    [SerializeField] private int waveNumber;
    private Wave currentWave;

    void Update()
    {
        if (!PlayerController.Instance.gameObject.activeSelf)
            return;

        if (waves == null || waves.Count == 0)
            return;

        if (waveNumber >= waves.Count)
        {
            PlayerController.Instance.UpdateExpMulti();
            waveNumber = 0;
        }

        currentWave = waves[waveNumber];

        GameObject enemyPrefab = currentWave.enemyPrefab;
        float spawnMult = currentWave.enemiesMultiplier;

        currentWave.spawnTimer += Time.deltaTime;
        if (currentWave.spawnTimer > currentWave.spawnInterval)
        {
            currentWave.spawnTimer = 0;
            Vector2 spawnPosition = RandomSpawnPoint();

            Enemy.EnemyType enemyType = enemyPrefab.GetComponent<Enemy>().enemyType;

            switch (enemyType)
            {
                case Enemy.EnemyType.Single:
                    StartCoroutine(SpawnEnemy(spawnPosition, enemyPrefab, spawnMult));
                    break;
                case Enemy.EnemyType.Group:
                    SpawnEnemyGroup(spawnPosition, enemyPrefab, spawnMult);
                    break;
            }

            currentWave.spawnedEnemyCount++;
        }
        if (currentWave.spawnedEnemyCount >= currentWave.enemiesPerWave)
        {
            currentWave.spawnedEnemyCount = 0;
            if (currentWave.spawnInterval >= 0.15f)
            {
                currentWave.enemiesPerWave += 5;
                currentWave.enemiesMultiplier *= 1.2f;
                currentWave.spawnInterval *= 0.9f;
            }

            waveNumber++;
        }
    }

    private System.Collections.IEnumerator SpawnEnemy(Vector2 spawnPosition, GameObject enemyPrefab, float mult)
    {
        GameObject spawn = Instantiate(spawnPointPrefab, spawnPosition, transform.rotation);
        float spawnDelay = spawn.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(spawnDelay);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, transform.rotation);
        ApplyEnemyMultiplier(enemy, mult);
    }

    private static void ApplyEnemyMultiplier(GameObject enemy, float mult)
    {
        if (mult == 1f)
            return;

        Enemy e = enemy.GetComponent<Enemy>();
        if (e == null)
            return;

        e.maxHealth *= mult;
        e.damage *= mult;
        e.moveSpeed *= mult;
    }

    private void SpawnEnemyGroup(Vector2 spawnPosition, GameObject enemyPrefab, float mult)
    {
        Enemy groupSettings = enemyPrefab.GetComponent<Enemy>();
        int minEnimies = groupSettings.minEnemies;
        int maxEnimies = groupSettings.maxEnemies;
        float groupRadius = groupSettings.groupRadius;
        int enemyCount = Random.Range(minEnimies, maxEnimies + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 enemyPosition = spawnPosition + Random.insideUnitCircle * groupRadius;
            StartCoroutine(SpawnEnemy(enemyPosition, enemyPrefab, mult));
        }
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // Up
                spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
                spawnPoint.y = maxPos.position.y;
                break;
            case 1: // Right
                spawnPoint.x = maxPos.position.x;
                spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
                break;
            case 2: // Down
                spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
                spawnPoint.y = minPos.position.y;
                break;
            case 3: // Left
                spawnPoint.x = minPos.position.x;
                spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
                break;
            default: // default Up
                spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
                spawnPoint.y = maxPos.position.y;
                break;
        }
        return spawnPoint;
    }
}
