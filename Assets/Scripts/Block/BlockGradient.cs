using UnityEngine;

public class BlockGradient : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color topColor = Color.magenta;
    [SerializeField] private Color bottomColor = Color.blue;


    private void Awake()
    {
        ApplyGradient();
    }

    [ContextMenu("Apply Gradient")]
    private void ApplyGradient()
    {
        Texture2D texture = new Texture2D(1, 256);
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < texture.height; y++)
        {
            float t = (float)y / (texture.height - 1);
            Color color = Color.Lerp(bottomColor, topColor, t);
            texture.SetPixel(0, y, color);
        }

        texture.Apply();

        meshRenderer.material.mainTexture = texture;
    }
}