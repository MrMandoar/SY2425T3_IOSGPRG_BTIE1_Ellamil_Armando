using UnityEngine;

public class VicinityZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.SetVicinityState(true);
            PlayerInputManager.Instance.RegisterEnemyInVicinity(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.SetVicinityState(false);
            PlayerInputManager.Instance.RemoveEnemyFromVicinity(enemy);
        }
    }
}
