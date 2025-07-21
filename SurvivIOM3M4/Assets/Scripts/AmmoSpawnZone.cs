using UnityEngine;

public class AmmoSpawnZone : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int minToSpawn = 2;
    public int maxToSpawn = 6;
    public Vector2 zoneSize = new Vector2(5f, 5f);
    public GameObject[] ammoPrefabs;

    [Header("Editor Only")]
    public Color gizmoColor = new Color(1, 1, 0, 0.25f); // yellow box

    private void Start()
    {
        SpawnAmmo();
    }

    private void SpawnAmmo()
    {
        if (ammoPrefabs == null || ammoPrefabs.Length == 0)
        {
            Debug.LogWarning("AmmoSpawnZone: No ammo prefabs assigned!");
            return;
        }

        int amountToSpawn = Random.Range(minToSpawn, maxToSpawn + 1);

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomPointInZone();
            GameObject ammoPrefab = ammoPrefabs[Random.Range(0, ammoPrefabs.Length)];
            Instantiate(ammoPrefab, spawnPos, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPointInZone()
    {
        Vector2 offset = new Vector2(
            Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f),
            Random.Range(-zoneSize.y / 2f, zoneSize.y / 2f)
        );

        return transform.position + new Vector3(offset.x, offset.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(zoneSize.x, zoneSize.y, 0.1f));
    }
}
