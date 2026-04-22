using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public List<WeaponStats> stats;
    public Sprite weaponImage;
    public string basicDescription;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
            if (weaponLevel >= stats.Count - 1)
            {
                PlayerController.Instance.maxLevelWeapons.Add(this);
                PlayerController.Instance.activeWeapons.Remove(this);
            }
        }
    }
    public float GetDamage()
    {
        return stats[weaponLevel].damage + PlayerController.Instance.GetBaseDamage();
    }
}

[System.Serializable]
public class WeaponStats {
    public float reload;
    public float duration;
    public float damage;
    public float range;
    public float attackSpeed;
    public float amount;

    public string description;
}