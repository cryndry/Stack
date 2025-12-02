using UnityEngine;

class BlockColorManager : LazySingleton<BlockColorManager>
{
    private const float saturation = 0.75f;
    private const float minBrightness = 0.4f;
    private const float maxBrightness = 0.9f;
    private const float hueSpeed = 0.02f; // Hue change per block
    private const int brightnessPeriod = 15; // Blocks per Light/Dark cycle
    
    private int blockIndex;
    public float LastBlockHue { get; private set; }


    private void Awake()
    {
        blockIndex = Random.Range(0, brightnessPeriod);
        LastBlockHue = Random.value;
    }

    public void GetNextGradient(out Color firstColor, out Color secondColor)
    {
        blockIndex++;

        LastBlockHue = (LastBlockHue + hueSpeed) % 1.0f;
        float secondaryHue = (LastBlockHue + hueSpeed * 2f) % 1.0f;

        float brightnessRatio = Mathf.PingPong(blockIndex, brightnessPeriod) / brightnessPeriod;
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, brightnessRatio);

        firstColor = Color.HSVToRGB(LastBlockHue, saturation, brightness);
        secondColor = Color.HSVToRGB(secondaryHue, saturation, brightness);
    }
}