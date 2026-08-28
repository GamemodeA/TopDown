using System.Collections;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeDuration = 0.15f;
    public bool showing { get; private set; }
    private string[] lines;
    private int lineIndex;
    private bool justOpened;

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    /* Click advances dialogue if it's open
     * The click that opens dialogue won't advance it */
    void Update()
    {
        if (!showing || justOpened) return;
        if (DialogueInput.WasInteractPressed()) Advance();
    }
    void LateUpdate() { justOpened = false; }

    public void ShowDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        lines = dialogueLines;
        lineIndex = 0;
        text.text = lines[lineIndex];

        showing = true;
        justOpened = true;
        StartCoroutine(Fade(canvasGroup.alpha, 1f));
    }

    private void Advance()
    {
        lineIndex++;
        if (lineIndex >= lines.Length)
        {
            Hide();
            return;
        }
        text.text = lines[lineIndex];
    }

    private void Hide()
    {
        showing = false;
        StartCoroutine(Fade(canvasGroup.alpha, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        canvasGroup.blocksRaycasts = to > 0.5;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
