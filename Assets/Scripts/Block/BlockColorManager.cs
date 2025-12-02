using UnityEngine;

class BlockColorManager : LazySingleton<BlockColorManager>
{
    private const float saturation = 0.75f;
    private const float minBrightness = 0.4f;
    private const float maxBrightness = 0.9f;
    private const float hueSpeed = 0.02f; // Hue change per block
    private const int brightnessPeriod = 15; // Blocks per Light/Dark cycle
    
    private int blockIndex;
    private float hue;


    private void Awake()
    {
        blockIndex = Random.Range(0, brightnessPeriod);
        hue = Random.value;
    }

    public void GetNextGradient(out Color firstColor, out Color secondColor)
    {
        blockIndex++;

        hue = (hue + hueSpeed) % 1.0f;
        float secondaryHue = (hue + hueSpeed * 2f) % 1.0f;

        float brightnessRatio = Mathf.PingPong(blockIndex, brightnessPeriod) / brightnessPeriod;
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, brightnessRatio);

        firstColor = Color.HSVToRGB(hue, saturation, brightness);
        secondColor = Color.HSVToRGB(secondaryHue, saturation, brightness);
    }
}