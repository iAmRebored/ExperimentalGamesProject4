using UnityEngine;
using UnityEngine.UI;

public class QueasyOverlay : MonoBehaviour
{
    public Image overlayImage;                  // Drag in the GreenOverlay image
    public Player PigStats;        
    public float maxAlpha = 0.5f;               // Max alpha at 100% fullness
    public float fadeSpeed = 1f;                // How quickly it fades

    private float currentAlpha = 0f;

    void Update()
    {
        float ratio = Mathf.Clamp01(PigStats.fullness / PigStats.maxFullness);
        Color c = overlayImage.color;
        c.a = ratio * maxAlpha;
        overlayImage.color = c;
    }
}
