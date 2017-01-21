using UnityEngine;

public class WaveFront : MonoBehaviour
{
    public Collider waveCollider;

    private Wave wave;

    void Awake()
    {
        wave = GetComponentInParent<Wave>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // handle contact with base : teleport
        Base baseComponent = other.GetComponentInParent<Base>();
        if (baseComponent != null && baseComponent != Player.INSTANCE.currentBase)
        {
            Player.INSTANCE.TeleportToBase(baseComponent);

            // kill the wave
            wave.gameObject.DestroyRecursive();
            return;
        }

        // handle contact with other vibrating objects
        WaveEmitter waveEmitter = other.GetComponentInParent<WaveEmitter>();
        if (waveEmitter != null)
        {
            if (waveEmitter.waveType != wave.waveType)
            {
                if (waveEmitter.canAbsorbWaveType)
                {
                    // change wave type of the object
                    waveEmitter.waveType = wave.waveType;
                }

                // kill the wave
                // wave.gameObject.DestroyRecursive();
                gameObject.DestroyRecursive();
            }
        }
    }
}