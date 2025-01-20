using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject hackingTool;
    public CurrentState currentState;
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

    private void Start() {
        currentState = CurrentState.Idle;
    }

    private void Update() {
        GatherInput();
        if (currentState != CurrentState.Caught) {
            HandleHacking();
        }
        if (currentState != CurrentState.Hacking &&
            currentState != CurrentState.Caught) {
            HandleMovement();
        }
    }

    private void GatherInput() {
        frameInput = playerInput.FrameInput;
    }

    private void HandleMovement() {
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
        if (frameInput.Hack && currentState != CurrentState.Caught) {
            currentState = CurrentState.Hacking;
            playerMovement.SetCurrentDirection(Vector2.zero);
            hackingTool.SetActive(true);
        } else {
            currentState = CurrentState.Idle;
            hackingTool.SetActive(false);
        }
    }

    public void WasCaught() {
        currentState = CurrentState.Caught;
        playerMovement.SetCurrentDirection(Vector2.zero);
        hackingTool.SetActive(false);
    }
}
public enum CurrentState { Idle, Walking, Hacking, Caught }
