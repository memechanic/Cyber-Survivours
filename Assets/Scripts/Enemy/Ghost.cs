using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ghost : MonoBehaviour
{
    [Header("Fade By Distance")]
    [SerializeField] private Transform target;
    [SerializeField] private float fullyVisibleDistance = 5f;
    [SerializeField] private float fullyInvisibleDistance = 10f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (target == null && PlayerController.Instance != null)
            target = PlayerController.Instance.transform;

        if (target == null)
            return;

        float distance = Vector2.Distance(transform.position, target.position);
        float alpha = Mathf.InverseLerp(fullyInvisibleDistance, fullyVisibleDistance, distance);

        Color color = spriteRenderer.color;
        color.a = Mathf.Clamp01(alpha);
        spriteRenderer.color = color;
    }
}
