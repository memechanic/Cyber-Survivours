using Unity.VisualScripting;
using UnityEngine;

public class AreaWeapon : Weapon
{
    [SerializeField] private GameObject prefab;
    private float reloadTime;

    void Update()
    {
        reloadTime -= Time.deltaTime;
        if (reloadTime <= 0)
        {
            reloadTime = stats[weaponLevel].reload;
            Instantiate(prefab, transform.position, transform.rotation, transform);
            
            AudioController.Instance.PlayModifiedSound(AudioController.Instance.areaWeapon);
        }
    }
}