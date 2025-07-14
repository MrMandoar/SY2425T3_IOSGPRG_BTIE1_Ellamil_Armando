using UnityEngine;

public class TowerScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;

    private Renderer _renderer;
    private Vector2 _offset;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _offset.y += scrollSpeed * Time.deltaTime;
        _renderer.material.mainTextureOffset = _offset;
    }
}
