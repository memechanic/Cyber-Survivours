using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    void Start()
    {
        // destroy the item after its lifetime expires
        if (itemData != null && gameObject != null)
        {
            Destroy(gameObject, itemData.itemLifetime);
        }
    }
}
