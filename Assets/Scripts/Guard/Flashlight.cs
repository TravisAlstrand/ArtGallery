using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Guard guard;

    private void Awake() {
        guard = GetComponentInParent<Guard>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (HasLineOfSightToPlayer(other.gameObject.transform)) {
                guard.CaughtPlayer();
            }
        }
    }

    private bool HasLineOfSightToPlayer(Transform player)
{
    Vector2 directionToPlayer = player.position - transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;

    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Walls"));

    // If the raycast doesn't hit anything, there's a clear line of sight
    return hit.collider == null;
}
}
