using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public List<WaveEmitter> waveEmitters = new List<WaveEmitter>();

    private void Awake()
    {
        INSTANCE = this;
    }

    public void OnDestroy()
    {
        INSTANCE = null;
    }

    public void UpdateWaveEmittersMaterials()
    {
        for (int i = 0; i < waveEmitters.Count; i++)
        {
            WaveEmitter currWaveEmitter = waveEmitters[i];
            currWaveEmitter.UpdateMaterial();
        }
    }
}