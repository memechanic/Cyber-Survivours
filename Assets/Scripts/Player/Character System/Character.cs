using UnityEngine;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class Character
{
    public string name;
    public SpriteLibraryAsset texture;

    [Header("Charasteristics")]
    public float maxHealth;
    public float movementSpeed;
}
