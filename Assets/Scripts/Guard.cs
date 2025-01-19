using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private PatrolType patrolType;
    [SerializeField] private CurrentState currentState;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private bool idlesAtPoints;
    [SerializeField] private float idleTime;
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float chaseSpeed = 10f;

    private void Start() {
        currentState = CurrentState.patrolling;
    }

}

public enum PatrolType { fullCycle, reverseCycle, noPatrol }
public enum CurrentState { idle, patrolling, chasing, catching }