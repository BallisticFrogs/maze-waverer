using UnityEngine;

public class BaseTube : MonoBehaviour
{
    public GameObject tube;

    public GameObject rotatingBase;
    public float rotationSpeed = 30f;

    public GameObject[] oscillatingObjects;
    public float oscillationFrequency = 1f;

    private float time;
    private float[] baseOscillatingObjectsHeight;

    void Awake()
    {
        baseOscillatingObjectsHeight = new float[oscillatingObjects.Length];

//        float baseHeight = 0.1f;
        float availableHeight = 1f;
        float splits = oscillatingObjects.Length;
        float splitHeight = availableHeight / splits;
        for (int i = 0; i < oscillatingObjects.Length; i++)
        {
            GameObject currObj = oscillatingObjects[i];
            baseOscillatingObjectsHeight[i] = currObj.transform.localPosition.y + splitHeight * 0.5f + splitHeight * i;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        // rotate base
        rotatingBase.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // oscillate objects in the tube
        float progress = (time % oscillationFrequency) / oscillationFrequency;
        float radians = progress * Mathf.PI * 2;
        float deltaY = Mathf.Sin(radians);

        for (int i = 0; i < oscillatingObjects.Length; i++)
        {
            GameObject currObj = oscillatingObjects[i];
            var pos = currObj.transform.localPosition;
            pos.y = baseOscillatingObjectsHeight[i] + deltaY * 0.1f;
            currObj.transform.localPosition = pos;
        }
    }
}