using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTransition : MonoBehaviour
{
    [SerializeField] private Transform destination; // empty GameObject marking where the player appears
    [SerializeField] private CameraMovement cameraFollow;
    [SerializeField] private Vector2 newMinBounds;
    [SerializeField] private Vector2 newMaxBounds;
    [SerializeField] private bool updatesCameraBounds = false;

    private bool isTransitioning = false;

    /* Ensure the collider is a trigger, not a solid wall. */
    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning) return;
        if (!other.CompareTag("Player")) return; // Require the tagged player GameObject

        StartCoroutine(DoTransition(other));
    }

    /* Fades, moves the player, then moves the camera */
    private IEnumerator DoTransition(Collider2D player)
    {
        PlayerMovement playerMvmt = player.GetComponent<PlayerMovement>();
        //playerMvmt.Freeze();
        isTransitioning = true;

        // Yielding directly on ScreenFader's IEnumerator
        yield return ScreenFader.Instance.FadeOut();

        // Transitions camera and player
        Rigidbody2D playerBody = player.attachedRigidbody;
        playerBody.position = destination.position;
        playerBody.linearVelocity = Vector2.zero;   // Clear leftover momentum

        if (updatesCameraBounds && cameraFollow != null)
        {
            cameraFollow.SetBounds(newMinBounds, newMaxBounds);
        }
        cameraFollow.SnapToTarget();
        yield return ScreenFader.Instance.FadeIn();
        isTransitioning = false;

        //playerMvmt.Unfreeze();
    }
}