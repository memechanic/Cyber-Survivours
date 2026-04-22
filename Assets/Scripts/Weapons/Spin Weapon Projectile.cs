using UnityEngine;

public class SpinWeaponProjectile : MonoBehaviour
{
    private SpinWeapon weapon;

    void Start()
    {
        weapon = GetComponentInParent<SpinWeaponPrefab>().weapon;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Enemy")){
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.GetDamage());
        }
    }
}