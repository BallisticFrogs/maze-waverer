using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public WaveEmitter doorWallWaveEmitter;

    private void Awake()
    {
        INSTANCE = this;
    }

    public void OnDestroy()
    {
        INSTANCE = null;
    }

    void Start()
    {
        Player.INSTANCE.TeleportToBase(StartBase.INSTANCE.playerBase);
    }

    public void TeleportPlayerToBase(Base baseComponent)
    {
        // when reaching the end
        // in fact, teleport to the start again
        if (baseComponent == EndBase.INSTANCE.playerBase)
        {
            baseComponent = StartBase.INSTANCE.playerBase;
        }

        // when going back to the start
        if (baseComponent == StartBase.INSTANCE.playerBase)
        {
            GameMenu.INSTANCE.UnloadLevel();
        }

        Player.INSTANCE.TeleportToBase(baseComponent);
        LevelController.INSTANCE.UpdateWaveEmittersMaterials();
    }

    public void ResetLevelData()
    {
        StartBase.INSTANCE.playerBase.waveType = WaveType.FLAT;
        doorWallWaveEmitter.ChangeWaveType(WaveType.FLAT);
        doorWallWaveEmitter.enabled = false;
    }

    public void OnLevelLoaded()
    {
        // checks for failfast with explicit messages
        if (LevelController.INSTANCE == null) throw new UnityException("EndBase is missing");
        if (EndBase.INSTANCE == null) throw new UnityException("EndBase is missing");
//        if (EndBase.INSTANCE == null) throw new UnityException("EndBase is missing");

        WaveType waveType = WaveType.SINE;
        if (LevelController.INSTANCE.firstBase != null) waveType = LevelController.INSTANCE.firstBase.waveType;

        // change base type
        StartBase.INSTANCE.playerBase.waveType = waveType;

        // change door-wall type
        doorWallWaveEmitter.enabled = true;
        doorWallWaveEmitter.waveType = waveType;
        doorWallWaveEmitter.UpdateMaterial();
    }
}