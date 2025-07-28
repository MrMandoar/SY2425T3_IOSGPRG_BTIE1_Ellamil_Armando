using UnityEngine;

public class BuildingRoofTrigger : MonoBehaviour
{
    [SerializeField] private GameObject roof;

    private SpriteRenderer roofRenderer;

    private void Start()
    {
        if (roof == null)
        {
            Debug.LogError("Roof object not assigned!");
            return;
        }

        roofRenderer = roof.GetComponent<SpriteRenderer>();
        if (roofRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the roof object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetRoofTransparency(0.1f); // fade out
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetRoofTransparency(1f); // fade in
        }
    }

    private void SetRoofTransparency(float alpha)
    {
        if (roofRenderer != null)
        {
            Color color = roofRenderer.color;
            color.a = alpha;
            roofRenderer.color = color;
        }
    }
}
