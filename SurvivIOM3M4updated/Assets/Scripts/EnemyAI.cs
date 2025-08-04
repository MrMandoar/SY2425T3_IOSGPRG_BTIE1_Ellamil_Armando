using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour
{
    [Header("General Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;

    [Header("Timers (Optional but used by SpawnZone)")]
    public float minChangeDirTime = 2f;
    public float maxChangeDirTime = 4f;
    public float minIdleTime = 1.5f;
    public float maxIdleTime = 2.5f;

    [Header("State")]
    public EnemyState currentState = EnemyState.Patrol;

    private Vector2 moveDirection;
    private float changeDirTimer;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (currentState == EnemyState.Patrol)
            SetRandomDirection();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // Do nothing
                break;

            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;
        }

        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void Patrol()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        changeDirTimer -= Time.deltaTime;
        if (changeDirTimer <= 0f)
        {
            StartCoroutine(ChangeDirectionRoutine());
        }
    }

    private void Chase()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        currentState = EnemyState.Idle;

        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);

        SetRandomDirection();
        currentState = EnemyState.Patrol;
    }

    private void SetRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        changeDirTimer = Random.Range(minChangeDirTime, maxChangeDirTime);
    }
}

