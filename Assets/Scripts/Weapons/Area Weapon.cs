using UnityEngine;

public class AreaWeapon : MonoBehaviour
{
    [Header("Main settings")]
    [SerializeField] private GameObject prefab;
    public float damage = 1f;
    public float range = 1f;
    public float reloadTime = 1f;
    public float cooldown = 2f;
    public float duration = 1f;

    [Header("Utility")]
    public float growDelta = 0.2f;
    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            spawnTimer = cooldown;
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}
