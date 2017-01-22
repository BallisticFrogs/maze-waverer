using UnityEngine;

[RequireComponent(typeof(Base))]
public class StartBase : MonoBehaviour
{
    public static StartBase INSTANCE;
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