using UnityEngine;

public static class GameObjectExtensions
{
    public static void DestroyRecursive(this GameObject obj, float ttl = 0)
    {
        for (int i = obj.transform.childCount - 1; i >= 0; i--)
        {
            Transform childTransform = obj.transform.GetChild(i);
            DestroyRecursive(childTransform.gameObject, ttl);
        }
        Object.DestroyObject(obj, ttl);
    }
}