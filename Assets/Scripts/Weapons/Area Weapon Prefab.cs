using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaWeaponPrefab : MonoBehaviour
{
    private GameManager gameManager;
    public AreaWeapon weapon;
    private Vector2 targetSize;
    private float growDelta;
    private float duration;
    private float counter;

    private List<Enemy> enemiesInRange;

    void Start()
    {
        gameManager = GameManager.Instance;

        weapon = GetComponentInParent<AreaWeapon>();
        growDelta = weapon.growDelta;
        duration = weapon.duration;
        counter = weapon.reloadTime;

        enemiesInRange = new();

        targetSize = Vector2.one * weapon.range;
        transform.localScale = Vector2.zero;

        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isPaused) return;
        transform.localScale = Vector2.MoveTowards(transform.localScale, targetSize, growDelta);
        
        counter -= Time.deltaTime;
        if(counter <= 0)
        {
            counter = weapon.reloadTime;
            if(enemiesInRange.Count != 0)
            {
                for(int i = 0; i < enemiesInRange.Count; i++)
                {
                    enemiesInRange[i].TakeDamage(weapon.damage);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.GetComponent<Enemy>());
        }
    }
}
