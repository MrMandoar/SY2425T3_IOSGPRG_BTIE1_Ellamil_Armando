using UnityEngine;

public class TestSwipe : MonoBehaviour
{
    private void Update()
    {
        if (InputManager.Instance.CurrentSwipe != InputManager.SwipeDirection.None)
        {
            Debug.Log($"Detected: {InputManager.Instance.CurrentSwipe}");
        }
    }
}
