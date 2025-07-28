using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = weaponData.worldSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player picked up weapon: " + weaponData.weaponName);
            PlayerInventory.Instance.PickUpWeapon(weaponData);
            Destroy(gameObject);
        }
    }

}
