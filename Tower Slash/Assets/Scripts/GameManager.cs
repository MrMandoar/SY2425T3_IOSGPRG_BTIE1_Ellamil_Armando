using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TMP_Text finalScoreText;

    private bool _isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
        gameOverCanvas.SetActive(false);
    }

    public void GameOver()
    {
        if (_isGameOver) return;

        _isGameOver = true;

        Time.timeScale = 0f;

        int finalScore = ScoreManager.Instance.GetCurrentScore();
        finalScoreText.text = $"Score: {finalScore}";

        gameOverCanvas.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
