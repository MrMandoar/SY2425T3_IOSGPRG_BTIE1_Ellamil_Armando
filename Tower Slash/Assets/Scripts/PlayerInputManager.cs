using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    private Vector2 _swipeStart;
    private Vector2 _swipeEnd;
    private bool _isSwiping;

    private readonly List<Enemy> _enemiesInVicinity = new();

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
        DetectKeyboardInput();
        DetectSwipeInput();
    }

    public void RegisterEnemyInVicinity(Enemy enemy)
    {
        if (!_enemiesInVicinity.Contains(enemy))
        {
            _enemiesInVicinity.Add(enemy);
        }
    }

    public void RemoveEnemyFromVicinity(Enemy enemy)
    {
        _enemiesInVicinity.Remove(enemy);
    }

    private void DetectKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) TryMatchInput(ArrowDirection.Up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) TryMatchInput(ArrowDirection.Down);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) TryMatchInput(ArrowDirection.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) TryMatchInput(ArrowDirection.Right);
    }

    private void DetectSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _isSwiping = true;
                _swipeStart = touch.position;
            }

            if (touch.phase == TouchPhase.Ended && _isSwiping)
            {
                _swipeEnd = touch.position;
                Vector2 swipeVector = _swipeEnd - _swipeStart;

                if (swipeVector.magnitude > 50f)
                {
                    ArrowDirection direction = GetSwipeDirection(swipeVector);
                    TryMatchInput(direction);
                }

                _isSwiping = false;
            }
        }
    }

    private ArrowDirection GetSwipeDirection(Vector2 swipe)
    {
        swipe.Normalize();

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            return swipe.x > 0 ? ArrowDirection.Right : ArrowDirection.Left;
        }
        else
        {
            return swipe.y > 0 ? ArrowDirection.Up : ArrowDirection.Down;
        }
    }

    private void TryMatchInput(ArrowDirection inputDirection)
    {
        List<Enemy> enemiesCopy = new(_enemiesInVicinity);

        foreach (Enemy enemy in enemiesCopy)
        {
            if (enemy == null || !enemy.IsInVicinity()) continue;

            ArrowDirection enemyDir = enemy.GetArrowDirection();
            bool isCorrect = false;

            if (enemy.IsGreen)
            {
                isCorrect = (enemyDir == inputDirection);
            }
            else if (enemy.IsRed)
            {
                isCorrect = (GetOppositeDirection(enemyDir) == inputDirection);
            }
            else if (enemy.IsYellow)
            {
                isCorrect = (enemyDir == inputDirection);
            }

            if (isCorrect)
            {
                if (enemy.GrantsExtraLife)
                {
                    PlayerController.Instance.AddLife();
                    ScoreManager.Instance.AddScore(5);
                }
                else
                {
                    ScoreManager.Instance.AddScore(1);
                }

                DashGaugeManager.Instance.AddGauge(5);

                Destroy(enemy.gameObject);
                _enemiesInVicinity.Remove(enemy);
            }
            else
            {
                PlayerController.Instance.TakeDamage();
            }

            break;
        }
    }

    private ArrowDirection GetOppositeDirection(ArrowDirection dir)
    {
        return dir switch
        {
            ArrowDirection.Up => ArrowDirection.Down,
            ArrowDirection.Down => ArrowDirection.Up,
            ArrowDirection.Left => ArrowDirection.Right,
            ArrowDirection.Right => ArrowDirection.Left,
            _ => dir
        };
    }
}
