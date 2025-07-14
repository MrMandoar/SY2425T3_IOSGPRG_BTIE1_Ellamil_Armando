using UnityEngine;
using UnityEngine.UI;

public class DashGaugeManager : MonoBehaviour
{
    public static DashGaugeManager Instance { get; private set; }

    [SerializeField] private Button dashButton;
    [SerializeField] private Image dashGaugeFill;
    [SerializeField] private float dashDuration = 2f;

    private float _dashGauge;
    private const float MaxGauge = 100f;
    private bool _isDashing;
    private float _dashTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dashButton != null)
        {
            dashButton.onClick.AddListener(ActivateDash);
            dashButton.gameObject.SetActive(false); 
        }

        UpdateGaugeUI();
    }

    private void Update()
    {
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
            }
        }
        if (dashButton != null)
        {
            dashButton.gameObject.SetActive(_dashGauge >= MaxGauge && !_isDashing);
        }
    }

    public void AddGauge(float amount)
    {
        if (_isDashing) return;

        _dashGauge = Mathf.Min(_dashGauge + amount, MaxGauge);
        UpdateGaugeUI();
    }

    private void ActivateDash()
    {
        if (_dashGauge < MaxGauge || _isDashing) return;

        _isDashing = true;
        _dashTimer = dashDuration;
        _dashGauge = 0f;
        UpdateGaugeUI();

        // Kill all enemies currently on screen
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                ScoreManager.Instance.AddScore(1);
                Destroy(enemy.gameObject);
            }
        }

        if (dashButton != null)
        {
            dashButton.gameObject.SetActive(false);
        }
    }

    private void UpdateGaugeUI()
    {
        if (dashGaugeFill != null)
        {
            dashGaugeFill.fillAmount = _dashGauge / MaxGauge;
        }
    }

    public bool IsDashing()
    {
        return _isDashing;
    }
}
