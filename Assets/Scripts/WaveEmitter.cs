using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
    public GameObject wavePrefab;
    public float emissionInterval = 1f;
    public WaveType waveType;

    private float chargeTime = float.MaxValue;

    void Update()
    {
        if (emissionInterval > 0)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime >= emissionInterval)
            {
                chargeTime = 0;

                // create new wave
                Quaternion forward = Quaternion.Euler(Random.Range(-10, 20), Random.Range(0, 360), 0);
                EmitWave(forward, 0.5f);
            }
        }
    }

    public void EmitWave(Quaternion dir, float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        GameObject obj = Instantiate(wavePrefab, pos, dir);

        // update wave type
        Wave wave = obj.GetComponent<Wave>();
        wave.waveType = waveType;
    }
}