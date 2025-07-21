using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public int minSpawnCount = 3;
    public int maxSpawnCount = 6;

    [Header("Area Settings")]
    public Vector2 spawnAreaSize = new Vector2(5f, 5f);

    [Header("Possible Start States")]
    public bool startAsPatrol = true;
    public bool startAsIdle = true;
    public bool startAsChase = false;

    private void Start()
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
            );

            Vector3 spawnPos = transform.position + (Vector3)randomOffset;

            GameObject enemyObj = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                spawnPos,
                Quaternion.identity
            );

            EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                // Assign initial state
                enemyAI.currentState = GetRandomState();

                // Randomize patrol behavior timers
                enemyAI.minChangeDirTime = 2f;
                enemyAI.maxChangeDirTime = 4f;
                enemyAI.minIdleTime = 1.5f;
                enemyAI.maxIdleTime = 2.5f;
            }
        }
    }

    private EnemyState GetRandomState()
    {
        var possibleStates = new System.Collections.Generic.List<EnemyState>();

        if (startAsPatrol) possibleStates.Add(EnemyState.Patrol);
        if (startAsIdle) possibleStates.Add(EnemyState.Idle);
        if (startAsChase) possibleStates.Add(EnemyState.Chase);

        if (possibleStates.Count == 0)
            return EnemyState.Patrol; // fallback

        return possibleStates[Random.Range(0, possibleStates.Count)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f); // translucent red
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
