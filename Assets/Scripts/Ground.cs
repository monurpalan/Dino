using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private float textureScale = 1f;
    private MeshRenderer meshRenderer;
    private Material groundMaterial;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Update()
    {
        ScrollTexture();
    }

    private void InitializeComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        groundMaterial = meshRenderer.material;
    }

    private void ScrollTexture()
    {
        float scrollSpeed = CalculateScrollSpeed();
        Vector2 offset = Vector2.right * scrollSpeed * Time.deltaTime;
        groundMaterial.mainTextureOffset += offset;
    }

    private float CalculateScrollSpeed()
    {
        // Kaydırma hızı, oyun hızına bağlı olarak ve zeminin ölçeği ile doku ölçeğine göre hesaplanır
        return GameManager.instance.gameSpeed / (transform.localScale.x * textureScale);
    }
}