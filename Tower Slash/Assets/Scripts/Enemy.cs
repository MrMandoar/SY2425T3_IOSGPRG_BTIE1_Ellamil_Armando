using UnityEngine;

public enum ArrowDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum ArrowColor
{
    Green,
    Red,
    Yellow
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private SpriteRenderer enemyRenderer; 
    [SerializeField] private Sprite arrowBackgroundSprite; 

    [SerializeField] private ArrowDirection arrowDirection;
    [SerializeField] private ArrowColor arrowColor = ArrowColor.Green;

    private GameObject _arrowInstance;
    private GameObject _arrowBackgroundObject;
    private SpriteRenderer _arrowRenderer;
    private SpriteRenderer _arrowBackgroundRenderer;

    private bool _isPlayerInVicinity;
    private float _yellowCycleTimer;
    private float _yellowCycleInterval = 0.2f;
    private bool _grantsExtraLife;

    private void Start()
    {
        SpawnArrow();
        DetermineSpecialStatus();
    }

    private void Update()
    {
        MoveDownward();

        if (IsYellow && !_isPlayerInVicinity)
        {
            HandleYellowArrowCycle();
        }
    }

    // Public Functions

    public void SetVicinityState(bool state)
    {
        _isPlayerInVicinity = state;

        if (_arrowRenderer != null)
        {
            _arrowRenderer.enabled = true;
        }

        if (_arrowBackgroundObject != null)
        {
            _arrowBackgroundObject.SetActive(state);
        }
    }

    public ArrowDirection GetArrowDirection() => arrowDirection;
    public bool IsGreen => arrowColor == ArrowColor.Green;
    public bool IsRed => arrowColor == ArrowColor.Red;
    public bool IsYellow => arrowColor == ArrowColor.Yellow;
    public bool GrantsExtraLife => _grantsExtraLife;
    public bool IsInVicinity() => _isPlayerInVicinity;

    public void SetArrowColor(ArrowColor color) => arrowColor = color;
    public void SetArrowDirection(ArrowDirection dir) => arrowDirection = dir;

    // Private Functions

    private void MoveDownward()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

    private void SpawnArrow()
    {
        _arrowInstance = new GameObject("Arrow");
        _arrowInstance.transform.SetParent(transform);
        _arrowInstance.transform.localPosition = new Vector3(-1f, 0f, 0f);

        _arrowBackgroundObject = new GameObject("ArrowBackground");
        _arrowBackgroundObject.transform.SetParent(_arrowInstance.transform);
        _arrowBackgroundObject.transform.localPosition = Vector3.zero;
        _arrowBackgroundRenderer = _arrowBackgroundObject.AddComponent<SpriteRenderer>();
        _arrowBackgroundRenderer.sprite = arrowBackgroundSprite;
        _arrowBackgroundRenderer.sortingOrder = 0; 
        _arrowBackgroundObject.SetActive(false);

        
        _arrowRenderer = _arrowInstance.AddComponent<SpriteRenderer>();
        _arrowRenderer.sprite = ArrowSpriteManager.Instance.GetArrowSprite(arrowDirection);
        _arrowRenderer.color = GetArrowColor();
        _arrowRenderer.sortingOrder = 1; 
    }

    private Color GetArrowColor()
    {
        return arrowColor switch
        {
            ArrowColor.Green => Color.green,
            ArrowColor.Red => Color.red,
            ArrowColor.Yellow => Color.yellow,
            _ => Color.white
        };
    }

    private void HandleYellowArrowCycle()
    {
        _yellowCycleTimer += Time.deltaTime;

        if (_yellowCycleTimer >= _yellowCycleInterval)
        {
            _yellowCycleTimer = 0f;
            arrowDirection = (ArrowDirection)Random.Range(0, 4);
            if (_arrowRenderer != null)
            {
                _arrowRenderer.sprite = ArrowSpriteManager.Instance.GetArrowSprite(arrowDirection);
            }
        }
    }

    private void DetermineSpecialStatus()
    {
        _grantsExtraLife = Random.value <= 0.03f;

        if (_grantsExtraLife && enemyRenderer != null)
        {
            enemyRenderer.color = Color.white;
        }
    }
}
