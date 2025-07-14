using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/New Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;

    [Header("Sprites")]
    public Sprite worldSprite;       // ground loot
    public Sprite equippedSprite;    // when equipped

    [Header("Stats")]
    public string ammoType;          // "9mm", "12g", "556"
    public int damage;
    public float fireRate;           // Shots per second
    public int magazineSize;         // Max bullets before reload
    public float reloadTime;         // reload time

    [Header("Prefabs")]
    public GameObject bulletPrefab;  // Bullet spawned when pew pew
}
