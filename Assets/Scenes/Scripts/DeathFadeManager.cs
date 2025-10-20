using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathFadeManager : MonoBehaviour
{
    [Header("Fade Layers (bottom → top)")]
    public List<Image> fadeImages = new List<Image>();

    [Header("Timing Settings")]
    [Range(0.1f, 3f)] public float fadeInDurationPerImage = 0.8f;
    [Range(0f, 2f)] public float holdDurationPerImage = 0.4f;
    [Range(0f, 2f)] public float crossFadeDuration = 0.35f;

    private bool isFading = false;

    private void Awake()
    {
        ResetFadeImmediate();
    }

    public void ResetFadeImmediate()
    {
        StopAllCoroutines();
        isFading = false;
        foreach (var img in fadeImages)
        {
            if (img == null) continue;
            Color c = img.color;
            c.a = 0f;
            img.color = c;
            img.gameObject.SetActive(false);
        }
    }

    public IEnumerator PlayDeathFadeRoutine()
    {
        isFading = true;

        for (int i = 0; i < fadeImages.Count; i++)
        {
            Image img = fadeImages[i];
            if (img == null) continue;

            img.gameObject.SetActive(true);
            float targetAlpha = Mathf.Clamp01(0.6f + i * 0.25f);
            yield return FadeImage(img, 0f, targetAlpha, fadeInDurationPerImage);

            yield return new WaitForSecondsRealtime(holdDurationPerImage);

            if (i < fadeImages.Count - 1)
                yield return FadeImage(img, img.color.a, 0f, crossFadeDuration);
        }

        // Fade out last image properly
        if (fadeImages.Count > 0)
        {
            Image top = fadeImages[fadeImages.Count - 1];
            if (top != null)
            {
                yield return FadeImage(top, top.color.a, 0f, crossFadeDuration);
                top.gameObject.SetActive(false);
            }
        }

        isFading = false;
    }

    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        if (img == null) yield break;

        float t = 0f;
        Color c = img.color;
        c.a = from;
        img.color = c;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / duration);
            c.a = Mathf.Lerp(from, to, p);
            img.color = c;
            yield return null;
        }

        c.a = to;
        img.color = c;

        if (to <= 0.01f)
            img.gameObject.SetActive(false);
    }
}