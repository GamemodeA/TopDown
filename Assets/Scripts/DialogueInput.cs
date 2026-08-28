using UnityEngine.InputSystem;

public static class DialogueInput
{
    /* Interact was pressed if right or left click was pressed */
    public static bool WasInteractPressed()
    {
        return Mouse.current != null && (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame);
    }
}
