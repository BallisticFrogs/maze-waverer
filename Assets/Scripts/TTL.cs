using UnityEngine;

public class TTL : MonoBehaviour
{
    public float timeToLive = 1f;

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        if (time >= timeToLive)
        {
            gameObject.DestroyRecursive();
        }
    }
}