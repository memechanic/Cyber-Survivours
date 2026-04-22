using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLevelUpButton : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void ActivateButton(Weapon weapon)
    {
        weaponName.text = weapon.name;
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        weaponIcon.sprite = weapon.weaponImage;

        assignedWeapon = weapon;
    }

    public void SelectUpgrade(){
        if (assignedWeapon.gameObject.activeSelf == true){
            assignedWeapon.LevelUp();
        } else {
            PlayerController.Instance.ActivateWeapon(assignedWeapon);
        }

        UIController.Instance.CloseLevelUpPanel();
    }
}
