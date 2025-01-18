using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject hackingTool;
    public CurrentState currentState;
    private float lastX = 0f;
    private float lastY = -1f;
    private bool canMove = true;

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
        HandleHacking();
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
            currentState = CurrentState.Walking;
            animator.SetFloat("xDir", frameInput.Move.x);
            animator.SetFloat("yDir", frameInput.Move.y);
            // Set these so when the player stops moving the direction stays
            lastX = frameInput.Move.x;
            lastY = frameInput.Move.y;
        } else {
            currentState = CurrentState.Idle;
            animator.SetFloat("xDir", lastX);
            animator.SetFloat("yDir", lastY);
        }
    }

    private void HandleHacking() {
        if (frameInput.Hack) {
            currentState = CurrentState.Hacking;
            playerMovement.SetCurrentDirection(Vector2.zero);
            canMove = false;
            hackingTool.SetActive(true);
        } else {
            currentState = CurrentState.Idle;
            canMove = true;
            hackingTool.SetActive(false);
        }
    }
}
public enum CurrentState { Idle, Walking, Hacking, Caught }
