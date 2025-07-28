using UnityEngine;

public class GunSpawnZone : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int minToSpawn = 1;
    public int maxToSpawn = 3;
    public Vector2 zoneSize = new Vector2(5f, 5f);
    public GameObject[] weaponPrefabs;

    [Header("Editor Only")]
    public Color gizmoColor = new Color(0, 1, 1, 0.25f); // cyan box

    private void Start()
    {
        SpawnWeapons();
    }

    private void SpawnWeapons()
    {
        if (weaponPrefabs == null || weaponPrefabs.Length == 0)
        {
            Debug.LogWarning("GunSpawnZone: No weapon prefabs assigned!");
            return;
        }

        int amountToSpawn = Random.Range(minToSpawn, maxToSpawn + 1);

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomPointInZone();
            GameObject weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            Instantiate(weaponPrefab, spawnPos, Quaternion.identity);
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
