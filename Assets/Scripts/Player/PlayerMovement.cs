using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    private Vector2 currentDirection;

    private Rigidbody2D rigidBody;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rigidBody.linearVelocity = new Vector2(currentDirection.x, currentDirection.y) * walkSpeed;
    }

    public void SetCurrentDirection(Vector2 direction) {
        currentDirection = direction;
    }
}
