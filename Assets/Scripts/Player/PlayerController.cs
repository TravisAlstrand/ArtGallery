using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private FrameInput frameInput;
    private PlayerMovement playerMovement;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        GatherInput();
        playerMovement.SetCurrentDirection(frameInput.Move);
    }

    private void GatherInput() {
        frameInput = playerInput.FrameInput;
    }
}
