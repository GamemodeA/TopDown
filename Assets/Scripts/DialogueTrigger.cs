using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 4)]
    [SerializeField] private string[] lines;
    [Header("Trigger Method")]
    [SerializeField] private bool triggerOnInteract = true;
    [SerializeField] private bool triggerOnPlayerCollision = true;
    private bool playerInRange = false;

    /* Open dialogue if player clicked a dialogue trigger */
    void Update()
    {
        if (!triggerOnInteract || !playerInRange) return;
        if (DialogueInput.WasInteractPressed()) TryOpenDialogue();
    }

    /* Open dialogue if player entered into a cutscene trigger */
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (triggerOnPlayerCollision) TryOpenDialogue();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }

    /* Open dialogue if possible */
    private void TryOpenDialogue()
    {
        if (DialogueUI.Instance.showing) return;
        DialogueUI.Instance.ShowDialogue(lines);
    }
}
