using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Ammo UI")]
    [SerializeField] private TMP_Text ammo9mmText;
    [SerializeField] private TMP_Text ammo12gText;
    [SerializeField] private TMP_Text ammo556Text;

    [Header("Weapon UI")]
    [SerializeField] private Image primaryWeaponIcon;
    [SerializeField] private Image secondaryWeaponIcon;

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

    // health
    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    // ammo
    public void UpdateAmmo(string ammoType, int amount)
    {
        switch (ammoType)
        {
            case "9mm":
                ammo9mmText.text = amount.ToString();
                break;
            case "12g":
                ammo12gText.text = amount.ToString();
                break;
            case "556":
                ammo556Text.text = amount.ToString();
                break;
        }
    }

    // weapon icons 
    public void SetWeaponIcon(Sprite weaponSprite, bool isPrimary)
    {
        if (isPrimary)
        {
            primaryWeaponIcon.sprite = weaponSprite;
        }
        else
        {
            secondaryWeaponIcon.sprite = weaponSprite;
        }
    }

    public void ShiftSecondaryToPrimary()
    {
        primaryWeaponIcon.sprite = secondaryWeaponIcon.sprite;
    }
}
