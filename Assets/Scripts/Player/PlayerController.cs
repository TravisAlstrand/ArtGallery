using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject hackingTool;
    [SerializeField] private GameObject artPiece;
    [SerializeField] private GameObject bustedText;
    [SerializeField] private GameObject hackTipText;
    [SerializeField] private GameObject outOfRangeText;
    [SerializeField] private GameObject hackingUI;
    public CurrentState currentState;
    private float lastX = 0f;
    private float lastY = -1f;
    private bool canTakeItem, canHack;

    private PlayerInput playerInput;
    private FrameInput frameInput;
    private PlayerMovement playerMovement;
    private Animator animator;
    private SceneController sceneController;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        sceneController = FindFirstObjectByType<SceneController>();
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
        if (canTakeItem && frameInput.Interact &&
            currentState != CurrentState.Hacking &&
            currentState != CurrentState.Caught) {
            StealArtPiece();
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
            if (canHack) {
                hackTipText.SetActive(false);
                hackingUI.SetActive(true);
            }
            else {
                outOfRangeText.SetActive(true);
            }
        } else {
            currentState = CurrentState.Idle;
            hackingTool.SetActive(false);
            if (canHack) {
                hackTipText.SetActive(true);
            }
            if (outOfRangeText.activeInHierarchy) {
                outOfRangeText.SetActive(false);
            }
            if (hackingUI.activeInHierarchy) {
                hackingUI.SetActive(false);
            }
        }
        
    }

    public void WasCaught() {
        currentState = CurrentState.Caught;
        playerMovement.SetCurrentDirection(Vector2.zero);
        hackingTool.SetActive(false);
    }

    private void StealArtPiece() {
        artPiece.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ArtPiece")) {
            canTakeItem = true;
        }
        else if (other.gameObject.CompareTag("Laser")) {
            bustedText.SetActive(true);
            WasCaught();
            sceneController.ReloadScene();
        }
        else if (other.gameObject.CompareTag("HackArea")) {
            canHack = true;
            hackTipText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("ArtPiece")) {
            canTakeItem = false;
        } else if (other.gameObject.CompareTag("HackArea")) {
            canHack = false;
            hackTipText.SetActive(false);
        }
    }
}
public enum CurrentState { Idle, Walking, Hacking, Caught }
