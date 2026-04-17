using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D item)
    {
        if(item.gameObject.CompareTag("Item"))
        {
            Item itemComponent = item.gameObject.GetComponent<Item>();
            if (itemComponent != null)
            {
                // Apply the item effect to the player
                playerController.ApplyItemEffect(itemComponent.itemData);
                
                // Destroy the item after collecting it
                Destroy(item.gameObject);
            }
        }
    }
}
