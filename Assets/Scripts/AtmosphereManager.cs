using UnityEngine;

public class AtmosphereManager : LazySingleton<AtmosphereManager>
{
    [SerializeField] private Camera mainCamera;

    private const float colorChangeSpeed = 1.0f;
    private const float saturation = 0.1f;
    private const float brightness = 0.9f;

    private Color? targetColor;
    private float transitionProgress;


    private void Start()
    {
        SetAtmosphereColor();
        InitializeFog();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnBlockGenerated += SetAtmosphereColor;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBlockGenerated -= SetAtmosphereColor;
    }

    private void Update()
    {
        if (targetColor == null) return;

        transitionProgress = Mathf.Clamp01(transitionProgress + Time.deltaTime * colorChangeSpeed);

        Color newColor = Color.Lerp(
            mainCamera.backgroundColor,
            targetColor.Value,
            transitionProgress
        );

        mainCamera.backgroundColor = newColor;

        if (transitionProgress >= 1.0f)
        {
            targetColor = null;
        }
    }

    private void InitializeFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(0.2f, 0.2f, 0.2f);
        RenderSettings.fogStartDistance = mainCamera.nearClipPlane;
        RenderSettings.fogEndDistance = mainCamera.farClipPlane;
    }

    private void SetAtmosphereColor()
    {
        float hue = BlockColorManager.Instance.LastBlockHue;
        targetColor = Color.HSVToRGB(hue, saturation, brightness);
        transitionProgress = 0f;
    }
}
