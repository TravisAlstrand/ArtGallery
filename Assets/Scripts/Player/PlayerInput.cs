using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    private InputSystem_Actions inputActions;
    private InputAction move;
    private InputAction interact;
    private InputAction hack;
    private InputAction submit;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        move = inputActions.Player.Move;
        interact = inputActions.Player.Interact;
        hack = inputActions.Player.Hack;
        submit = inputActions.UI.Submit;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = move.ReadValue<Vector2>(),
            Interact = interact.WasPressedThisFrame(),
            Hack = hack.IsPressed(),
            SubmitRelease = submit.WasReleasedThisFrame(),
        };
    }

    public void SwitchToUIMap()
    {
        inputActions.Player.Disable();
        inputActions.UI.Enable();
    }

    public void SwitchToGameplayMap()
    {
        inputActions.UI.Disable();
        inputActions.Player.Enable();
    }
}

public struct FrameInput
{
    public Vector2 Move;
    public bool Interact;
    public bool Hack;
    public bool SubmitRelease;
}