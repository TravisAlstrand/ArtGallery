using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Guard guard;

    private void Awake() {
        guard = GetComponentInParent<Guard>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            guard.CaughtPlayer();
        }
    }
}
