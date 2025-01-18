using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastX = 0f;
    private float lastY = -1f;
    private bool canMove = true;
    public CurrentState currentState;

    private PlayerInput playerInput;
    private FrameInput frameInput;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        currentState = CurrentState.Idle;
    }

    private void Update() {
        GatherInput();
        HandleMovement();
    }

    private void GatherInput() {
        frameInput = playerInput.FrameInput;
    }

    private void HandleMovement() {
        if (!canMove) return;
        // Move the player
        playerMovement.SetCurrentDirection(frameInput.Move);

        // Handle the animation direction
        if (frameInput.Move != Vector2.zero) {
            animator.SetFloat("xDir", frameInput.Move.x);
            animator.SetFloat("yDir", frameInput.Move.y);
            // Set these so when the player stops moving the direction stays
            lastX = frameInput.Move.x;
            lastY = frameInput.Move.y;
        } else {
            animator.SetFloat("xDir", lastX);
            animator.SetFloat("yDir", lastY);
        }
    }
}
public enum CurrentState { Idle, Walking, Hacking, Caught }
