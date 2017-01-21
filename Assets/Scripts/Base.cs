using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base START;
    public static Base END;

    public Collider teleportCollider;

    public WaveType waveType;

    public bool start;
    public bool end;

    void Awake()
    {
        if (start) START = this;
        if (end) END = this;
    }

    private void OnDestroy()
    {
        if (start) START = null;
        if (end) END = null;
    }

//    void Update()
//    {
//    }
}