using UnityEngine;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class Character
{
    public string name;
    public int cost;

    public SpriteLibraryAsset texture;
    public Sprite face;

    [Header("Charasteristics")]
    public float maxHealth;
    public float armor;
    public float movementSpeed;
    public float baseDamage;
    public string weapon;
}
