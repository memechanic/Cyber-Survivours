using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPrefab : MonoBehaviour
{
    private GameManager gameManager;
    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;
    public List<Enemy> enemiesInRange;

    private float counter;
    private float scaleSpeed;

    void Start()
    {
        gameManager = GameManager.Instance;

        weapon = GetComponentInParent<AreaWeapon>();
        targetSize = Vector2.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector2.zero;
        timer = weapon.stats[weapon.weaponLevel].duration;

        scaleSpeed = weapon.stats[weapon.weaponLevel].range / (weapon.stats[weapon.weaponLevel].duration * 0.3f); // 30% of the duration
    }

    void Update() {
        GrowAndSrink();
    }

    public void GrowAndSrink()
    {
        if(gameManager.isPaused) return;
        // grow and shrink towards targetSize
        transform.localScale = Vector2.MoveTowards(transform.localScale, targetSize, scaleSpeed * Time.deltaTime);
        // shrink and only then destroy
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector2.zero;
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);
            }
        }
        // periodic damage
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = weapon.stats[weapon.weaponLevel].attackSpeed;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].TakeDamage(weapon.GetDamage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Enemy")){
            enemiesInRange.Add(collider.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if (collider.CompareTag("Enemy")){
            enemiesInRange.Remove(collider.GetComponent<Enemy>());
        }
    }
}