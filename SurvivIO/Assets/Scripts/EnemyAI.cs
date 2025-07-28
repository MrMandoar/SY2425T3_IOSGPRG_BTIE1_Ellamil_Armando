using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("General Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;

    [Header("Timers")]
    public float minChangeDirTime = 2f;
    public float maxChangeDirTime = 4f;
    public float minIdleTime = 1.5f;
    public float maxIdleTime = 2.5f;

    [Header("Weapon")]
    public List<WeaponData> possibleWeapons; 
    private WeaponData currentWeapon;
    private GameObject currentGunInstance;
    public Transform weaponHolder; 
    private Transform firePoint;

    [Header("State")]
    public EnemyState currentState = EnemyState.Patrol;

    private Vector2 moveDirection;
    private float changeDirTimer;
    private Transform player;
    private float fireCooldown;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        AssignRandomWeapon();
        if (currentState == EnemyState.Patrol)
            SetRandomDirection();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.Idle:
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

        UpdateWeaponRotation();
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

        if (fireCooldown <= 0f)
        {
            Shoot(dir);
            fireCooldown = 1f / currentWeapon.fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (currentWeapon.weaponType == WeaponType.Shotgun)
        {
            FireShotgunCone(direction, 8, 15f);
        }
        else
        {
            FireBullet(direction);
        }
    }

    private void FireBullet(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction.normalized);
            bulletScript.SetDamage(currentWeapon.damage);
        }
    }

    private void FireShotgunCone(Vector2 baseDirection, int bulletCount, float spreadAngle)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Vector2 spreadDir = Quaternion.Euler(0f, 0f, angleOffset) * baseDirection;
            FireBullet(spreadDir.normalized);
        }
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

    private void AssignRandomWeapon()
    {
        if (possibleWeapons.Count > 0)
        {
            currentWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Count)];

            if (currentWeapon.weaponPrefab != null && weaponHolder != null)
            {
                currentGunInstance = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
                firePoint = currentGunInstance.transform.Find("FirePoint");

                if (firePoint == null)
                    Debug.LogError("FirePoint not found on enemy weapon prefab!");
            }
        }
        else
        {
            Debug.LogWarning("No weapons assigned to EnemyAI.");
        }
    }

    private void UpdateWeaponRotation()
    {
        if (player == null || currentGunInstance == null) return;

        Vector2 direction = player.position - weaponHolder.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentGunInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
