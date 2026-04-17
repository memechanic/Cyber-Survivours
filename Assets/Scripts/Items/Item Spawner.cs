using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    public List<Item> items;

    public int maxItemsOnMap;
    public float itemSpawnDelay;
    private float spawnDelay;

    void Start()
    {
        spawnDelay = itemSpawnDelay;
    }
    // Update is called once per frame
    void Update()
    {
        spawnDelay -= Time.deltaTime;
        if (spawnDelay <= 0)
        {
            spawnDelay = itemSpawnDelay;
            Item item = items[Random.Range(0, items.Count)];
            Vector2 spawnPoint = RandomSpawnPoint();
            SpawnItem(item, spawnPoint);
        }
    }
    
    public void SpawnItem(Item item, Vector2 spawnPoint)
    {
        Instantiate(item, spawnPoint, Quaternion.identity);
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
