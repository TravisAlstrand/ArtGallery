using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Guard : MonoBehaviour
{
    [SerializeField] private PatrolType patrolType;
    [SerializeField] private CurrentGuardState currentGuardState;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private int firstPatrolIndex = 0;
    [SerializeField] private bool idlesAtPoints;
    [SerializeField] private float idleTime;
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject bustedText;

    private int totalPatrolPoints;
    private int currentPatrolTarget;
    private int direction = 1;
    private Vector2 movementDirection;
    private float lastYDir;
    private float lastXDir;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private PlayerController player;
    private SceneController sceneController;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
        sceneController = FindFirstObjectByType<SceneController>();
    }

    private void Start()
    {
        currentGuardState = CurrentGuardState.patrolling;
        totalPatrolPoints = patrolPoints.Length;
        currentPatrolTarget = firstPatrolIndex;
        StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        if (currentGuardState == CurrentGuardState.chasing) {
            ChaseBehavior();
        }
        UpdateAnimatorParameters();
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (currentGuardState == CurrentGuardState.patrolling)
            {
                Vector2 targetPosition = patrolPoints[currentPatrolTarget].transform.position;
                movementDirection = (targetPosition - (Vector2)transform.position).normalized;
                rigidBody.linearVelocity = movementDirection * patrolSpeed;

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    rigidBody.linearVelocity = Vector2.zero;
                    movementDirection = Vector2.zero;
                    
                    if (idlesAtPoints)
                    {
                        currentGuardState = CurrentGuardState.idle;
                        yield return new WaitForSeconds(idleTime);
                        currentGuardState = CurrentGuardState.patrolling;
                    }
                    
                    UpdatePatrolTarget();
                }
            }
            yield return null;
        }
    }

    private void UpdateAnimatorParameters()
    {
        if (rigidBody.linearVelocity != Vector2.zero) {
            float xDir = Mathf.Abs(movementDirection.x) < 0.1f ? 0 : Mathf.Sign(movementDirection.x);
            float yDir = Mathf.Abs(movementDirection.y) < 0.1f ? 0 : Mathf.Sign(movementDirection.y);
            lastXDir = xDir;
            lastYDir = yDir;
            animator.SetFloat("xDir", xDir);
            animator.SetFloat("yDir", yDir);
        } else {
            animator.SetFloat("xDir", lastXDir);
            animator.SetFloat("yDir", lastYDir);
        }
        
    }

    private void UpdatePatrolTarget()
    {
        switch (patrolType)
        {
            case PatrolType.fullCycle:
                currentPatrolTarget = (currentPatrolTarget + 1) % totalPatrolPoints;
                break;
            case PatrolType.reverseCycle:
                currentPatrolTarget += direction;
                if (currentPatrolTarget >= totalPatrolPoints || currentPatrolTarget < 0)
                {
                    direction *= -1;
                    currentPatrolTarget += direction * 2;
                }
                break;
            case PatrolType.noPatrol:
                // Do nothing if no patrol
                break;
        }
    }

    public void CaughtPlayer() {
        currentGuardState = CurrentGuardState.spotting;
        rigidBody.linearVelocity = new(0f, 0f);
        exclamationMark.SetActive(true);
        player.WasCaught();
        StartCoroutine(WaitToChase());
    }

    private IEnumerator WaitToChase() {
        yield return new WaitForSeconds(1f);
        currentGuardState = CurrentGuardState.chasing;
    }

    private void ChaseBehavior() {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer > 2.25f)
        {
            rigidBody.linearVelocity = directionToPlayer * chaseSpeed;
            movementDirection = directionToPlayer;
        }
        else
        {
            rigidBody.linearVelocity = Vector2.zero;
            currentGuardState = CurrentGuardState.idle;
            bustedText.SetActive(true);
            StartCoroutine(WaitToReloadScene());
        }
    }

    private IEnumerator WaitToReloadScene() {
        yield return new WaitForSeconds(2f);
        sceneController.ReloadScene();
    }
}

public enum PatrolType { fullCycle, reverseCycle, noPatrol }
public enum CurrentGuardState { idle, patrolling, spotting, chasing }
