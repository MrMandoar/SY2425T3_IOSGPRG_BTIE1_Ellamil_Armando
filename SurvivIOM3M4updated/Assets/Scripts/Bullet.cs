using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 20f;
    private float lifetime = 3f;

    private int damage;
    private GameObject shooter;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetShooter(GameObject shooterObj)
    {
        shooter = shooterObj;
    }

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.gameObject == shooter) return; // Don't hit self
        if (shooter == null)
        {
            Debug.LogWarning("Bullet: Shooter is null. Cannot determine target.");
            Destroy(gameObject);
            return;
        }

        // If the shooter is Enemy and hits Player
        if (shooter.CompareTag("Enemy") && collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Enemy bullet hit Player for {damage} damage.");
            }
        }
        // If the shooter is Player and hits Enemy
        else if (shooter.CompareTag("Player") && collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Player bullet hit Enemy for {damage} damage.");
            }
        }

        // Destroy bullet on impact with any 2D collider 
        if (collision is BoxCollider2D || collision is CircleCollider2D ||
            collision is EdgeCollider2D || collision is PolygonCollider2D)
        {
            Destroy(gameObject);
        }
    }
}
