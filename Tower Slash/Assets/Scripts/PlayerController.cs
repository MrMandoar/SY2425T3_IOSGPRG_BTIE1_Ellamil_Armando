using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Life System")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int currentLives;
    [SerializeField] private GameObject[] heartIcons;

    [Header("Dash System")]
    [SerializeField] private Button dashButton;
    [SerializeField] private Image dashGaugeBar;
    [SerializeField] private float dashDuration = 2f;

    private float dashGauge = 0f;
    private float dashGaugeMax = 100f;
    private bool isDashing = false;
    private float dashTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateHeartUI();
        UpdateDashGaugeUI();
        dashButton.gameObject.SetActive(false);
        dashButton.onClick.AddListener(ActivateDash);
    }

    private void Update()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            float percent = 1f - (dashTimer / dashDuration);
            dashGaugeBar.fillAmount = percent;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                dashGauge = 0f;
                UpdateDashGaugeUI();
            }
        }
    }

    public void TakeDamage()
    {
        if (isDashing) return;

        currentLives--;
        UpdateHeartUI();

        if (currentLives <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateHeartUI();
        }
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentLives);
        }
    }

    public void AddDashProgress(float amount)
    {
        if (isDashing) return;

        dashGauge += amount;
        dashGauge = Mathf.Clamp(dashGauge, 0, dashGaugeMax);
        UpdateDashGaugeUI();

        if (dashGauge >= dashGaugeMax)
        {
            dashButton.gameObject.SetActive(true);
        }
    }

    private void UpdateDashGaugeUI()
    {
        float fill = isDashing ? 1f - (dashTimer / dashDuration) : dashGauge / dashGaugeMax;
        dashGaugeBar.fillAmount = fill;
    }

    private void ActivateDash()
    {
        if (dashGauge < dashGaugeMax || isDashing) return;

        isDashing = true;
        dashTimer = 0f;
        dashButton.gameObject.SetActive(false);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                ScoreManager.Instance.AddScore(1);
                Destroy(enemy.gameObject);
            }
        }
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    
    public void ResetPlayer()
    {
        currentLives = maxLives;
        UpdateHeartUI();

        dashGauge = 0f;
        isDashing = false;
        dashTimer = 0f;

        dashButton.gameObject.SetActive(false);
        UpdateDashGaugeUI();
    }
}
