using UnityEngine;

public class ArrowSpriteManager : MonoBehaviour
{
    public static ArrowSpriteManager Instance { get; private set; }

    [Header("Arrow Sprites")]
    [SerializeField] private Sprite upArrow;
    [SerializeField] private Sprite downArrow;
    [SerializeField] private Sprite leftArrow;
    [SerializeField] private Sprite rightArrow;

    [Header("Arrow Background")]
    [SerializeField] private Sprite arrowBackgroundBox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Sprite GetArrowSprite(ArrowDirection direction)
    {
        return direction switch
        {
            ArrowDirection.Up => upArrow,
            ArrowDirection.Down => downArrow,
            ArrowDirection.Left => leftArrow,
            ArrowDirection.Right => rightArrow,
            _ => null
        };
    }

    public Sprite GetBackgroundBoxSprite()
    {
        return arrowBackgroundBox;
    }
}
