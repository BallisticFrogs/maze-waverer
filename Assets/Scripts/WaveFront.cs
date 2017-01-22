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
            GameController.INSTANCE.TeleportPlayerToBase(baseComponent);

            // kill the wave
            wave.gameObject.DestroyRecursive();
            return;
        }

        // handle contact with other vibrating objects
        WaveEmitter waveEmitter = other.GetComponentInParent<WaveEmitter>();
        if (waveEmitter != null && waveEmitter.enabled)
        {
            if (waveEmitter.waveType == WaveType.FLAT || waveEmitter.waveType != wave.waveType)
            {
                if (waveEmitter.canAbsorbWaveType)
                {
                    // change wave type of the object
                    waveEmitter.ChangeWaveType(wave.waveType);
                }

                // kill the wave
                wave.gameObject.DestroyRecursive();
//                gameObject.DestroyRecursive();
            }
            else
            {
                // wave can go through
                // no op
            }
        }
        else
        {
            // kill wave
//            wave.gameObject.DestroyRecursive();
        }
    }
}