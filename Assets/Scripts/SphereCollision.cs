using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    public GameObject sphereToCollideWith;
    private float r1;
    private float r2;
    private Vector3 v;

    void Start()
    {
        var sphereProperties = GetComponent<Sphere>();
        r1 = GetComponent<MeshFilter>().mesh.bounds.extents.x;
        r2 = sphereToCollideWith.GetComponent<MeshFilter>().mesh.bounds.extents.x;
        v = sphereProperties.velocity;
    }

    void Update()
    {
        var a = transform.position - sphereToCollideWith.transform.position;
        var q = Angle(v, a);
        var d = Mathf.Sin(q) * Vector3.Magnitude(a);
        if (!SpheresCanCollide(d)) return;
        var e = Mathf.Sqrt(Mathf.Pow(r1 + r2, 2) - Mathf.Pow(d, 2));
        var vc = Mathf.Cos(q) * Vector3.Magnitude(a) - e;
        if (SpheresHaveCollided(vc)) StopSphere();
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres) { return shortestDistanceBetweenSpheres < r1 + r2; }

    private bool SpheresHaveCollided(float distanceBetweenSpheres) { return distanceBetweenSpheres <= r1 + r2; }
    
    private void StopSphere()
    {
        var sphereProperties = GetComponent<Sphere>();
        var emptyVector = new Vector3(0, 0, 0);
        sphereProperties.velocity = emptyVector;
        sphereProperties.acceleration = emptyVector;
        GetComponent<EulerTrajectory>().UpdateProperties();
    }
}
