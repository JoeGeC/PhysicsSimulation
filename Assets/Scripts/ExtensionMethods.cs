using UnityEngine;

public static class ExtensionMethods
{
    public static float Radius(this GameObject sphere)
    {
        return sphere.GetComponent<MeshFilter>().mesh.bounds.extents.x;
    }
}