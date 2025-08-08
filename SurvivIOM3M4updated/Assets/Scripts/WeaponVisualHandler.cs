using UnityEngine;

public class WeaponVisualHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 2; // layering for the guns
    }

    public void SetWeaponSprite(Sprite weaponSprite)
    {
        spriteRenderer.sprite = weaponSprite;
    }

    public void ClearWeapon()
    {
        spriteRenderer.sprite = null;
    }

    public void RotateToward(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
