using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    private float floatSpeed;

    void Start()
    {
        floatSpeed = Random.Range(0.5f, 1f);
        Destroy(gameObject, 1);
    }

    void Update()
    {
        transform.position += floatSpeed * Time.deltaTime * Vector3.up;
    }
    
    public void SetValue(int value)
    {
        damageText.text = value.ToString();
    }
}
