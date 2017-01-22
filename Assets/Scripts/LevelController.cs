using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController INSTANCE;

    public Base firstBase;

    [HideInInspector] public readonly List<WaveEmitter> waveEmitters = new List<WaveEmitter>();

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