using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("Victory UI")]
    [SerializeField] private GameObject victoryScreen;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();

    private void Start()
    {
        Time.timeScale = 1f;

        if (victoryScreen != null)
            victoryScreen.SetActive(false);
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyHealth enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

        if (enemies.Count == 0)
        {
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        Debug.Log("[VICTORY] All enemies defeated!");
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
