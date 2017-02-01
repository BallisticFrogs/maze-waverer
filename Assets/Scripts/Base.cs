using UnityEngine;

public class Base : MonoBehaviour
{
    public Collider teleportCollider;
    public WaveType defaultWaveType;

    public BaseTube baseTubeCircle;
    public BaseTube baseTubeSquare;
    public BaseTube baseTubeTriangle;
    public BaseTube baseTubeCircleSquare;
    public BaseTube baseTubeCircleTriangle;
    public BaseTube baseTubeSquareTriangle;
    public BaseTube baseTubeCircleSquareTriangle;

    private WaveType waveType;

    [HideInInspector]
    public WaveType WaveType
    {
        get { return waveType; }
        set
        {
            waveType = value;
            UpdateAspect();
        }
    }

    private bool playerHere;
    private BaseTube currentBaseTube;

    private void Awake()
    {
        if (baseTubeCircle != null) baseTubeCircle.gameObject.SetActive(false);
        if (baseTubeSquare != null) baseTubeSquare.gameObject.SetActive(false);
        if (baseTubeTriangle != null) baseTubeTriangle.gameObject.SetActive(false);
        if (baseTubeCircleSquare != null) baseTubeCircleSquare.gameObject.SetActive(false);
        if (baseTubeCircleTriangle != null) baseTubeCircleTriangle.gameObject.SetActive(false);
        if (baseTubeSquareTriangle != null) baseTubeSquareTriangle.gameObject.SetActive(false);
        if (baseTubeCircleSquareTriangle != null) baseTubeCircleSquareTriangle.gameObject.SetActive(false);

        WaveType = defaultWaveType;
    }

    private void UpdateAspect()
    {
//        if (GetComponent<StartBase>() != null) return;
//        if (GetComponent<EndBase>() != null) return;

        if (currentBaseTube != null)
        {
            currentBaseTube.gameObject.SetActive(false);
        }

        if (WaveType == WaveType.SINE) currentBaseTube = baseTubeCircle;
        if (WaveType == WaveType.SQUARE) currentBaseTube = baseTubeSquare;
        if (WaveType == WaveType.TRIANGLE) currentBaseTube = baseTubeTriangle;

        if (WaveType == (WaveType.SINE | WaveType.SQUARE)) currentBaseTube = baseTubeCircleSquare;
        if (WaveType == (WaveType.SINE | WaveType.TRIANGLE)) currentBaseTube = baseTubeCircleTriangle;
        if (WaveType == (WaveType.SQUARE | WaveType.TRIANGLE)) currentBaseTube = baseTubeSquareTriangle;

        if (WaveType == (WaveType.SQUARE | WaveType.TRIANGLE | WaveType.SINE))
            currentBaseTube = baseTubeCircleSquareTriangle;

        if (currentBaseTube != null)
        {
            currentBaseTube.gameObject.SetActive(true);
            UpdateTubeVisibility();
        }
    }

    public void OnPlayerLeft()
    {
        playerHere = false;
        UpdateTubeVisibility();
    }

    public void OnPlayerArrived()
    {
        playerHere = true;
        UpdateTubeVisibility();
    }

    private void UpdateTubeVisibility()
    {
        if (currentBaseTube != null)
        {
            currentBaseTube.tube.SetActive(!playerHere);
        }
    }
}