using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    private Button button;
    private Image image;

    [Header("Visual Settings")]
    public float hoverScale = 1.08f;
    public float clickScale = 0.95f;
    public float transitionSpeed = 10f;
    public Color hoverTint = new Color(1f, 0.94f, 0.8f); // warm parchment tone
    public Color normalTint = Color.white;
    public Color clickTint = new Color(0.9f, 0.85f, 0.7f);

    private bool isHovering = false;
    private bool isClicking = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Skip visual updates if button is disabled
        if (!button.interactable) return;

        // Smooth scale transitions
        Vector3 targetScale = originalScale;

        if (isClicking)
            targetScale = originalScale * clickScale;
        else if (isHovering)
            targetScale = originalScale * hoverScale;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);

        // Smooth color transitions
        if (image != null)
        {
            Color targetColor = normalTint;
            if (isClicking)
                targetColor = clickTint;
            else if (isHovering)
                targetColor = hoverTint;

            image.color = Color.Lerp(image.color, targetColor, Time.unscaledDeltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;
    }
}
