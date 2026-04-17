using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarContoller : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] private GameObject barObject;
    [SerializeField] private TMP_Text barText;
    [SerializeField] private Gradient barGradient;
    private RectTransform barRect;
    private Image barImage;
    private float barWidth;
    [Space]

    [SerializeField] private float animDuration;
    [SerializeField] private AnimationCurve animationCurve;
    private Coroutine animCoroutine;

    
    void Awake()
    {
        barRect = barObject.GetComponent<RectTransform>();
        barImage = barObject.GetComponent<Image>();
        barWidth = barRect.rect.width;
    }
    
    public void UpdateBarValue(float maxValue, float curValue)
    {
        if (player == null)
        {
            player = PlayerController.Instance;
        }
        if (player == null) return;

        float ratio = curValue / maxValue;
        float offset = barWidth * (1 - ratio);

        if(animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }

        animCoroutine = StartCoroutine(AnimateBar(offset, ratio));
    
        barText.text = $"{curValue} / {maxValue}";
    }

    private IEnumerator AnimateBar(float offset, float ratio)
    {
        float time = 0f;

        Vector2 startPos = barRect.anchoredPosition;
        Color startColor = barImage.color;

        Vector2 targetPos = new(-offset, 0);
        Color targetColor = barGradient.Evaluate(ratio);

        while (time < animDuration)
        {
            time += Time.deltaTime;
            float t = time / animDuration;
            float smoothT = animationCurve.Evaluate(t);

            barRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, smoothT);
            barImage.color = Color.Lerp(startColor, targetColor, smoothT);
            yield return null;
        }

        barRect.anchoredPosition = targetPos;
        barImage.color = targetColor;
    }
}
