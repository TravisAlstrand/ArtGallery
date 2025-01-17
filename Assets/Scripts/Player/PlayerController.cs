using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastX = 0f;
    private float lastY = -1f;

    private PlayerInput playerInput;
    private FrameInput frameInput;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        GatherInput();
        HandleMovement();
    }

    private void GatherInput() {
        frameInput = playerInput.FrameInput;
    }

    private void HandleMovement() {
        playerMovement.SetCurrentDirection(frameInput.Move);
        if (frameInput.Move != Vector2.zero) {
            animator.SetFloat("xDir", frameInput.Move.x);
            animator.SetFloat("yDir", frameInput.Move.y);
            lastX = frameInput.Move.x;
            lastY = frameInput.Move.y;
        } else {
            animator.SetFloat("xDir", lastX);
            animator.SetFloat("yDir", lastY);
        }
    }
}
