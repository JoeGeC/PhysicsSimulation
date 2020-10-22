using UnityEngine;

public static class ExtensionMethods
{
    public static float Radius(this GameObject sphere)
    {
        return sphere.transform.localScale.x * 0.5f;
    }
}