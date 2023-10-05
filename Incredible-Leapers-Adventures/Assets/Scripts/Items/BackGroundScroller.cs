using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float xScrollSpeed = 10;
    [SerializeField] private float yScrollSpeed = 10;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(xScrollSpeed, yScrollSpeed) * Time.deltaTime;
    }
}
