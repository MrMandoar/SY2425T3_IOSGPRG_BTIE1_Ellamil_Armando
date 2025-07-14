using UnityEngine;
using System.Collections;

public class WeaponShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Joystick aimJoystick;

    private float fireCooldown;
    private int currentMagazine;
    private bool isReloading;

    private WeaponData CurrentWeapon => PlayerInventory.Instance.PrimaryWeapon;

    private void Start()
    {
        InitializeWeapon();
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        Vector2 aimInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (aimInput.magnitude > 0.2f && CanShoot())
        {
            Shoot(aimInput);
        }
    }

    private void InitializeWeapon()
    {
        if (CurrentWeapon == null)
        {
            Debug.LogWarning("No primary weapon equipped.");
            return;
        }

        currentMagazine = CurrentWeapon.magazineSize;
        fireCooldown = 0f;
        isReloading = false;
    }

    private bool CanShoot()
    {
        return CurrentWeapon != null
            && currentMagazine > 0
            && !isReloading
            && fireCooldown <= 0f;
    }

    private void Shoot(Vector2 direction)
    {
        fireCooldown = 1f / CurrentWeapon.fireRate;
        currentMagazine--;

        SpawnBullet(direction);
        PlayerInventory.Instance.ConsumeAmmo(CurrentWeapon.ammoType, 1);

        if (currentMagazine <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void SpawnBullet(Vector2 direction)
    {
        if (CurrentWeapon.bulletPrefab == null || firePoint == null)
        {
            return;
        }

        GameObject bulletObj = Instantiate(CurrentWeapon.bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(direction.normalized);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(CurrentWeapon.reloadTime);

        int reserve = PlayerInventory.Instance.GetAmmoAmount(CurrentWeapon.ammoType);
        int needed = CurrentWeapon.magazineSize;
        int toReload = Mathf.Min(needed, reserve);

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
