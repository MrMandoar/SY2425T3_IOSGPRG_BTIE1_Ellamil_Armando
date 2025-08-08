using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Weapon Setup")]
    public GameObject[] weaponPrefabs;
    public Transform weaponVisualParent;
    public Transform player;

    private GameObject currentWeaponInstance;
    private WeaponData currentWeaponData;
    private Transform firePoint;
    private float fireCooldown = 0f;

    private void Start()
    {
        // Validate weaponPrefabs
        if (weaponPrefabs == null || weaponPrefabs.Length == 0)
        {
            Debug.LogError("EnemyShooter: No weaponPrefabs assigned.");
            return;
        }

        // Validate parent object for weapon visuals
        if (weaponVisualParent == null)
        {
            Debug.LogError("EnemyShooter: WeaponVisualParent not assigned.");
            return;
        }

        // Randomly pick a weapon prefab
        int index = Random.Range(0, weaponPrefabs.Length);
        currentWeaponInstance = Instantiate(weaponPrefabs[index], weaponVisualParent);

        // Reset local transform of weapon visual
        currentWeaponInstance.transform.localPosition = Vector3.zero;
        currentWeaponInstance.transform.localRotation = Quaternion.identity;
        currentWeaponInstance.transform.localScale = Vector3.one;

        // Get WeaponData from WeaponVisual component
        WeaponVisual visual = currentWeaponInstance.GetComponent<WeaponVisual>();
        if (visual == null || visual.weaponData == null)
        {
            Debug.LogError("EnemyShooter: WeaponData missing in WeaponVisual component.");
            return;
        }
        currentWeaponData = visual.weaponData;

        // Locate the FirePoint
        firePoint = currentWeaponInstance.transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("EnemyShooter: No FirePoint found in weapon prefab.");
            return;
        }

        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    private void Update()
    {
        if (player == null || currentWeaponInstance == null || firePoint == null)
            return;

        // Aim weapon toward the player
        Vector2 aimDirection = (player.position - weaponVisualParent.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        weaponVisualParent.rotation = Quaternion.Euler(0, 0, angle);

        // Flip weapon 
        Vector3 scale = currentWeaponInstance.transform.localScale;
        scale.y = aimDirection.x < 0 ? -1f : 1f;
        currentWeaponInstance.transform.localScale = scale;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot(aimDirection);
            fireCooldown = 1f / currentWeaponData.fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (currentWeaponData.weaponType == WeaponType.Shotgun)
        {
            // Fire 8 pellets in a spread
            int pelletCount = 8;
            float spreadAngle = 30f;

            for (int i = 0; i < pelletCount; i++)
            {
                float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                float finalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
                Vector2 spreadDirection = new Vector2(
                    Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                    Mathf.Sin(finalAngle * Mathf.Deg2Rad)
                );
                FireBullet(spreadDirection.normalized);
            }
        }
        else
        {
            // Normal bullet
            FireBullet(direction.normalized);
        }
    }

    private void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(currentWeaponData.bulletPrefab, firePoint.position, Quaternion.identity);

        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
                bulletScript.SetDamage(currentWeaponData.damage);
                bulletScript.SetShooter(gameObject);
            }

            bullet.transform.right = direction; // Bullet should face direction
        }
        else
        {
            Debug.LogWarning("EnemyShooter: Failed to instantiate bullet.");
        }
    }
}
