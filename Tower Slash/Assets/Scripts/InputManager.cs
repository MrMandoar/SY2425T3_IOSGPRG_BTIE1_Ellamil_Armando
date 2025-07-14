using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public SwipeDirection CurrentSwipe { get; private set; }

    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _minSwipeDist = 50f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        DetectSwipe();
        DetectKeyPress();
    }

    private void DetectSwipe()
    {
        CurrentSwipe = SwipeDirection.None;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            CheckSwipeDirection();
        }
#else
        // Actual mobile swipe detection
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _endPos = touch.position;
                CheckSwipeDirection();
            }
        }
#endif
    }

    private void DetectKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentSwipe = SwipeDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentSwipe = SwipeDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentSwipe = SwipeDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentSwipe = SwipeDirection.Right;
        }
    }

    private void CheckSwipeDirection()
    {
        Vector2 swipe = _endPos - _startPos;

        if (swipe.magnitude < _minSwipeDist)
        {
            CurrentSwipe = SwipeDirection.None;
            return;
        }

        swipe.Normalize();

        if (Vector2.Dot(swipe, Vector2.up) > 0.7f)
        {
            CurrentSwipe = SwipeDirection.Up;
        }
        else if (Vector2.Dot(swipe, Vector2.down) > 0.7f)
        {
            CurrentSwipe = SwipeDirection.Down;
        }
        else if (Vector2.Dot(swipe, Vector2.left) > 0.7f)
        {
            CurrentSwipe = SwipeDirection.Left;
        }
        else if (Vector2.Dot(swipe, Vector2.right) > 0.7f)
        {
            CurrentSwipe = SwipeDirection.Right;
        }
    }
}
