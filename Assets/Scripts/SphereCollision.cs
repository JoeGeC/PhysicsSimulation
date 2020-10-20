using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    public GameObject sphereToCollideWith;
    public Trajectory trajectory;
    private float r1;
    private float r2;

    void Start()
    {
        r1 = GetComponent<MeshFilter>().mesh.bounds.extents.x;
        r2 = sphereToCollideWith.GetComponent<MeshFilter>().mesh.bounds.extents.x;
    }

    void Update()
    {
        var a = sphereToCollideWith.transform.position - transform.position;
        var q = Angle(trajectory.velocity, a);
        var d = Mathf.Sin(q) * Vector3.Magnitude(a);
        if (!SpheresCanCollide(d)) return;
        var e = Mathf.Sqrt((r1 + r2) * 2 - d * d);
        var vc = Mathf.Cos(q) * Vector3.Magnitude(a) - e;
        if (SpheresHaveCollided(vc)) trajectory.Stop();
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres) { return shortestDistanceBetweenSpheres < r1 + r2; }

    private bool SpheresHaveCollided(float distanceBetweenSpheres) { return distanceBetweenSpheres <= r1 + r2; }
}
