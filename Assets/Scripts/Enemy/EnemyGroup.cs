using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int minEnimies;
    [SerializeField] private int maxEnimies;
    [SerializeField] private float groupRadius;

    void Start()
    {
        int enemyCount = Random.Range(minEnimies, maxEnimies + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * groupRadius;
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
        
        Destroy(gameObject, 0.1f);
    }

}
