using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 25f;
    private int damage;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate the bullet to face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D || collision is CircleCollider2D ||
            collision is PolygonCollider2D || collision is EdgeCollider2D)
        {
            Destroy(gameObject);
        }
    }

    public int GetDamage()
    {
        return damage;
    }
}
