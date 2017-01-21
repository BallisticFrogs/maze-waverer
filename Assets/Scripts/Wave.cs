using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;
    public float frequency = 1f;

    public GameObject waveFront;
    public TrailRenderer trailRenderer;
    public float trailFadeTime = 1f;

    private float time;

    void Awake()
    {
    }

    void Update()
    {
        time += Time.deltaTime;
        Move();
        FadeTrail();
    }

    private void Move()
    {
        Vector3 pos = waveFront.transform.localPosition;
        Vector3 dir = transform.forward;

        // move forward
        Vector3 delta = dir.normalized * speed * Time.deltaTime;
        pos += delta;

        // wave
        float progress = (time % frequency) / frequency;
        pos.y = ComputeHeightFactor(progress) * amplitude;

        waveFront.transform.localPosition = pos;
    }

    private void FadeTrail()
    {
        // compute alpha
        float progress = Mathf.Clamp(time / trailFadeTime, 0, 1);
        float alpha = 1f - progress;

        // update material color
        Color materialColor = trailRenderer.material.GetColor("_TintColor");
        materialColor.a = alpha;
        trailRenderer.material.SetColor("_TintColor", materialColor);
    }

    private float ComputeHeightFactor(float progress)
    {
        float radians = progress * Mathf.PI * 2;
        return Mathf.Sin(radians);
    }
}