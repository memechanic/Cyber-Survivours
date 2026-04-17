using UnityEngine;

public class DamagePopUpsController : MonoBehaviour
{
    public static DamagePopUpsController Instance;
    public DamagePopUp prefab;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreatePopUp(float value, Vector2 position)
    {
        DamagePopUp popUp = Instantiate(prefab, position, transform.rotation, transform);
        popUp.SetValue(Mathf.RoundToInt(value));
    }
}
