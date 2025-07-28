using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchButton : MonoBehaviour
{
    [SerializeField] private bool isPrimaryButton; // true - primary, false - secondary

    public void OnWeaponSlotClicked()
    {
        PlayerInventory.Instance.SwapWeapons();
    }
}

