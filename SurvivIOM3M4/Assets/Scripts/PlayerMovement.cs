using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -100f;
    [SerializeField] private float maxX = 100f;
    [SerializeField] private float minY = -60;
    [SerializeField] private float maxY = 60f;

    [Header("Aiming")]
    [SerializeField] private Joystick aimJoystick;
    [SerializeField] private Transform weaponHolder;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (movementJoystick == null)
        {
            Debug.LogError("Movement Joystick not assigned!");
        }

        if (aimJoystick == null)
        {
            Debug.LogError("Aim Joystick not assigned!");
        }

        if (weaponHolder == null)
        {
            Debug.LogError("Weapon Holder not assigned!");
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleAiming();
    }

    private void HandleMovement()
    {
        if (movementJoystick == null || rb == null)
        {
            return;
        }

        float x = movementJoystick.Horizontal;
        float y = movementJoystick.Vertical;

        Vector2 input = new Vector2(x, y);

        if (input.magnitude < 0.01f)
        {
            return;
        }

        Vector2 newPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rb.MovePosition(newPos);
    }

    private void HandleAiming()
    {
        if (aimJoystick == null || weaponHolder == null)
        {
            return;
        }

        Vector2 aimInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (aimInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
            weaponHolder.rotation = Quaternion.Euler(0f, 0f, angle);

           
        }
    }
}
