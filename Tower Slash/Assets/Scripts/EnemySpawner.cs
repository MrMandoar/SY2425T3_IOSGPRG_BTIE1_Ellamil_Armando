using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int minEnemiesPerWave = 1;
    [SerializeField] private int maxEnemiesPerWave = 3;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        int enemyCount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);
        Transform[] selectedSpawnPoints = GetRandomSpawnPoints(enemyCount);

        foreach (Transform spawnPoint in selectedSpawnPoints)
        {
            SpawnEnemy(spawnPoint.position);
        }
    }

    private Transform[] GetRandomSpawnPoints(int count)
    {
        Transform[] selected = new Transform[count];
        System.Collections.Generic.List<int> usedIndices = new();

        for (int i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);
            selected[i] = spawnPoints[randomIndex];
        }

        return selected;
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();

        // rando arrow color
        ArrowColor randomColor = (ArrowColor)Random.Range(0, 3);
        enemyScript.SetArrowColor(randomColor);

        // rando direction
        ArrowDirection randomDirection = (ArrowDirection)Random.Range(0, 4);
        enemyScript.SetArrowDirection(randomDirection);
    }
}
