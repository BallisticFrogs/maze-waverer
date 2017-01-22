using UnityEngine;

public class EndBase : MonoBehaviour
{
    public static EndBase INSTANCE;
    [HideInInspector] public Base playerBase;

    void Awake()
    {
        INSTANCE = this;
        playerBase = GetComponent<Base>();
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }
}