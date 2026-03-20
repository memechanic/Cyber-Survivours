using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnInterval;
        public int enemiesPerWave;
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
        if (PlayerController.Instance.gameObject.activeSelf)
        {
            currentWave = waves[waveNumber];

            currentWave.spawnTimer += Time.deltaTime;
            if (currentWave.spawnTimer > currentWave.spawnInterval)
            {
                currentWave.spawnTimer = 0;
                StartCoroutine("SpawnEnemy");
            }
            if (currentWave.spawnedEnemyCount >= currentWave.enemiesPerWave)
            {
                currentWave.spawnedEnemyCount = 0;
                if (currentWave.spawnInterval >= 0.3f)
                {
                    currentWave.spawnInterval *= 0.9f;
                }
                waveNumber++;
            }
            if (waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }
    }

    private  System.Collections.IEnumerator SpawnEnemy()
    {
        Vector2 spawnPosition = RandomSpawnPoint();
        GameObject spawn = Instantiate(spawnPointPrefab, spawnPosition, transform.rotation);
        float spawnDelay = spawn.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(currentWave.enemyPrefab, spawnPosition, transform.rotation);
        currentWave.spawnedEnemyCount++;
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;

        spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);

        return spawnPoint;
    }
}
