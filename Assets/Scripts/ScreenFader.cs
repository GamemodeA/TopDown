using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    /* Allows any script to reach the fader without a scene reference */
    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false; // Don't block inputs while invisible
    }

    public IEnumerator FadeOut() { yield return Fade(0f, 1f); }

    public IEnumerator FadeIn() { yield return Fade(1f, 0f); }

    private IEnumerator Fade(float from, float to)
    {
        canvasGroup.blocksRaycasts = true; // Cover screen for the whole fade
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;              // Wait a frame, then loop
        }
        canvasGroup.alpha = to;
        
        // Stay blocking only if it ended completely opaque
        canvasGroup.blocksRaycasts = to > 0.5f;
    }
}