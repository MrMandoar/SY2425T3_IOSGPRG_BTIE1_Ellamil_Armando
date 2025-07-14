using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    public WeaponData Data => weaponData;

    public void Initialize(WeaponData newData)
    {
        weaponData = newData;
        GetComponentInChildren<SpriteRenderer>().sprite = weaponData.equippedSprite;
    }
}
