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

    private WeaponData CurrentWeapon => PlayerInventory.Instance.PrimaryWeapon;

    private void Start()
    {
        GameObject joystickObj = GameObject.Find("Fixed Joystick R");
        if (joystickObj != null)
        {
            aimJoystick = joystickObj.GetComponent<FloatingJoystick>();
        }
        else
        {
            Debug.LogWarning("WeaponShooter: Could not find 'Fixed Joystick R' in the scene.");
        }

        InitializeWeapon();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public void OnShootButtonPressed()
    {
        if (CanShoot())
        {
            Shoot();
        }
        else
        {
            Debug.Log("Can't shoot: either no ammo, reloading, cooldown, or no weapon equipped.");
        }
    }

    private void InitializeWeapon()
    {
        if (CurrentWeapon == null)
        {
            Debug.LogWarning("No primary weapon equipped.");
            return;
        }

        currentMagazine = Mathf.Min(CurrentWeapon.magazineSize, PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType));
        fireCooldown = 0f;
        isReloading = false;
    }

    private bool CanShoot()
    {
        if (CurrentWeapon == null || isReloading || fireCooldown > 0f)
            return false;

        if (currentMagazine <= 0)
            return false;

        int reserveAmmo = PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType);
        return reserveAmmo >= 0; // allow last bullet in mag to shoot
    }

    private void Shoot()
    {
        fireCooldown = 1f / CurrentWeapon.fireRate;
        currentMagazine--;

        // read joystick direction
        Vector2 shootDirection = Vector2.right;
        if (aimJoystick != null)
        {
            shootDirection = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
            if (shootDirection.sqrMagnitude < 0.01f)
            {
                shootDirection = firePoint.right; // fallback
            }
        }
        shootDirection.Normalize();

        // rotate bullet to face direction //WIP
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bullet = Instantiate(CurrentWeapon.bulletPrefab, firePoint.position, rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
        }

        // inventory drain // WIP I THINK
        PlayerInventory.Instance.ConsumeAmmo(CurrentWeapon.ammoType, 1);

        // auto reload if empty // WIP
        if (currentMagazine <= 0 && PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType) > 0)
        {
            StartCoroutine(Reload());
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
        {
            StartCoroutine(Reload());
        }
    }

    public void OnWeaponSwitched()
    {
        InitializeWeapon();
    }
}
