using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject healthBarCanvas; // Assign the entire prefab here

    private Image healthBarFill;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarCanvas != null)
        {
            // Automatically find the Image named "Fill" (or whatever your fill object is called)
            healthBarFill = healthBarCanvas.GetComponentInChildren<Image>();
        }

        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log($"Enemy took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(currentHealth < maxHealth); // Show only when damaged
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}
