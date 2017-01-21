using UnityEngine;

public class Vibrator : MonoBehaviour
{
    public float frequency = 1f;
    public float scaleFactor = 1f;

    private Vector3 initialScale;
    private float time;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        time += Time.deltaTime;
        float progress = (time % frequency) / frequency;
        float scaleVibrationFactor = ComputeScaleFactor(progress);
        transform.localScale = initialScale + initialScale * scaleFactor * scaleVibrationFactor;
    }

    private float ComputeScaleFactor(float progress)
    {
        float radians = progress * Mathf.PI * 2;
        return Mathf.Sin(radians);
    }
}