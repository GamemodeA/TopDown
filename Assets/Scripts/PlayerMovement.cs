using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float mvSpeed = 5f;    // default 5f
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool frozen = false;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 playerMv;

    private enum FacingDirection { Down = 0, Up = 1, Side = 2 };
    
    void Awake()
    {
        if (body == null) body = GetComponent<Rigidbody2D>();
        if (animator == null) TryGetComponent(out animator);
        if (spriteRenderer == null) TryGetComponent(out spriteRenderer);
    }

    /* Always read user input and update movement when collision matters */
    void Update()
    {
        if (!frozen)
        {
            playerMv = ReadMovementInput();
            UpdateAnimation();
        }
    }
    void FixedUpdate() 
    { 
        if (!frozen) ApplyMovement(); 
    }

    private Vector2 ReadMovementInput()
    {
        // In dialogue or no input means No movement
        if (DialogueUI.Instance != null && DialogueUI.Instance.showing) return Vector2.zero;
        if (Keyboard.current == null) return Vector2.zero;

        // Move player with WASD
        Vector2 input = Vector2.zero;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.wKey.isPressed) input.y += 1;
        return input.normalized;
    }

    private void ApplyMovement() { body.linearVelocity = playerMv * mvSpeed; }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        bool isWalking = playerMv != Vector2.zero;
        animator.SetBool("IsWalking", isWalking);
        if (isWalking)
        {
            if (Mathf.Abs(playerMv.x) > Mathf.Abs(playerMv.y))
            {
                animator.SetInteger("Direction", (int)FacingDirection.Side);
                if (spriteRenderer != null) spriteRenderer.flipX = playerMv.x < 0;
            } 
            else
            {
                animator.SetInteger("Direction", playerMv.y > 0 ? (int)FacingDirection.Up : (int)FacingDirection.Down);
            }
        }
    }

    public void Freeze() { frozen = true; }
    public void Unfreeze() { frozen = false; }
}
