using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0);

    private Transform enemy;

    private void Awake()
    {
        enemy = transform.parent;
    }

    private void LateUpdate()
    {
        // Always face the camera and follow the enemy
        transform.position = enemy.position + offset;
        transform.rotation = Quaternion.identity; 
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
