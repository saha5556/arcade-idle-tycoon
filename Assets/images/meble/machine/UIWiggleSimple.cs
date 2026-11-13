using UnityEngine;

public class UIWiggleSimple : MonoBehaviour
{
    [Header("Settings")]
    public float waitTime = 3f;
    public float actionDuration = 1.5f;
    public float maxAngle = 25f;
    public float pulseAmount = 0.1f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private float timer = 0f;
    private bool isWiggling = false;
    private float wiggleTimer = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalScale = rectTransform.localScale;
            originalRotation = rectTransform.localRotation;
        }
    }

    void Update()
    {
        if (rectTransform == null) return;

        if (!isWiggling)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                isWiggling = true;
                wiggleTimer = 0f;
                timer = 0f;
            }
        }
        else
        {
            wiggleTimer += Time.deltaTime;
            if (wiggleTimer < actionDuration)
            {
                float progress = wiggleTimer / actionDuration;

                float smoothProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
                float currentAngleMultiplier = Mathf.Lerp(1f, 0f, Mathf.Pow(progress, 3f));

                float angle = Mathf.Sin(progress * Mathf.PI * 4f) * maxAngle * currentAngleMultiplier;
                rectTransform.localRotation = originalRotation * Quaternion.Euler(0, 0, angle);

                float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI * 6f) * pulseAmount * currentAngleMultiplier;
                rectTransform.localScale = originalScale * scaleFactor;
            }
            else
            {
                rectTransform.localRotation = originalRotation;
                rectTransform.localScale = originalScale;
                isWiggling = false;
            }
        }
    }
}