using UnityEngine;
using System.Collections;

public class WeaponShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    private FloatingJoystick aimJoystick;
    private float fireCooldown;
    private int currentMagazine;
    private bool isReloading;
    private bool isFiring;

    private WeaponData CurrentWeapon => PlayerInventory.Instance.PrimaryWeapon;

    private void Start()
    {
        GameObject joystickObj = GameObject.Find("Fixed Joystick R");
        if (joystickObj != null)
            aimJoystick = joystickObj.GetComponent<FloatingJoystick>();
        else
            Debug.LogWarning("WeaponShooter: Could not find 'Fixed Joystick R'.");

        InitializeWeapon();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (isFiring && CurrentWeapon != null && CurrentWeapon.weaponType == WeaponType.AR)
        {
            if (CanShoot())
            {
                Shoot();
            }
        }
    }

    public void OnShootButtonPressed()
    {
        if (CurrentWeapon == null || !CanShoot())
            return;

        if (CurrentWeapon.weaponType == WeaponType.AR)
        {
            isFiring = true;
        }
        else
        {
            Shoot();
        }
    }

    public void OnShootButtonReleased()
    {
        isFiring = false;
    }

    private void InitializeWeapon()
    {
        if (CurrentWeapon == null)
        {
            Debug.LogWarning("WeaponShooter: No primary weapon equipped.");
            return;
        }

        currentMagazine = Mathf.Min(CurrentWeapon.magazineSize, PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType));
        fireCooldown = 0f;
        isReloading = false;
    }

    private bool CanShoot()
    {
        return CurrentWeapon != null &&
               !isReloading &&
               fireCooldown <= 0f &&
               currentMagazine > 0 &&
               PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType) > 0;
    }

    private void Shoot()
    {
        fireCooldown = 1f / CurrentWeapon.fireRate;
        currentMagazine--;

        Vector2 direction = (firePoint.right).normalized; // Only use firePoint orientation

        switch (CurrentWeapon.weaponType)
        {
            case WeaponType.Pistol:
                FireBullet(direction);
                break;

            case WeaponType.AR:
                FireBullet(direction);
                break;

            case WeaponType.Shotgun:
                FireShotgunCone(direction, 8, 15f);
                break;
        }

        PlayerInventory.Instance.ConsumeAmmo(CurrentWeapon.ammoType, 1);

        if (currentMagazine <= 0 && PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType) > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void FireBullet(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bullet = Instantiate(CurrentWeapon.bulletPrefab, firePoint.position, rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
            bulletScript.SetDamage(CurrentWeapon.damage);
            bulletScript.SetShooter(gameObject);
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

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(CurrentWeapon.reloadTime);

        int reserve = PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType);
        int toReload = Mathf.Min(CurrentWeapon.magazineSize, reserve);

        currentMagazine = toReload;
        PlayerInventory.Instance.ConsumeAmmo(CurrentWeapon.ammoType, toReload);

        isReloading = false;
    }

    public void ForceReload()
    {
        if (!isReloading && CurrentWeapon != null)
            StartCoroutine(Reload());
    }

    public void OnWeaponSwitched()
    {
        InitializeWeapon();
    }
}
