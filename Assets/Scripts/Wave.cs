using UnityEngine;

public class Wave : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public WaveType waveType;

    public GameObject waveFront;
    public LineRenderer trailRenderer;
    public int trailMaxPoints = 10;
    public float trailFadeTime = 1f;

    private float time;
    private float waveDeltaY;
    private Vector3 baseLocalPos;
    private Vector3 lastBaseLocalPos;

    private Vector3[] trailPoints;
    private int actualTrailPoints;
    private float lastTrailUpdateTime;


    // TODO handle combined wave types
    // this field should not even exist
    // but rendering a wave with combined types will be tough...
    private WaveType visualWaveType;

    void Start()
    {
        baseLocalPos = waveFront.transform.localPosition;
        lastBaseLocalPos = waveFront.transform.localPosition;

        trailPoints = new Vector3[trailMaxPoints];
        trailRenderer.numPositions = trailMaxPoints;
        trailRenderer.enabled = false;

        if ((waveType & WaveType.TRIANGLE) != 0) visualWaveType = WaveType.TRIANGLE;
        else if ((waveType & WaveType.SQUARE) != 0) visualWaveType = WaveType.SQUARE;
        else if ((waveType & WaveType.SINE) != 0) visualWaveType = WaveType.SINE;

        if (visualWaveType == WaveType.TRIANGLE) trailRenderer.numCornerVertices = 0;
        if (visualWaveType == WaveType.SQUARE) trailRenderer.numCornerVertices = 0;
        if (visualWaveType == WaveType.SINE) trailRenderer.numCornerVertices = 3;
    }

    void Update()
    {
        time += Time.deltaTime;
        Move();
        UpdateTrail();
        FadeTrail();
    }

    private void Move()
    {
        if (waveFront == null) return;

        Vector3 dir = transform.forward;

        // remove wave effect on local position
        waveFront.transform.localPosition = baseLocalPos;

        // move forward
        Vector3 pos = waveFront.transform.position;
        Vector3 delta = dir.normalized * speed * Time.deltaTime;
        pos += delta;
        waveFront.transform.position = pos;
        lastBaseLocalPos = baseLocalPos;
        baseLocalPos = waveFront.transform.localPosition;

        // compute and apply new wave effect
        Vector3 localPos = waveFront.transform.localPosition;
        float progress = (time % frequency) / frequency;
        float displacement = ComputeLocalDisplacement(progress);
        waveDeltaY = displacement * amplitude * waveFront.transform.localScale.y;
        waveFront.transform.localPosition = new Vector3(localPos.x, localPos.y + waveDeltaY, localPos.z);
    }

    private void UpdateTrail()
    {
        if (waveFront == null) return;
        if (time - lastTrailUpdateTime < 0.0f) return;

        float progress = (time % frequency) / frequency;
        float lastProgress = ((time - Time.deltaTime) % frequency) / frequency;

        // check if we will need an extra point because the wave has a mandatory breakpoint between last frame and this one
        float additionalPointProgress = -1;
        if (actualTrailPoints > 0)
        {
            additionalPointProgress = ComputeProgressForLineBreakpoint(lastProgress, progress);
        }

        // retreive points and shift them, loosing the oldest one(s)
        int pointsToAdd = additionalPointProgress >= 0 ? 2 : 1;
        trailRenderer.GetPositions(trailPoints);
        for (int i = trailMaxPoints - 1 - pointsToAdd; i >= 0; i--)
        {
            trailPoints[i + pointsToAdd] = trailPoints[i];
        }

        trailPoints[0] = waveFront.transform.position;

        if (additionalPointProgress >= 0)
        {
            Vector3 backupTransformLocalPosition = waveFront.transform.localPosition;

            float normalizedProgress = progress < lastProgress ? progress + 1 : progress;
            float ratio = (additionalPointProgress - lastProgress) / (normalizedProgress - lastProgress);
            Vector3 lerpedLocPos = Vector3.Lerp(lastBaseLocalPos, baseLocalPos, ratio);

            Vector3 locPosToUse = lerpedLocPos;
            if (visualWaveType == WaveType.SQUARE) locPosToUse = baseLocalPos;

            float bonusDisplacement = ComputeLocalDisplacement(additionalPointProgress);
            float bonusDeltaY = bonusDisplacement * amplitude * waveFront.transform.localScale.y;
            waveFront.transform.localPosition = new Vector3(locPosToUse.x, locPosToUse.y + bonusDeltaY,
                locPosToUse.z);
            trailPoints[1] = waveFront.transform.position;

            waveFront.transform.localPosition = backupTransformLocalPosition;
        }

        // copy the last point until the end of the array (so the segments are not rendered)
        actualTrailPoints = Mathf.Min(trailMaxPoints, actualTrailPoints + pointsToAdd);
        for (int i = actualTrailPoints; i < trailMaxPoints; i++)
        {
            trailPoints[i] = trailPoints[actualTrailPoints - 1];
        }

//            Debug.Log(trailPoints[0]);

        // update line
        if (actualTrailPoints > 0)
        {
            trailRenderer.enabled = true;
            trailRenderer.SetPositions(trailPoints);
        }

        lastTrailUpdateTime = time;
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

    private float ComputeLocalDisplacement(float progress)
    {
        if (visualWaveType == WaveType.SINE)
        {
            float radians = progress * Mathf.PI * 2;
            return Mathf.Sin(radians);
        }
        if (visualWaveType == WaveType.SQUARE)
        {
            return progress <= 0.5f ? -1 : 1;
        }
        if (visualWaveType == WaveType.TRIANGLE)
        {
            if (progress < 0.25f)
            {
                float subProgress = 1 - (0.25f - progress) / 0.25f;
                return subProgress;
            }
            else if (progress < 0.75f)
            {
                float subProgress = 1 - (0.75f - progress) / 0.5f;
                return 1 - subProgress * 2f;
            }
            else
            {
                float subProgress = 1 - (1f - progress) / 0.25f;
                return -1 + subProgress;
            }
        }

        return 0;
    }

    private float ComputeProgressForLineBreakpoint(float lastProgress, float progress)
    {
        if (visualWaveType == WaveType.SQUARE)
        {
            if (lastProgress > 0.5f && progress < 0.5f) return 1f;
            if (lastProgress < 0.5f && progress > 0.5f) return 0.5f;
        }
        if (visualWaveType == WaveType.TRIANGLE)
        {
            if (lastProgress > 0.25f && progress < 0.25f) return 1f;
            if (lastProgress < 0.25f && progress > 0.25f) return 0.25f;
            if (lastProgress < 0.75f && progress > 0.75f) return 0.75f;
        }
        return -1;
    }
}