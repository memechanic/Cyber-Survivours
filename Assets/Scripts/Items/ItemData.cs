using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Settings/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Base settings")]
    public string itemName;
    public GameObject itemPrefab;
    public enum EffectType { Exp, Heal, Coin }
    public EffectType effectType;
    [Tooltip("Time in seconds before the item disappears after being spawned in the world")]
    public float itemLifetime = 100f;

    [Tooltip("Value of the item effect. For Coin - amount, for Heal - percentage, for Exp - amount")]
    public float value = 0;
}
