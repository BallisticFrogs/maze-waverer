using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public WaveType waveType;

    public GameObject waveFront;
    public TrailRenderer trailRenderer;
    public float trailFadeTime = 1f;

    private float time;
    private float waveDeltaY;

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
        Vector3 dir = transform.forward;

        {
            // remove wave effect on local position
            Vector3 localPos = waveFront.transform.localPosition;
            localPos.y -= waveDeltaY;
            waveFront.transform.localPosition = localPos;
        }

        // move forward
        Vector3 pos = waveFront.transform.position;
        Vector3 delta = dir.normalized * speed * Time.deltaTime;
        pos += delta;
        waveFront.transform.position = pos;

        {
            // compute and apply new  wave effect
            Vector3 localPos = waveFront.transform.localPosition;
            float lastProgress = ((time - Time.deltaTime) % frequency) / frequency;
            float progress = (time % frequency) / frequency;
            float heightFactor = ComputeHeightFactor(lastProgress, progress);
            waveDeltaY = heightFactor * amplitude * waveFront.transform.localScale.y;
            localPos.y += waveDeltaY;
            waveFront.transform.localPosition = localPos;
        }
    }

    private void FadeTrail()
    {
        if (trailFadeTime > 0)
        {
            // compute alpha
            float progress = Mathf.Clamp(time / trailFadeTime, 0, 1);
            float alpha = 1f - progress;

            // update material color
            Color materialColor = trailRenderer.material.GetColor("_TintColor");
            materialColor.a = alpha;
            trailRenderer.material.SetColor("_TintColor", materialColor);
        }
    }

    private float ComputeHeightFactor(float lastProgress, float progress)
    {
        if (waveType == WaveType.SINE)
        {
            float radians = progress * Mathf.PI * 2;
            return Mathf.Sin(radians);
        }
        if (waveType == WaveType.SQUARE)
        {
            return progress < 0.5f ? -1 : 1;
        }
        if (waveType == WaveType.TRIANGLE)
        {
            if (progress < 0.25f)
            {
//                if (lastProgress > 0.25f) return 0;
                float subProgress = 1 - (0.25f - progress) / 0.25f;
                return subProgress;
            }
            else if (progress < 0.75f)
            {
//                if (lastProgress < 0.25f) return 1;
                float subProgress = 1 - (0.75f - progress) / 0.5f;
                return 1 - subProgress * 2f;
            }
            else
            {
//                if (lastProgress < 0.75f) return -1;
                float subProgress = 1 - (1f - progress) / 0.25f;
                return -1 + subProgress;
            }
        }

        return 0;
    }
}