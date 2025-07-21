using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    // Ammo
    public int Ammo9mm { get; private set; }
    public int Ammo12g { get; private set; }
    public int Ammo556 { get; private set; }

    // Weapons 
    public WeaponData PrimaryWeapon { get; private set; }
    public WeaponData SecondaryWeapon { get; private set; }

    // Visuals
    [Header("Visual References")]
    [SerializeField] private WeaponVisualHandler weaponVisualHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ammo Management 

    public void AddAmmo(string ammoType, int amount)
    {
        switch (ammoType)
        {
            case "9mm":
                Ammo9mm += amount;
                UIManager.Instance.UpdateAmmo("9mm", Ammo9mm);
                break;

            case "12g":
                Ammo12g += amount;
                UIManager.Instance.UpdateAmmo("12g", Ammo12g);
                break;

            case "556":
                Ammo556 += amount;
                UIManager.Instance.UpdateAmmo("556", Ammo556);
                break;
        }
    }

    public void ConsumeAmmo(string ammoType, int amount)
    {
        switch (ammoType)
        {
            case "9mm":
                Ammo9mm = Mathf.Max(0, Ammo9mm - amount);
                UIManager.Instance.UpdateAmmo("9mm", Ammo9mm);
                break;

            case "12g":
                Ammo12g = Mathf.Max(0, Ammo12g - amount);
                UIManager.Instance.UpdateAmmo("12g", Ammo12g);
                break;

            case "556":
                Ammo556 = Mathf.Max(0, Ammo556 - amount);
                UIManager.Instance.UpdateAmmo("556", Ammo556);
                break;
        }
    }

    public int GetAmmoAmount(string ammoType)
    {
        return ammoType switch
        {
            "9mm" => Ammo9mm,
            "12g" => Ammo12g,
            "556" => Ammo556,
            _ => 0
        };
    }

    // Pickup Logic (weapon)

    public void PickUpWeapon(WeaponData newWeapon)
    {
        if (PrimaryWeapon == null)
        {
            PrimaryWeapon = newWeapon;
            UIManager.Instance.SetWeaponIcon(newWeapon.equippedSprite, true);
        }
        else if (SecondaryWeapon == null)
        {
            SecondaryWeapon = newWeapon;
            UIManager.Instance.SetWeaponIcon(newWeapon.equippedSprite, false);
        }
        else
        {
            PrimaryWeapon = SecondaryWeapon;
            UIManager.Instance.ShiftSecondaryToPrimary();
            weaponVisualHandler.SetWeaponSprite(PrimaryWeapon.equippedSprite);

            SecondaryWeapon = newWeapon;
            UIManager.Instance.SetWeaponIcon(newWeapon.equippedSprite, false);
        }

        weaponVisualHandler.SetWeaponSprite(PrimaryWeapon.equippedSprite);
    }

    // Swap Logic (weapon)

    public void SwapWeapons()
    {
        if (PrimaryWeapon == null || SecondaryWeapon == null)
        {
            return;
        }

        WeaponData temp = PrimaryWeapon;
        PrimaryWeapon = SecondaryWeapon;
        SecondaryWeapon = temp;

        UIManager.Instance.SetWeaponIcon(PrimaryWeapon.equippedSprite, true);
        UIManager.Instance.SetWeaponIcon(SecondaryWeapon.equippedSprite, false);
        weaponVisualHandler.SetWeaponSprite(PrimaryWeapon.equippedSprite);
    }
}
