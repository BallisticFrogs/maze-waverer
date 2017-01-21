using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    // Use this for initialization
    private void Awake()
    {
        INSTANCE = this;
    }

    public void OnDestroy()
    {
        INSTANCE = null;
    }
}