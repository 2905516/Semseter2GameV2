using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathFadeManager : MonoBehaviour
{
    [Header("Fade Images (stacked on Canvas)")]
    public Image fadeImage1; // light blood
    public Image fadeImage2; // medium
    public Image fadeImage3; // heavy

    [Header("Timing (seconds) - exposed for tweaking")]
    public float fadeInDurationPerImage = 0.5f;
    public float holdDurationPerImage = 0.2f;
    public float crossFadeDuration = 0.25f;

    private Coroutine running;

    private void Awake()
    {
        // Ensure images start transparent
        ResetAllImages();
    }

    private void ResetAllImages()
    {
        SetImageAlpha(fadeImage1, 0f);
        SetImageAlpha(fadeImage2, 0f);
        SetImageAlpha(fadeImage3, 0f);
    }

    private void SetImageAlpha(Image img, float a)
    {
        if (img == null) return;
        var c = img.color;
        c.a = Mathf.Clamp01(a);
        img.color = c;
    }

    /// <summary>
    /// Starts the layered fade sequence (image1 -> image2 -> image3). Uses unscaled time so it plays regardless of timeScale.
    /// Calls onComplete when finished.
    /// </summary>
    public void PlayDeathFade(Action onComplete = null)
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(DeathFadeSequence(onComplete));
    }

    private IEnumerator DeathFadeSequence(Action onComplete)
    {
        // ensure images start transparent
        ResetAllImages();

        // Fade in image 1
        yield return StartCoroutine(FadeImage(fadeImage1, 0f, 1f, fadeInDurationPerImage));

        // hold
        yield return new WaitForSecondsRealtime(holdDurationPerImage);

        // Fade image1 -> image2 crossfade
        yield return StartCoroutine(CrossFadeImages(fadeImage1, fadeImage2, crossFadeDuration));

        // hold
        yield return new WaitForSecondsRealtime(holdDurationPerImage);

        // Crossfade image2 -> image3
        yield return StartCoroutine(CrossFadeImages(fadeImage2, fadeImage3, crossFadeDuration));

        // hold final
        yield return new WaitForSecondsRealtime(holdDurationPerImage);

        running = null;
        onComplete?.Invoke();
    }

    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        if (img == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, duration <= 0f ? 1f : t / duration);
            SetImageAlpha(img, a);
            yield return null;
        }
        SetImageAlpha(img, to);
    }

    private IEnumerator CrossFadeImages(Image outImg, Image inImg, float duration)
    {
        if (outImg == null && inImg == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = duration <= 0f ? 1f : t / duration;
            if (outImg != null) SetImageAlpha(outImg, Mathf.Lerp(1f, 0f, p));
            if (inImg != null) SetImageAlpha(inImg, Mathf.Lerp(0f, 1f, p));
            yield return null;
        }
        if (outImg != null) SetImageAlpha(outImg, 0f);
        if (inImg != null) SetImageAlpha(inImg, 1f);
    }

    /// <summary>
    /// Immediately stop any running fade and clear the images (transparent).
    /// </summary>
    public void ResetFadeImmediate()
    {
        if (running != null) StopCoroutine(running);
        running = null;
        ResetAllImages();
    }
}
