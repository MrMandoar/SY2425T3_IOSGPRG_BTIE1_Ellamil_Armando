using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;

    //da bounds 
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private float camHeight;
    private float camWidth;

    private void Start()
    {
        Camera cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = cam.aspect * camHeight;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        float clampedX = Mathf.Clamp(desiredPosition.x, minX + camWidth, maxX - camWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY + camHeight, maxY - camHeight);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        transform.position = Vector3.Lerp(transform.position, clampedPosition, followSpeed * Time.deltaTime);
    }
}
