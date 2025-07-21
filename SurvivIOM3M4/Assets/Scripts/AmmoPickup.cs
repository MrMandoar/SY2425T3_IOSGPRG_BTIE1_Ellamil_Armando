using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Tooltip("Accepted values: 9mm, 12g, 556")]
    public string ammoType; 

    private int GetRandomAmount()
    {
        switch (ammoType)
        {
            case "9mm":
                return Random.Range(1, 9); // 1 to 8
            case "556":
                return Random.Range(5, 16); // 5 to 15
            case "12g":
                return Random.Range(1, 3); // 1 to 2
            default:
                Debug.LogWarning("Unknown ammo type: " + ammoType);
                return 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int amount = GetRandomAmount();
            if (amount > 0)
            {
                PlayerInventory.Instance.AddAmmo(ammoType, amount);
                Debug.Log($"Picked up {amount} rounds of {ammoType}");
            }

            Destroy(gameObject);
        }
    }
}
