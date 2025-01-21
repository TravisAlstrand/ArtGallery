using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Hacking")]
    [SerializeField] private GameObject hackingTool;
    [SerializeField] private GameObject hackArea;
    [SerializeField] private float hackTimeTotal = 10f;
    [SerializeField] private GameObject hackingUI;
    [SerializeField] private Slider hackingSlider;
    [SerializeField] private GameObject hackTipText;
    [SerializeField] private GameObject outOfRangeText;
    [Header("Stealing")]
    [SerializeField] private GameObject artPiece;
    [SerializeField] private GameObject stealTipText;
    [SerializeField] private GameObject[] lasers;
    [SerializeField] private CircleCollider2D stealArea;
    [Header("General")]
    [SerializeField] private GameObject bustedText;
    [SerializeField] private GameObject escapeTipText;
    public CurrentState currentState;
    private float lastX = 0f;
    private float lastY = -1f;
    private bool canTakeItem, canHack, hasItem;

    private PlayerInput playerInput;
    private FrameInput frameInput;
    private PlayerMovement playerMovement;
    private Animator animator;
    private SceneController sceneController;
    private float hackTimer = 0f;

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
            // Actually hacking the laser grid
            if (canHack) {
                hackTipText.SetActive(false);
                hackingUI.SetActive(true);
                hackTimer += Time.deltaTime;
                hackingSlider.value = hackTimer / 10;
                if (hackTimer >= hackTimeTotal) {
                    hackTimer = hackTimeTotal;
                    hackArea.SetActive(false);
                    foreach (GameObject laser in lasers) {
                        laser.SetActive(false);
                    }
                    hackingUI.SetActive(false);
                }
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
        stealTipText.SetActive(false);
        escapeTipText.SetActive(true);
        stealArea.enabled = false;
        StartCoroutine(WaitToRemoveEscapeText());
    }

    private IEnumerator WaitToRemoveEscapeText() {
        yield return new WaitForSeconds(4f);
        escapeTipText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ArtPiece")) {
            canTakeItem = true;
            stealTipText.SetActive(true);
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
        else if (other.gameObject.CompareTag("EndArea")) {
            if (hasItem) {
                Debug.Log("WON!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("ArtPiece")) {
            canTakeItem = false;
            stealTipText.SetActive(false);
        } else if (other.gameObject.CompareTag("HackArea")) {
            canHack = false;
            hackTipText.SetActive(false);
        }
    }
}
public enum CurrentState { Idle, Walking, Hacking, Caught }
